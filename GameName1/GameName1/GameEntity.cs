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
        private int xpReward;
        public float direction { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Rectangle hitbox;
        private bool remove;
        private bool collidable;
        private int targetType;
        public int maxHealth;
        public int health;
        private bool hasHealth;
        public int velocityX { get; set; }
        public int velocityY { get; set; }
        public int accelX { get; set; }
        public int accelY { get; set; }
        protected Seizonsha game;
        public Texture2D sprite { get; set; }
        public Color color { get; set; } //delete when we have actual sprites
        private int frozen; // stop entity from moving for a period of time

        protected Dictionary<int, Rectangle> FramesToAnimation;
        protected Rectangle? spriteSource = null;
        protected float scale = 1.0f;

        //TODO: create depth variable that Game object can sort entities by to determine which to draw first (so effects can go on top etc)

		// ALEX: new variables for mouse aim bullets and sprite rotation
		public Vector2 vectorDirection {get ; set; }
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

            if (spriteSource == null)
            {
                spriteBatch.Draw(sprite, this.hitbox, spriteSource, color, this.direction, new Vector2(0, 0), SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(sprite, this.hitbox, spriteSource, color, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
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
            this.xpReward = 0;


            this.FramesToAnimation = new Dictionary<int, Rectangle>(); //for animations


        }

        protected abstract void OnDie();
        public abstract void OnSpawn();
        public abstract void collideWithWall();
        public abstract void collide(GameEntity entity);
        virtual public bool shouldCollide(GameEntity entity) //if collision flag is on, control over collision can be more specific.  for instance, bullets should not collide with other bullets
        {
            return true;
        }

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

        public virtual void rotateToAngle(float angle){
            this.direction = angle;
            vectorDirection = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));

        }

        public void rotate(float angle)
        {
            rotateToAngle(this.direction + angle);

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

        /*
        public bool damage(int amount, int damageType) // true if killed
        {
            if (game.ShouldDamage(damageType, targetType))
            {
                TextEffect text = new TextEffect(this.game, amount +"", 10, this.getCenterX(), this.getCenterY()-60, Color.Red );
                game.Spawn(text);
                return incHealth(-1 * amount);
            }
            return false;
        }

        public void heal(int amount)
        {
            
            incHealth(amount);
            TextEffect text = new TextEffect(this.game, amount + "", 10, this.getCenterX(), this.getCenterY() - 60, Color.Green);
            game.Spawn(text);
        }

        public bool incHealth(int amount){ //true if dead
            health += amount;
            if (health > maxHealth){
                health = maxHealth;
            }
            if (health < 0)
            {
                health = 0;
                die();
                return true;
            }
            return false;
        }
         * 
         */

        public void die()
        {
            OnDie();
            setRemove(true);
        }


        public virtual void OnKillOther(GameEntity entity)
        {
        }

        public virtual void OnDamageOther(GameEntity entity, int amount)
        {
        }

        public void setXPReward(int amount)
        {
            if (amount < 0)
            {
                amount = 0;
            }

            xpReward = amount;
        }

        public int getXPReward()
        {
            return xpReward;
        }



    }
}
