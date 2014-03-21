using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.Effects;

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
        private bool hasHealth;
        public int velocityX { get; set; }
        public int velocityY { get; set; }
        public int accelX { get; set; }
        public int accelY { get; set; }
        protected Seizonsha game;
        public Texture2D sprite { get; set; }
        public Color color { get; set; } //delete when we have actual sprites
        private int frozen; // stop entity from moving for a period of time

        protected Rectangle? source = null;
        protected float scale = 1.0f;

        //TODO: create depth variable that Game object can sort entities by to determine which to draw first (so effects can go on top etc)

		// ALEX: new variables for mouse aim bullets and sprite rotation
		public Vector2 alexDirection {get ; set; }
		//-ALEX


        virtual public void Draw(SpriteBatch spriteBatch)
        {

            if (sprite == null)
            {
                return;
            }

            if(this.hasHealth){
                int barWidth = 60;
                //Debug.WriteLine(this.health);
                double green = ((double)this.health/(double)this.maxHealth) * barWidth;
                Rectangle greenCoordinates = new Rectangle(this.getCenterX() - (barWidth/2), this.y - 20, (int)green, 5);
                Rectangle redCoordinates = new Rectangle(this.getCenterX() - (barWidth/2), this.y - 20, barWidth, 5);
			    // spriteBatch.Draw(sprite, hitbox, color);
                Texture2D whiteRectangle = new Texture2D(game.GraphicsDevice, 1, 1);
                whiteRectangle.SetData(new[] { Color.White });
                spriteBatch.Draw(whiteRectangle, redCoordinates, Color.Red);
                spriteBatch.Draw(whiteRectangle, greenCoordinates, Color.Green);
            }

            if (source == null)
            {
                spriteBatch.Draw(sprite, this.hitbox, source, color, this.direction, new Vector2(0, 0), SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(sprite, this.hitbox, source, color, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
            }

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
            if(targetType!=Static.TARGET_TYPE_NOT_DAMAGEABLE)
            {
                this.hasHealth = true;
            }
            this.remove = false;
            this.hitbox = new Rectangle(x, y, width, height);
            this.velocityX = 0;
            this.velocityY = 0;
            this.game = game;
            this.sprite = sprite;
            this.rotateToAngle(0);
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
            alexDirection = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));

        }

        public void rotate(float angle)
        {
            this.direction += angle;
            alexDirection = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));

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
                TextEffect text = new TextEffect(this.game, amount +"", 10, this.getCenterX(), this.getCenterY()-60, Color.Red );
                game.Spawn(text);
            }
        }

        public void heal(int amount)
        {
            
            incHealth(amount);
            TextEffect text = new TextEffect(this.game, amount + "", 10, this.getCenterX(), this.getCenterY() - 60, Color.Green);
            game.Spawn(text);
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
