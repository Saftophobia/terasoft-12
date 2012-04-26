using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mechanect.Cameras;

namespace Mechanect.Classes
{
    class Environment3
    {
        private Hole hole;
        public Ball ball;
        public User3 user;
        private float wind;
        public float friction;
        private bool hasCollidedWithBall, ballShot;
        public static int angleTolerance { get; set; }
        public static int velocityTolerance { get; set; }
        private Vector2 tolerance;
        public Bar distanceBar;
        private GraphicsDevice device;

        private Effect effect;
        private VertexPositionColorNormal[] vertices;
        //private Matrix viewMatrix;
        //private Matrix projectionMatrix;
        private short[] indices;

        private float angle;
        public short terrainWidth {get; private set;}
        public short terrainHeight { get; private set; }

        private float[,] heightData; //2D array
        private VertexBuffer myVertexBuffer;
        private IndexBuffer myIndexBuffer;

        private Vector3 cameraPosition;
        private float leftrightRot;
        private float updownRot;
        private const float rotationSpeed = 0.3f;
        private const float moveSpeed = 30.0f;
        private MouseState originalMouseState;

        private Texture2D[] skyboxTextures;
        private Model skyboxModel;
        private ContentManager Content;

        public Environment3(SpriteBatch spriteBatch, ContentManager Content2, GraphicsDevice device,User3 user)
        {
            #region dummyInitializations
            /* the values used here should allow the ball to reach the user's feet.
             * the velocity is added to the position. friction is reduced
             * from the Z component of the velocity each time.  
             */
            this.user = user;
            Content = Content2;
            this.device = device;
            ball = new Ball(0.5f, 5f,device,Content);
            ball.InitialBallPosition = new Vector3(-60, 3, 2);//-60,3,30
            user.ShootingPosition = new Vector3(0f, 3, 62f);
            //friction = 2f/3600;
            friction = 0f;
            wind = 0f;
            ball.Position = ball.InitialBallPosition;
            ball.InitialVelocity = new Vector3(10, 0, 10)/60;//30x
            ball.Radius = 1;
            ball.Velocity = ball.InitialVelocity;
            ball.Mass = 2;
            user.AssumedLegMass = 20;
            
            #endregion
           
            Vector3 ballPos = ball.Position;
            Vector3 ballInitPos = ball.InitialBallPosition;
            Vector3 shootingPos = user.ShootingPosition;
            distanceBar = new Bar(new Vector2((0.95f*device.Viewport.Width), (0.5f*device.Viewport.Height)), spriteBatch, new Vector2(ballInitPos.X, ballInitPos.Z), new Vector2(ballPos.X, ballPos.Z), new Vector2(shootingPos.X, shootingPos.Z), Content);
            leftrightRot = MathHelper.PiOver2;
            updownRot = -MathHelper.Pi / 10.0f;
            angle = 0f;

        }


        /// <summary>
        /// This method verifies whether a method is solvable.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Ahmed Badr </para>
        ///</remarks>
        /// <returns>Retuns an int that represents the type of the problem with the experiment</returns>
        public int IsSolvable()
        {

            if (ball.Radius <= 0)
                return Constants3.negativeBRradius;
            if (ball.Mass <= 0)
                return Constants3.negativeBMass;
            if (hole.Radius <= 0)
                return Constants3.negativeHRadius;
            if (user.AssumedLegMass <= 0)
                return Constants3.negativeLMass;
            //hole position not before the leg position
            if (hole.Position.Z - user.ShootingPosition.Z > 0)
                return Constants3.negativeHPosZ;
            if (friction <= 0)
                return Constants3.negativeFriction;
            if (ball.Radius > hole.Radius)
                return Constants3.negativeRDifference;

            var finalPos = Vector3.Zero;
            finalPos = ballFinalPosition(GetVelocityAfterCollision(new Vector3(Constants3.maxVelocityX, 0, Constants3.maxVelocityZ)));

            if (Vector3.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector3.Subtract(hole.Position, user.ShootingPosition).LengthSquared())
                return Constants3.holeOutOfFarRange;

            finalPos = ballFinalPosition(GetVelocityAfterCollision(new Vector3(Constants3.minVelocityX, 0, Constants3.minVelocityZ)));

            if (Vector3.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector3.Subtract(hole.Position, user.ShootingPosition).LengthSquared()) //length squared used for better performance than length
                return Constants3.holeOutOfNearRange;

            return Constants3.solvableExperiment;
        }

