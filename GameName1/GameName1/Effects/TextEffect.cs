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
        Color textColor;
        static List<TextEffect> liveTextEffects = new List<TextEffect>();
        static List<TextEffect> deadTextEffects = new List<TextEffect>();

        public static TextEffect getInstance(Seizonsha game, string text, int duration,Vector2 velocity, Color textColor)
        {
            if (deadTextEffects.Count == 0)
                return new TextEffect(game, text, duration, velocity, textColor);
            else
            {
                TextEffect te = deadTextEffects[0];
                deadTextEffects.Remove(te);
                liveTextEffects.Add(te);
                te.set(game, text, duration, velocity, textColor);
                return te;
            }

        }

        public static void removeInstance(TextEffect te){
            liveTextEffects.Remove(te);
            deadTextEffects.Add(te);
        }

        protected TextEffect(Seizonsha game, string text, int duration,Vector2 velocity, Color textColor) : base(game, null, 0, 0, duration)
        {
            this.incVelocityY((int)velocity.Y);
            this.incVelocityX((int)velocity.X);
            this.text = text;
            this.textColor = textColor;
        }

        protected void set(Seizonsha game, string text, int duration, Vector2 velocity, Color textColor)
        {
            base.set(game, null, 0, 0, duration);
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
