﻿using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class SwordSlash : GameName1.Effects.Effect
    {

        private int damageType;
        private int amount;
        private GameEntity user;

        public SwordSlash(Seizonsha game, GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, int duration, Vector2 direction)
            : base(game, sprite, bounds.Width, bounds.Height, duration)
        {
            this.amount = amount;
            this.damageType = damageType;
            this.user = user;
        }


        public override void OnSpawn()
        {
            game.damageArea(user, this.getHitbox(), amount, damageType);
        }


        protected override void OnDie()
        {
        }


    }
}
