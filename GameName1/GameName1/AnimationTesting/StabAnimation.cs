using GameName1.AnimationTesting;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.Skills;
using GameName1.NPCs;

namespace GameName1.AnimationTesting
{
    class StabAnimation : Animation
    {

        private Sword sword;

        // Animation
        private float elapsed;
        private float delay;
        private int currentFrame = 5;
        private static readonly int stabFrames = 3;
        private int recharge_time;

        public StabAnimation(Sword sword, GameEntity target, int duration) : base(target, duration)
        {
            this.sword = sword;
            this.delay = duration*4; //make durations based on time (floats)
            this.recharge_time = duration; 
        }


        protected override void UpdateAnimation(GameEntity target, GameTime gameTime)
        {
            if (target is Player || target is BasicEnemy || target is BossEnemy)
            {
                elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (elapsed > delay)
                {
                    if (currentFrame <= stabFrames - 1)
                    {
                        currentFrame = 5;
                    }

                    else
                    {
                        currentFrame--;
                    }

                    elapsed = 0;
                }


                    Rectangle? swordSource = sword.getSwordSource();
                    if (Math.Cos(target.direction) > .5)
                    {
                        //spriteSource = FramesToAnimation[RIGHT_ANIMATION];
                        swordSource = new Rectangle(64 * currentFrame, 3 * 64, 64, 64);
                        target.spriteSource = new Rectangle(64 * currentFrame, 15 * 64, 64, 64);

                    }
                    else if (Math.Sin(target.direction) > .5)
                    {
                        swordSource = new Rectangle(64 * currentFrame, 2 * 64, 64, 64);
                        target.spriteSource = new Rectangle(64 * currentFrame, 14 * 64, 64, 64);

                    }
                    else if (Math.Sin(target.direction) < -.5)
                    {
                        //spriteSource = FramesToAnimation[UP_ANIMATION];
                        swordSource = new Rectangle(64 * currentFrame, 0 * 64, 64, 64);
                        target.spriteSource = new Rectangle(64 * currentFrame, 12 * 64, 64, 64);

                    }
                    else if (Math.Cos(target.direction) < -.5)
                    {
                        //spriteSource = FramesToAnimation[LEFT_ANIMATION];
                        swordSource = new Rectangle(64 * currentFrame, 1 * 64, 64, 64);
                        target.spriteSource = new Rectangle(64 * currentFrame, 13 * 64, 64, 64);
                    }
                    sword.setSwordSource(swordSource);
                
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
