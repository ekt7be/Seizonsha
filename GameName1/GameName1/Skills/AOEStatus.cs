﻿using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class AOEStatus : GameName1.Effects.Effect
    {
        private int damageType;
        private int amount;
        private GameEntity user;
        private Skill origin;


        public AOEStatus(Seizonsha game, GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, int duration)
            : base(game, sprite, bounds.Width, bounds.Height, duration)
        {
            this.amount = amount;
            this.damageType = damageType;
            this.user = user;
            this.origin = origin;
        }

        protected override void OnDie()
        {

        }

        public override void OnSpawn()
        {
            if (amount < 0)
            {
                game.healArea(user, this.getHitbox(), -amount, damageType);
            }
            else
            {
                //game.damageArea(user, this.getHitbox(), amount, damageType);
                foreach (GameEntity entity in game.getEntitiesInBounds(this.getHitbox()))
                {
                    if (entity.getTargetType() == Static.TARGET_TYPE_ENEMY) this.origin.affect(entity);
                }
            }
        }

        


        public override void Update(GameTime gameTime)
        {
            if (amount < 0)
            {
                game.healArea(user, this.getHitbox(), -amount, damageType);
            }
            else
            {
                //game.damageArea(user, this.getHitbox(), amount, damageType);
                foreach (GameEntity entity in game.getEntitiesInBounds(this.getHitbox()))
                {
                    this.origin.affect(entity);
                }
            }
            base.Update(gameTime);
        }

        public override string getName()
        {
            return Static.TYPE_AOE_STATUS;
        }


        public void reset(GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, int duration)
        {
            base.reset(duration);
            this.user = user;
            setSprite(sprite);
            this.origin = origin;
            this.width = bounds.Width;
            this.height = bounds.Height;
            this.damageType = damageType;
            this.amount = amount;


        }




    }
}