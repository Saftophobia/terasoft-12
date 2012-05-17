using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class Environment1
    {
        float Distance = 1000.0f;
        bool chase = true;
        drawstring drawstring;
        SpriteFont font1;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Model xwingModel;
        Model xwingModel2;
        double avatarprogUI = 10;
        double avatarprogUI2 = 10;
        Effect effect;
        VertexPositionColorNormal[] vertices;
        int[] indices;
        private float angle = 0f;
        private int terrainWidth = 4;
        private int terrainHeight = 3;
        private float[,] heightData;
        VertexBuffer myVertexBuffer;
        IndexBuffer myIndexBuffer;
        Texture2D[] skyboxTextures;
        ChaseCamera c, c1, c2;
        Model skyboxModel;
        ContentManager content;
        int translation1 = 0;
        int translation2 = 0;

        /// <summary>
        /// Constructor for Environment1
        /// </summary>
        /// <param name="content">setting content for later use in ScreenManager</param>
        /// <param name="device">setting device for later use in ScreenManager</param>
        /// <remarks>
        /// <para>Author: Safty</para>
        /// <para>Date Written 15/5/2012</para>
        /// <para>Date Modified 15/5/2012</para>
        /// </remarks>
        public Environment1(ContentManager content, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            this.content = content;
            this.device = device;
            this.spriteBatch = spriteBatch;
        }


        /// <summary>
        /// Loads Effect/heightmap/skybox from contentmanager and set up vertices/indices/normals/buffers from exp3 library
        /// </summary>
        /// <remarks>
        /// <para>Author: Safty</para>
        /// <para>Date Written 15/5/2012</para>
        /// <para>Date Modified 15/5/2012</para>
        /// </remarks>
        public void LoadContent()
        {
            effect = content.Load<Effect>("Exp1/effects");
            font1 = content.Load<SpriteFont>("SpriteFont1");
            drawstring = new drawstring(new Vector2(400, 200));
            drawstring.Font1 = font1;
            xwingModel = LoadModel("Exp1/xwing");
            xwingModel2 = LoadModel("Exp1/xwing");
            Texture2D heightMap = content.Load<Texture2D>("Exp1/heightmap6");
            skyboxModel = LoadModel("Exp1/skybox2", out skyboxTextures);
            c1 = new ChaseCamera(new Vector3(0, 150, -350), Vector3.Zero, Vector3.Zero, device);
            c2 = new ChaseCamera(new Vector3(0, 70, 150), new Vector3(0, 35, 0), new Vector3(0, 0, 0), device);
            LoadHeightData(heightMap);
            SetUpVertices();
            SetUpIndices();
            CalculateNormals();
            CopyToBuffers();

        }
        /// <summary>
        /// updates Environment Logic ( Camera, avatar position, etc)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <remarks>
        /// <para>Author: Safty</para>
        /// <para>Date Written 15/5/2012</para>
        /// <para>Date Modified 15/5/2012</para>
        /// </remarks>
        public void update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.K))
            {
                TargetCam();
            }
            if (state.IsKeyDown(Keys.L))
            {
                ChaseCam();
            }

            if (chase)
            {
                c2.Rotate(new Vector3(0, 0.003f, 0));
                c = c2;
                drawstring.Update("KNEES UP! and get in the !@#$ing Range");
            }
            else
            {
                c = c1;
                //drawstring.Update("");
            }
            c.Update();
        }
        /// <summary>
        /// Draws Environment by calling Exp3 library
        /// </summary>
        /// <param name="gameTime"></param>
        /// <remarks>
        /// <para>Author: Safty</para>
        /// <para>Date Written 15/5/2012</para>
        /// <para>Date Modified 15/5/2012</para>
        /// </remarks>
        public void Draw(GameTime gameTime)
        {
            DrawEnvironment(gameTime);
            DrawModel(xwingModel, -20, 60, translation1);
            DrawModel(xwingModel2, 20, 60, translation2);
            //xwingModel2

            if (chase)
            {
                spriteBatch.Begin();
                drawstring.Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 16/5/2012</para>
        /// <para>Date Modified 16/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function MoveAvatar is used to increase the Z-Position of an avatar by a given value.
        /// </summary>
        /// <param name="player">The required player.</param>
        /// <param name="value">The value to increase.</param>
        /// <returns>void.</returns>
        public void MoveAvatar(int player, int value)
        {
            switch (player)
            {
                case 1: translation1 += value; break;
                case 2: translation2 += value; break;
            }
        }

        public void TargetCam()
        {
            if (chase)
            {
                chase = false;
            }
        }
        public void ChaseCam()
        {
            if (!chase)
            {
                chase = true;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 16/5/2012</para>
        /// <para>Date Modified 16/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawModel is used to Draw the model and set the rotation,translation and scaling factors.
        /// </summary>
        /// <param name="xwingModels"> The model to be drawn.</param>
        /// <param name="x">X-Coordinate of the Model.</param>
        /// <param name="y">Y-Coordinate of the Model.</param>
        /// <param name="z">Z-Coordinate of the Model.</param>  
        /// <returns>void.</returns>
        private void DrawModel(Model xwingModels, int x, int y, int z)
        {
            Matrix worldMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f) * Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(new Vector3(x, y, z));
            Matrix[] xwingTransforms = new Matrix[xwingModels.Bones.Count];
            xwingModels.CopyAbsoluteBoneTransformsTo(xwingTransforms);
            foreach (ModelMesh mesh in xwingModels.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Colored"];
                    currentEffect.Parameters["xWorld"].SetValue(xwingTransforms[mesh.ParentBone.Index] * worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(c.View);
                    currentEffect.Parameters["xProjection"].SetValue(c.Projection);
                }
                mesh.Draw();
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 16/5/2012</para>
        /// <para>Date Modified 16/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function LoadModel is used to Load the Model from the content Manager.
        /// </summary>
        /// <param name="assetName">The Model file's name.</param>
        /// <returns>void.</returns>
        private Model LoadModel(string assetName)
        {
            Model newModel = content.Load<Model>(assetName); foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();
            return newModel;
        }



        #region Waiting for Sanad's library its documented

        /// <summary>
        /// Draws the environment. Similar to the Draw() method of XNA and should be called in it.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        ///<param name="gameTime">Provides a snapshot of timing values.</param
        protected void DrawEnvironment(GameTime gameTime)
        {

            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawSkybox();
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;
            device.RasterizerState = rs;
            Matrix worldMatrix = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f);// *Matrix.CreateRotationY(angle);

            //Sets the effects to be used from the fx file such as coloring the terrain and adding lighting.
            effect.CurrentTechnique = effect.Techniques["Colored"];
            Vector3 lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
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
        }

        /// <summary>
        /// Creates, draws and adds effects to the skybox to display the sky all in all directions with a constant distance from the camera.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        private void DrawSkybox()
        {
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            device.SamplerStates[0] = ss;
            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            device.DepthStencilState = dss;
            Matrix[] skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
            skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
            int i = 0;
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(c.Position);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(c.View);
                    currentEffect.Parameters["xProjection"].SetValue(c.Projection);
                    currentEffect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
                }
                mesh.Draw();
            }
            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            device.DepthStencilState = dss;
        }


        /// <summary>
        /// Creates the vertices for the triangles used to generate the terrain, and sets their color and height according to the height map.
        /// </summary>
        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        private void SetUpVertices()
        {
            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    if (heightData[x, y] < minHeight)
                        minHeight = heightData[x, y];
                    if (heightData[x, y] > maxHeight)
                        maxHeight = heightData[x, y];
                }
            }

            vertices = new VertexPositionColorNormal[terrainWidth * terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y], -y);

                    if (heightData[x, y] < minHeight + (maxHeight - minHeight) / 4)
                        vertices[x + y * terrainWidth].Color = new Color(84, 74, 70);

                    else if (heightData[x, y] < minHeight + (maxHeight - minHeight) * 2 / 4)
                        vertices[x + y * terrainWidth].Color = new Color(84, 74, 70);
                    //                vertices[x + y * terrainWidth].Color = Color.DimGray;
                    else if (heightData[x, y] < minHeight + (maxHeight - minHeight) * 3 / 4)
                        vertices[x + y * terrainWidth].Color = new Color(84, 74, 70);
                    else
                        vertices[x + y * terrainWidth].Color = Color.White;
                }
            }

            for (int x = 50; x <= 61; x++)
            {
                for (int y = 50; y <= 61; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y] - 20, -y);
                }
            }
        }

        /// <summary>
        /// Creates the indices of the triangles used to generate the terrain. 
        /// This data is used to connect the vertices previously created to make them into triangles.
        /// </summary>
        ///<remarks><para>AUTHOR: Ahmad Sanad</para></remarks>
        private void SetUpIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainHeight - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = (int)(x + y * terrainWidth);
                    int lowerRight = (int)((x + 1) + y * terrainWidth);
                    int topLeft = (int)(x + (y + 1) * terrainWidth);
                    int topRight = (int)((x + 1) + (y + 1) * terrainWidth);

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
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
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R / 5.0f;
        }



        private void CopyToBuffers()
        {
            myVertexBuffer = new VertexBuffer(device, VertexPositionColorNormal.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            myVertexBuffer.SetData(vertices);
            myIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            myIndexBuffer.SetData(indices);
        }


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
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);
            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;

            }

            for (int i = 0; i < vertices.Length; i++)
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

            Model newModel = content.Load<Model>(assetName);
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


        #endregion
    }
}
