using GameName1.Interfaces;
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

	class BossEnemy : Enemy, AI
	{


		double closestDistance;


		private static float elapsed;
		private float elapsed2;
		private static readonly float delay = 200f;

		private int currentFrame = 0;
		private Sword sword;
		private Gun gun; 


		private static readonly int UP_ANIMATION = 0;
		private static readonly int DOWN_ANIMATION = 2;
		private static readonly int LEFT_ANIMATION = 1;
		private static readonly int RIGHT_ANIMATION = 3;
		private static readonly int WALK_ANIMATION_FRAMES = 9;

		public BossEnemy(Seizonsha game)
			: base(game, Seizonsha.spriteMappings[Static.SPRITE_BASIC_ENEMY_INT], Static.BOSS_ENEMY_WIDTH-1, Static.BOSS_ENEMY_HEIGHT-1, 200, Static.BOSS_ENEMY_XP)
		{
			base.scale = 1.0f;


			sword = new RustySword(game, this);
			sword.OnEquip();

			//gun = new Gun(game, this, 30, 10, 10f);
            gun = new OKGun(game, this);
            gun.setUnlimitedAmmo(true);
            gun.setTint(Color.Black);

			gun.OnEquip();
		
			this.tint = Color.Black;
			this.defaultTint = Color.Black;  

		}


		void next_goal() {
			path.RemoveAt(0);

			if(path.Count == 0) {
				return; 
			}
		}


		public override void AI(GameTime gameTime)
		{
			// find closest player
			List<Player> players = game.getPlayers();

			Player closest = null;
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
			//if (closestDistance < this.width*1.7*50)

			if (closestDistance < this.width*1.7)
				sword.Use();
			else {
				gun.Use();
			}

            base.AI(gameTime);

		}

		public override void collide(GameEntity entity)
		{

		}

		public override void UpdateAnimation(GameTime gameTime)
		{
			base.UpdateAnimation(gameTime);

			if (this.getLastMovement().X!=0 || this.getLastMovement().Y!=0)
			{


				if (Math.Cos(this.direction) > .5)
				{
					//spriteSource = FramesToAnimation[RIGHT_ANIMATION];
					base.spriteSource = new Rectangle(64 * currentFrame, RIGHT_ANIMATION * 64, 64, 64);

				}
				else if (Math.Sin(this.direction) > .5)
				{
					base.spriteSource = new Rectangle(64 * currentFrame, DOWN_ANIMATION * 64, 64, 64);

				}
				else if (Math.Sin(direction) < -.5)
				{
					//spriteSource = FramesToAnimation[UP_ANIMATION];
					base.spriteSource = new Rectangle(64 * currentFrame, UP_ANIMATION * 64, 64, 64);

				}
				else if (Math.Cos(direction) < -.5)
				{
					//spriteSource = FramesToAnimation[LEFT_ANIMATION];
					base.spriteSource = new Rectangle(64 * currentFrame, LEFT_ANIMATION * 64, 64, 64);      
				}


			}

			else
			{

				if (Math.Cos(this.direction) > .5)
				{
					//spriteSource = FramesToAnimation[RIGHT_ANIMATION];
					base.spriteSource = new Rectangle(64 * 0, RIGHT_ANIMATION * 64, 64, 64);

				}
				else if (Math.Sin(this.direction) > .5)
				{
					base.spriteSource = new Rectangle(64 * 0, DOWN_ANIMATION * 64, 64, 64);

				}
				else if (Math.Sin(direction) < -.5)
				{
					//spriteSource = FramesToAnimation[UP_ANIMATION];
					base.spriteSource = new Rectangle(64 * 0, UP_ANIMATION * 64, 64, 64);

				}
				else if (Math.Cos(direction) < -.5)
				{
					//spriteSource = FramesToAnimation[LEFT_ANIMATION];
					base.spriteSource = new Rectangle(64 * 0, LEFT_ANIMATION * 64, 64, 64);
				}

			}

		}

		public override void collideWithWall()
		{
		}

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
            sword.Draw(spriteBatch);
            gun.Draw(spriteBatch);
        }


		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			sword.Update();
			gun.Update();

			elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			elapsed2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;



			if (elapsed2 >= 10000) {
				elapsed2 = 0;			
			}

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

		}

		protected override void OnDie()
		{

            double rand = random.NextDouble();
            if (rand < .1)
            {
                game.Spawn(new WeaponDrop(game, Static.PIXEL_THIN, 30, 30, new Revolver(game, this)), x + 40, y - 20);

            }
            else if (rand < .25)
            {
                game.Spawn(new WeaponDrop(game, Static.PIXEL_THIN, 30, 30, new DankSword(game, this)), x + 10, y);
            }
            else
            {
                game.Spawn(new Food(game, "Entire Turkey", Static.PIXEL_THIN, 100), x - 30, y - 30);
            }

			game.decreaseNumberEnemies();
		}

		public override void rotateToAngle(float angle) //animation is based on rotation which is used by both movement and aiming
		{
			base.rotateToAngle(angle);


		}

		public override string getName()
		{
			return Static.TYPE_BOSS_ENEMY;
			//return Static.TYPE_BASIC_ENEMY;


		}

	}
}



