using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.Effects;
using GameName1.AnimationTesting;

namespace GameName1
{
    public abstract class GameEntity : Spawnable
    {

        public Vector2 movement;
        public Vector2 lastMovement;

        public int x { get; set; }
        public int y { get; set; }
        protected int xpReward;
        public float floatx { get; set; }
        public float floaty { get; set; }
        public float direction { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Rectangle hitbox;
        protected bool remove;
        protected bool collidable;
        protected int targetType;
        public int maxHealth;
        public int health;
        protected bool hasHealth;
        public float velocityX { get; set; }
        public float velocityY { get; set; }
        public int accelX { get; set; }
        public int accelY { get; set; }
        protected Seizonsha game;
        public Texture2D sprite { get; set; }

        protected int frozen; // stop entity from moving for a period of time
        protected List<StatusEffect> statusEffects;
        protected List<StatusEffect> incomingStatusEffects;
        protected List<StatusEffect> outgoingStatusEffects;

        public Color tint { get; set; }
        protected Color defaultTint;
        private List<Animation> animations;
        private List<Animation> outgoingAnimations;
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
                spriteBatch.Draw(sprite, this.hitbox, spriteSource, tint, this.direction, new Vector2(0, 0), SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(sprite, this.hitbox, spriteSource, tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
            }

        }

        public void setTint(Color tint)
        {
            this.tint = tint;
        }

        public void setDefaultTint()
        {
            this.tint = defaultTint;
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (StatusEffect statusEffect in this.outgoingStatusEffects)
            {
                this.statusEffects.Remove(statusEffect);
            }
            this.outgoingStatusEffects.Clear();
            foreach (StatusEffect statusEffect in this.incomingStatusEffects)
            {
                this.statusEffects.Add(statusEffect);
            }
            this.incomingStatusEffects.Clear();
            foreach (StatusEffect statusEffect in this.statusEffects)
            {
                statusEffect.Update();
            }
        }

        public void UpdateAll(GameTime gameTime)
        {

            Update(gameTime);

            //reset movement
            movement.X = 0;
            movement.Y = 0;


            incVelocityX(accelX);
            incVelocityY(accelY);



            if (frozen > 0)
            {
                frozen--;
            }
            else
            {
                this.move(velocityX, velocityY);
            }
        }

        public virtual void UpdateAnimation()
        {
            foreach (Animation animation in animations)
            {
                animation.Update();
                if (animation.shouldRemove())
                {
                    RemoveAnimation(animation);
                }
            }
        }

        public void AddAnimation(Animation animation)
        {
            animations.Add(animation);
        }

        public void RemoveAnimation(Animation animation)
        {
            outgoingAnimations.Add(animation);
        }

        public void ClearAnimations()
        {
            foreach (Animation animation in outgoingAnimations)
            {
                animation.OnRemove(this);
                animations.Remove(animation);
            }

            outgoingAnimations.Clear();
        }



        public GameEntity(Seizonsha game, Texture2D sprite, int width, int height, int targetType, int maxHealth)
        {

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
            this.velocityX = 0;
            this.velocityY = 0;
            this.movement = new Vector2(0, 0);
            this.lastMovement = new Vector2(0, 0);
            this.game = game;
            this.sprite = sprite;
            this.collidable = true;
            this.tint = Color.White;
            this.defaultTint = tint;
            this.frozen = 0;
            this.xpReward = 0;
            this.statusEffects = new List<StatusEffect>();
            this.incomingStatusEffects = new List<StatusEffect>();
            this.outgoingStatusEffects = new List<StatusEffect>();

            this.animations = new List<Animation>();
            this.outgoingAnimations = new List<Animation>();


            this.FramesToAnimation = new Dictionary<int, Rectangle>(); //for animations


        }

        protected abstract void OnDie();

        public void OnSpawnAll(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.floatx = x;
            this.floaty = y;
            this.hitbox = new Rectangle(x, y, width, height);
            this.rotateToAngle(0);
            OnSpawn();
        }

        public abstract void OnSpawn();
        public abstract void collideWithWall();
        public abstract void collide(GameEntity entity);
        virtual public bool shouldCollide(GameEntity entity) //if collision flag is on, control over collision can be more specific.  for instance, bullets should not collide with other bullets
        {
            return collidable;
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

        public void addStatusEffect(StatusEffect statusEffect)
        {
            this.incomingStatusEffects.Add(statusEffect);
        }

        public void removeStatusEffect(StatusEffect statusEffect)
        {
            this.outgoingStatusEffects.Add(statusEffect);
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
        public void move(float dx, float dy)
        {
            if (isFrozen())
            {
                return;
            }

                movement.X += dx;
                movement.Y += dy;
        }

        public void executeMovement(){
            game.moveGameEntity(this, movement.X, movement.Y);

        }

        public Vector2 getLastMovement()
        {
            return lastMovement;
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
        public float getLeftEdgeFloatX()
        {
            return floatx;
        }


        public int getRightEdgeX()
        {
            return hitbox.Right;
        }

        public float getRightEdgeFloatX()
        {
            return floatx + width;
        }


        public int getTopEdgeY()
        {
            return hitbox.Top;
        }

        public float getTopEdgeFloatY()
        {
            return floaty;
        }

        public int getBottomEdgeY()
        {
            return hitbox.Bottom;
        }

        public float getBottomEdgeFloatY()
        {
            return floaty + height;
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

        virtual public void reset()
        {

            this.health = maxHealth;
            this.remove = false;
            this.velocityX = 0;
            this.velocityY = 0;
            this.collidable = true;
            this.tint = Color.White;
            this.frozen = 0;
            this.xpReward = 0;
            this.statusEffects.Clear();
            this.incomingStatusEffects.Clear();
            this.outgoingStatusEffects.Clear();
            this.outgoingAnimations.Clear();
            this.animations.Clear();
            this.scale = 1.0f;
        }

        public void setSprite(Texture2D sprite)
        {
            this.sprite = sprite;
        }

       public abstract String getName();


    }
}
