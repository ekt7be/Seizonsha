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
        private bool remove;

        public Animation(GameEntity target, int duration)
        {
            this.duration = duration;
            this.target = target;
            this.remove = false;
        }


        public void Update(GameTime gameTime)
        {
            if (duration <= 0)
            {
                remove = true;
            }

            UpdateAnimation(target, gameTime);
            duration--;

        }

        public bool shouldRemove()
        {
            return remove;
        }
        protected abstract void UpdateAnimation(GameEntity target, GameTime gameTime);

        public abstract void OnRemove(GameEntity target);

        virtual public void reset(GameEntity target, int duration)
        {
            this.remove = false;
            this.target = target;
            this.duration = duration;
        }

    }
}
