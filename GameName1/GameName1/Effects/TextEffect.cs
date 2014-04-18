using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Effects
{
    public class TextEffect : Effect
    {
        string text;



        public TextEffect(Seizonsha game, string text, int duration,Vector2 velocity, Color textColor) : base(game, null, 0, 0, duration)
        {
            this.incVelocityY((int)velocity.Y);
            this.incVelocityX((int)velocity.X);
            this.text = text;
            this.tint = textColor;
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
            Static.DrawBorderedText(spriteBatch, game.getSpriteFont(), text, x, y, Color.Black, this.tint);

            //spriteBatch.DrawString(game.getSpriteFont(), text, new Vector2(x, y), this.tint);
        }


        protected override void OnDie()
        {
        }


        public void reset(String text, Color textColor, Vector2 velocity, int duration)
        {
            base.reset(duration);
            this.text = text;
            this.tint= textColor;
            this.velocityX = velocity.X;
            this.velocityY = velocity.Y;

        }




        public override string getName()
        {
            return Static.TYPE_TEXT_EFFECT;

        }

    }
}
