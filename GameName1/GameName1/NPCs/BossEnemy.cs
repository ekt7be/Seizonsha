﻿using GameName1.Interfaces;
using GameName1.Skills;
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
		Tile enemyTile; 
		Tile playerTile; 


		double closestDistance;


		private static float elapsed;
		private float elapsed2;
		private static readonly float delay = 200f;

		private int currentFrame = 0;
		private Sword sword;
		private Gun gun; 

		HashSet<Tile> closed;
		HashSet<Tile> open;

		Random random; 

		private static readonly int UP_ANIMATION = 0;
		private static readonly int DOWN_ANIMATION = 2;
		private static readonly int LEFT_ANIMATION = 1;
		private static readonly int RIGHT_ANIMATION = 3;
		private static readonly int WALK_ANIMATION_FRAMES = 9;

		public BossEnemy(Seizonsha game)
			: base(game, Seizonsha.spriteMappings[Static.SPRITE_BASIC_ENEMY_INT], Static.BOSS_ENEMY_WIDTH-1, Static.BOSS_ENEMY_HEIGHT-1, 200, Static.BOSS_ENEMY_XP)
		{
			base.scale = 1.0f;


			sword = new Sword(game, this, 15, 20);
			sword.OnEquip();

			//gun = new Gun(game, this, 30, 10, 10f);
			gun = new Gun(game, this, 20, 25, 10f);

			gun.OnEquip();
		
			this.tint = Color.Black;
			this.defaultTint = Color.Black;  

		}

		float speed_x, speed_y;
		bool stop = false;

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

			// rotate to face player
			float playerDirection = (float)Math.Atan2(closest.y-this.y, closest.x - this.x);
			rotateToAngle(playerDirection);

			// attack with sword if in range
			//if (closestDistance < this.width*1.7*50)

			if (closestDistance < this.width*1.7)
				sword.Use();
			else {
				gun.Use();
			}


			// ** PATHFINDING ** 
			playerTile = game.getTileFromIndex(game.getTileIndexFromLeftEdgeX(closest.x), game.getTileIndexFromTopEdgeY(closest.y));
			enemyTile = game.getTileFromIndex(game.getTileIndexFromLeftEdgeX(this.x), game.getTileIndexFromTopEdgeY(this.y));

			// findPath(1, playerTile);
			// System.Console.WriteLine(timer + ": " + this.x + " " + this.y + " " + this.velocityX + " " + this.velocityY + " " + this.lastX + " " + " " + this.isMoving());

			if ((this.getLastMovement() == new Vector2(0, 0) && this.sinceLastPathFind >= Static.BASIC_ENEMY_PATH_REFRESH) || 
				path.Count == 0 || 
				this.sinceLastPathFind >= Static.BASIC_ENEMY_PATH_REFRESH) {
				if (playerTile.capacity < Math.Max(tilesWide, tilesHigh)) 
					findPath(1, randomTile(playerTile, 1), null); 
				else
					findPath(1, playerTile, closest); 
			}

			update_moving();
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

		public override void OnSpawn()
		{

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
			game.Spawn(new Food(game), x, y);

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



