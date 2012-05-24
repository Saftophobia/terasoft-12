using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Cameras;
using UI.Components;
using Mechanect.Common;
using Physics;
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
        private static float friction;
        public static float Friction
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
        private VertexPositionNormalTexture[] vertices;
        private int[] indices;

        private int terrainWidth;
        private int terrainHeight;

        public int TerrainWidth
        {
            get
            {
                return terrainWidth;
            }
        }

        public int TerrainHeight
        {
            get
            {
                return terrainHeight;
            }
        }

        private float[,] heightData; //2D array
        private VertexBuffer myVertexBuffer;
        private IndexBuffer myIndexBuffer;

        private ContentManager Content;
        private SpriteBatch sprite;

        public SkinnedCustomModel PlayerModel { get; private set; }
        public KineckAnimation PlayerAnimation { get; private set; }

        private Vector3 ballInitialPosition, ballInitialVelocity;
        public float arriveVelocity { get; set; }
        
        private Texture2D grassTexture;
        private Texture2D cloudMap;
        private Model skyDome;
        private RenderTarget2D cloudsRenderTarget;
        private Texture2D cloudStaticMap;
        private VertexPositionTexture[] fullScreenVertices;

        public Environment3(User3 user)
        {
            this.user = user;
            friction = -2f;
            int holeRadius = GenerateRadius(angleTolerance);
            Vector3 holePosition = Functions.GeneratePosition(holeRadius,(int) Constants3.maxHolePosX,(int) Constants3.maxHolePosZ);
            hole = new Hole(holeRadius, holePosition, (int) Constants3.maxHolePosX,(int) Constants3.maxHolePosZ);

            PlayerModel = new SkinnedCustomModel(new Vector3(0, 0, 45),
                new Vector3(0, 9.3f, 0), new Vector3(0.3f, 0.3f, 0.3f));
            PlayerAnimation = new KineckAnimation(PlayerModel, user);
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
            if (user.assumedLegMass <= 0)
                return Constants3.negativeLMass;
            if (hole.Position.Z - user.shootingPosition.Z > 0)
                return Constants3.negativeHPosZ;
            if (friction <= 0)
                return Constants3.negativeFriction;
            if (ball.Radius > hole.Radius)
                return Constants3.negativeRDifference;

            Vector3 finalPos = BallFinalPosition(GetVelocityAfterCollision(new Vector3(0, 0, Constants3.maxVelocityZ)));
            if (Vector3.DistanceSquared(finalPos, user.shootingPosition) < Vector3.DistanceSquared(hole.Position, user.shootingPosition))
                return Constants3.holeOutOfFarRange;

            finalPos = BallFinalPosition(GetVelocityAfterCollision(new Vector3(0, 0, Constants3.minVelocityZ)));
            if (Vector3.DistanceSquared(finalPos, user.shootingPosition) > Vector3.DistanceSquared(hole.Position, user.shootingPosition)) //length squared used for better performance than length
                return Constants3.holeOutOfNearRange;
            
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
            if (hole.Position.Z >= Constants3.maxHolePosZ - hole.Radius)
                hole.Position = new Vector3(hole.Position.X, hole.Position.Y, Constants3.maxHolePosZ - hole.Radius);
            if (Math.Abs(hole.Position.X) > Constants3.maxHolePosX - hole.Radius)
                hole.Position = new Vector3(Constants3.maxHolePosX - hole.Radius, hole.Position.Y, hole.Position.Z);
            if (hole.Position.Z <= user.shootingPosition.Z)
                hole.Position = new Vector3(hole.Position.X, hole.Position.Y, Constants3.maxHolePosZ - hole.Radius);
       
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
                        else hole.Position = new Vector3(hole.Position.X/2, hole.Position.Y, hole.Position.Z+1); break; 
                    case Constants3.negativeRDifference: int tmp = (int)ball.Radius; ball.Radius = (hole.Radius); hole.Radius = (tmp); break;
                    case Constants3.negativeLMass: user.assumedLegMass *= -1; break;
                    case Constants3.negativeBMass: ball.Mass *= -1; break;
                    case Constants3.negativeBRradius: ball.Radius *= -1; break;
                    case Constants3.negativeHRadius: hole.Radius *= -1; break;
                    case Constants3.negativeFriction: friction *= -1; break;
                    case Constants3.negativeHPosZ: hole.Position = Vector3.Add(hole.Position, new Vector3(0, 0, 1)); break;
                }
            } while (x != Constants3.solvableExperiment);    
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
        /// <para>AUTHOR: Ahmad Sanad </para>
        /// </remarks>
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
               
                Tools3.DisplayIsWin(sprite,Content,Vector2.Zero, true);
            }

            else
            {
                Tools3.DisplayIsWin(sprite, Content, Vector2.Zero, false);
            }
        }

        public void InitializeUI(ContentManager content, GraphicsDevice graphics)
        {
            this.Content = content;
            device = graphics;
        }

        /// <summary>
        /// Loads the content of the environment.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        public void LoadContent()
        {
            
            //loads the height data from the height map
            Texture2D heightMap = Content.Load<Texture2D>("Textures/heightmaplargeflat");
            LoadHeightData(heightMap);
            SetUpVertices();
            LoadEnvironmentContent();

            
            PlayerModel.LoadContent(Content.Load<Model>("dude"));

            CreateCircularHole(hole.Position, hole.Radius);
           // CreateAlmostCircularHole(hole.Position, hole.Radius);
            //CreateSquareHole(hole.Position, hole.Radius);



            //ball.LoadContent();

        }

        /// <summary>
        /// Draws the environment.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <param name="c">The camera the environment is viewed from.</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(Camera c, GameTime gameTime)
        {
            DrawEnvironment(c, gameTime);
            hole.InitialzeUI(Content, device);
            hole.Draw(c);
            PlayerModel.Draw(c);
        }

        #region Environment Generation Code
            
        /// <summary>
        /// Loads the content of the environment.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        public void LoadEnvironmentContent()
        {

            //loads the the fx file to use the effects defined in it
            effect = Content.Load<Effect>("Textures/MYHLSL");
            skyDome = Content.Load<Model>("Models/dome");
            cloudMap = Content.Load<Texture2D>("Textures/cloudMap");
            skyDome.Meshes[0].MeshParts[0].Effect = effect;

            PresentationParameters presentationParameters = device.PresentationParameters;
            cloudsRenderTarget = new RenderTarget2D(device, presentationParameters.BackBufferWidth, presentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            cloudStaticMap = CreateStaticMap(32);

            grassTexture = Content.Load<Texture2D>("Textures/grass2");

            SetUpIndices();
            CalculateNormals();
            CopyToBuffers();

        }




        /// <summary>
        /// Draws the environment. Similar to the Draw() method of XNA and should be called in it.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void DrawEnvironment(Camera camera, GameTime gameTime)
        {
            var time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;
            GeneratePerlinNoise(time);



            //Clears the Z buffer
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            DrawSkyDome(camera);

            //Creates a rasterizer state removes culling, and makes the fill mode solid, for the triangles to be filled
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;
            device.RasterizerState = rasterizerState;

            var worldMatrix = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f);
            //Matrix worldMatrix = Matrix.Identity;
            //Sets the effects to be used from the fx file such as coloring the terrain and adding lighting.
            effect.CurrentTechnique = effect.Techniques["Textured"];
            var lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();
            effect.Parameters["xLightDirection"].SetValue(lightDirection);
            effect.Parameters["xAmbient"].SetValue(0.1f);
            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xView"].SetValue(camera.View);
            effect.Parameters["xProjection"].SetValue(camera.Projection);
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

        /// <summary>
        /// Creates the vertices for the triangles used to generate the terrain, and sets their color and height according to the height map.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
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

        }
          
       



        /// <summary>
        /// Creates the indices of the triangles used to generate the terrain. 
        /// This data is used to connect the vertices previously created to make them into triangles.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        private void SetUpIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            var counter = 0;
            for (var y = 0; y < terrainHeight - 1; y++)
            {
                for (var x = 0; x < terrainWidth - 1; x++)
                {
                    var lowerLeft = x + (y * terrainWidth);
                    var lowerRight = (x + 1) + (y * terrainWidth);
                    var topLeft = x + ((y + 1) * terrainWidth);
                    var topRight = (x + 1) + ((y + 1) * terrainWidth);

                    indices[counter++] = (int)topLeft;
                    indices[counter++] = (int)lowerRight;
                    indices[counter++] = (int)lowerLeft;

                    indices[counter++] = (int)topLeft;
                    indices[counter++] = (int)topRight;
                    indices[counter++] = (int)lowerRight;
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
            terrainWidth = (int)heightMap.Width;
            terrainHeight = (int)heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainHeight];
            for (var x = 0; x < terrainWidth; x++)
                for (var y = 0; y < terrainHeight; y++)
                    heightData[x, y] = (heightMapColors[x + (y * terrainWidth)].R / 5.0f) - 20;

        }

        
        /// <summary>
        /// Copies the vertices and indices to GPU buffers. 
        /// This allows the data to be called from the GPU's memory directly without having to send it to the GPU everytime the Draw() method is called.
        /// This should increase performance as the GPU's memory is generally faster.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        private void CopyToBuffers()
        {
            myVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            myVertexBuffer.SetData(vertices);
            myIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            myIndexBuffer.SetData(indices);
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


        /// <summary>
        /// Draws the skydome to display the sky, and fills it with moving clouds.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <param name="c">The camera used in the environment.</param>
        private void DrawSkyDome(Camera camera)
        {
            var depthStencilState = new DepthStencilState();
            depthStencilState.DepthBufferWriteEnable = false;
            device.DepthStencilState = depthStencilState;

            Matrix[] modelTransforms = new Matrix[skyDome.Bones.Count];
            skyDome.CopyAbsoluteBoneTransformsTo(modelTransforms);

            var wMatrix = Matrix.CreateTranslation(0, -0.3f, 0) * Matrix.CreateScale(100) * Matrix.CreateTranslation(camera.Position);
            foreach (ModelMesh mesh in skyDome.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    var worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
                    currentEffect.CurrentTechnique = currentEffect.Techniques["SkyDome"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(camera.View);
                    currentEffect.Parameters["xProjection"].SetValue(camera.Projection);
                    currentEffect.Parameters["xTexture"].SetValue(cloudMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);
                }
                mesh.Draw();
            }
            depthStencilState = new DepthStencilState();
            depthStencilState.DepthBufferWriteEnable = true;
            device.DepthStencilState = depthStencilState;
        }


        /// <summary>
        /// Creates a noise map, based on Perlin's noise.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <param name="resolution">Desired resolution of the map that will be created.</param>
        /// <returns>Returns the generated noise map.</returns>
        private Texture2D CreateStaticMap(int resolution)
        {
            var rand = new Random();
            Color[] noisyColors = new Color[resolution * resolution];

            for (var x = 0; x < resolution; x++)
            {
                for (var y = 0; y < resolution; y++)
                {
                    noisyColors[x + y * resolution] = new Color(new Vector3((float)rand.Next(1000) / 1000.0f, 0, 0));
                }
            }
            var noiseImage = new Texture2D(device, 32, 32, false, SurfaceFormat.Color);
            noiseImage.SetData(noisyColors);
            return noiseImage;
        }

        /// <summary>
        /// Sets vertices for the triangles, which allow rendering the noise maps using HLSL effects.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <returns>Array of the vertices.</returns>
        private VertexPositionTexture[] SetUpFullscreenVertices()
        {
            VertexPositionTexture[] vertices = new VertexPositionTexture[4];
            vertices[0] = new VertexPositionTexture(new Vector3(-1, 1, 0f), new Vector2(0, 1));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 1, 0f), new Vector2(1, 1));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, -1, 0f), new Vector2(0, 0));
            vertices[3] = new VertexPositionTexture(new Vector3(1, -1, 0f), new Vector2(1, 0));

            return vertices;
        }


        /// <summary>
        /// Generates the Perlin noise maps, renders them, and does all the needed effects on them, using HLSL, 
        /// then sets the cloudmap to the generated noise map.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        /// <param name="time">The time used to control how fast the clouds move.</param>
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

        #endregion

        #region Hole methods

        /// <summary>
        /// Generates a value for the radius given angle tolerance
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="angletolerance">The tolerance value set by the user.</param>
        /// <returns>integer value which is the radius of the hole </returns>
        public static int GenerateRadius(int angletolerance)
        {
            switch (angletolerance)
            {
                case 1:  return 15; 
                case 2:  return 18; 
                case 3:  return 21; 
                case 4:  return 24; 
                default: return 15;
            }
        }

        /// <summary>
        /// Makes a square hole with circular surroundings in the environment given a certain position by looping through the terrain vertices in the hole position and decrease their heights.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="position">The central position where the hole should be made around.</param>
        /// <param name="radius">The radius of the hole.</param>
        protected void CreateAlmostCircularHole(Vector3 position, int radius)
        {
            radius = radius / (int)2.5f;
            double angleStep = 1f / radius;
            int a;
            int b;
            try
            {
                for (float x = hole.Position.X - radius; x <= hole.Position.X + radius; x++)
                {
                    for (float z = hole.Position.Z - radius; z <= hole.Position.Z + radius; z++)
                    {
                        for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
                        {
                            a = (int)Math.Round(radius * Math.Cos(angle));
                            b = (int)Math.Round(radius * Math.Sin(angle));
                            heightData[(int)(x + a + (terrainWidth / 2)), (int)(-z - b + (terrainHeight / 2))] -= -20;
                        }
                    }
                }
                hole.Position = new Vector3(position.X, GetHeight(position) + 21, position.Z);
            }

            catch (IndexOutOfRangeException)
            {
                CreateAlmostCircularHole(position, radius);
            }
        }

        /// <summary>
        /// Makes a real circular hole in the environment given a certain position by looping through the terrain vertices in the hole position and decrease their heights.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="position">The central position where the hole should be made around.</param>
        /// <param name="radius">The radius of the hole.</param>
        protected void CreateCircularHole(Vector3 position, int radius)
        {
            float posX = hole.Position.X;
            float posZ = hole.Position.Z;
            try
            {
                for (float x = posX - radius; x <= posX + radius; x++)
                {
                    for (float z = posZ - (float)Math.Sqrt((radius + x - posX) * (radius - x + posX));
                        z <= posZ + (float)Math.Sqrt((radius + x - posX) * (radius - x + posX)); z++)
                    {
                        heightData[(int)(x + (terrainWidth / 2)), (int)(-z + (terrainHeight / 2))] -= 20;
                    }
                }

                hole.Position = new Vector3(posX, GetHeight(position) + 21, posZ);
            }
            catch (IndexOutOfRangeException)
            {
                CreateAlmostCircularHole(position, radius);
            }
        }

        /// <summary>
        /// Makes a real square hole in the environment given a certain position by looping through the terrain vertices in the hole position and decrease their heights.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="position">The central position where the hole should be made around.</param>
        /// <param name="radius">The radius of the hole.</param>
        protected void CreateSquareHole(Vector3 position, int radius)
        {
            try
            {
                for (float x = hole.Position.X - radius; x <= hole.Position.X + radius; x++)
                {
                    for (float z = hole.Position.Z - radius; z <= hole.Position.Z + radius; z++)
                    {
                        heightData[(int)(x + (terrainWidth / 2)), (int)(-z + (terrainHeight / 2))] -= 20;
                    }
                }
                hole.Position = new Vector3(hole.Position.X, GetHeight(position) + 21, hole.Position.Z);
            }
            catch (IndexOutOfRangeException)
            {
                CreateSquareHole(position, radius);
            }
        }
        #endregion
        #region Ball Control Methods


        /// <summary>
        /// Gets the height of the terrain at any point.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Omar Abdulaal.</para>
        /// </remarks>
        /// <param name="Position">
        /// Specifies the point you want to get the height of the terrain at.</param>
        public float GetHeight(Vector3 Position)
        {
            int xComponent = (int)Position.X + terrainWidth / 2;
            int zComponent = -(int)Position.Z + terrainHeight / 2;
            if ((xComponent > 0) && (xComponent < heightData.GetLength(0))
                && (zComponent > 0) && (zComponent < heightData.GetLength(1)))
            {
                return heightData[xComponent, zComponent];
            }
            else
                return 0;
        }

        ///<remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Calculates velocity after collision using conservation of momentum laws.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity before collision.</param>
        /// <returns>Vector3 velocity after collision.</returns>
        [Obsolete("Method will be moved to Physics Library")]
        public Vector3 GetVelocityAfterCollision(Vector3 initialVelocity)
        {
            double ballMass, legMass, initialLegVelocity;

            initialLegVelocity = initialVelocity.Length() * Constants3.velocityScale;
            ballMass = ball.Mass;
            legMass = user.assumedLegMass;

            float finalVelocity = (float)(((legMass * initialLegVelocity) + (ballMass * arriveVelocity) - (0)) / ballMass);
            Vector3 normalizedVector = Vector3.Normalize(initialVelocity);
            return normalizedVector * finalVelocity;
        }

        #endregion
    }
}
