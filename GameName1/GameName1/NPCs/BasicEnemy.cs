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

        HashSet<Tile> closed;
        HashSet<Tile> open;

		double closestDistance;
        int tilesWide;
        int tilesHigh;

		bool drawPath = false; 

		bool reachedDest; 
		Tile currentDest; 

		private static float elapsed, elapsed2;
		private static readonly float delay = 200f;
		private int currentFrame = 0;
		private Sword sword;


        Random random; 


		private static readonly int UP_ANIMATION = 0;
		private static readonly int DOWN_ANIMATION = 2;
		private static readonly int LEFT_ANIMATION = 1;
		private static readonly int RIGHT_ANIMATION = 3;
		private static readonly int WALK_ANIMATION_FRAMES = 9;


		public BasicEnemy(Seizonsha game)
			: base(game, Seizonsha.spriteMappings[Static.SPRITE_BASIC_ENEMY_INT], Static.BASIC_ENEMY_WIDTH, Static.BASIC_ENEMY_HEIGHT, Static.DAMAGE_TYPE_ENEMY, 200)
		{
			base.scale = 1.0f;
			setXPReward(50);

			reachedDest = true;

			sword = new Sword(game, this, 5, 20);
			sword.OnEquip();

            path = new List<Tile>();
            closed = new HashSet<Tile>();
            open = new HashSet<Tile>();
            random = new Random();

            tilesWide = (int)Math.Floor((double)(width / Static.TILE_WIDTH)) + 1;
            tilesHigh = (int)Math.Floor((double)(height / Static.TILE_HEIGHT)) + 1;
		}

		public void AI()
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

			// attack with sword if close enough
			if (closestDistance < this.width*1.7)
			{
				sword.Use();
			} 

			//**PATHFINDING** 
			playerTile = game.getTileFromCoord(closest.getCenterX(), closest.getCenterY());
			enemyTile = game.getTileFromCoord(this.getCenterX(), this.getCenterY());

			float speed = Static.BASIC_ENEMY_SPEED; 

			// findPath(1, playerTile);
			// System.Console.WriteLine(timer + ": " + this.x + " " + this.y + " " + this.velocityX + " " + this.velocityY + " " + this.lastX + " " + " " + this.isMoving());

            if (currentDest != null)
            {
                if (currentDest.isTouching(this))
                {
                    // System.Console.WriteLine("reached destination!");
                    this.reachedDest = true;
                    path.Clear();
                }
            }


			if (!reachedDest) {
				// System.Console.Write ("time to move!");
				Tile tileAtThisDir;

				if (path == null)
					return;

				if (path.Count > 0) {	// like pacman gobbling up nodes
					if (path[0].isTouching(this)) {
						// System.Console.WriteLine("removed a node!"); 
						path.RemoveAt(0);							
					}
				}

				float speedX = 0;
				float speedY = 0; 

				// move along a path
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {

						bool currentTile = (x == 0 && y == 0);
						bool isHorVert = (x == 0) || (y == 0); 
						bool isDiagonal = (x != 0 && y != 0);

						if (!currentTile) {

							tileAtThisDir = game.getTileFromIndex (enemyTile.xIndex + x, enemyTile.yIndex + y);

							if (path.Count > 0 && path != null) {

								// movement
								if (path [0].getCenterX() > this.getCenterX()) { // move right
									speedX = speed; 
									speedY = 0;
									this.move(speedX, speedY);
								}
								if (path [0].getCenterX() < this.getCenterX()) { // move left
									speedX = -speed; 
									speedY = 0; 
									this.move(speedX, speedY);

								}
								if (path [0].getCenterY() > this.getCenterY()) { // move up
									speedX = 0; 
									speedY = speed; 
									this.move(speedX, speedY);

								}
								if (path [0].getCenterY() < this.getCenterY()) { // move down
									speedX = 0; 
									speedY = -speed; 
									this.move(speedX, speedY);
								}

							}
						}

					}
				}


			}
			else {
				// System.Console.WriteLine("finding path...");
				findPath(1, playerTile); 
			}

			if (currentDest == null)
				return;
		}

		public void findPath(int searchMethod, Tile target) {
			if (target == null)
				return;

			if (path != null)
				path.Clear();

			reachedDest = false; 

			//HashSet<Tile> open = new HashSet<Tile>(); 
			//HashSet<Tile> closed = new HashSet<Tile>(); 

			Tile selected = enemyTile;	// start at enemy's current tile 
			Tile start = selected;

			//System.Console.WriteLine(game.getTileIndexFromLeftEdgeX(this.getCenterX()) +" "+ game.getTileIndexFromTopEdgeY(this.getCenterY()));
			//System.Console.WriteLine(selected.xIndex +":"+ selected.yIndex + " (start)"); 

			open.Add(selected); 

			// find closest player

			while (!closed.Contains(target)) {
				// check squares in all 8 directions 
				Tile tileAtThisDir;
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						bool currentTile = (x == 0 && y == 0);
						bool isDiagonal = (x != 0 && y != 0);
						bool search = !currentTile && !isDiagonal; 

						tileAtThisDir = game.getTileFromIndex(selected.xIndex + x, selected.yIndex + y); 
						if (tileAtThisDir == null)
							return;

						switch (searchMethod) {
							case 0: 
								search = !currentTile && !isDiagonal; 
								break;
							case 1: 
								search = !currentTile; 
								break;
						}

						if (search) {
							int addG = 0;

							if (!(y != 0) && (x != 0))	
								addG = 10;	// up, down, left, right
							else
								addG = 14;	// diagonal

							if (open.Contains(tileAtThisDir)) {
								if (tileAtThisDir.G < (selected.G + addG)) {	// try to find better path with smaller G
									tileAtThisDir.parent = selected; 
								}
							}




                            if (!(tileAtThisDir.isObstacle()) && !(closed.Contains(tileAtThisDir)))
                            {
                                tileAtThisDir.parent = selected;
                                tileAtThisDir.G = tileAtThisDir.parent.G + addG;
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

				foreach (Tile t in open) {
					int moveX = Math.Abs(playerTile.xIndex - t.xIndex);	// calculate tiles in x to player
					int moveY = Math.Abs(playerTile.yIndex - t.yIndex);	// calculate tiles in y to player

					t.H = (moveX + moveY) * 10; 
					t.F = t.G + t.H; 

					// System.Console.WriteLine (t.xIndex + ", " + t.yIndex + " | G: " + t.G + " H: " + t.H + "(" + moveX + ", " + moveY + ")" + " F: " + t.F);
					if (t.F <= minF) {
						minF = t.F; 
						minFTile = t; 
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
				else {
					//System.Console.WriteLine ("minFTile is null");
					return;
					//findPath (); 
				}
			}

			open.Clear ();
			closed.Clear (); 

			//System.Console.WriteLine ("debug2");

			Tile end = target;
			Tile current = end; 

			path.Clear(); 

			while (current != start) {
				//System.Console.WriteLine ("debug3");
				//System.Console.WriteLine("started at: " + start.xIndex + " " + start.yIndex);
				if (current.parent != null) {
					//System.Console.WriteLine("current is: " + current.xIndex + " " + current.yIndex + " with parent: " + current.parent.xIndex + " " + current.parent.yIndex + "\n");
					current = current.parent; 
					path.Add(current); 
				} 
				else
					return;
			}

			path.Reverse();
			path.Add(target);				// add end

			foreach(Tile t in path) {
				// System.Console.Write("(" + t.xIndex + ", " + t.yIndex + ") -> "); 
			}

			// System.Console.WriteLine(); 


			/* intelligence: recalculate the path when they get to this node 
			 * path[path.Count-1] means travel the whole path before recalculating = dumber AI
			 * */

			//this.currentDest = path[path.Count-1];
			this.currentDest = path[path.Count/2];

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

			if (!tileAtThisDir.isObstacle())
				return tileAtThisDir; 
			else 
				return null; 

			// System.Console.WriteLine(enemyTile.xIndex + rx + " " + enemyTile.yIndex);
		}

		public override void collide(GameEntity entity)
		{
			if (entity.getTargetType() == Static.TARGET_TYPE_ENEMY) {

				if (elapsed2 >= 1000) {
					findPath(1, randomTile(enemyTile, 3)); 
					elapsed2 = 0;
				}
				else {
					// if close enough, try to surround the player
					if (closestDistance < this.width*1.7*2)
						findPath(1, playerTile);
					else
						findPath(1, randomTile(playerTile, 3));
				}
			}
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


			int dotSize = 6; 

			if (path != null) {
				foreach (Tile t in path) {
					Rectangle rect = new Rectangle (t.x+Static.TILE_WIDTH/2-dotSize/2, 
						t.y+Static.TILE_HEIGHT/2-dotSize/2, 
						dotSize, dotSize); 
					spriteBatch.Draw(Static.PIXEL_THIN, rect, Color.LightPink);
				}
			}

		}

		public override void Draw(SpriteBatch spriteBatch)
		{
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
           reachedDest = true;
           path.Clear();
           open.Clear();
           closed.Clear(); 
        }
    }
}



