using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Components;

namespace Mechanect.Exp3
{
    /// <summary>
    /// This class represents the 3D model of the hole.
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Khaled Salah </para>
    /// </remarks>
   public class Hole :CustomModel
    {
        private int radius;
        public int Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        /// <summary>
        /// Creates a hole and sets values for the hole radius, hole position and sets the model scale according to radius.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="radius">The value of the radius of the hole.</param>
        /// <param name="position">The 3D position of the hole.</param>
        public Hole(int radius, Vector3 position):
            base(position, Vector3.Zero, new Vector3((float)Constants3.scaleRatio * radius))
        {
            this.radius = radius;
            Position = position;
        }

        /// <summary>
        /// Loads the UI components needed for the hole. The method takes as parameter the model of the hole and loads it.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="model">The model of the hole which should be drawn.</param>
        public void LoadContent(Model model)
        {
            base.LoadContent(model);
          //  Scale = (radius / intialBoundingSphere.Radius) * Vector3.One;
        }
    }
}
