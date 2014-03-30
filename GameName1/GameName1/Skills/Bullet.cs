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

        static List<Bullet> liveBullets = new List<Bullet>();
        static List<Bullet> deadBullets = new List<Bullet>();

        public static Bullet getInstance(Seizonsha game, GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, int duration, float bulletSpeed, Vector2 alexDirection)
        {
            if (deadBullets.Count == 0)
                return new Bullet(game, user, sprite, bounds, amount, damageType, duration, bulletSpeed, alexDirection);
            else
            {
                Bullet te = deadBullets[0];
                deadBullets.Remove(te);
                liveBullets.Add(te);
                te.set(game, user, sprite, bounds, amount, damageType, duration, bulletSpeed, alexDirection);
                return te;
            }

        }

        public static void removeInstance(Bullet te)
        {
            liveBullets.Remove(te);
            deadBullets.Add(te);
        }

        public void removeMe()
        {
            removeInstance(this);
        }

		protected Bullet(Seizonsha game, GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, int duration, float bulletSpeed, Vector2 alexDirection)
			: base(game, sprite, bounds.Width, bounds.Height, Static.TARGET_TYPE_NOT_DAMAGEABLE, 30)
		{
			this.amount = amount;
			this.damageType = damageType;
			this.bulletSpeed = bulletSpeed;
            this.velocityX = (int)(bulletSpeed * alexDirection.X);
            this.velocityY = (int)(bulletSpeed * alexDirection.Y);
            this.user = user;
		}

        protected void set(Seizonsha game, GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, int duration, float bulletSpeed, Vector2 alexDirection)
        {
            base.set(game, sprite, bounds.Width, bounds.Height, Static.TARGET_TYPE_NOT_DAMAGEABLE, 30);

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

        public override void collideWithWall()
        {
            setRemove(true);
        }


		protected override void OnDie()
		{
		}
	}
}
