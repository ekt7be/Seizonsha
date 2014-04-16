using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
	abstract class Gun : Weapon
	{


		private int damage;
		private float bulletSpeed;
        public int ammo;
        public int clipSize;

        //for enemies
        private Boolean unlimitedAmmo;


		public Gun(Seizonsha game, GameEntity user, int damage, int recharge_time, int freezeTime, float bulletSpeed, int level, string name, int clipSize, Color tint) : base(game, user,recharge_time, freezeTime, level, name, tint)
		{

			this.damage = damage;
			this.bulletSpeed = bulletSpeed;
            this.clipSize = clipSize;
            this.ammo = clipSize;
            this.unlimitedAmmo = false;
		}

        public void refillAmmo()
        {
            ammo = clipSize;
        }

        public void setUnlimitedAmmo(bool unlimited)
        {
            this.unlimitedAmmo = unlimited;
            if (unlimited)
            {
                this.ammo = clipSize;
            }
        }

		protected override void UseSkill()
		{


            int bulletWidth = 10;
            int bulletHeight = 10;
            
            Rectangle bulletBounds = new Rectangle((int)(user.getCenterX() - bulletWidth/2), (int)(user.getCenterY() - bulletHeight/2), bulletWidth, bulletHeight);

			game.Spawn(EntityFactory.getBullet(game, this, Seizonsha.spriteMappings[Static.SPRITE_BULLET], bulletBounds, damage, damageType, bulletSpeed, user.direction), bulletBounds.Left, bulletBounds.Top);
            if (!unlimitedAmmo)
            {
                this.ammo--;
            }

		}

        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, damage, damageType);
        }


		public override string getDescription()
		{
			return "A GUN";
		}

        public override bool Available()
        {
            if (ammo == 0)
            {
                return false;
            }
            return base.Available();
        }

    }
}
