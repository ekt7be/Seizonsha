using GameName1.AnimationTesting;
using GameName1.Skills;
using GameName1.Skills.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.NPCs
{
    class ExplodeEnemy :Enemy
    {

		double closestDistance;





        private GameEntity closest;
        private int ExplosionCountdown;
        private bool readyExplode;
        private bool exploded;
        private int level;


		public ExplodeEnemy(Seizonsha game, int level)
			: base(game, Seizonsha.spriteMappings[Static.SPRITE_EXPLODE_ENEMY_INT], Static.EXPLODE_ENEMY_WIDTH, Static.EXPLODE_ENEMY_HEIGHT, Static.EXPLODE_ENEMY_HEALTH_1, Static.EXPLODE_ENEMY_SPEED_1, Static.EXPLODE_ENEMY_XP_1)
		{
			base.scale = Static.EXPLODE_ENEMY_SPRITE_SCALE;
            init(level);
		}


        public void init(int level)
        {
            this.level = level;

            if (level == 1)
            {
                this.speed = Static.EXPLODE_ENEMY_SPEED_1;
                this.maxHealth = Static.EXPLODE_ENEMY_HEALTH_1;
                this.health = maxHealth;

            }
            else if (level == 2)
            {
                this.speed = Static.BASIC_ENEMY_SPEED_2;
               // this.defaultTint = Color.Green;
               // setDefaultTint();
                this.maxHealth = Static.BASIC_ENEMY_HEALTH_2;
                this.health = maxHealth;
            }

            this.readyExplode = false;
            this.exploded = false;
            this.ExplosionCountdown = 0;
        }




		public override void AI(GameTime gameTime)
		{


            if (ExplosionCountdown > 0)
            {

                ExplosionCountdown--;
                if (ExplosionCountdown == 0)
                {
                    Explode();
                    return;
                }
            }
			// find closest player
			List<Player> players = game.getPlayers();

			closestDistance = Double.PositiveInfinity;
            
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

            if (closest != null)
            {
                setTarget(closest);
            }

            if (closestDistance < Static.TILE_WIDTH * Static.EXPLODE_ENEMY_TILE_DISTANCE_EXPLODE && !readyExplode)
            {
                readyToExplode();
            }

            base.AI(gameTime);
			
		}

        private void readyToExplode()
        {
            if (this.level == 1)
            {
                this.ExplosionCountdown = Static.EXPLODE_ENEMY_TIMER_1;
                this.Freeze(Static.EXPLODE_ENEMY_TIMER_1);
                this.AddAnimation(new PulseAnimation(this, Static.EXPLODE_ENEMY_TIMER_1));
            }
            else if (this.level == 2)
            {
                this.ExplosionCountdown = Static.EXPLODE_ENEMY_TIMER_2;
                this.Freeze(Static.EXPLODE_ENEMY_TIMER_2);
                this.AddAnimation(new PulseAnimation(this, Static.EXPLODE_ENEMY_TIMER_2));
            }
            this.Stop();
            this.readyExplode = true;
        }
        private void Explode()
        {
            setRemove(true);
            this.OnDie();
            int explosionWidth = 80;
            int explosionHeight = 80;
            Rectangle expBounds = new Rectangle((int)(getCenterX() - explosionWidth / 2), (int)(getCenterY() - explosionWidth / 2), explosionWidth, explosionHeight);
            if (level == 1)
            {
                game.Spawn(new AOECone(game, Seizonsha.spriteMappings[Static.SPRITE_FIREBALL], null, expBounds, Static.EXPLODE_ENEMY_EXPLOSION_DAMAGE_1, Static.DAMAGE_TYPE_BAD, 10, 1f), expBounds.Left, expBounds.Top);

            }
            else if (level == 2)
            {
                game.Spawn(new AOECone(game, Seizonsha.spriteMappings[Static.SPRITE_FIREBALL], null, expBounds, Static.EXPLODE_ENEMY_EXPLOSION_DAMAGE_2, Static.DAMAGE_TYPE_BAD, 10, 1f), expBounds.Left, expBounds.Top);

            }
            exploded = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);

            if (this.level == 2)
            {
                Color equipColor = Color.White;
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_FEET], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_PANTS], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_GLOVES], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_ARMS_SHOULDER], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_PLATE_ARMOR_TORSO], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
                //spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_HEAD], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
            }
        }

		public override void collide(GameEntity entity)
		{
		}


		public override void collideWithWall()
		{
		}




        protected override void OnDie()
        {
            base.OnDie();
            game.orcDeathSound.Play();
            double rand = random.NextDouble();

            if (exploded)
            {
                return;
            }

            if (rand < .95)
            {
                game.Spawn(new WeaponDrop(game, Static.PIXEL_THIN, 20, 20, new RustyShank(game, this)), x, y);

            }
            else if (rand < .99)
            {
                game.Spawn(new Food(game, "Chicken Nuggets", Static.PIXEL_THIN, 20), x, y);
            }
            else
            {
                game.Spawn(new WeaponDrop(game, Static.PIXEL_THIN, 20, 20, new OKGun(game, this)), x, y);
            }
        }

        public override string getName()
        {
            return Static.TYPE_EXPLODE_ENEMY;

        }

        public void reset(int level)
        {
            if (level == 1)
            {
                base.reset(Static.EXPLODE_ENEMY_XP_1, Static.BASIC_ENEMY_SPEED_1);
            }
            else
            {
                base.reset(Static.EXPLODE_ENEMY_XP_2, Static.EXPLODE_ENEMY_SPEED_2);

            }
            init(level);
        }

    }
}
