using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
	class Gun : Weapon
	{


		private int damage;
		private float bulletSpeed; 


		public Gun(Seizonsha game, GameEntity user, int damage, int recharge_time, int freezeTime, float bulletSpeed) : base(game, user,recharge_time, freezeTime)
		{

			this.damage = damage;
			this.bulletSpeed = bulletSpeed; 
		}


		protected override void UseSkill()
		{


            int bulletWidth = 10;
            int bulletHeight = 10;
            
            Rectangle bulletBounds = new Rectangle((int)(user.getCenterX() - bulletWidth/2), (int)(user.getCenterY() - bulletHeight/2), bulletWidth, bulletHeight);

			game.Spawn(EntityFactory.getBullet(game, this, Seizonsha.spriteMappings[Static.SPRITE_BULLET], bulletBounds, damage, damageType, bulletSpeed, user.direction), bulletBounds.Left, bulletBounds.Top);


		}

        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, damage, damageType);
        }


		public override string getDescription()
		{
			return "A GUN";
		}

		public override string getName()
		{
			return Static.GUN_NAME;
		}


    }
}
