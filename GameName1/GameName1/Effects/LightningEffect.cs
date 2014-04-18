using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Effects
{
    class LightningEffect : Effect
    {
        GameEntity g1;
        GameEntity g2;

        public LightningEffect(Seizonsha game, Texture2D sprite, int duration, GameEntity g1, GameEntity g2): base(game, sprite, 0, 0, duration){
            this.g1 = g1;
            this.g2 = g2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.hitbox = new Rectangle((int)(g1.getCenterX()), (int)(g1.getCenterY()), (int)Math.Sqrt(Math.Pow(g1.getCenterX() - g2.getCenterX(), 2) + Math.Pow(g1.getCenterY() - g2.getCenterY(),2)), 3);
            spriteBatch.Draw(sprite, hitbox, null,
                tint, (float)Math.Atan2(g2.getCenterY() - g1.getCenterY(), g2.getCenterX() - g1.getCenterX()), new Vector2(0f, 0f), SpriteEffects.None, 1f);
            // base.Draw(spriteBatch);
        }
    }
}
