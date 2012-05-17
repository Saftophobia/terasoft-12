using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.Components;

namespace UI.Animation
{
    public abstract class Animation
    {
        protected TimeSpan elapsedTime;
        protected CustomModel model;

        protected Animation(CustomModel model)
        {
            this.model = model;
            elapsedTime = TimeSpan.FromSeconds(0);
        }

        public abstract void Update(TimeSpan elapsed);

        public abstract bool Finished();

    }
}
