using GameName1.Interfaces;
using GameName1.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Effects
{
    class Burning :  GameName1.Interfaces.StatusEffect
    {
        protected int amount;
        protected int damageType;

        public Burning(Seizonsha game, GameEntity user, Skill origin, Texture2D sprite, GameEntity afflicted, int amount, int damageType, int duration) : base(game, user, origin, sprite, afflicted, duration)
        {
            this.amount = amount;
            this.damageType = damageType;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (time % 30 < 3 || time % 30 > 28)
            {
                int w = 35;
                int h = 35;
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_BURN], new Rectangle(afflicted.getCenterX() - 3 * w / 4, afflicted.getCenterY() - h / 2, w, h), Color.White);
            }
        }
        public override void onEnd()
        {

        }
        public override void Update()
        {
            if(time % 30 == 0){
             game.damageEntity(user, afflicted, amount, this.damageType);
            }
 	        base.Update();
        }


    }
}
