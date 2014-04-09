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

    abstract class Enemy : GameEntity, AI
    {
        protected List<Tile> path;

        protected int tilesWide;
        protected int tilesHigh;

        protected bool drawPath = false;
        protected bool drawHitbox = false;

        protected bool stopped;


        protected float sinceLastPathFind;

        protected GameEntity target;

        HashSet<Tile> closed;
        HashSet<Tile> open;

        Random random;



        public Enemy(Seizonsha game, Texture2D sprite, int width, int height, int health, int XPReward)
            : base(game, sprite, width, height, Static.DAMAGE_TYPE_BAD, health)
        {
            setXPReward(XPReward);
            closed = new HashSet<Tile>();
            open = new HashSet<Tile>();

            path = new List<Tile>();

            random = new Random();

            tilesWide = (int)Math.Floor((double)(width / Static.TILE_WIDTH)) + 1;
            tilesHigh = (int)Math.Floor((double)(height / Static.TILE_WIDTH)) + 1;
            this.stopped = false;
        }

        float speed_x, speed_y;
        bool stop = false;

        private void next_goal()
        {
            path.RemoveAt(0);

            if (path.Count == 0)
            {
                return;
            }


        }


        public void Stop()
        {
            stopped = true;
        }

        public void Resume()
        {
            stopped = false;
        }
        public void update_moving()
        {
            if (path.Count == 0)
                stop = true;
            else
                stop = false;

            if (stop)
                return;

            if (path.Count != 0)
            {
                speed_x = (path[0].x - this.x);
                speed_y = (path[0].y - this.y);

                //speed_x = (path[0].x - this.getLeftEdgeX());
                //speed_y = (path[0].y - this.getTopEdgeFloatY());

                if (speed_x != 0 || speed_y != 0)
                {
                    Vector2 allSpeed = new Vector2(speed_x, speed_y);

                    allSpeed.Normalize();
                    allSpeed = allSpeed * Static.BASIC_ENEMY_SPEED;
                    //System.Console.Write(" | vec speed: " + allSpeed.X + " " + allSpeed.Y + "\n"); 

                    this.move(allSpeed.X, allSpeed.Y);
                }
            }

            //System.Console.Write("next goal: " + path[0].x + " " + path[0].y); 

            if (path.Count > 0)
            {
                if (game.getTileFromIndex(game.getTileIndexFromLeftEdgeX(getLeftEdgeX()), game.getTileIndexFromTopEdgeY(getTopEdgeY())) == path[0])
                    //if ( Math.Abs ( (this.floatx + this.floaty) - (path[0].x + path[0].y)) <= 1 ) 
                    next_goal();
            }
        }

        public void setTarget(GameEntity target)
        {
            this.target = target;
        }

        public virtual void AI(GameTime gameTime)
        {
            if (target == null)
            {
                return;
            }
            else
            {
                //face target
                float targetDirection = (float)Math.Atan2(target.y - this.y, target.x - this.x);
                rotateToAngle(targetDirection);


                //pathfinding

                if (!stopped)
                {
                    Tile targetTile = game.getTileFromIndex(game.getTileIndexFromLeftEdgeX(target.x), game.getTileIndexFromTopEdgeY(target.y));

                    if ((this.getLastMovement() == new Vector2(0, 0) && this.sinceLastPathFind >= Static.BASIC_ENEMY_PATH_REFRESH) ||
                        path.Count == 0 ||
                        this.sinceLastPathFind >= Static.BASIC_ENEMY_PATH_REFRESH)
                    {
                        if (targetTile.capacity < Math.Max(tilesWide, tilesHigh))
                            findPath(1, randomTile(targetTile, 1));
                        else
                            findPath(1, targetTile);
                    }

                    update_moving();
                }
            }
        }

        public void findPath(int searchMethod, Tile targetTile)
        {
            this.sinceLastPathFind = 0;

            if (targetTile == null)
                return;

            if (path != null || path.Count > 0)
            {
                path.Clear();
            }

            Tile selected = game.getTileFromIndex(game.getTileIndexFromLeftEdgeX(getLeftEdgeX()), game.getTileIndexFromTopEdgeY(getTopEdgeY())); ;	// start at enemy's current tile 
            Tile start = selected;



            open.Add(start);

            while (!closed.Contains(targetTile))
            {
                Tile tileAtThisDir;

                // check squares in all 8 directions 
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {

                        bool currentTile = (x == 0 && y == 0);
                        bool isDiagonal = (x != 0 && y != 0);

                        bool search = !currentTile && !isDiagonal;

                        tileAtThisDir = game.getTileFromIndex(selected.xIndex + x, selected.yIndex + y);

                        if (tileAtThisDir == null)
                            continue;

                        switch (searchMethod)
                        {
                            case 0: search = !currentTile && !isDiagonal;	// horizontal and vertical only pathfinding
                                break;
                            case 1: search = !currentTile;	// includes diagonals in pathfinding
                                break;
                        }

                        bool checkSize = tileAtThisDir.capacity >= Math.Max(tilesHigh, tilesWide);

                        if (search)
                        {
                            int addG = 0;

                            if (!(y != 0) && (x != 0)) addG = 10;	// up, down, left, right
                            else addG = 14;	// diagonal

                            if (open.Contains(tileAtThisDir))
                            {
                                if (tileAtThisDir.G < (selected.G + addG))
                                {	// try to find better path with smaller G
                                    tileAtThisDir.parent = selected;
                                }
                            }

                            //bool willNotCollide = !(game.willCollide(this, tileAtThisDir.x, tileAtThisDir.y));
                           
                            bool willCollide = tileAtThisDir.isObstacle();

                            /*
                            //(!(tileAtThisDir.isObstacle())
                            if (targetEntity != null)
                            {
                                if (!(tileAtThisDir.isTouching(targetEntity)))
                                {
                                    willCollide = game.willCollide(this, tileAtThisDir.x, tileAtThisDir.y);
                                }
                            }
                             * */

                            if (!willCollide && !(closed.Contains(tileAtThisDir)))
                            {
                                tileAtThisDir.parent = selected;
                                tileAtThisDir.G = tileAtThisDir.parent.G + addG;
                                if (checkSize || tileAtThisDir == targetTile)
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

                foreach (Tile openTile in open)
                {
                    // manhattan distance
                    int moveX = Math.Abs(targetTile.xIndex - openTile.xIndex);	// calculate tiles in x to player
                    int moveY = Math.Abs(targetTile.yIndex - openTile.yIndex);	// calculate tiles in y to player

                    openTile.H = (moveX + moveY) * 10;
                    openTile.F = openTile.G + openTile.H;

                    // System.Console.WriteLine (t.xIndex + ", " + t.yIndex + " | G: " + t.G + " H: " + t.H + "(" + moveX + ", " + moveY + ")" + " F: " + t.F);
                    if (openTile.F <= minF)
                    {
                        minF = openTile.F;
                        minFTile = openTile;
                    }
                }


                if (minFTile != null)
                {
                    selected = minFTile;
                    closed.Add(minFTile);
                    open.Remove(minFTile);
                }
                else return;
            }

            open.Clear();
            closed.Clear();


            Tile end = targetTile;
            Tile current = end;

            while (current != start)
            {

                if (current.parent != null)
                {
                    current = current.parent;

                    if (path.Count > 100)
                    {
                        path.Reverse();
                        path.Add(targetTile);
                        return;
                    }
                    else
                    {
                        path.Add(current);
                    }
                }
                else return;
            }

            if (path.Count > 0)
                path.RemoveAt(path.Count - 1);

            path.Reverse();
            path.Add(targetTile);	// add target tile to path


            /* intelligence: recalculate the path when they get to this node 
             * path[path.Count-1] means travel the whole path before recalculating = dumber AI
             * */

            //this.currentDest = path[path.Count-1];
            //this.currentDest = path[path.Count/2];
        }

        public Tile randomTile(Tile target, int within)
        {
            int r = random.Next(1, 9);
            int rx = 0; int ry = 0;

            Tile tileAtThisDir;

            switch (r)
            {
                case 1: rx = -1; ry = -1; break;
                case 2: rx = 0; ry = -1; break;
                case 3: rx = 1; ry = -1; break;
                case 4: rx = -1; ry = 0; break;
                case 5: rx = 1; ry = 0; break;
                case 6: rx = -1; ry = 1; break;
                case 7: rx = 0; ry = 1; break;
                case 8: rx = 1; ry = 1; break;
            }

            r = random.Next(1, within + 1);		// clump together more, but more aggressive
            //r = r * within;						// makes them much less predictable

            tileAtThisDir = game.getTileFromIndex(target.xIndex + rx * r, target.yIndex + ry * r);

            if (tileAtThisDir == null)
                return null;

            if (!tileAtThisDir.isObstacle() && tileAtThisDir.capacity >= this.width / 32)
                return tileAtThisDir;
            else
                return null;

        }

        public abstract override void collide(GameEntity entity);




        public abstract override void collideWithWall();


        public abstract override void OnSpawn();

       
        public void DrawPath(SpriteBatch spriteBatch)
        {


            int dotSize = 6;

            if (path != null)
            {
                foreach (Tile t in path)
                {

                    Rectangle rect = new Rectangle(
                        t.x + Static.TILE_WIDTH / 2 - dotSize / 2,
                        t.y + Static.TILE_WIDTH / 2 - dotSize / 2,
                        dotSize,
                        dotSize);
                    spriteBatch.Draw(Static.PIXEL_THIN, rect, Color.LightPink);
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (drawHitbox)
            {
                Rectangle rect = new Rectangle(
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


            this.sinceLastPathFind += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        protected override void OnDie()
        {
            game.decreaseNumberEnemies();
        }

        public override void rotateToAngle(float angle)
        {
            base.rotateToAngle(angle);


        }

        public abstract override string getName();

        public virtual void reset(int XPReward)
        {
            base.reset();
            path.Clear();
            open.Clear();
            closed.Clear();
            setXPReward(XPReward);

        }

    }
}



