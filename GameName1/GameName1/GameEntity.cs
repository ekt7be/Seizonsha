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
        private int targetType;
        private int maxHealth;
        private int health;
        public int velocityX { get; set; }
        public int velocityY { get; set; }
        public int accelX { get; set; }
        public int accelY { get; set; }
        protected Seizonsha game;
        public Texture2D sprite { get; set; }
        public Color color { get; set; } //delete when we have actual sprites
        private int frozen; // stop entity from moving for a period of time

        //TODO: create depth variable that Game object can sort entities by to determine which to draw first (so effects can go on top etc)


        virtual public void Draw(SpriteBatch spriteBatch)
        {

            if (sprite == null)
            {
                return;
            }
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
            if (frozen > 0)
            {
                frozen--;
            }
        }


        public GameEntity(Seizonsha game, Texture2D sprite, int x, int y, int width, int height, int targetType, int maxHealth)
        {

            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.targetType = targetType;
            this.maxHealth = maxHealth;
            this.health = maxHealth;
            this.remove = false;
            this.hitbox = new Rectangle(x, y, width, height);
            this.velocityX = 0;
            this.velocityY = 0;
            this.game = game;
            this.sprite = sprite;
            this.direction = 0;
            this.collidable = true;
            this.color = Color.White;
            this.frozen = 0;

        }

        protected abstract void OnDie();
        public abstract void OnSpawn();
        public abstract void collideWithWall();
        public abstract void collide(GameEntity entity);

        public void setCollidable(bool collidable)
        {
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

        public void Freeze(int time)
        {
            frozen = time;
        }
        public bool isFrozen()
        {
            return frozen > 0;
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

        public void rotateToAngle(float angle){
            this.direction = angle;
        }

        public void rotate(float angle)
        {
            this.direction += angle;
        }

        public float getDirectionAngle()
        {
            return direction;
        }
        public void move(int dx, int dy)
        {
            if (isFrozen())
            {
                return;
            }
            game.moveGameEntity(this, dx, dy);
        }

        public int getCenterX()
        {
            return x + width / 2;
        }

        public int getCenterY()
        {
            return y + height / 2;
        }
        public int getLeftEdgeX()
        {
            return hitbox.Left;
        }


        public int getRightEdgeX()
        {
            return hitbox.Right;
        }

        public int getTopEdgeY()
        {
            return hitbox.Top;
        }

        public int getBottomEdgeY()
        {
            return hitbox.Bottom;
        }

        public Rectangle getHitbox()
        {
            return hitbox;
        }

        public int getHealth()
        {
            return health;
        }

        public int getTargetType()
        {
            return targetType;
        }
        public void damage(int amount, int damageType)
        {
            if ((targetType == Static.TARGET_TYPE_ENEMY && damageType == Static.TARGET_TYPE_FRIENDLY) 
                || (targetType == Static.TARGET_TYPE_FRIENDLY && damageType == Static.TARGET_TYPE_ENEMY)
                || (targetType != Static.TARGET_TYPE_NOT_DAMAGEABLE && damageType == Static.DAMAGE_TYPE_ALL)
                || (targetType == Static.TARGET_TYPE_ALL))
            {
                incHealth(-1 * amount);
            }
        }

        public void heal(int amount)
        {
            incHealth(amount);
        }


        public void incHealth(int amount){
            health += amount;
            if (health > maxHealth){
                health = maxHealth;
            }
            if (health < 0)
            {
                health = 0;
                die();
            }
        }

        public void die()
        {
            OnDie();
            setRemove(true);
        }



    }
}
