using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class ExplodingBullet : GameEntity
    {

        private int damageType;
        private int amount;
        private Vector2 bulletSpeed;
        private Vector2 direction;

        public ExplodingBullet(Seizonsha game, Texture2D sprite, Rectangle bounds, int amount, int damageType, int duration, Vector2 bulletSpeed, Vector2 alexDirection)
            : base(game, sprite, bounds.Left, bounds.Top, bounds.Width, bounds.Height, Static.TARGET_TYPE_NOT_DAMAGEABLE, 30)
        {
            this.amount = amount;
            
            // entity.Freeze(recharge_time);
            this.damageType = damageType;
            this.bulletSpeed = bulletSpeed;
            this.direction = alexDirection;
        }


        public override void OnSpawn()
        {
            this.velocityX = (int)(bulletSpeed.X * direction.X);
            this.velocityY = (int)(bulletSpeed.Y * direction.Y);
        }

        public override void Update()
        {
            this.hitbox = new Rectangle(this.x, this.y, this.width, this.height);
        }

        public override void collide(GameEntity entity)
        {

            
            int explosionWidth = 80;
            int explosionHeight = 80;
            Rectangle slashBounds = new Rectangle((int)(entity.getCenterX() -explosionWidth/2), (int)(entity.getCenterY() -explosionWidth/2), explosionWidth, explosionHeight);
            game.Spawn(new AOECone(game, game.getTestSprite(slashBounds, Color.Green), slashBounds, amount, this.damageType, 10, entity.alexDirection));

            setRemove(true);
            // game.damageArea(this.getHitbox(), amount, damageType);

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
