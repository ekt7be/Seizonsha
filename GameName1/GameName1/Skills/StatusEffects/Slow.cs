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
    class Slow : GameName1.Interfaces.StatusEffect
    {
        protected float amount;
        protected int damageType;

        public Slow(Seizonsha game, GameEntity user, Skill origin, Texture2D sprite, GameEntity afflicted, float amount, int damageType, int duration)
            : base(game, user, origin, sprite, afflicted, duration)
        {
            this.amount = amount;
            this.damageType = damageType;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //
        }

        public override void Update()
        {
            afflicted.modifySpeed(amount);
            base.Update();
        }

        public override void onEnd()
        {
            //afflicted.modifySpeed((float)(1.0 / amount));
        }

        public override void onCreate()
        {
           // afflicted.modifySpeed(amount);
        }


    }
}
