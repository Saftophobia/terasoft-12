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
using UI.Cameras;
using Mechanect.Common;
using UI.Components;
using Mechanect.Exp3;
using Mechanecht.Common;
namespace Mechanect.Exp3
{
    public class Environment3
    {
        private Hole hole;
        public Hole HoleProperty
        {
            set
            {
                hole = value;
            }
            get
            {
                return hole;
            }
        }
        public Ball ball;
        public User3 user;
        private float wind;
        private float friction;
        public float Friction
        {
           
            set
            {
                friction = value;
            }
            get
            {
                return friction;
            }
        }

        public static int angleTolerance { get; set; }
        public static int velocityTolerance { get; set; }
        private Bar distanceBar;
        public Bar DistanceBar
        {
            set
            {
                distanceBar = value;
            }
            get
            {
                return distanceBar;
            }
        }
        private GraphicsDevice device;

        private Effect effect;
        private VertexPositionColorNormal[] vertices;
        private short[] indices;
        public short terrainWidth {get; private set;}
        public short terrainHeight { get; private set; }

        private float[,] heightData; //2D array
        private VertexBuffer myVertexBuffer;
        private IndexBuffer myIndexBuffer;

        private Texture2D[] skyboxTextures;
        private Model skyboxModel;
        private ContentManager Content;
        private SpriteBatch sprite;

        public SkinnedCustomModel PlayerModel { get; private set; }

        private Vector3 ballInitialPosition, ballInitialVelocity;
        //Texture2D grassTexture;
        //Texture2D cloudMap;
        //Model skyDome;
        //RenderTarget2D cloudsRenderTarget;
        //Texture2D cloudStaticMap;
        //VertexPositionTexture[] fullScreenVertices;



        public Environment3(Vector3 initialBallPosition, Vector3 initialBallVelocity, SpriteBatch spriteBatch, ContentManager Content2, GraphicsDevice device,User3 user, Ball ball)
        {
            #region dummyInitializations
            /* the values used here should allow the ball to reach the user's feet.
             * the velocity is added to the position. friction is reduced
             * from the Z component of the velocity each time.  
             */
            this.user = user;
            Content = Content2;
            this.device = device;
            this.ball = ball;
           // ball = new Ball(10000.0f, 10001.0f, device, Content);
            user.ShootingPosition = new Vector3(0f, 3, 62f);
            //friction = 0.5f/3600;
            wind = 0f;
            //ball.InitialVelocity = new Vector3(11.25f, 0, 11.25f)/60;
            ball.Radius = 1;
            ball.Mass = 2;
            user.AssumedLegMass = 0.01;
            
            #endregion
            sprite = spriteBatch;
            Vector3 ballPos = ball.Position;

            ballInitialPosition = initialBallPosition;
            ballInitialVelocity = initialBallVelocity;

            Vector3 shootingPos = user.ShootingPosition;
            distanceBar = new Bar(new Vector2((0.95f*device.Viewport.Width), (0.5f*device.Viewport.Height)), spriteBatch, new Vector2(ballInitialPosition.X, ballInitialPosition.Z), new Vector2(ballPos.X, ballPos.Z), new Vector2(shootingPos.X, shootingPos.Z), Content);
            friction = 1.0f;

            PlayerModel = new SkinnedCustomModel(Content2.Load<Model>("dude"), shootingPos, 
                new Vector3(0, 9.3f, 0), new Vector3(0.5f, 0.5f, 0.5f));
        }


        /// <summary>
        /// This method verifies whether the experiment is solvable.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Ahmed Badr. </para>
        ///</remarks>
        /// <returns>Retuns an int that represents the type of the problem with the experiment.</returns>
        private int IsSolvable()
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
           
            if (Vector3.DistanceSquared(finalPos, user.ShootingPosition) < Vector3.DistanceSquared(hole.Position, user.ShootingPosition))
                return Constants3.holeOutOfFarRange;

            
            if (Vector3.DistanceSquared(finalPos, user.ShootingPosition) > Vector3.DistanceSquared(hole.Position, user.ShootingPosition)) //length squared used for better performance than length
                return Constants3.holeOutOfNearRange;
            else
                return Constants3.solvableExperiment;
        }

