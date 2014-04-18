using GameName1.NPCs;
using GameName1.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace GameName1.AnimationTesting
{
    class HealAnimation : AOECone
    {
        private HealingTouch heal;

        // Animation
        private float elapsed;
        private float delay;
        private int currentFrame = 0;
        private static readonly int healFrames = 10;
        private Rectangle? healSource;

        public HealAnimation(Seizonsha game, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, int duration)
            : base(game, sprite, origin, bounds, amount, damageType, duration, 1f)
        {
            this.delay = 40; //make durations based on time (floats)
            this.healSource = new Rectangle(0, 0, 64, 64);
            
        }


        public override void UpdateAnimation(GameTime gameTime)
        {

                elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (elapsed > delay)
                {
                    if (currentFrame >= healFrames - 1)
                    {
                        currentFrame = 0;
                    }

                    else
                    {
                        currentFrame++;
                    }

                    elapsed = 0;
                }


                healSource = new Rectangle(64 * currentFrame, 0, 64, 64);
                    
                
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(sprite, this.hitbox, healSource, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
        }

        public void reset(GameEntity target){
            base.reset();
            currentFrame = 0;
            this.elapsed = 0;
            //reset other stuff
        }

        public void OnRemove(GameEntity target)
        {
           
        }
    }
}
