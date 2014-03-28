using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Effects
{
    class TextEffect : Effect
    {
        string text;
        Color textColor;

        public TextEffect(Seizonsha game, string text, int duration,Vector2 velocity, Color textColor) : base(game, null, 0, 0, duration)
        {
            this.incVelocityY((int)velocity.Y);
            this.incVelocityX((int)velocity.X);
            this.text = text;
            this.textColor = textColor;
        }

        public override void OnSpawn()
        {
        }

        public override void collideWithWall()
        {
        }

        public override void collide(GameEntity entity)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(game.getSpriteFont(), text, new Vector2(x, y), this.textColor);
        }


        protected override void OnDie()
        {
        }
    }
}
