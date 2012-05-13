using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Cameras;

namespace UI
{
    /// <summary>
    /// represents a 3d model with its  position, orientation and scale
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public class CustomModel
    {

        /// <summary>
        /// the model's position
        /// </summary>
        public Vector3 Position { get; set; }
        
        /// <summary>
        /// the model's orientation
        /// </summary>
        public Vector3 Rotation { get; set; }
        
        /// <summary>
        /// the model's scale
        /// </summary>
        public Vector3 Scale { get; set; }

        private Model model;
        private Matrix[] modelTransforms;

        private GraphicsDevice graphicsDevice;
        
        /// <summary>
        /// constructs a CustomModel instance 
        /// </summary>
        /// <param name="model">the 3d model</param>
        /// <param name="position">the model's position</param>
        /// <param name="rotation">the model's orientation</param>
        /// <param name="scale">the model's scale</param>
        /// <param name="graphicsDevice"></param>
        public CustomModel(Model model, Vector3 position, Vector3 rotation, Vector3 scale, GraphicsDevice graphicsDevice){
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.graphicsDevice = graphicsDevice;
            this.model = model;
            modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
        }

        /// <summary>
        /// draws the 3d model
        /// </summary>
        /// <param name="camera">a camera instance</param>
        public void Draw(Camera camera)
        {
            Matrix world = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * world;
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    BasicEffect e = (BasicEffect)part.Effect;
                    e.World = localWorld;
                    e.View = camera.View;
                    e.Projection = camera.Projection;
                    e.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }
    }
}
