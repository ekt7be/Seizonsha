using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public abstract class GameEntity
    {

        public int x;
        public int y;
        public int width;
        public int height;
        protected Rectangle hitbox;
        public bool remove;
        public int velocityX;
        public int velocityY;
        public int accelX;
        public int accelY;
        public Game1 game;
        public Texture2D sprite;


        virtual public void Draw(SpriteBatch spriteBatch)
        {

              spriteBatch.Draw(sprite, hitbox, Color.Aquamarine);

        }

        virtual public void Update()
        {
            incVelocityX(accelX);
            incVelocityY(accelY);
            if (velocityX != 0 || velocityY != 0)
            {
                move(velocityX, velocityY);
            }

        }

        virtual public void onSpawn()
        {
            //When this entity is spawned into the game
        }


        public GameEntity(Game1 game, Texture2D sprite, int x, int y, int width, int height)
        {

            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.remove = false;
            this.hitbox = new Rectangle(x, y, width, height);
            this.velocityX = 0;
            this.velocityY = 0;
            this.game = game;
            this.sprite = sprite;

        }


        public void setRemove(bool remove)
        {
            this.remove = remove;
        }

        public void setVelocityX(int vX)
        {
            velocityX = vX;
        }

        public void incVelocityX(int dVX)
        {
            velocityX += dVX;
        }

        public void setVelocityY(int vY)
        {
            velocityY = vY;
        }

        public void incVelocityY(int dVY)
        {
            velocityY += dVY;
        }

        public void setAccelX(int aX)
        {
            this.accelX = aX;
        }

        public void incAccelX(int dAX)
        {
            this.accelX += dAX;
        }

        public void setAccelY(int aY)
        {
            this.accelY = aY;
        }

        public void incAccelY(int dAY)
        {
            this.accelY += dAY;
        }

        protected void move(int dx, int dy){

            int tilesX = (int)Math.Floor((float)Math.Abs(dx) / Static.TILE_WIDTH)+1;
            int tilesY = (int)Math.Floor((float)Math.Abs(dy) / Static.TILE_HEIGHT)+1;

            int leftEdgeTile = getLeftEdgeTileIndex();
            int rightEdgeTile = getRightEdgeTileIndex();
            int topEdgeTile = getTopEdgeTileIndex();
            int bottomEdgeTile = getBottomEdgeTileIndex();

            int distanceToTravelX = dx;
            int distanceToTravelY = dy;

            //find distance to level boundary in movement direction and see if it is less move amount
            //figure out how many tiles your movement translates to in each direction
            //scan tiles in front of you to find closest obstacle in that direction
            //final movement is min of original movement and distance to obstacle


            if (dx > 0)
            {
                //right

                int distanceToBoundary = game.currLevel.GetTilesHorizontal() * Static.TILE_WIDTH - getRightEdgeX();
                if (distanceToBoundary < distanceToTravelX)
                {
                    distanceToTravelX = distanceToBoundary;
                }

                int tilesToScanX = tilesX;

                if (rightEdgeTile + tilesToScanX > game.currLevel.GetTilesHorizontal() - 1)
                {
                    tilesToScanX = game.currLevel.GetTilesHorizontal() - 1 - rightEdgeTile;
                }


                for (int i = 1; i <= tilesToScanX; i++)
                {
                    for (int j = topEdgeTile; j <= bottomEdgeTile; j++)
                    {
                        Tile currTile = game.currLevel.getTile(rightEdgeTile + i, j);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = currTile.x - getRightEdgeX();
                            if (distanceToTile < distanceToTravelX)
                            {
                                distanceToTravelX = distanceToTile;
                            }
                        }
                    }
                }


            }else if (dx < 0)
            {
                //left


                int distanceToBoundary = -1 * getLeftEdgeX();
                if (distanceToBoundary > distanceToTravelX)
                {
                    distanceToTravelX = distanceToBoundary;
                }

                int tilesToScanX = tilesX;

                if (leftEdgeTile - tilesToScanX < 0)
                {
                    tilesToScanX = leftEdgeTile;
                }


                for (int i = 1; i <= tilesToScanX; i++)
                {
                    for (int j = topEdgeTile; j <= bottomEdgeTile; j++){
                        Tile currTile = game.currLevel.getTile(leftEdgeTile - i, j);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = (currTile.x + Static.TILE_WIDTH) - getLeftEdgeX();
                            if (distanceToTile > distanceToTravelX)
                            {
                                distanceToTravelX = distanceToTile;
                            }
                        }
                    }

                }
            }

            this.x = this.x + distanceToTravelX;
            hitbox.Offset(distanceToTravelX, 0);
            leftEdgeTile = getLeftEdgeTileIndex();
            rightEdgeTile = getRightEdgeTileIndex();


            if (dy > 0)
            { //down

                int distanceToBoundary = game.currLevel.GetTilesVertical() * Static.TILE_HEIGHT - getBottomEdgeY();
                if (distanceToBoundary < distanceToTravelY)
                {
                    distanceToTravelY = distanceToBoundary;
                }
              

                int tilesToScanY = tilesY;

                if (bottomEdgeTile + tilesToScanY > game.currLevel.GetTilesVertical() - 1)
                {
                    tilesToScanY = game.currLevel.GetTilesVertical() - 1 - bottomEdgeTile;
                }

                for (int i = 1; i <= tilesToScanY; i++)
                {
                    for (int j = leftEdgeTile; j <= rightEdgeTile; j++)
                    {
                        Tile currTile = game.currLevel.getTile(j, bottomEdgeTile + i);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = currTile.y - getBottomEdgeY();
                            if (distanceToTile < distanceToTravelY)
                            {
                                distanceToTravelY = distanceToTile;
                            }
                        }
                    }
                }

            }
            else if (dy < 0)
            { //up


                int distanceToBoundary = -1 * getTopEdgeY();
                if (distanceToBoundary > distanceToTravelY)
                {
                    distanceToTravelY = distanceToBoundary;
                }

                int tilesToScanY = tilesY;

                if (topEdgeTile - tilesToScanY < 0)
                {
                    tilesToScanY = topEdgeTile;
                }
                

                for (int i = 1; i <= tilesToScanY; i++)
                {
                    for (int j = leftEdgeTile; j <= rightEdgeTile; j++)
                    {
                        Tile currTile = game.currLevel.getTile(j, topEdgeTile - i);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = (currTile.y + Static.TILE_HEIGHT) - getTopEdgeY();
                            if (distanceToTile > distanceToTravelY)
                            {
                                distanceToTravelY = distanceToTile;
                            }
                        }
                    }

                }

            }

            this.y = this.y + distanceToTravelY; 
            hitbox.Offset(0, distanceToTravelY);

        }



        protected void moveTo(int x, int y)
        {
            this.x = x;
            this.y = y;
            hitbox.X = x;
            hitbox.Y = y;

        }


        public int getLeftEdgeTileIndex()
        {
            return (int)Math.Floor((float)getLeftEdgeX() / Static.TILE_WIDTH);
        }

        public int getLeftEdgeX()
        {
            return hitbox.Left;
        }


        public int getRightEdgeTileIndex()
        {

            return (int)Math.Ceiling(((float)getRightEdgeX() / Static.TILE_WIDTH)) - 1;
        }
        public int getRightEdgeX()
        {
            return hitbox.Right;
        }

        public int getTopEdgeTileIndex()
        {
            return (int)Math.Floor((float)getTopEdgeY() / Static.TILE_HEIGHT);
        }
        public int getTopEdgeY()
        {
            return hitbox.Top;
        }

        public int getBottomEdgeTileIndex()
        {
            return (int)Math.Ceiling(((float)getBottomEdgeY() / Static.TILE_HEIGHT)) - 1;
        }
        public int getBottomEdgeY()
        {
            return hitbox.Bottom;
        }

        public Rectangle getHitbox()
        {
            return hitbox;
        }
    }
}
