using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.Effects;
using Microsoft.Xna.Framework;
using GameName1.NPCs;

namespace GameName1.AnimationTesting
{
    class ShockAnimation : Animation
    {

        private LightningEffect lightningEffect;

        // Animation
        private float elapsed;
        private float delay;
        private int currentFrame = 0;
        private static readonly int slashFrames = 4;
        private int recharge_time;

        public ShockAnimation(GameEntity target, int duration) : base(target, duration)
        {
            this.delay = duration*2; //make durations based on time (floats)
            this.recharge_time = duration;
        }


        protected override void UpdateAnimation(GameEntity target, GameTime gameTime)
        {
            if (target is Player || target is BasicEnemy || target is BossEnemy)
            {
                elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (elapsed > delay)
                {
                    if (currentFrame >= slashFrames - 1)
                    {
                        currentFrame = 0;
                    }

                    else
                    {
                        currentFrame++;
                    }

                    elapsed = 0;
                }

                Rectangle? lightningSource;
                try
                {
                    lightningSource = lightningEffect.getLightningSource();
                }
                catch
                {
                    lightningSource = new Rectangle(0, 8*64, 64, 64);
                }
                lightningSource = new Rectangle(64 * currentFrame, 5 * 64, 64, 64);
                
            }
        }

        public void reset(GameEntity target){
            base.reset(target, recharge_time);
            currentFrame = 0;
            this.elapsed = 0;
            //reset other stuff
        }

        public override void OnRemove(GameEntity target)
        {
           
        }
    }
}
