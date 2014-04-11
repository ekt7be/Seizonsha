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
    class Stun : GameName1.Interfaces.StatusEffect
    {
        protected int damageType;

        public Stun(Seizonsha game, GameEntity user, Skill origin, Texture2D sprite, GameEntity afflicted, int damageType, int duration)
            : base(game, user, origin, sprite, afflicted, duration)
        {
            this.damageType = damageType;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //
        }

        public override void Update()
        {
            afflicted.Freeze(2);
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