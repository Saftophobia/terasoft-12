using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Cameras;

namespace UI.Components
{
    /// <summary>
    /// Represents a 3d model with its  position, orientation and scale.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem.
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
        /// Creates a new CustomModel instance.
        /// </summary>
        /// <param name="model">3D model.</param>
        /// <param name="position">Model's position.</param>
        /// <param name="rotation">Model's orientation.</param>
        /// <param name="scale">Model's scale.</param>
        /// <param name="graphicsDevice">Displays graphics on the screen.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public CustomModel(Model model, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.model = model;
            modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            createBoundingSphere();
        }

        /// <summary>
        /// Draws the 3D model.
        /// </summary>
        /// <param name="camera">Camera instance.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
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
        /// Creates the intial BoundingSphere.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
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
