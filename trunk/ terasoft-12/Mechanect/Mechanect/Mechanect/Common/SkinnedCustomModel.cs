using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkinnedModel;


namespace Mechanect.Common
{
    public class SkinnedCustomModel
    {
        # region Fields

        private Model model;
        private SkinningData skinningData;

        private Matrix[] originalBones;
        private Matrix[] boneTransforms;
        private Matrix[] worldTransforms;
        private Matrix[] skinTransforms;

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        #endregion

        #region Initialization

        /// <summary>
        /// used for a 3D model with skeleton and skin.
        /// </summary>
        /// <param name="model">the model</param>
        /// <param name="position">the position of the model</param>
        /// <param name="rotation">the rotation of the model</param>
        /// <param name="scale">scaling the model</param>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        public SkinnedCustomModel(Model model, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.model = model;
            this.skinningData = model.Tag as SkinningData;
            
            this.originalBones = new Matrix[skinningData.BindPose.Count];
            this.skinningData.CopyBindPose(originalBones);

            this.boneTransforms = new Matrix[skinningData.BindPose.Count];
            this.skinningData.CopyBindPose(boneTransforms);

            this.worldTransforms = new Matrix[skinningData.BindPose.Count];
            this.skinTransforms = new Matrix[skinningData.BindPose.Count];

            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// updating the model if any achanges occurred to its skin or bones
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        public void Update()
        {
            UpdateWorldTransforms();
            UpdateSkinTransforms();
        }

        /// <summary>
        /// updating the world view of every bone according to the changes to its parent bone
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        private void UpdateWorldTransforms()
        {
            Matrix rootTransform = Matrix.Identity;
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // Child bones.
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningData.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }

        /// <summary>
        /// updating the changes of the skin according to the changes in the bones.
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        private void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningData.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }

        /// <summary>
        /// drawing the model
        /// </summary>
        /// <param name="gameTimem">GameTime object</param>
        /// <param name="view">The Camera's View</param>
        /// <param name="projection">The Camera's Projection</param>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        public void Draw(GameTime gameTimem, Matrix view, Matrix projection)
        {
            Matrix[] bones = skinTransforms;

            // Render the skinned mesh.
            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix world = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = world;

                    effect.EnableDefaultLighting();
                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
            }
        }

        #endregion

        #region Animation

        /// <summary>
        /// moving a specific bone 
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="boneName">the name of the bone, same as in the model</param>
        /// <param name="offset">the number of extra bone information</param>
        /// <param name="bend">the binding of the bone in 3D</param>
        public void MoveBone(string boneName, int offset, Vector3 bend)
        {
            int boneId = model.Bones[boneName].Index + offset;
            boneTransforms[boneId] = Matrix.CreateFromYawPitchRoll(bend.X, bend.Y, bend.Z)
                * originalBones[boneId];
        }

        #endregion
    }
}
