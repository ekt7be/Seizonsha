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

		private Sword sword;
		//private Gun gun; 
        private Fireball fireball;


		public BossEnemy(Seizonsha game)
			: base(game, Seizonsha.spriteMappings[Static.SPRITE_BASIC_ENEMY_INT], Static.BOSS_ENEMY_WIDTH-1, Static.BOSS_ENEMY_HEIGHT-1, 200, Static.BOSS_ENEMY_SPEED, Static.BOSS_ENEMY_XP)
		{
			base.scale = 1.0f;


			sword = new RustySword(game, this);
			sword.OnEquip();

            fireball = new Fireball(game, this, 30, 50, 10f);
			//gun = new Gun(game, this, 30, 10, 10f);
            /*
            gun = new OKGun(game, this);
            gun.setUnlimitedAmmo(true);
            gun.setTint(Color.Black);

			gun.OnEquip();
             * */
            fireball.OnEquip();
		
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
				//gun.Use();
                fireball.Use();
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
            //gun.Draw(spriteBatch);
        }


		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			sword.Update();
			//gun.Update();
            fireball.Update();

		}

		protected override void OnDie()
		{
            base.OnDie();
            double rand = random.NextDouble();
            if (rand < .1)
            {
                game.Spawn(new WeaponDrop(game, Seizonsha.spriteMappings[Static.SPRITE_BOW_DROP], 30, 30, new Revolver(game, this)), x + 40, y - 20);

            }
            else if (rand < .25)
            {
                game.Spawn(new WeaponDrop(game, Seizonsha.spriteMappings[Static.SPRITE_SWORD_DROP], 30, 30, new DankSword(game, this)), x + 10, y);
            }
            else
            {
                game.Spawn(new Food(game, "Tasty Steak", Seizonsha.spriteMappings[Static.SPRITE_MEAT_DROP], 100), x - 30, y - 30);
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



