﻿using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
	class Gun : Skill
	{


		private int damage;
        private int damageType;
		private float bulletSpeed; 


		public Gun(Seizonsha game, GameEntity user, int damage, int recharge_time, float bulletSpeed) : base(game, user, 0, recharge_time,0,0)
		{

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
			this.bulletSpeed = bulletSpeed; 
		}


		protected override void UseSkill()
		{


            int bulletWidth = 10;
            int bulletHeight = 10;
            
            Rectangle bulletBounds = new Rectangle((int)(user.getCenterX()), (int)(user.getCenterY() + user.vectorDirection.Y), bulletWidth, bulletHeight);

			game.Spawn(EntityFactory.getBullet(game, user, Seizonsha.spriteMappings[Static.SPRITE_BULLET], bulletBounds, damage, damageType, bulletSpeed, user.direction), bulletBounds.Left, bulletBounds.Top);


		}

        public override void affect(GameEntity affected)
        {
            throw new NotImplementedException();
        }


		public override string getDescription()
		{
			return "A GUN";
		}

		public override string getName()
		{
			return "Gun";
		}


	}
}
