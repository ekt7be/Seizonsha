﻿using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class Sword : Skill
    {

        private int damage;
        private int damageType;
        GameEntity entity;


        public Sword(Seizonsha game, GameEntity user,int damage, int recharge_time) : base(game, user, 0, recharge_time, 0, 10)
        {
            entity = user;
            this.damage = damage;
            this.damageType = Static.DAMAGE_TYPE_NO_DAMAGE;
            if (user.getTargetType() == Static.TARGET_TYPE_FRIENDLY)
            {
                damageType = Static.DAMAGE_TYPE_FRIENDLY;
            }
            if (user.getTargetType() == Static.TARGET_TYPE_ENEMY)
            {
                damageType = Static.DAMAGE_TYPE_ENEMY;
            }
        }



        public override string getDescription()
        {
            return "A SWORD";
        }

        public override string getName()
        {
            return "SWORD";
        }

        public override void affect(GameEntity affected)
        {
          //  throw new NotImplementedException();
        }


        protected override void UseSkill()
        {
            entity.setSlashing(true);
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * user.width / 2 - user.width / 4), (int)(user.getCenterY() + user.vectorDirection.Y * user.height / 2 - user.height / 4), user.width / 2, user.height / 2);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            AOECone attack = EntityFactory.getAOECone(game, user, null, this, slashBounds, damage, damageType, 10);
            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
        }

    }
}
