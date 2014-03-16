using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public abstract class GameEntity : Spawnable
    {

        public int x { get; set; }
        public int y { get; set; }
        public float direction { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Rectangle hitbox;
        private bool remove;
        private bool collidable;
        public int velocityX { get; set; }
        public int velocityY { get; set; }
        public int accelX { get; set; }
        public int accelY { get; set; }
        protected Seizonsha game;
        public Texture2D sprite { get; set; }
        public Color color { get; set; } //delete when we have actual sprites

        //TODO: create depth variable that Game object can sort entities by to determine which to draw first (so effects can go on top etc)


        virtual public void Draw(SpriteBatch spriteBatch)
        {

              spriteBatch.Draw(sprite, hitbox, color);

        }

        public abstract void Update();

        public void UpdateAll()
        {
            Update();
            incVelocityX(accelX);
            incVelocityY(accelY);
            if (velocityX != 0 || velocityY != 0)
            {
                move(velocityX, velocityY);
            }
        }


        public GameEntity(Seizonsha game, Texture2D sprite, int x, int y, int width, int height)
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
            this.direction = 0;
            this.collidable = true;
            this.color = Color.White;

        }

        public abstract void OnSpawn();
        public abstract void collideWithWall();
        public abstract void collide(GameEntity entity);

        public void setCollidable(bool collidable)
        {
            game.BindEntityToTiles(this, collidable);
            this.collidable = collidable;
        }

        public bool isCollidable()
        {
            return this.collidable;
        }

        public bool OverlapsY(GameEntity entity)
        {
            return ((this.getBottomEdgeY() > entity.getTopEdgeY() && this.getBottomEdgeY() < entity.getBottomEdgeY())
                || (this.getTopEdgeY() < entity.getBottomEdgeY() && this.getTopEdgeY() > entity.getTopEdgeY()) 
                || (this.getTopEdgeY() >= entity.getTopEdgeY() && this.getBottomEdgeY() <= entity.getBottomEdgeY())
                || (this.getTopEdgeY() <= entity.getTopEdgeY() && this.getBottomEdgeY() >= entity.getBottomEdgeY()));
        }

        public bool OverlapsX(GameEntity entity)
        {
            return ((this.getRightEdgeX() > entity.getLeftEdgeX() && this.getRightEdgeX() < entity.getRightEdgeX())
                || (this.getLeftEdgeX() < entity.getRightEdgeX() && this.getLeftEdgeX() > entity.getLeftEdgeX())
                || (this.getLeftEdgeX() >= entity.getLeftEdgeX() && this.getRightEdgeX() <= entity.getRightEdgeX())
                || (this.getLeftEdgeX() <= entity.getLeftEdgeX() && this.getRightEdgeX() >= entity.getRightEdgeX()));

        }

        public void setRemove(bool remove)
        {
            this.remove = remove;
        }
        public bool shouldRemove()
        {
            return remove;
        }

        public void incVelocityX(int dVX)
        {
            velocityX += dVX;
        }

        public void incVelocityY(int dVY)
        {
            velocityY += dVY;
        }

        public void incAccelX(int dAX)
        {
            this.accelX += dAX;
        }

        public void incAccelY(int dAY)
        {
            this.accelY += dAY;
        }

        public void move(int dx, int dy)
        {
            game.moveGameEntity(this, dx, dy);
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
