using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class Arrow : GameName1.Effects.Effect
    {
        private int damageType;
        private int amount;
        private GameEntity user;
        private Skill origin;
        private PolygonIntersection.Polygon polygon;
        private float pDirection;
        private bool shouldPierce;



        public Arrow(Seizonsha game, GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, PolygonIntersection.Polygon polygon, int amount, int damageType, int duration, Vector2 velocity, float direction, bool shouldPierce)
            : base(game, sprite, bounds.Width, bounds.Height, duration)
        {
            this.amount = amount;
            this.damageType = damageType;
            this.user = user;
            this.origin = origin;
            this.polygon = polygon;
            this.pDirection = direction;
            this.velocityX = velocity.X;
            this.velocityY = velocity.Y;
            this.shouldPierce = shouldPierce;
        }

        protected override void OnDie()
        {

        }

        public override void OnSpawn()
        {

                foreach (GameEntity entity in game.getEntitiesInBounds(this.polygon))
                {
                    this.origin.affect(entity);
                }
        }


        public override string getName()
        {
            return Static.TYPE_ARROW;
        }


        public void reset(GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, int duration)
        {
            base.reset(duration);
            this.user = user;
            setSprite(sprite);
            this.origin = origin;
            this.width = bounds.Width;
            this.height = bounds.Height;
            this.damageType = damageType;
            this.amount = amount;


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.rotateToAngle(pDirection);
            Static.Debug("" + pDirection);
            base.Draw(spriteBatch);
        }

        public override void UpdateAnimation(GameTime gameTime)
        {
            this.rotateToAngle(direction);
            base.UpdateAnimation(gameTime);
            this.rotateToAngle(direction);
        }

        public override void Update(GameTime gameTime)
        {
            polygon.Offset(new PolygonIntersection.Vector(this.velocityX, this.velocityY));
            foreach (GameEntity entity in game.getEntitiesInBounds(this.polygon))
            {
                this.origin.affect(entity);
                if (game.ShouldDamage(this.damageType, entity.getTargetType()) && !this.shouldPierce) this.setRemove(true);
            }
            base.Update(gameTime);
        }



    }
}
