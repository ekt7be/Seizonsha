using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.AnimationTesting
{
    class PulseAnimation : Animation
    {

        private int maxDuration;

        public PulseAnimation(GameEntity target, int duration)
            : base(target, duration)
        {
            this.maxDuration = duration;
        }
        protected override void UpdateAnimation(GameEntity target, GameTime gameTime)
        {
            target.scale = (float)Math.Abs(Math.Cos((double)duration/20)/4.0) + .9f;
        }

        public override void OnRemove(GameEntity target)
        {
            target.scale = target.defaultScale;
        }

        public void reset(GameEntity target)
        {
            base.reset(target, maxDuration);
        }
    }
}
