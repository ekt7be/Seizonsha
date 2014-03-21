using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.NPCs
{
    class BasicEnemy : GameEntity, AI
    {

        int count = 0;

        int currentFrame = 0;

        public BasicEnemy(Seizonsha game, Texture2D sprite, int x, int y)
            : base(game, sprite, x, y, Static.BASIC_ENEMY_WIDTH, Static.BASIC_ENEMY_HEIGHT, Static.DAMAGE_TYPE_ENEMY, 200)
        {
            base.source = new Rectangle(sprite.Width / 4 * currentFrame, 0, sprite.Width / 4, sprite.Height);
            base.scale = 1.0f;
        }

        public void AI()
        {
            //just sits there
        }

        public override void collide(GameEntity entity)
        {
          //  Static.Debug("NPC collision with entity");
			if(entity.getTargetType() == Static.TARGET_TYPE_FRIENDLY){
				entity.damage(2, Static.DAMAGE_TYPE_ENEMY);
			}
        }

        public override void collideWithWall()
        {
        }

        public override void OnSpawn()
        {
            this.velocityX = 0;
            this.velocityY = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //draw armor and weapons equipped etc

        }

        public override void Update()
        {

            List<Player> players = game.getPlayers();

            Player closest = null;
            double closestDistance = Double.PositiveInfinity;


            foreach (Player p in players)
            {
                if (p == null || p.isDead())
                    continue;
                if (Math.Sqrt(Math.Pow(p.x - this.x, 2) + Math.Pow(p.y - this.y, 2)) < closestDistance)
                {
                    closest = p;
                    closestDistance = Math.Sqrt(Math.Pow(p.x - this.x, 2) + Math.Pow(p.y - this.y, 2));
                }
            }

            if (closest == null)
                return;

            this.hitbox = new Rectangle(this.x, this.y, this.width, this.height);
            if (closest.x < this.x)
                this.velocityX = -1;
            if (closest.x > this.x)
                this.velocityX = 1;
            if (closest.y < this.y)
                this.velocityY = -1;
            if (closest.y > this.y)
                this.velocityY = 1;

            float playerDirection = (float)Math.Atan2(this.y - closest.y, closest.x - this.x);
            if (playerDirection < 0)
                playerDirection += (2.0f * (float)Math.PI);

            //up
            if (playerDirection >= .79 && playerDirection < 2.3)
                currentFrame = 0;
            //right
            else if (playerDirection >= 2.3 && playerDirection < 3.9)
                currentFrame = 1;
            //down
            else if (playerDirection >= 3.9 && playerDirection < 5.5)
                currentFrame = 2;
            //left
            else
                currentFrame = 3;

            base.source = new Rectangle(sprite.Width / 4 * currentFrame, 0, sprite.Width / 4, sprite.Height);
        }

        protected override void OnDie()
        {
            count++;
        }

    }
}
