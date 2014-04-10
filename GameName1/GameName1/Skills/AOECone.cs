using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class AOECone : GameName1.Effects.Effect
    {
        private int damageType;
        private int amount;
        private Skill origin;


        public AOECone(Seizonsha game, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, int duration)
            : base(game, sprite, bounds.Width, bounds.Height, duration)
        {
            this.amount = amount;
            this.damageType = damageType;
            this.origin = origin;
        }

        protected override void OnDie()
        {
            
        }

        public override void OnSpawn()
        {
            game.affectArea(origin, this.hitbox);
        }


        public override string getName()
        {
            return Static.TYPE_AOE_CONE;
        }


        public void reset(Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, int duration)
        {
            base.reset(duration);
            setSprite(sprite);
            this.origin = origin;
            this.width = bounds.Width;
            this.height = bounds.Height;
            this.damageType = damageType;
            this.amount = amount;


        }




    }
}
