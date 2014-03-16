using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Effects
{
    class TextEffect : GameEntity
    {
        int duration = 30;
        string text;
        public TextEffect(Seizonsha game, string text, int duration, int x, int y) : base(game, null, x, y, 0, 0)
        {
            setCollidable(false);
            incVelocityY(-3);
            this.text = text;
            this.duration = duration;

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
            spriteBatch.DrawString(game.getSpriteFont(), text, new Vector2(x, y), Color.White);
        }

        public override void Update()
        {
            duration--;
            if (duration <= 0)
            {
                setRemove(true);
            }
        }
    }
}
