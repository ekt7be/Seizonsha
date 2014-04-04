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
		Tile enemyTile; 
		Tile playerTile; 
		List<Tile> path;

		double closestDistance;
        int tilesWide;
        int tilesHigh;

		bool drawPath = false;  
		bool drawHitbox = false; 

		private static float elapsed;
		private float elapsed2;
		private static readonly float delay = 200f;

		private float sinceLastPathFind;

		private int currentFrame = 0;
		private Sword sword;

		HashSet<Tile> closed;
		HashSet<Tile> open;

        Random random; 

		private static readonly int UP_ANIMATION = 0;
		private static readonly int DOWN_ANIMATION = 2;
		private static readonly int LEFT_ANIMATION = 1;
		private static readonly int RIGHT_ANIMATION = 3;
		private static readonly int WALK_ANIMATION_FRAMES = 9;


		public BasicEnemy(Seizonsha game)
			: base(game, Seizonsha.spriteMappings[Static.SPRITE_BASIC_ENEMY_INT], Static.BASIC_ENEMY_WIDTH-1, Static.BASIC_ENEMY_HEIGHT-1, Static.DAMAGE_TYPE_ENEMY, 200)
		{
			base.scale = 1.0f;
			setXPReward(50);

		  	closed = new HashSet<Tile>();
			open = new HashSet<Tile>();

			sword = new Sword(game, this, 5, 20);
			sword.OnEquip();

            path = new List<Tile>();

            random = new Random();

            tilesWide = (int)Math.Floor((double)(width / Static.TILE_WIDTH)) + 1;
            tilesHigh = (int)Math.Floor((double)(height / Static.TILE_WIDTH)) + 1;
		}

		float speed_x, speed_y;
		bool stop = false;

		void next_goal() {
			path.RemoveAt(0);

			if(path.Count == 0) {
				return; 
			}


		}

		void update_moving()
		{
			if(path.Count == 0)
				stop = true; 
			else 
				stop = false;
				
			if(stop)
				return;

			if (path.Count != 0) {
				speed_x = (path[0].x - this.x);
				speed_y = (path[0].y - this.y);

				if (speed_x!=0 || speed_y!=0){
					Vector2 allSpeed = new Vector2(speed_x,speed_y);

				allSpeed.Normalize();
					allSpeed = allSpeed * Static.BASIC_ENEMY_SPEED;
					//System.Console.Write(" | vec speed: " + allSpeed.X + " " + allSpeed.Y + "\n"); 

					this.move(allSpeed.X, allSpeed.Y);
				}
			}

				//System.Console.Write("next goal: " + path[0].x + " " + path[0].y); 

			if(path.Count > 0) {
				if (game.getTileFromIndex(game.getTileIndexFromLeftEdgeX(getLeftEdgeX()), game.getTileIndexFromTopEdgeY(getTopEdgeY())) == path[0])
					next_goal();
			}
		}


		public void AI(GameTime gameTime)
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
			if (closestDistance < this.width*1.7)
				sword.Use();
				
			// ** PATHFINDING ** 
			playerTile = game.getTileFromCoord(closest.x, closest.y);
			enemyTile = game.getTileFromCoord(this.x, this.y);

			// findPath(1, playerTile);
			// System.Console.WriteLine(timer + ": " + this.x + " " + this.y + " " + this.velocityX + " " + this.velocityY + " " + this.lastX + " " + " " + this.isMoving());

			if (this.getLastMovement() == new Vector2(0, 0) || 
				path.Count == 0 || 
				this.sinceLastPathFind >= Static.BASIC_ENEMY_PATH_REFRESH) {
					if (playerTile.capacity < Math.Max(tilesWide, tilesHigh)) 
						findPath(1, randomTile(playerTile, 1)); 
					else
						findPath(1, playerTile); 
			}
				 
			update_moving();
		}

		public void findPath(int searchMethod, Tile target) {
				//System.Console.WriteLine("finding path");

			this.sinceLastPathFind = 0;

			if (target == null)
				return;

			if (path != null || path.Count > 0)
				path.Clear();


			Tile selected = enemyTile;	// start at enemy's current tile 
			Tile start = selected;

				//System.Console.WriteLine(game.getTileIndexFromLeftEdgeX(this.getCenterX()) +" "+ game.getTileIndexFromTopEdgeY(this.getCenterY()));
				//System.Console.WriteLine(selected.xIndex +":"+ selected.yIndex + " (start)"); 

			open.Add(start); 

			while (!closed.Contains(target)) {
				Tile tileAtThisDir;

				// check squares in all 8 directions 
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {

						bool currentTile = (x == 0 && y == 0);
						bool isDiagonal = (x != 0 && y != 0);

						bool search = !currentTile && !isDiagonal; 

						tileAtThisDir = game.getTileFromIndex(selected.xIndex + x, selected.yIndex + y); 

						if (tileAtThisDir == null)
							continue;

						switch (searchMethod) {
							case 0: search = !currentTile && !isDiagonal;	// horizontal and vertical only pathfinding
									break;
							case 1: search = !currentTile;	// includes diagonals in pathfinding
									break;
						}

						bool checkSize = tileAtThisDir.capacity >= Math.Max(tilesHigh,tilesWide);

						if (search) {
							int addG = 0;

							if (!(y != 0) && (x != 0)) 	addG = 10;	// up, down, left, right
							else 						addG = 14;	// diagonal

							if (open.Contains(tileAtThisDir)) {
								if (tileAtThisDir.G < (selected.G + addG)) {	// try to find better path with smaller G
									tileAtThisDir.parent = selected; 
								}
							}

							//bool willNotCollide = !(game.willCollide(this, tileAtThisDir.x, tileAtThisDir.y));
							bool willCollide = tileAtThisDir.isObstacle();

							//(!(tileAtThisDir.isObstacle())
														
							if (!willCollide && !(closed.Contains(tileAtThisDir))) {
                                tileAtThisDir.parent = selected;
                                tileAtThisDir.G = tileAtThisDir.parent.G + addG;
								if (checkSize || tileAtThisDir == playerTile)
                        			open.Add(tileAtThisDir);
                            }
						}
					}
				}
					
				closed.Add(selected); 
				open.Remove(selected); 

				// find minF tile
				Tile minFTile = null; 
				int minF = 100000000; 

				foreach (Tile openTile in open) {
					// manhattan distance
					int moveX = Math.Abs(playerTile.xIndex - openTile.xIndex);	// calculate tiles in x to player
					int moveY = Math.Abs(playerTile.yIndex - openTile.yIndex);	// calculate tiles in y to player

					openTile.H = (moveX + moveY) * 10; 
					openTile.F = openTile.G + openTile.H; 

						// System.Console.WriteLine (t.xIndex + ", " + t.yIndex + " | G: " + t.G + " H: " + t.H + "(" + moveX + ", " + moveY + ")" + " F: " + t.F);
					if (openTile.F <= minF) {
						minF = openTile.F; 
						minFTile = openTile; 
					} 
				}

					//System.Console.WriteLine ("debug1");
					// System.Console.WriteLine ("__");

				if (minFTile != null) {
						// System.Console.WriteLine ("MinF: " + minF.xIndex + ", " + minF.yIndex + " | F: " + minF.F);
					selected = minFTile;
					closed.Add(minFTile); 
					open.Remove(minFTile); 
				} 
				else return;
			}

			open.Clear ();
			closed.Clear (); 

				//System.Console.WriteLine ("debug2");

			Tile end = target;
			Tile current = end; 

			while (current != start) {
					//System.Console.WriteLine ("debug3");
					//System.Console.WriteLine("started at: " + start.xIndex + " " + start.yIndex);
				if (current.parent != null) {
						//System.Console.WriteLine("current is: " + current.xIndex + " " + current.yIndex + " with parent: " + current.parent.xIndex + " " + current.parent.yIndex + "\n");
					current = current.parent; 
					//Static.Debug(path.Count+"");
					if (path.Count > 100) {
						path.Reverse();
						path.Add(target);
						return;
					}
					else {
						path.Add(current); 
					}
						// System.Console.Write("path node added - "); 
				} 
				else return;
			}

			if (path.Count > 0)
				path.RemoveAt(path.Count-1); 

			path.Reverse();
			path.Add(target);	// add player tile to path

			/*
			foreach(Tile t in path) {
				System.Console.Write("(" + t.xIndex + ", " + t.yIndex + ") -> "); 
			}
			*/

				// System.Console.WriteLine(); 

				/* intelligence: recalculate the path when they get to this node 
				 * path[path.Count-1] means travel the whole path before recalculating = dumber AI
				 * */

			//this.currentDest = path[path.Count-1];
			//this.currentDest = path[path.Count/2];
		}

		public Tile randomTile(Tile target, int within) {
			int r = random.Next(1, 9);
			int rx = 0; int ry = 0; 

			Tile tileAtThisDir;

			switch(r) {
				case 1: rx = -1; ry = -1; break;
				case 2: rx = 0; ry = -1; break;
				case 3: rx = 1; ry = -1; break;
				case 4: rx = -1; ry = 0; break;
				case 5: rx = 1; ry = 0; break;
				case 6: rx = -1; ry = 1; break;
				case 7: rx = 0; ry = 1; break;
				case 8: rx = 1; ry = 1; break; 
			} 

			r = random.Next(1, within+1);		// clump together more, but more aggressive
			//r = r * within;						// makes them much less predictable

			tileAtThisDir = game.getTileFromIndex(target.xIndex + rx * r, target.yIndex + ry * r); 

			if(tileAtThisDir == null) 
				return null; 

			if (!tileAtThisDir.isObstacle() && tileAtThisDir.capacity >= this.width/32)
				return tileAtThisDir; 
			else 
				return null; 

			// System.Console.WriteLine(enemyTile.xIndex + rx + " " + enemyTile.yIndex);
		}

		public override void collide(GameEntity entity)
		{
			/*
			if (entity.getTargetType() == Static.TARGET_TYPE_ENEMY) {

				if (elapsed2 >= 1000) {
					findPath(1, randomTile(enemyTile, 6)); 
				}
				else {
					// if close enough, try to surround the player
					findPath(1, randomTile(playerTile, 6));
				}
			}
			*/
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
			this.velocityX = 0;
			this.velocityY = 0;
		}

		void DrawPath(SpriteBatch spriteBatch) {


			int dotSize = 32; 

			if (path != null) {
				foreach (Tile t in path) {

					Rectangle rect = new Rectangle(
						t.x+Static.TILE_WIDTH/2-dotSize/2, 
						t.y+Static.TILE_WIDTH/2-dotSize/2, 
						dotSize, 
						dotSize); 
					spriteBatch.Draw(Static.PIXEL_THIN, rect, Color.LightPink);
				}
			}

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

			if (drawHitbox) {
				Rectangle rect = new Rectangle (
					this.x, 
					this.y, 
					this.hitbox.Width,
					this.hitbox.Height); 
				spriteBatch.Draw(Static.PIXEL_THIN, rect, Color.Blue);
			}


			if (drawPath) 
				DrawPath(spriteBatch); 
			base.Draw(spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			sword.Update();

			elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			elapsed2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

			this.sinceLastPathFind += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


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
			game.decreaseNumberEnemies();
		}

		public override void rotateToAngle(float angle) //animation is based on rotation which is used by both movement and aiming
		{
			base.rotateToAngle(angle);


		}

        public override string getName()
        {
            return Static.TYPE_BASIC_ENEMY;

        }

        public override void reset()
        {
           base.reset();
           setXPReward(50);
           path.Clear();
           open.Clear();
           closed.Clear(); 
        }
    }
}



