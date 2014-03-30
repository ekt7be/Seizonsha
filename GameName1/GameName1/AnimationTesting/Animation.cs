using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.AnimationTesting
{
    public abstract class Animation
    {

        private GameEntity target;
        private int duration;
        private float lifetime;
        private bool remove;

        public Animation(GameEntity target, int duration)
        {
            this.duration = duration;
            this.target = target;
            this.remove = false;
            this.lifetime = 0;
        }


        public void Update()
        {
            if (lifetime >= duration)
            {
                remove = true;
            }

            UpdateAnimation(target);
            lifetime++;

        }

        public bool shouldRemove()
        {
            return remove;
        }
        protected abstract void UpdateAnimation(GameEntity target);

        public abstract void OnRemove(GameEntity target);

    }
}
