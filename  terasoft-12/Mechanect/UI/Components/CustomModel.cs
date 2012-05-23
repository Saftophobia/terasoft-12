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

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        protected Model model;
        private Matrix[] modelTransforms;

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
        public CustomModel(Model model, Vector3 position, Vector3 rotation, Vector3 scale){
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.model = model;
            modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            createBoundingSphere();
        }

        /// <summary>
        /// draws the 3d model
        /// </summary>
        /// <param name="camera">a camera instance</param>
        public virtual void Draw(Camera camera)
        {
            Matrix world = Matrix.CreateScale(Scale) 
                * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) 
                * Matrix.CreateTranslation(Position);

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

        /// <summary>
        /// creates the intial Bounding Sphere instance
        /// </summary>
        private void createBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            foreach (ModelMesh mesh in model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
            boundingSphere = sphere;
        }
    }
}
