using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Cameras;

namespace Mechanect.Common
{
    public class CustomModel
    {

        public Vector3 position { get; set; }
        public Vector3 rotation { get; set; }
        public Vector3 scale { get; set; }

        private Model model;
        private Matrix[] modelTransforms;

        private GraphicsDevice graphicsDevice;

        public void setRotation(float x, float y, float z)
        {
            rotation = new Vector3(x, y, z);

        }

        public CustomModel(Model model, Vector3 Position, Vector3 Rotation, Vector3 Scale, GraphicsDevice graphicsDevice)
        {
            this.position = Position;
            this.rotation = Rotation;
            this.scale = Scale;
            this.graphicsDevice = graphicsDevice;
            this.model = model;
            modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
        }

        public void Draw(Camera camera)
        {
            Matrix world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(position);
            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * world;
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    BasicEffect e = (BasicEffect)part.Effect;
                    e.World = localWorld;
                    e.View = camera.view;
                    e.Projection = camera.projection;
                    e.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }
    }
}
