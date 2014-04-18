using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.AnimationTesting;
using GameName1.NPCs;

namespace GameName1.Effects
{
    class LightningEffect : Effect
    {
        GameEntity g1;
        GameEntity g2;
        Rectangle? lightningSource;

        private float elapsed;
        private float delay = 20f;
        private int currentFrame = 0;
        private static readonly int shockFrames = 4;
        private int recharge_time;




        public LightningEffect(Seizonsha game, Texture2D sprite, int duration, GameEntity g1, GameEntity g2)
            : base(game, sprite, 0, 0, duration)
        {
            this.g1 = g1;
            this.g2 = g2;
            lightningSource = new Rectangle(0, 0, 64, 64);
            sprite = Seizonsha.spriteMappings[Static.SPRITE_SHOCK];

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.hitbox = new Rectangle((int)(g1.getCenterX()), (int)(g1.getCenterY()), (int)Math.Sqrt(Math.Pow(g1.getCenterX() - g2.getCenterX(), 2) + Math.Pow(g1.getCenterY() - g2.getCenterY(), 2)), 20);
            spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_SHOCK], hitbox, lightningSource,
                tint, (float)Math.Atan2(g2.getCenterY() - g1.getCenterY(), g2.getCenterX() - g1.getCenterX()), new Vector2(0f, 0f), SpriteEffects.None, 1f);
            // base.Draw(spriteBatch);
        }

        public Rectangle? getLightningSource()
        {
            return lightningSource;
        }

        public override void UpdateAnimation(GameTime gameTime)
        {
            base.UpdateAnimation(gameTime);
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed > delay)
            {
                if (currentFrame >= shockFrames - 1)
                {
                    currentFrame = 0;
                }

                else
                {
                    currentFrame++;
                }

                elapsed = 0;
            }


            lightningSource = new Rectangle(64 * currentFrame, 0 * 64, 64, 64);

        }




    }
}
