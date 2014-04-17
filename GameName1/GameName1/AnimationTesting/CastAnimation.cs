using GameName1.NPCs;
using GameName1.Skills;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.AnimationTesting
{
    class CastAnimation : Animation
    {

        // Animation
        private float elapsed;
        private float delay;
        private int currentFrame = 0;
        private static readonly int castFrames = 7;
        private int recharge_time;

        public CastAnimation(GameEntity target, int duration) : base(target, duration)
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
                    if (currentFrame >= castFrames - 1)
                    {
                        currentFrame = 0;
                    }

                    else
                    {
                        currentFrame++;
                    }

                    elapsed = 0;
                }


                if (Math.Cos(target.direction) > .5)
                {
                    //spriteSource = FramesToAnimation[RIGHT_ANIMATION];
                    target.spriteSource = new Rectangle(64 * currentFrame, 3 * 64, 64, 64);

                }
                else if (Math.Sin(target.direction) > .5)
                {
                    target.spriteSource = new Rectangle(64 * currentFrame, 2 * 64, 64, 64);

                }
                else if (Math.Sin(target.direction) < -.5)
                {
                    //spriteSource = FramesToAnimation[UP_ANIMATION];
                    target.spriteSource = new Rectangle(64 * currentFrame, 0 * 64, 64, 64);

                }
                else if (Math.Cos(target.direction) < -.5)
                {
                    //spriteSource = FramesToAnimation[LEFT_ANIMATION];
                    target.spriteSource = new Rectangle(64 * currentFrame, 1 * 64, 64, 64);
                }

            }
        }

        public void reset(GameEntity target)
        {
            if (target is Player)
                ((Player)target).drawWeapon = false;
            base.reset(target, recharge_time);
            currentFrame = 0;
            this.elapsed = 0;
            //reset other stuff
        }

        public override void OnRemove(GameEntity target)
        {
            if (target is Player)
                ((Player)target).drawWeapon = true;
        }

    }
}
