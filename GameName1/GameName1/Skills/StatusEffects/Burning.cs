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
            //
        }
        public override void onEnd()
        {

        }
        public override void Update()
        {
            game.damageEntity(user, afflicted, amount, this.damageType);
 	        base.Update();
        }


    }
}
