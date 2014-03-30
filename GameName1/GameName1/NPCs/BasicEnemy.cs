using GameName1.Interfaces;
using GameName1.Skills;
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
        private static float elapsed;
        private static readonly float delay = 200f;
        private static int currentFrame = 0;
        private Sword sword;

        private static readonly int UP_ANIMATION = 2;
        private static readonly int DOWN_ANIMATION = 0;
        private static readonly int LEFT_ANIMATION = 1;
        private static readonly int RIGHT_ANIMATION = 3;
        private static readonly int WALK_ANIMATION_FRAMES = 9;

        // This states that they are always going to be walking... which is true for now. But when we add things such as attacks for enemies, we need to change it so that it sets this to false and sets isAttacking, or something, to true
        private bool isWalking = true;
        
        private bool up;
        private bool down;
        private bool left;
        private bool right;

        public BasicEnemy(Seizonsha game)
            : base(game, Seizonsha.spriteMappings[Static.SPRITE_BASIC_ENEMY_INT], Static.BASIC_ENEMY_WIDTH, Static.BASIC_ENEMY_HEIGHT, Static.DAMAGE_TYPE_ENEMY, 200)
        {
            base.scale = 1.0f;
            setXPReward(50);

            FramesToAnimation.Add(UP_ANIMATION, new Rectangle(sprite.Width / 4 * UP_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            FramesToAnimation.Add(DOWN_ANIMATION, new Rectangle(sprite.Width / 4 * DOWN_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            FramesToAnimation.Add(LEFT_ANIMATION, new Rectangle(sprite.Width / 4 * LEFT_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            FramesToAnimation.Add(RIGHT_ANIMATION, new Rectangle(sprite.Width / 4 * RIGHT_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            sword = new Sword(game, this, 5, 20);
            sword.OnEquip();
        }

        public void AI()
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
                this.move(-1, 0);
            if (closest.x > this.x)
                this.move(1, 0);
            if (closest.y < this.y)
                this.move(0, -1);
            if (closest.y > this.y)
                this.move(0, 1);

            float playerDirection = (float)Math.Atan2(closest.y-this.y, closest.x - this.x);

            rotateToAngle(playerDirection);

            if (closestDistance < this.width*2)
            {
                sword.Use();
            }
        }

        public override void collide(GameEntity entity)
        {

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            sword.Update();


            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed > delay)
            {
                if (currentFrame >= WALK_ANIMATION_FRAMES - 1)
                {
                    currentFrame = 0;
                }
                else
                {
                    currentFrame++;
                }
                elapsed = 0;
            }

            if (isWalking)
            {
                if (up)
                    base.spriteSource = new Rectangle(64 * currentFrame, DOWN_ANIMATION * 64, 64, 64);
                else if (left)
                    base.spriteSource = new Rectangle(64 * currentFrame, LEFT_ANIMATION * 64, 64, 64);
                else if (down)
                    base.spriteSource = new Rectangle(64 * currentFrame, UP_ANIMATION * 64, 64, 64);
                else if (right)
                    base.spriteSource = new Rectangle(64 * currentFrame, RIGHT_ANIMATION * 64, 64, 64);
            }
            else
            {
                if (up)
                    base.spriteSource = new Rectangle(64 * 0, DOWN_ANIMATION * 64, 64, 64);
                else if (left)
                    base.spriteSource = new Rectangle(64 * 0, LEFT_ANIMATION * 64, 64, 64);
                else if (down)
                    base.spriteSource = new Rectangle(64 * 0, UP_ANIMATION * 64, 64, 64);
                else if (right)
                    base.spriteSource = new Rectangle(64 * 0, RIGHT_ANIMATION * 64, 64, 64);
            }
        }

        protected override void OnDie()
        {
            game.decreaseNumberEnemies();
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
                //base.spriteSource = FramesToAnimation[RIGHT_ANIMATION];
                right = true;
                up = left = down = false;
            }
            else if (Math.Sin(angle) > .5)
            {
                //base.spriteSource = FramesToAnimation[DOWN_ANIMATION];
                down = true;
                up = left = right = false;
            }
            else if (Math.Sin(angle) < -.5)
            {
                //base.spriteSource = FramesToAnimation[UP_ANIMATION];
                up = true;
                right = left = down = false;
            }
            else if (Math.Cos(angle) < -.5)
            {
                //base.spriteSource = FramesToAnimation[LEFT_ANIMATION];
                left = true;
                up = right = down = false;
            }
        }

    }
}
