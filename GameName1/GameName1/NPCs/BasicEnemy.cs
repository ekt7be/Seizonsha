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


        private static readonly int UP_ANIMATION = 2;
        private static readonly int DOWN_ANIMATION = 0;
        private static readonly int LEFT_ANIMATION = 1;
        private static readonly int RIGHT_ANIMATION = 3;

        public BasicEnemy(Seizonsha game, Texture2D sprite, int x, int y)
            : base(game, sprite, x, y, Static.BASIC_ENEMY_WIDTH, Static.BASIC_ENEMY_HEIGHT, Static.DAMAGE_TYPE_ENEMY, 200)
        {
            base.scale = 1.0f;
            setXPReward(50);

            FramesToAnimation.Add(UP_ANIMATION, new Rectangle(sprite.Width / 4 * UP_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            FramesToAnimation.Add(DOWN_ANIMATION, new Rectangle(sprite.Width / 4 * DOWN_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            FramesToAnimation.Add(LEFT_ANIMATION, new Rectangle(sprite.Width / 4 * LEFT_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            FramesToAnimation.Add(RIGHT_ANIMATION, new Rectangle(sprite.Width / 4 * RIGHT_ANIMATION, 0, sprite.Width / 4, sprite.Height));
        }

        public void AI()
        {
            //just sits there
        }

        public override void collide(GameEntity entity)
        {
          //  Static.Debug("NPC collision with entity");
			if(entity.getTargetType() == Static.TARGET_TYPE_FRIENDLY){
				game.damageEntity(this, entity, 2, Static.DAMAGE_TYPE_ENEMY);
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

            rotateToAngle(playerDirection);
            base.Update();
        }

        protected override void OnDie()
        {
            count++;
        }


        public override void rotateToAngle(float angle) //animation is based on rotation which is used by both movement and aiming
        {
            base.rotateToAngle(angle);

            if (FramesToAnimation == null) //gameentity class calls this during initialization too
            {
                return;
            }

            if (Math.Cos(angle) > .5)
            {
                base.spriteSource = FramesToAnimation[RIGHT_ANIMATION];
            }
            else if (Math.Sin(angle) > .5)
            {
                base.spriteSource = FramesToAnimation[DOWN_ANIMATION];
            }
            else if (Math.Sin(angle) < -.5)
            {
                base.spriteSource = FramesToAnimation[UP_ANIMATION];
            }
            else if (Math.Cos(angle) < -.5)
            {
                base.spriteSource = FramesToAnimation[LEFT_ANIMATION];
            }
        }

    }
}
