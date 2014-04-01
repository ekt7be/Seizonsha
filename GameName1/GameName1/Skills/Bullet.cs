using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
	public class Bullet : GameEntity
	{
		protected int damageType;
		protected int amount;
		private float bulletSpeed; 
        protected GameEntity user;




		public Bullet(Seizonsha game, GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, float bulletSpeed, float directionAngle)
			: base(game, sprite, bounds.Width, bounds.Height, Static.TARGET_TYPE_NOT_DAMAGEABLE, 30)
		{
			this.amount = amount;
			this.damageType = damageType;
			this.bulletSpeed = bulletSpeed;
            this.rotateToAngle(directionAngle);
            this.velocityX = (int)(bulletSpeed * vectorDirection.X);
            this.velocityY = (int)(bulletSpeed * vectorDirection.Y);
            this.user = user;
		}




		public override void OnSpawn()
		{	

		}

		public override void Update(GameTime gameTime)
		{
			this.hitbox = new Rectangle(this.x, this.y, this.width, this.height); 
		}

        public override void collide(GameEntity entity)
        {

            game.damageEntity(user, entity, amount, damageType);

            setRemove(true);


        }

        public override bool shouldCollide(GameEntity entity)
        {
            if (entity == user || !game.ShouldDamage(damageType,entity.getTargetType()))
            {
                return false;
            }
            if (entity is Bullet)
            {
                return false;
            }
            return true;
        }

        public void reset(GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, float bulletSpeed, float directionAngle)
        {
            base.reset();
            this.amount = amount;
            this.damageType = damageType;
            this.bulletSpeed = bulletSpeed;
            rotateToAngle(directionAngle);
            velocityX = (int)(bulletSpeed * vectorDirection.X);
            velocityY = (int)(bulletSpeed * vectorDirection.Y);
            setSprite(sprite);
            width = bounds.Width;
            height = bounds.Height;

        }

        public override void collideWithWall()
        {
            setRemove(true);
        }


		protected override void OnDie()
		{
		}


        public override string getName()
        {
            return Static.TYPE_BULLET;

        }


	}
}
