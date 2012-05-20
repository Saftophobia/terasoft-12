using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Cameras;

namespace UI.Components
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

        private Matrix[] modelTransforms;

        private GraphicsDevice graphicsDevice;

        protected Model Model { get; private set; }

        private BoundingSphere boundingSphere;
        public BoundingSphere BoundingSphere
        {
            get
            {
                Matrix worldTransform = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
                BoundingSphere transformed = boundingSphere;
                return transformed.Transform(worldTransform);
            }
        }
        
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
            Model = model;
            modelTransforms = new Matrix[model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            createBoundingSphere();
        }

        /// <summary>
        /// draws the 3d model
        /// </summary>
        /// <param name="camera">a camera instance</param>
        public void Draw(Camera camera)
        {
            Matrix world = Matrix.CreateScale(Scale) 
                * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) 
                * Matrix.CreateTranslation(Position);

            foreach (ModelMesh mesh in Model.Meshes)
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

        private void createBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
            boundingSphere = sphere;
        }
    }
}
