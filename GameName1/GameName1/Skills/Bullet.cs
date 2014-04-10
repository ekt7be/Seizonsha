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
        protected Skill origin;




		public Bullet(Seizonsha game, Skill origin, Texture2D sprite, Rectangle bounds, int amount, int damageType, float bulletSpeed, float directionAngle)
			: base(game, sprite, bounds.Width, bounds.Height, Static.TARGET_TYPE_NOT_DAMAGEABLE, 30)
		{
			this.amount = amount;
			this.damageType = damageType;
			this.bulletSpeed = bulletSpeed;
            this.bulletSpeed = 0.3f;
            this.rotateToAngle(directionAngle);
            this.velocityX = (bulletSpeed * vectorDirection.X);
            this.velocityY = (bulletSpeed * vectorDirection.Y);
            this.origin = origin;
		}




		public override void OnSpawn()
		{	

		}

        public override void collide(GameEntity entity)
        {

            //game.damageEntity(user, entity, amount, damageType);
            origin.affect(entity);

            setRemove(true);


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override bool shouldCollide(GameEntity entity)
        {
            if (entity == origin.getUser() || !game.ShouldDamage(damageType,entity.getTargetType()))
            {
                return false;
            }
            if (entity is Bullet)
            {
                return false;
            }
            return true;
        }

        public void reset(Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, float bulletSpeed, float directionAngle)
        {
            base.reset();
            this.amount = amount;
            this.damageType = damageType;
            this.bulletSpeed = bulletSpeed;
            rotateToAngle(directionAngle);
            velocityX = (bulletSpeed * vectorDirection.X);
            velocityY = (bulletSpeed * vectorDirection.Y);
            setSprite(sprite);
            width = bounds.Width;
            height = bounds.Height;
            this.origin = origin;
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