        /// <summary>
        /// Generates a solvable experiment.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Ahmed Badr. </para>
        ///</remarks>
        public void GenerateSolvable()
        {

            var x = Constants3.solvableExperiment;
            do
            {
                x = IsSolvable();
                switch (x)
                {
                    case Constants3.holeOutOfNearRange: friction++; break;
                    case Constants3.holeOutOfFarRange: 
                        if (friction > 1)
                            friction--;
                        else if (wind > 1) 
                            wind--;
                        else hole.Position = new Vector3(hole.Position.X / 2, hole.Position.Y, hole.Position.Z); break; 
                    case Constants3.negativeRDifference: int tmp = ball.Radius; ball.Radius = (hole.Radius); hole.Radius = (tmp); break;
                    case Constants3.negativeLMass: user.AssumedLegMass *= -1; break;
                    case Constants3.negativeBMass: ball.Mass *= -1; break;
                    case Constants3.negativeBRradius: ball.Radius *= -1; break;
                    case Constants3.negativeHRadius: hole.Radius *= -1; break;
                    case Constants3.negativeFriction: friction *= -1; break;
                    case Constants3.negativeHPosZ: hole.Position = Vector3.Add(hole.Position, new Vector3(0, 0, 1)); break;
                }
            } while (x != Constants3.solvableExperiment);
           
            if (hole.Position.Z >= Constants3.maxHolePosZ-hole.Radius)
                hole.Position = new Vector3(hole.Position.X, hole.Position.Y, Constants3.maxHolePosZ - hole.Radius);
            if (Math.Abs(hole.Position.X) > Constants3.maxHolePosX-hole.Radius)
                hole.Position = new Vector3(Constants3.maxHolePosX - hole.Radius, hole.Position.Y, hole.Position.Z);
            if(hole.Position.Z<=user.ShootingPosition.Z)
                hole.Position = new Vector3(hole.Position.X, hole.Position.Y, Constants3.maxHolePosZ - hole.Radius);

        }





        /// <summary>
        /// Takes the initial velocity of the ball and the friction and calculates its final position.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Ahmad Sanad </para>
        ///</remarks>
        /// <param name="velocity">
        /// The initial velocity of the ball after being shot.
        /// </param>
        /// <returns>
        /// Returns the position of the ball when its velocity reaches 0.
        /// </returns>
        private Vector3 BallFinalPosition(Vector3 velocity)
        {
            var vxsquared = (float)Math.Pow(velocity.X, 2);
            var vzsquared = (float)Math.Pow(velocity.Z, 2);
            float x = (vxsquared / (2 * friction)) + ballInitialPosition.X;
            float z = (vzsquared / (2 * friction)) + ballInitialPosition.Z;
            return new Vector3(x, 0, z);
        }

       
        /// <summary>
        /// Checks whether or not the ball will reach the hole with zero velocity, by checking if the user shot it with the optimum velocity, then calls methods to inform the user if he won or not.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Ahmad Sanad </para>
        ///</remarks>
        private void HasScored()
        {
            Vector3 hole = this.hole.Position;
            Vector3 ballVelocity = ballInitialVelocity;
            Vector3 InitialPosition = ballInitialPosition;
            var xComp = (float)(velocityTolerance * Math.Cos(angleTolerance));
            var yComp = (float)(velocityTolerance * Math.Sin(angleTolerance));
            var tolerance = new Vector2(xComp, yComp);
            var optimumVx = (float)Math.Sqrt((2 * (wind + friction)) * (hole.X - InitialPosition.X));
            var optimumVy = (float)Math.Sqrt((2 * (wind + friction)) * (hole.Y - InitialPosition.Y));

            if ((ballVelocity.X <= (optimumVx + tolerance.X)) && (ballVelocity.Y <= (optimumVy + tolerance.X + this.hole.Radius))
            && (ballVelocity.X >= (optimumVx - tolerance.Y)) && (ballVelocity.Y >= (optimumVy - tolerance.Y + this.hole.Radius)))
            {
                //BallFallIntoHole();
                Tools3.DislayIsWin(sprite,Content,Vector2.Zero, true);
            }

            else
            {
                Tools3.DislayIsWin(sprite, Content, Vector2.Zero, false);
            }
        }

        #region Environment Generation Code


        
        /// <summary>
        /// Initializes the Environment.
        /// </summary>
        /// <param name="g">The graphics device used to display graphics on the screen.</param>
        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        public void InitializeEnvironment(GraphicsDevice g)
        {
            device = g;
               

        }

       
        /// <summary>
        /// Loads the content of the environment.
        /// </summary>
        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        public void LoadEnvironmentContent()
        {
            
            effect = Content.Load<Effect>("Textures/effects");
            
            //effect = Content.Load<Effect>("MYHLSL");
            //grassTexture = Content.Load<Texture2D>("grass2");
            //skyDome = Content.Load<Model>("dome");
            //cloudMap = Content.Load<Texture2D>("cloudMap");
            //skyDome.Meshes[0].MeshParts[0].Effect = effect.Clone();
            //Texture2D heightMap = Content.Load<Texture2D>("Textures/heightmaplargeflat");

            Texture2D heightMap = Content.Load<Texture2D>("Textures/heightmap");
            LoadHeightData(heightMap);
           
            InitializeHole(4);
            
            skyboxModel = LoadModel("Models/skybox2", out skyboxTextures);

            SetUpVertices();
            SetUpIndices();
            CalculateNormals();
            CopyToBuffers();
           
            //ball.LoadContent();

        }




