using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public void Draw(Matrix view, Matrix projection)
        {
            Matrix world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(position);
            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * world;
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    BasicEffect e = (BasicEffect)part.Effect;
                    e.World = localWorld;
                    e.View = view;
                    e.Projection = projection;
                    e.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }
    }
}