        /// <summary>
        /// Generates a solvable experiment.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Ahmed Badr </para>
        ///</remarks>
        public void GenerateSolvable()
        {
            hole.Radius =3;
            ball.Radius = 1;

            hole.SetHoleValues();

            var x = Constants3.solvableExperiment;
            do
            {
                x = IsSolvable();
                switch (x)
                {
                    case Constants3.holeOutOfNearRange: if (hole.Position.X == Constants3.maxHolePosX && hole.Position.Z != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Add(hole.Position, new Vector3(0, 0, -1)));
                        else if (hole.Position.Z == Constants3.maxHolePosX && hole.Position.X != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Add(hole.Position, new Vector3(1, 0, 0)));
                        else friction++; break;
                    case Constants3.holeOutOfFarRange: if (hole.Position.X == Constants3.maxHolePosX && hole.Position.Z != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Subtract(hole.Position, new Vector3(0, 0, -1)));
                        else if (hole.Position.Y == Constants3.maxHolePosX && hole.Position.X != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Subtract(hole.Position, new Vector3(1, 0, 0)));
                        else if (wind != 0)
                            wind--;
                        else if (friction != 1)
                            friction--;
                        else if (friction <= 1)
                            friction -= 0.1f;
                        break;
                    case Constants3.negativeRDifference: int tmp = ball.Radius; ball.Radius = (hole.Radius); hole.Radius = (tmp); break;
                    case Constants3.negativeLMass: user.AssumedLegMass *= -1; break;
                    case Constants3.negativeBMass: ball.Mass *= -1; break;
                    case Constants3.negativeBRradius: ball.Radius *= -1; break;
                    case Constants3.negativeHRadius: hole.Radius *= -1; break;
                    case Constants3.negativeFriction: friction *= -1; break;
                    case Constants3.negativeHPosZ: hole.Position = Vector3.Subtract(hole.Position, new Vector3(1, 0, 0)); break;
                }
            } while (x != Constants3.solvableExperiment);
        }




        /// <remarks>
        ///<para>AUTHOR: Ahmad Sanad </para>
        ///</remarks>
        /// <summary>
        /// Takes the initial velocity of the ball and the friction and calculates it's final position
        /// </summary>
        /// <param name="velocity">
        /// The initial velocity of the ball after being shot.
        /// </param>
        /// <returns>
        /// Returns the position of the ball when its velocity reaches 0.
        /// </returns>
        private Vector3 ballFinalPosition(Vector3 velocity)
        {

            var vxsquared = (float)Math.Pow(velocity.X, 2);
            var vzsquared = (float)Math.Pow(velocity.Z, 2);
            float x = (vxsquared / (2 * friction)) + ball.InitialBallPosition.X;
            float z = (vzsquared / (2 * friction)) + ball.InitialBallPosition.Z;
            return new Vector3(x, 0, z);
        }

        /// <remarks>
        ///<para>AUTHOR: Ahmad Sanad </para>
        ///</remarks>
        /// <summary>
        /// Checks whether or not the ball will reach the hole with zero velocity, by checking if the user shot it with the optimum velocity, and calls methods to inform the user if he won or not.
        /// </summary>
        private void hasScored()
        {
            Vector3 hole = this.hole.Position;
            Vector3 ballVelocity = ball.Velocity;
            Vector3 InitialPosition = ball.Position;
            var optimumVx = (float)Math.Sqrt((2 * (wind + friction)) * (hole.X - InitialPosition.X));
            var optimumVy = (float)Math.Sqrt((2 * (wind + friction)) * (hole.Y - InitialPosition.Y));

            if ((ballVelocity.X <= (optimumVx + tolerance.X)) && (ballVelocity.Y <= (optimumVy + tolerance.X + this.hole.Radius))
            && (ballVelocity.X >= (optimumVx - tolerance.Y)) && (ballVelocity.Y >= (optimumVy - tolerance.Y + this.hole.Radius)))
            {
                BallFallIntoHole();
                //winningWord(); to be implemented by Hegazy, commented to remove error
            }

            else
            {
                //losingWord(); to be implemented by Hegazy, commented to remove error
            }
        }

