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
using GameName1.Skills;

namespace GameName1
{
    public abstract class GameEntity : Spawnable, IComparable<GameEntity>
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
        public Rectangle spriteBox;
        public Rectangle hitbox;
        protected bool remove;
        protected bool collidable;
        protected int targetType;
        public float maxHealth;
        public float health;
        public float shield;
        public float speedModifier;
        public float damageModifier;
        protected bool hasHealth;
        public float velocityX { get; set; }
        public float velocityY { get; set; }
        public int accelX { get; set; }
        public int accelY { get; set; }
        protected Seizonsha game;
        public Texture2D sprite { get; set; }
        public float scale = 1.0f;
        public float defaultScale = 1.0f;
        public float depth = .9f;


        private DamageAnimation damageAnimation;
        private CastAnimation castAnimation;

        protected int frozen; // stop entity from moving for a period of time
        protected List<StatusEffect> statusEffects;
        protected List<StatusEffect> incomingStatusEffects;
        protected List<StatusEffect> outgoingStatusEffects;

        public Color tint { get; set; }
        protected Color defaultTint;
        private List<Animation> animations;
        private List<Animation> outgoingAnimations;
        public Rectangle? spriteSource = null;



        //TODO: create depth variable that Game object can sort entities by to determine which to draw first (so effects can go on top etc)

		public Vector2 vectorDirection {get ; set; }


        virtual public void Draw(SpriteBatch spriteBatch)
        {

            if (sprite == null)
            {
                return;
            }

            if(this.hasHealth && this.health > 0){
                int barWidth = 60;
                //Debug.WriteLine(this.health);
                double green = ((double)this.health/(double)this.maxHealth) * barWidth;
                Rectangle greenCoordinates = new Rectangle(this.getCenterX() - (barWidth/2), this.y - 20, (int)green, 5);
                Rectangle redCoordinates = new Rectangle(this.getCenterX() - (barWidth/2), this.y - 20, barWidth, 5);
			    // spriteBatch.Draw(sprite, hitbox, color);

                spriteBatch.Draw(Static.PIXEL_THIN, redCoordinates, Color.Red);
                spriteBatch.Draw(Static.PIXEL_THIN, greenCoordinates, Color.Green);
            }

            spriteBox = hitbox;
            spriteBox.Inflate((int)((scale-1f) * hitbox.Width/2), (int)((scale-1f) * hitbox.Height/2));

            if (spriteSource == null)
            {
                spriteBatch.Draw(sprite, spriteBox, spriteSource, tint, this.direction, new Vector2(0, 0), SpriteEffects.None, depth);
            }
            else
            {
                spriteBatch.Draw(sprite, spriteBox, spriteSource, tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
            }

            foreach (StatusEffect s in statusEffects)
            {
                s.Draw(spriteBatch);
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
            int slowCount = 0;
            foreach (StatusEffect statusEffect in this.statusEffects)
            {
                statusEffect.Update();
                if (statusEffect is Slow) slowCount++;
            }
            if (slowCount == 0) this.speedModifier = 1f;
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

        public PolygonIntersection.Polygon getPolygonFromHitbox()
        {
            List<PolygonIntersection.Vector> points = new List<PolygonIntersection.Vector>();
            points.Add(new PolygonIntersection.Vector(this.x, this.y));
            points.Add(new PolygonIntersection.Vector(this.x+this.width, this.y));
            points.Add(new PolygonIntersection.Vector(this.x+this.width, this.y+this.height));
            points.Add(new PolygonIntersection.Vector(this.x+this.width, this.y+this.height));
            PolygonIntersection.Polygon ret = new PolygonIntersection.Polygon(points);
            return ret;
        }

        public void addShield(float amount)
        {
            this.shield += amount;
        }

        public void modifySpeed(float amount)
        {
            this.speedModifier*=amount;
        }

        public virtual void UpdateAnimation(GameTime gameTime)
        {
            foreach (Animation animation in animations)
            {
                animation.Update(gameTime);
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

        public bool HasAnimation(Animation animation)
        {
            return animations.Contains(animation);
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
            this.shield = 0;
            this.speedModifier = 1;

            this.animations = new List<Animation>();
            this.outgoingAnimations = new List<Animation>();
            this.damageAnimation = new DamageAnimation(this);



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
        public List<StatusEffect> getStatusEffects(){
            return this.statusEffects;
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
            if (isFrozen())
            {
                return;
            }
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
            //float dist = (float)Math.Sqrt(dx * dx + dy * dy);   
            game.moveGameEntity(this, movement.X * speedModifier, movement.Y*speedModifier);
            speedModifier = 1f;

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

        public float getHealth()
        {
            return health;
        }

        public int getTargetType()
        {
            return targetType;
        }

        public void makeDamageAnimation()
        {
            this.damageAnimation.reset(this);
            if (!this.animations.Contains(this.damageAnimation)){
                this.AddAnimation(damageAnimation);
            }
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
            this.defaultTint = Color.White;
            this.tint = Color.White;
            this.frozen = 0;
            this.xpReward = 0;
            this.statusEffects.Clear();
            this.incomingStatusEffects.Clear();
            this.outgoingStatusEffects.Clear();
            this.outgoingAnimations.Clear();
            this.animations.Clear();
            this.scale = 1.0f;
            this.shield = 0f;
            this.depth = 1f;
            this.damageAnimation.reset(this);
        }

        public void setSprite(Texture2D sprite)
        {
            this.sprite = sprite;
        }

       public abstract String getName();

       public void setWidth(int width)
       {
           this.width = width;
           this.hitbox = new Rectangle(x, y, width, height);
       }
       public void setHeight(int height)
       {
           this.height = height;
           this.hitbox = new Rectangle(x, y, width, height);
       }


       public int CompareTo(GameEntity y)
       {
           if (depth > y.depth)
           {
               return 1;
           }
           else if (depth < y.depth)
           {
               return -1;
           }
           else
           {
               return 0;
           }
       }
    }
}
