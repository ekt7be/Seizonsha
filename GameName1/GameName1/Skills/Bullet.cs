using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
	class Bullet : GameName1.Effects.Effect
	{

		private int damageType;
		private int amount;
		private Vector2 bulletSpeed; 
		private Vector2 direction; 

		public Bullet(Seizonsha game, Texture2D sprite, Rectangle bounds, int amount, int damageType, int duration, Vector2 bulletSpeed, Vector2 alexDirection)
			: base(game, sprite, bounds.Left, bounds.Top, bounds.Width, bounds.Height, duration)
		{
			this.amount = amount;
			this.damageType = damageType;
			this.bulletSpeed = bulletSpeed;
			this.direction = alexDirection;
		}


		public override void OnSpawn()
		{	
			this.velocityX = (int)(bulletSpeed.X * direction.X); 
			this.velocityY = (int)(bulletSpeed.Y * direction.Y);

			// this.velocityX = this.velocityY = bulletSpeed; 
		}

		public override void Update()
		{
			this.hitbox = new Rectangle(this.x, this.y, this.width, this.height); 
			game.damageArea(this.getHitbox(), amount, damageType);

		}


		protected override void OnDie()
		{
		}
	}
}