        #region Environment Generation Code


        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Inizializes the Environment
        /// </summary>
        public void InitializeEnvironment(GraphicsDevice g)
        {
            device = g;
               

        }

        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Loads the content of the environment. Similar to the LoadConent() method of XNA
        /// </summary>
        public void LoadEnvironmentContent()
        {
            
            effect = Content.Load<Effect>("Textures/effects");
            //SetUpCamera();


            //UpdateViewMatrix();
            Mouse.SetPosition(device.Viewport.Width / 2, device.Viewport.Height / 2);
            originalMouseState = Mouse.GetState();

            Texture2D heightMap = Content.Load<Texture2D>("Textures/heightmap");
            LoadHeightData(heightMap);
           
            InitializeHole();
            
            skyboxModel = LoadModel("Models/skybox2", out skyboxTextures);

            SetUpVertices();
            SetUpIndices();
            CalculateNormals();
            CopyToBuffers();
           
            ball.LoadContent();

        }

        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Updates the environment. Similar to the Update() method of XNA, and to be called in it.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /*public void UpdateEnvironment(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Delete))
                angle += 0.05f;
            if (keyState.IsKeyDown(Keys.PageDown))
                angle -= 0.05f;
            float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            ProcessInput(timeDifference);
        }*/


        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Draws the environment. Similar to the Draw() method of XNA
        /// </summary>
        ///<param name="gameTime">Provides a snapshot of timing values.</param>
        public void DrawEnvironment(Camera c, GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawSkybox(c);
            var rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;
            device.RasterizerState = rs;
            var worldMatrix = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f) * Matrix.CreateRotationY(angle);
            effect.CurrentTechnique = effect.Techniques["Colored"];
            var lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();
            effect.Parameters["xLightDirection"].SetValue(lightDirection);
            effect.Parameters["xAmbient"].SetValue(0.1f);
            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xView"].SetValue(c.view);
            effect.Parameters["xProjection"].SetValue(c.projection);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.Indices = myIndexBuffer;
                device.SetVertexBuffer(myVertexBuffer);

                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionColorNormal.VertexDeclaration);
            }
            DrawHole(c);
            distanceBar.Draw();
        }

        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Creates the vertices for the triangles used to generate the terrain, and sets their height and color according to the height map.
        /// </summary>
        private void SetUpVertices()
        {
            var minHeight = float.MaxValue;
            var maxHeight = float.MinValue;
            for (var x = 0; x < terrainWidth; x++)
            {
                for (var y = 0; y < terrainHeight; y++)
                {
                    if (heightData[x, y] < minHeight)
                        minHeight = heightData[x, y];
                    if (heightData[x, y] > maxHeight)
                        maxHeight = heightData[x, y];
                }
            }

            vertices = new VertexPositionColorNormal[terrainWidth * terrainHeight];
            for (var x = 0; x < terrainWidth; x++)
            {
                for (var y = 0; y < terrainHeight; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y], -y);
                    vertices[x + y * terrainWidth].Color = Color.Green;

                }
            }
            

        }




        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Creates the indices of the triangles used to generate the terrain. 
        /// This data is used to connect the vertices previously created to make them into triangles.
        /// </summary>
        private void SetUpIndices()
        {
            indices = new short[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            var counter = 0;
            for (var y = 0; y < terrainHeight - 1; y++)
            {
                for (var x = 0; x < terrainWidth - 1; x++)
                {
                    var lowerLeft = x + y * terrainWidth;
                    var lowerRight = (x + 1) + y * terrainWidth;
                    var topLeft = x + (y + 1) * terrainWidth;
                    var topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = (short)topLeft;
                    indices[counter++] = (short)lowerRight;
                    indices[counter++] = (short)lowerLeft;

                    indices[counter++] = (short)topLeft;
                    indices[counter++] = (short)topRight;
                    indices[counter++] = (short)lowerRight;
                }
            }
        }


        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Iterates on evey pixel in the grayscale heightmap, and adds height data depending on the color of each pixel to the 2D array heightMap
        /// </summary>
        /// <param name="heightMap">The grayscale picture that will be used to define the heightmap</param>
        private void LoadHeightData(Texture2D heightMap)
        {
            terrainWidth = (short)heightMap.Width;
            terrainHeight = (short)heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainHeight];
            for (var x = 0; x < terrainWidth; x++)
                for (var y = 0; y < terrainHeight; y++)
                    heightData[x, y] = (heightMapColors[x + y * terrainWidth].R / 5.0f) - 20;

        }

        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Copies the vertices and indeces to GPU buffers. 
        /// This allow the data to be called from the GPU's memory directly without having to send it to the GPU everytime the Draw() method is called.
        /// This should increase performance as the GPU memory is generally faster.
        /// </summary>
        private void CopyToBuffers()
        {
            myVertexBuffer = new VertexBuffer(device, VertexPositionColorNormal.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            myVertexBuffer.SetData(vertices);
            myIndexBuffer = new IndexBuffer(device, typeof(short), indices.Length, BufferUsage.WriteOnly);
            myIndexBuffer.SetData(indices);
        }

        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Contains the position, color and normal of the vertices
        /// </summary>
        public struct VertexPositionColorNormal
        {
            public Vector3 Position;
            public Color Color;
            public Vector3 Normal;

            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
            (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
            );
        }


        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Calculates the normal to the planes of the triangles and adds this info to the normal of vertices defined by VertexPositioColorNormal.
        /// </summary>
        private void CalculateNormals()
        {
            for (var i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);
            for (var i = 0; i < indices.Length / 3; i++)
            {
                var index1 = indices[i * 3];
                var index2 = indices[i * 3 + 1];
                var index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;

            }

            for (var i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }


        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Updates the view matrix depending on the movement of the camera.
        /// </summary> 
        /*private void UpdateViewMatrix()
        {
            var cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            var cameraOriginalTarget = new Vector3(0, 0, -1);
            var cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            var cameraFinalTarget = cameraPosition + cameraRotatedTarget;

            var cameraOriginalUpVector = new Vector3(0, 1, 0);
            var cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }*/


        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Processes the movement of the mouse, and the keys pressed on the keyboard.
        /// </summary>
        /// <param name="amount"></param>
        /*private void ProcessInput(float amount)
        {
            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != originalMouseState)
            {
                var xDifference = currentMouseState.X - originalMouseState.X;
                var yDifference = currentMouseState.Y - originalMouseState.Y;
                leftrightRot -= rotationSpeed * xDifference * amount;
                updownRot -= rotationSpeed * yDifference * amount;
                Mouse.SetPosition(device.Viewport.Width / 2, device.Viewport.Height / 2);
                UpdateViewMatrix();
            }
            var moveVector = new Vector3(0, 0, 0);
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                moveVector += new Vector3(0, 0, -1);
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                moveVector += new Vector3(0, 0, 1);
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                moveVector += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                moveVector += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.Q))
                moveVector += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.Z))
                moveVector += new Vector3(0, -1, 0);
            AddToCameraPosition(moveVector * amount);
        }*/


        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Creates a matrix to be able to rotate the camera, and updates the view matrix correspondingly.
        /// </summary>
        /// <param name="vectorToAdd"></param>
        /*private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            var cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            cameraPosition += moveSpeed * rotatedVector;
            UpdateViewMatrix();
        }*/

        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Loads a model and adds a texture to it.
        /// </summary>
        /// <param name="assetName">The name of the model to be loaded</param>
        /// <param name="textures">The name of the texture to be mapped on the model</param>
        /// <returns>Returns the model after adding the texture effect</returns>
        private Model LoadModel(string assetName, out Texture2D[] textures)
        {

            Model newModel = Content.Load<Model>(assetName);
            textures = new Texture2D[newModel.Meshes.Count];
            var i = 0;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    textures[i++] = currentEffect.Texture;

            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();

            return newModel;
        }

        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Creates, draws and adds effects to the skybox to display the sky all in all directions with a constant distance from the camera.
        /// </summary>
        private void DrawSkybox(Camera c)
        {
            var ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            device.SamplerStates[0] = ss;

            var dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            device.DepthStencilState = dss;

            Matrix[] skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
            skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
            var i = 0;
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    var worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(c.position);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(c.view);
                    currentEffect.Parameters["xProjection"].SetValue(c.projection);
                    currentEffect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
                }
                mesh.Draw();
            }
            //DrawHole();

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            device.DepthStencilState = dss;
        }

        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <summary>
        /// Sets up the view matrix and projection matrix defining the camera.
        /// </summary>
        /*private void SetUpCamera()
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1.0f, 300.0f);
        }*/

        #endregion

        #region Hole methods

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </pfzara>
        ///</remarks>
        /// <summary>
        /// Initializes all variables needed to draw the hole.
        /// </summary>

        protected void InitializeHole()
        {
            // TODO: Add your initialization logic here
            hole = new Hole(Content,device ,terrainWidth ,terrainHeight ,4 ,user.ShootingPosition);
        }


        
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Draws the 3D hole by rendering each effect in each mesh in the hole model.
        /// </summary>
        protected void DrawHole(Camera cam)
        {
            hole.DrawHole(cam);
            // TODO: Add your drawing code here

            //base.Draw(gameTime);
        }

        #endregion

        #region Omar's Methods

        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Update Method.
        /// </summary>
        public void Update()
        {
            Tools3.update_MeasuringVelocityAndAngle(user);
            CheckCollision();
            Shoot();
        }

        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Initialize Method.
        /// </summary>
        public void Initialize()
        {
            hasCollidedWithBall = false;
            ballShot = false;
        }

        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Updates the balls velocity according to the speed and angle the user shot with.
        /// </summary>
        private void Shoot()
        {
            float relativeVelocity = Tools3.setVelocityRelativeToGivenMass(user);
            Vector3 initialLegVelocity; //This variable represents the velocity of the leg with which the user has shot the ball.
            initialLegVelocity = new Vector3((float)(relativeVelocity * Math.Cos(user.Angle)), 0, -(float)(relativeVelocity * Math.Sin(user.Angle)));
            
            if (hasCollidedWithBall && !ballShot)
            {
                ballShot = true;
                Vector3 velocityAfterCollision = GetVelocityAfterCollision(initialLegVelocity); //calculate the velocity of the ball right after the collision
                ball.Velocity = velocityAfterCollision; // update the velocity of the ball

                this.ball.Velocity = velocityAfterCollision;
            }
        }
        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Checks if the users leg has collided with the ball.
        /// </summary>
        private void CheckCollision()
        {

            Vector3 legPosition; //Current position of leg.
            if (user.RightLeg)
                legPosition = new Vector3((float)user.CurrentRightLegPositionX, 0, (float)user.CurrentRightLegPositionZ);
            else
                legPosition = new Vector3((float)user.CurrentLeftLegPositionX, 0, (float)user.CurrentLeftLegPositionZ);

            if (Math.Abs(Vector3.Subtract(ball.Position, legPosition).Length()) < 5)
            {
                hasCollidedWithBall = true;
                
            }
            else
                hasCollidedWithBall = false;
        }
        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Calculates the balls velocity after collision using conservation of momentum.
        /// </summary>
        /// <param name="initialVelocity">Legs initial velocity prior to collision.</param>
        /// <returns>Vector3 Ball velocity after collision.</returns>
        private Vector3 GetVelocityAfterCollision(Vector3 initialVelocity)
        {
            double initialVelocityLeg, initialVelocityBall, finalVelocityBall, angle;

            //Get the mass of the leg.
            double assumedLegMass = user.AssumedLegMass;

            //Get the mass of the ball.
            double ballMass = ball.Mass;

            double acceleration = -(friction + wind); //Deceleration of the ball due to resistance.

            //Get the velocity of the ball right before the collision. 
            //If shooting the ball .. initial balls velocity is its current velocity.. else calculate it.
            if (!ballShot)
                initialVelocityBall = Math.Sqrt((ball.InitialVelocity.Length() + (2 * acceleration * Math.Abs(Vector3.Distance(ball.Position, user.ShootingPosition)))));
            else
                initialVelocityBall = ball.Velocity.Length();

            initialVelocityLeg = initialVelocity.Length();

            //Calculate the angle with which the user has shot the ball.
            angle = Math.Atan2(-initialVelocity.Z, initialVelocity.X);

            //Calculate what will the ball's speed be after collision using conservation of momentum equation.
            finalVelocityBall = ((assumedLegMass * initialVelocityLeg) + (ballMass * initialVelocityBall) - (assumedLegMass * (initialVelocityLeg * (1 - ballMass / ball.maxMass)))) / ballMass;

            //Return a vector containing the ball's speed and direction.
            return new Vector3((float)(finalVelocityBall * Math.Cos(angle)), 0, -(float)(finalVelocityBall * Math.Sin(angle)));
        }


        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Simulates ball falling into a hole by updating its velocity when it reaches the hole.
        /// </summary>
        private void BallFallIntoHole()
        {
            //Waiting for completed class Hole from Khaled Salah

            //Get the balls Velocity.
            Vector3 newVelocity = ball.Velocity;

            //Set the balls velocity to the old velocity plus -10j (Downward Motion)
            newVelocity = Vector3.Add(newVelocity, new Vector3(0, -10, 0));

            //Update the balls velocity
            ball.Velocity = newVelocity;
        }
        #endregion
    }
}