        /// <summary>
        /// Draws the environment. Similar to the Draw() method of XNA and should be called in it.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        ///<param name="gameTime">Provides a snapshot of timing values.</param
        public void DrawEnvironment(Camera c, GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawSkybox(c);
            var rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;
            device.RasterizerState = rs;
            var worldMatrix = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f);
            effect.CurrentTechnique = effect.Techniques["Colored"];
            var lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();
            effect.Parameters["xLightDirection"].SetValue(lightDirection);
            effect.Parameters["xAmbient"].SetValue(0.1f);
            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xView"].SetValue(c.View);
            effect.Parameters["xProjection"].SetValue(c.Projection);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.Indices = myIndexBuffer;
                device.SetVertexBuffer(myVertexBuffer);

                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionColorNormal.VertexDeclaration);
            }
            DrawHole(c);
        }

        /*
        protected void DrawEnvironment(GameTime gameTime)
       {
           float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;

           //Clears the Z buffer
           device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
           GeneratePerlinNoise(time);
           DrawSkyDome(viewMatrix);

           //Creates a rasterizer state removes culling, and makes the fill mode solid, for the triangles to be filled
           RasterizerState rs = new RasterizerState();
           rs.CullMode = CullMode.None;
           rs.FillMode = FillMode.Solid;
           device.RasterizerState = rs;

           Matrix worldMatrix = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f) * Matrix.CreateRotationY(angle);
           //Matrix worldMatrix = Matrix.Identity;
           //Sets the effects to be used from the fx file such as coloring the terrain and adding lighting.
           effect.CurrentTechnique = effect.Techniques["Textured"];
           Vector3 lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
           lightDirection.Normalize();
           effect.Parameters["xLightDirection"].SetValue(lightDirection);
           effect.Parameters["xAmbient"].SetValue(0.1f);
           effect.Parameters["xEnableLighting"].SetValue(true);
           effect.Parameters["xView"].SetValue(viewMatrix);
           effect.Parameters["xProjection"].SetValue(projectionMatrix);
           effect.Parameters["xWorld"].SetValue(worldMatrix);
           effect.Parameters["xTexture"].SetValue(grassTexture);
           foreach (EffectPass pass in effect.CurrentTechnique.Passes)
           {
               pass.Apply();

               device.Indices = myIndexBuffer;
               device.SetVertexBuffer(myVertexBuffer);

               device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionNormalTexture.VertexDeclaration);
               
           }
       }
        
         */

        /// <summary>
        /// Creates the vertices for the triangles used to generate the terrain, and sets their color and height according to the height map.
        /// </summary>
        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
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
                    vertices[x + (y * terrainWidth)].Position = new Vector3(x, heightData[x, y], -y);
                    vertices[x + (y * terrainWidth)].Color = Color.Green;

                }
            }
            

        }

        /*
           private void SetUpVertices()
       {
           vertices = new VertexPositionNormalTexture[terrainWidth * terrainHeight];

           for (int x = 0; x < terrainWidth; x++)
           {
               for (int y = 0; y < terrainHeight; y++)
               {
                   vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y], -y);
                   vertices[x + y * terrainWidth].TextureCoordinate.X = (float)x / 10.0f;
                   vertices[x + y * terrainWidth].TextureCoordinate.Y = (float)y / 10.0f;
               }
           }

           fullScreenVertices = SetUpFullscreenVertices();
           fullScreenVertexDeclaration = new VertexDeclaration(VertexPositionTexture.VertexDeclaration.GetVertexElements());
       }
          
       }*/



        /// <summary>
        /// Creates the indices of the triangles used to generate the terrain. 
        /// This data is used to connect the vertices previously created to make them into triangles.
        /// </summary>
        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        private void SetUpIndices()
        {
            indices = new short[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            var counter = 0;
            for (var y = 0; y < terrainHeight - 1; y++)
            {
                for (var x = 0; x < terrainWidth - 1; x++)
                {
                    var lowerLeft = x + (y * terrainWidth);
                    var lowerRight = (x + 1) + (y * terrainWidth);
                    var topLeft = x + ((y + 1) * terrainWidth);
                    var topRight = (x + 1) + ((y + 1) * terrainWidth);

                    indices[counter++] = (short)topLeft;
                    indices[counter++] = (short)lowerRight;
                    indices[counter++] = (short)lowerLeft;

                    indices[counter++] = (short)topLeft;
                    indices[counter++] = (short)topRight;
                    indices[counter++] = (short)lowerRight;
                }
            }
        }



        /// <summary>
        /// Iterates on evey pixel in the grayscale heightmap, and adds height data depending on the color of each pixel to the 2D array heightMap.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <param name="heightMap">The grayscale picture that will be used to define the heightmap.</param>
        private void LoadHeightData(Texture2D heightMap)
        {
            terrainWidth = (short)heightMap.Width;
            terrainHeight = (short)heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainHeight];
            for (var x = 0; x < terrainWidth; x++)
                for (var y = 0; y < terrainHeight; y++)
                    heightData[x, y] = (heightMapColors[x + (y * terrainWidth)].R / 5.0f) - 20;

        }

        
        /// <summary>
        /// Copies the vertices and indices to GPU buffers. 
        /// This allow the data to be called from the GPU's memory directly without having to send it to the GPU everytime the Draw() method is called.
        /// This should increase performance as the GPU memory is generally faster.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        private void CopyToBuffers()
        {
            myVertexBuffer = new VertexBuffer(device, VertexPositionColorNormal.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            myVertexBuffer.SetData(vertices);
            myIndexBuffer = new IndexBuffer(device, typeof(short), indices.Length, BufferUsage.WriteOnly);
            myIndexBuffer.SetData(indices);
        }

        /// <summary>
        /// Contains the position, color and normal of the vertices.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
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



        /// <summary>
        /// Calculates the normal to the planes of the triangles and adds this info to the normal of vertices defined by VertexPositioColorNormal.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        private void CalculateNormals()
        {
            for (var i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);
            for (var i = 0; i < indices.Length / 3; i++)
            {
                var index1 = indices[i * 3];
                var index2 = indices[(i * 3) + 1];
                var index3 = indices[(i * 3) + 2];

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
        /// Loads a model and adds a texture to it.
        /// </summary>
        /// <param name="assetName">The name of the model to be loaded.</param>
        /// <param name="textures">The name of the texture to be mapped on the model.</param>
        /// <returns>Returns the model after adding the texture effect.</returns>
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


        /// <summary>
        /// Creates, draws and adds effects to the skybox to display the sky all in all directions with a constant distance from the camera.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
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
                    var worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(c.Position);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(c.View);
                    currentEffect.Parameters["xProjection"].SetValue(c.Projection);
                    currentEffect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
                }
                mesh.Draw();
            }
            //DrawHole();

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            device.DepthStencilState = dss;
        }

        /*
        private void DrawSkyDome(Camera c,Matrix currentViewMatrix)
        {
            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferWriteEnable = false;
            device.DepthStencilState = dss;

            Matrix[] modelTransforms = new Matrix[skyDome.Bones.Count];
            skyDome.CopyAbsoluteBoneTransformsTo(modelTransforms);

            Matrix wMatrix = Matrix.CreateTranslation(0, -0.3f, 0) * Matrix.CreateScale(100) * Matrix.CreateTranslation(cameraPosition);
            foreach (ModelMesh mesh in skyDome.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(c.View);
                    currentEffect.Parameters["xProjection"].SetValue(c.Projection);
                    currentEffect.Parameters["xTexture"].SetValue(cloudMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);
                }
                mesh.Draw();
            }
            DepthStencilState dss2 = new DepthStencilState();
            dss2.DepthBufferWriteEnable = true;
            device.DepthStencilState = dss2;
        }
        

        private Texture2D CreateStaticMap(int resolution)
        {
            Random rand = new Random();
            Color[] noisyColors = new Color[resolution * resolution];

            for (int x = 0; x < resolution; x++)
            {
                for (int y = 0; y < resolution; y++)
                {
                    noisyColors[x + y * resolution] = new Color(new Vector3((float)rand.Next(1000) / 1000.0f, 0, 0));
                }
            }
            Texture2D noiseImage = new Texture2D(device, 32, 32, false, SurfaceFormat.Color);
            noiseImage.SetData(noisyColors);
            return noiseImage;
        }

        private VertexPositionTexture[] SetUpFullscreenVertices()
        {
            VertexPositionTexture[] vertices = new VertexPositionTexture[4];
            vertices[0] = new VertexPositionTexture(new Vector3(-1, 1, 0f), new Vector2(0, 1));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 1, 0f), new Vector2(1, 1));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, -1, 0f), new Vector2(0, 0));
            vertices[3] = new VertexPositionTexture(new Vector3(1, -1, 0f), new Vector2(1, 0));

            return vertices;
        }



        private void GeneratePerlinNoise(float time)
        {

            device.SetRenderTarget(cloudsRenderTarget);
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            effect.CurrentTechnique = effect.Techniques["PerlinNoise"];
            effect.Parameters["xTexture"].SetValue(cloudStaticMap);
            effect.Parameters["xOvercast"].SetValue(1.1f);
            effect.Parameters["xTime"].SetValue(time / 1000.0f);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {

                pass.Apply();
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, fullScreenVertices, 0, 2);


            }

            device.SetRenderTarget(null);
            cloudMap = cloudsRenderTarget;
        }
        */
        #endregion

        #region Hole methods
        /// <summary>
        /// Initializes all variables needed to draw the hole.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </pfzara>
        ///</remarks>
        
        protected void InitializeHole(int radius)
        {
            // TODO: Add your initialization logic here
            hole = new Hole(Content,device ,terrainWidth ,terrainHeight ,radius ,user.ShootingPosition);
        }


        /// <summary>
        /// Draws the 3D hole by rendering each effect in each mesh in the hole model.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        
        protected void DrawHole(Camera cam)
        {
            hole.DrawHole(cam);
            int radius = hole.Radius;
            Vector3 scale = new Vector3((float)0.02 * radius);
            int outerRadius = radius * 2 + 2;
             double angleStep = 1f / radius;
                for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
                {
                    int a = (int)Math.Round(radius + radius * Math.Cos(angle));
                    int b = (int)Math.Round(radius + radius * Math.Sin(angle));

                
                  //for (int x = 50; x <= 61; x++)
                  //  {
                  //      for (int y = 50; y <= 61; y++)
                  //      {
                  //          vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y] - 20, -y);
                  //          vertices[x + y * terrainWidth].Color = Color.Transparent;
                  //      }
                  //  }
                }
                for (int x = 50; x <= 61; x++)
                {
                    for (int y = 50; y <= 61; y++)
                    {
                        for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
                        {
                            int a = (int)Math.Round(radius + radius * Math.Cos(angle));
                            int b = (int)Math.Round(radius + radius * Math.Sin(angle));
                            vertices[a+x + (b+y) * terrainWidth].Color = Color.White;
                            vertices[a+x + (b+y) * terrainWidth].Position = new Vector3(x+a, heightData[x+a, y+b] - 20, -y-b);
                        }
                    }
                }

            }
            // TODO: Add your drawing code here

            //base.Draw(gameTime);
        

        #endregion

        #region Ball Control Methods

        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Returns the balls velocity according to the speed and angle the user shot with.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>Vector3 Ball velocity after collision.</returns>
        public Vector3 Shoot(GameTime gameTime)
        {
            user.UpdateMeasuringVelocityAndAngle(gameTime);
            Vector3 legVelocity = user.velocity;
            return legVelocity;


        }

        /// <remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Checks whether the ball entered the region user can shoot in.
        /// </summary>
        /// <returns>A boolean representing whether the ball entered the shooting region or not.</returns>
        public bool hasBallEnteredShootRegion()
        {
            float ballPosX = ball.Position.X;
            float ballPosZ = ball.Position.Z;
            return ballPosX <= Constants3.maxShootingX && ballPosX >= Constants3.minShootingX && ballPosZ <= Constants3.maxShootingZ && ballPosZ >= Constants3.minShootingZ;

        }

   

        /// <summary>
        /// Gets the height of the terrain at any point.
        /// </summar
        /// <remarks>
        /// <para>AUTHOR: Omar Abdulaal.</para>
        /// </remarks>
        /// <param name="Position">
        /// Specifies the point you want to get the height of the terrain at.</param>
        public float GetHeight(Vector3 Position)
        {
            try
            {
                int xComponent = (int)Position.X;
                int zComponent = (int)Position.Z;
                return heightData[xComponent + terrainWidth / 2, zComponent + terrainHeight / 2];
            }
            catch (ArgumentOutOfRangeException e)
            {
                return 0 + ball.Radius;
            }
        }
        #endregion
    }
}
