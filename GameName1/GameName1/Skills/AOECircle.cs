﻿using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class AOECircle : GameName1.Effects.Effect
    {
        private int damageType;
        private int amount;
        private GameEntity user;

        public AOECircle(Seizonsha game, GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, int duration, Vector2 direction)
            : base(game, sprite, bounds.Width, bounds.Height, duration)
        {
            this.amount = amount;
            this.damageType = damageType;
            this.user = user;
        }

        protected override void OnDie()
        {

        }

        public override void OnSpawn()
        {
            if (amount < 0)
            {
                game.healArea(user,this.getHitbox(), -amount, damageType);
            }
            else
            {
                game.damageArea(user, this.getHitbox(), amount, damageType);
            }
        }


        public override string getName()
        {
            return Static.TYPE_AOE_CIRCLE;

        }

        public void setAmount(int amount)
        {
            this.amount = amount;
        }

        public void setUser(GameEntity user)
        {
            this.user = user;
        }

        public void setDamageType(int damageType)
        {
            this.damageType = damageType;
        }

        public override void reset()
        {
            base.reset();
        }

    }
}
