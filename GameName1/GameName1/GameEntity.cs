using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public abstract class GameEntity : GameObject
    {

        public int velocityX;
        public int velocityY;
        public int accelX;
        public int accelY;
        public Game1 game;
        public Texture2D sprite;
        public bool falling;


        public override void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(sprite, new Rectangle(x - game.cameraX, y, this.width, this.height), Color.Aquamarine);
            spriteBatch.Draw(sprite, hitbox, Color.Aquamarine);

        }

        public override void Update()
        {
            incVelocityX(accelX);
            incVelocityY(accelY);
            move(velocityX, velocityY);

        }


        public GameEntity(Game1 game, Texture2D sprite, int x, int y, int width, int height) : base(x, y, width, height)
        {
            this.velocityX = 0;
            this.velocityY = 0;
            this.game = game;
            this.sprite = sprite;
            this.accelY = Game1.gravity;
            this.falling = false;

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

            int tilesX = (int)(Math.Floor((float)Math.Abs(dx) / (float)Game1.TileWidth)+1);
            int tilesY = (int)(Math.Floor((float)Math.Abs(dy) / (float)Game1.TileHeight)+1);

            int leftEdgeX = this.x;
            int leftEdgeTile = (int)(Math.Floor((float)(leftEdgeX) / (float)(Game1.TileWidth)));

            int rightEdgeX = this.x + this.width;
            int rightEdgeTile = (int)(Math.Ceiling((float)(rightEdgeX )/(Game1.TileWidth))-1);

            int topEdgeY = this.y;
            int topEdgeTile = (int)(Math.Floor((float)(topEdgeY) / (float)(Game1.TileHeight))); 

            int bottomEdgeY = this.y + height;
            int bottomEdgeTile = (int)(Math.Ceiling((float)(bottomEdgeY) / (float)(Game1.TileHeight))-1);


            int distanceToTravelX = dx;
            int distanceToTravelY = dy;


            if (dx > 0)
            {
                //right


                int tilesToScanX = tilesX;

                if (rightEdgeTile + tilesToScanX > game.currLevel.GetTilesHorizontal() - 1)
                {
                    tilesToScanX = game.currLevel.GetTilesHorizontal() - 1 - rightEdgeTile;
                }

                if (tilesToScanX == 0) //right boundary?
                {
                    int distanceToBoundary = game.currLevel.GetTilesHorizontal() * Game1.TileWidth - rightEdgeX;
                    if (distanceToBoundary < distanceToTravelX)
                    {
                        distanceToTravelX = distanceToBoundary;
                    }
                }

                for (int i = 1; i <= tilesToScanX; i++)
                {
                    for (int j = topEdgeTile; j <= bottomEdgeTile; j++)
                    {
                        Tile currTile = game.currLevel.getTile(rightEdgeTile + i, j);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = currTile.x - rightEdgeX;
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

                int tilesToScanX = tilesX;

                if (leftEdgeTile - tilesToScanX < 0)
                {
                    tilesToScanX = leftEdgeTile;
                }

                if (tilesToScanX == 0) //near boundary 
                {

                    int distanceToBoundary = -1*leftEdgeX;
                    if (distanceToBoundary > distanceToTravelX)
                    {
                        distanceToTravelX = distanceToBoundary;
                    }
                }

                for (int i = 1; i <= tilesToScanX; i++)
                {
                    for (int j = topEdgeTile; j <= bottomEdgeTile; j++){
                        Tile currTile = game.currLevel.getTile(leftEdgeTile - i, j);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = (currTile.x + Game1.TileWidth) - leftEdgeX;
                            if (distanceToTile > distanceToTravelX)
                            {
                                distanceToTravelX = distanceToTile;
                            }
                        }
                    }

                }
            }

            if (dy > 0)
            { //down
                int tilesToScanY = tilesY;
                if (bottomEdgeTile + tilesToScanY > game.currLevel.GetTilesVertical() - 1)
                {
                    tilesToScanY = game.currLevel.GetTilesVertical() - 1 - bottomEdgeTile;
                }

                if (tilesToScanY == 0)
                { //bottom boundary
                    int distanceToBoundary = game.currLevel.GetTilesVertical() * Game1.TileHeight - bottomEdgeY;
                    if (distanceToBoundary < distanceToTravelX)
                    {
                        distanceToTravelY = distanceToBoundary;
                        if (distanceToBoundary == 0){
                            LandOnGround();
                        }
                    }
                }


                for (int i = 1; i <= tilesToScanY; i++)
                {
                    for (int j = leftEdgeTile; j <= rightEdgeTile; j++)
                    {
                        Tile currTile = game.currLevel.getTile(j, bottomEdgeTile + i);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = currTile.y - bottomEdgeY;
                            if (distanceToTile < distanceToTravelY)
                            {
                                distanceToTravelY = distanceToTile;
                                if (distanceToTile == 0)
                                {
                                    LandOnGround();
                                }
                            }
                        }
                    }
                }

            }
            else if (dy < 0)
            { //up
                int tilesToScanY = tilesY;
                if (topEdgeTile - tilesToScanY < 0)
                {
                    tilesToScanY = topEdgeTile;
                }

                if (tilesToScanY == 0)
                { //top boundary
                    int distanceToBoundary = -1 * topEdgeY;
                    if (distanceToBoundary > distanceToTravelY)
                    {
                        distanceToTravelY = distanceToBoundary;
                    }
                }

                for (int i = 1; i <= tilesToScanY; i++)
                {
                    for (int j = leftEdgeTile; j <= rightEdgeTile; j++)
                    {
                        Tile currTile = game.currLevel.getTile(j, topEdgeTile - i);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = (currTile.y + Game1.TileHeight) - topEdgeY;
                            if (distanceToTile > distanceToTravelY)
                            {
                                distanceToTravelY = distanceToTile;
                            }
                        }
                    }

                }

            }

            if (distanceToTravelY != 0)
            {
                falling = true;
            }

            this.x = this.x+distanceToTravelX;
            this.y = this.y + distanceToTravelY ;
            hitbox.Offset(distanceToTravelX,distanceToTravelY);
        }

        
        protected void moveTo(int x, int y)
        {
            this.x = x;
            this.y = y;
            hitbox.X = x;
            hitbox.Y = y;

        }

        private void LandOnGround()
        {
            this.velocityY = 0;
            this.falling = false;

        }

        public int getLeftEdgeTileIndex()
        {
            int leftEdgeX = this.x;
            return (int)(Math.Floor((float)(leftEdgeX) / (float)(Game1.TileWidth)));
        }

        public int getRightEdgeTileIndex()
        {
            int rightEdgeX = this.x + this.width;
            return (int)(Math.Ceiling((float)(rightEdgeX) / (Game1.TileWidth)) - 1);
        }

        public int getTopEdgeTileIndex()
        {
            int topEdgeY = this.y;
            return (int)(Math.Floor((float)(topEdgeY) / (float)(Game1.TileHeight)));

            
        }

        public int getBottomEdgeTileIndex()
        {
            int bottomEdgeY = this.y + height;
            return (int)(Math.Ceiling((float)(bottomEdgeY) / (float)(Game1.TileHeight)) - 1);
        }

        public Rectangle getHitbox()
        {
            return hitbox;
        }
    }
}
