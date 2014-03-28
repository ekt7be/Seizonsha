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
	class Bullet : GameEntity
	{
		protected int damageType;
		protected int amount;
		private float bulletSpeed; 
        protected GameEntity user;

		public Bullet(Seizonsha game, GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, int duration, float bulletSpeed, Vector2 alexDirection)
			: base(game, sprite, bounds.Left, bounds.Top, bounds.Width, bounds.Height, Static.TARGET_TYPE_NOT_DAMAGEABLE, 30)
		{
			this.amount = amount;
			this.damageType = damageType;
			this.bulletSpeed = bulletSpeed;
            this.velocityX = (int)(bulletSpeed * alexDirection.X);
            this.velocityY = (int)(bulletSpeed * alexDirection.Y);
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
            if (entity == user)
            {
                return false;
            }
            if (entity is Bullet)
            {
                return false;
            }
            return true;
        }

        public override void collideWithWall()
        {
            setRemove(true);
        }


		protected override void OnDie()
		{
		}
	}
}
