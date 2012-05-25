using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Components;

namespace Mechanect.Exp3
{
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
        /// The constructor for class hole sets values for the hole radius, hole position and sets the model scale according to radius.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="radius">The value of the radius of the hole.</param>
        /// <param name="position">The 3d position of the hole.</param>
        public Hole(int radius, Vector3 position):
            base(position, Vector3.Zero, new Vector3((float)Constants3.scaleRatio * 1f * radius))
        {
            this.radius = radius;
            Position = position;
        }

        /// <summary>
        /// Loads the UI components needed for the hole... the method takes as parameter the model of the hole and loads it so that it becomes ready to be drawn.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="model">The model of the hole which should be drawn.</param>
        public override void LoadContent(Model model)
        {
            base.LoadContent(model);
          //  Scale = (radius / intialBoundingSphere.Radius) * Vector3.One;
        }
    }
}
