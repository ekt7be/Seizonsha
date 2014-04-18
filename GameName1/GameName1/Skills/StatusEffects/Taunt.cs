using GameName1.Interfaces;
using GameName1.NPCs;
using GameName1.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Effects
{
    class Taunt : GameName1.Interfaces.StatusEffect
    {
        protected int damageType;

        public Taunt(Seizonsha game, GameEntity user, Skill origin, Texture2D sprite, GameEntity afflicted, int damageType, int duration)
            : base(game, user, origin, sprite, afflicted, duration)
        {
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
            if (afflicted is Enemy)
            {
                ((Enemy)afflicted).setTarget(user);
            }
            base.Update();
        }


    }
}
