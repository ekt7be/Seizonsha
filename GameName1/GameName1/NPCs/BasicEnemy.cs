using GameName1.Interfaces;
using GameName1.Skills;
using GameName1.Skills.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.NPCs
{

	class BasicEnemy : Enemy, AI
	{

        //float elapsed;
		double closestDistance;





		//private int currentFrame = 0;
		private Sword sword;



        private GameEntity closest;



        private int level;


		public BasicEnemy(Seizonsha game, int level)
			: base(game, Seizonsha.spriteMappings[Static.SPRITE_BASIC_ENEMY_INT], Static.BASIC_ENEMY_WIDTH-1, Static.BASIC_ENEMY_HEIGHT-1, Static.BASIC_ENEMY_HEALTH_1, Static.BASIC_ENEMY_SPEED_1, Static.BASIC_ENEMY_XP_1)
		{
			base.scale = Static.BASIC_ENEMY_SPRITE_SCALE;
            init(level);
		}


        public void init(int level)
        {

            this.level = level;
            if (level == 1)
            {
                sword = new Sword(game, this, Static.BASIC_ENEMY_DAMAGE_1, Static.BASIC_ENEMY_EXTRA_ATTACK_RECHARGE_1, 1, "Skeleton sword 1", Color.White);
                sword.OnEquip();
                this.speed = Static.BASIC_ENEMY_SPEED_1;
                this.maxHealth = Static.BASIC_ENEMY_HEALTH_1;
                this.health = maxHealth;

            }
            else if (level == 2)
            {

                sword = new Sword(game, this, Static.BASIC_ENEMY_DAMAGE_2, Static.BASIC_ENEMY_EXTRA_ATTACK_RECHARGE_2, 1, "Skeleton sword 2", Color.White);
                sword.OnEquip();
                this.speed = Static.BASIC_ENEMY_SPEED_2;
               // this.defaultTint = Color.Green;
               // setDefaultTint();
                this.maxHealth = Static.BASIC_ENEMY_HEALTH_2;
                this.health = maxHealth;
            }
            else if (level == 3)
            {
                sword = new Sword(game, this, Static.BASIC_ENEMY_DAMAGE_3, Static.BASIC_ENEMY_EXTRA_ATTACK_RECHARGE_3, 1, "Skeleton sword 3", Color.White);
                sword.OnEquip();
                this.speed = Static.BASIC_ENEMY_SPEED_3;
                // this.defaultTint = Color.Green;
                // setDefaultTint();
                this.maxHealth = Static.BASIC_ENEMY_HEALTH_3;
                this.health = maxHealth;
            }
        }




		public override void AI(GameTime gameTime)
		{
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

			if (closest == null)
				return;

            setTarget(closest);


			// attack with sword if in range
            if (closestDistance < this.width * 1.2)
            {
                sword.Use();
            }

            base.AI(gameTime);
			
		}

		public override void collide(GameEntity entity)
		{
		}


		public override void collideWithWall()
		{
		}


	

		public override void Draw(SpriteBatch spriteBatch)
		{

			base.Draw(spriteBatch);
            sword.Draw(spriteBatch);

            Color equipColor = Color.White;

            if (this.level == 2)
            {
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_FEET], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_PANTS], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_GLOVES], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_ARMS_SHOULDER], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_TORSO], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_HEAD], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
            } else if (this.level == 3){
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_FEET], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_PANTS], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_GLOVES], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_ARMS_SHOULDER], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_TORSO], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
                //spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_HEAD], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, this.depth);
             }
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			sword.Update();

            /*
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
             * 
             * */
				
		}

		protected override void OnDie()
		{
            base.OnDie();
            double rand = random.NextDouble();


            if (level == 1)
            {
                if (rand < .3)
                {
                    game.Spawn(new WeaponDrop(game, Seizonsha.spriteMappings[Static.SPRITE_SWORD_DROP], 20, 20, new RustyShank(game, this)), x, y);

                }
                else if (rand < .2)
                {
                    game.Spawn(new Food(game, "Apple", Seizonsha.spriteMappings[Static.SPRITE_APPLE_DROP], 20), x, y);
                }
                else if (rand < .02) 
                {
                    game.Spawn(new WeaponDrop(game, Seizonsha.spriteMappings[Static.SPRITE_BOW_DROP], 20, 20, new OKGun(game, this)), x, y);
                }
            }
            else if (level == 2)
            {
                if (rand < .2)
                {
                    game.Spawn(new WeaponDrop(game, Seizonsha.spriteMappings[Static.SPRITE_SWORD_DROP], 20, 20, new RustyShank(game, this)), x, y);

                }
                else if (rand < .1)
                {
                    game.Spawn(new Food(game, "Pineapple", Seizonsha.spriteMappings[Static.SPRITE_PINEAPPLE_DROP], 50), x, y);
                }
                else if (rand < .02) 
                {
                    game.Spawn(new WeaponDrop(game, Seizonsha.spriteMappings[Static.SPRITE_BOW_DROP], 20, 20, new OKGun(game, this)), x, y);
                }
            }
            else if (level == 3)
            {
                if (rand < .2)
                {
                    game.Spawn(new WeaponDrop(game, Seizonsha.spriteMappings[Static.SPRITE_SWORD_DROP], 20, 20, new RustyShank(game, this)), x, y);

                }
                else if (rand < .1)
                {
                    game.Spawn(new Food(game, "Steak", Seizonsha.spriteMappings[Static.SPRITE_MEAT_DROP], 100), x, y);
                }
                else if (rand < .02)
                {
                    game.Spawn(new WeaponDrop(game, Seizonsha.spriteMappings[Static.SPRITE_BOW_DROP], 20, 20, new OKGun(game, this)), x, y);
                }
            }
		}


        public override string getName()
        {
            return Static.TYPE_BASIC_ENEMY;

        }

        public void reset(int level)
        {
            if (level == 1)
            {
                base.reset(Static.BASIC_ENEMY_XP_1, Static.BASIC_ENEMY_SPEED_1);
            }
            else if (level == 2)
            {
                base.reset(Static.BASIC_ENEMY_XP_2, Static.BASIC_ENEMY_SPEED_2);

            }
            else if (level == 3)
            {
                base.reset(Static.BASIC_ENEMY_XP_3, Static.BASIC_ENEMY_SPEED_3);

            }
            sword.OnUnequip();
            init(level);
        }

    }
}



