using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class TargetedAbility : GameName1.Effects.Effect
    {
        private int damageType;
        private int amount;
        private GameEntity user;
        private GameEntity target;

        public TargetedAbility(Seizonsha game, GameEntity user, Texture2D sprite, GameEntity target, int amount, int damageType, int duration, Vector2 direction)
            : base(game, sprite, user.x, user.y, user.width, user.height, duration)
        {
            this.amount = amount;
            this.damageType = damageType;
            this.user = user;
            this.target = target;
        }

        protected override void OnDie()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(sprite, hitbox, null,
                Color.White, (float)Math.Atan2(target.getCenterY() - user.getCenterY(), target.getCenterX() - user.getCenterX()), new Vector2(0f, 0f), SpriteEffects.None, 1f);
           // base.Draw(spriteBatch);
        }

        public double getDistanceToTarget()
        {
            return Math.Sqrt(Math.Pow((double)user.x - (double)target.x, 2) + Math.Pow((double)user.y - (double)target.y, 2));
        }

        public void endAbility()
        {
            setRemove(true);
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX()), (int)(user.getCenterY()), (int)this.getDistanceToTarget(), 5);
            //this.hitbox = new Rectangle(this.x, this.y, this.width, this.height);
            this.sprite = game.getTestSprite(slashBounds, Color.Black);
            this.hitbox = new Rectangle((int)(user.getCenterX()), (int)(user.getCenterY()), (int)this.getDistanceToTarget(), 5);
            if (!target.shouldRemove()) game.damageEntity(user, target, amount, damageType);
            else setRemove(true);

        }

        public override void OnSpawn()
        {
                game.damageEntity(user, target, amount, damageType);
                game.healEntity(user, user, -amount, damageType);
            
        }

        public override void collideWithWall()
        {
            endAbility();
            base.collideWithWall();
        }
    }
}
