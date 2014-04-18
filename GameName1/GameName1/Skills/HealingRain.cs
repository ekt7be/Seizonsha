using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class HealingRain : Skill, Unlockable
    {

        private int damage;
        private int duration;

        Rectangle? healingSource;

        private float elapsed;
        private float delay = 100f;
        private int currentFrame = 0;
        private static readonly int healingFrames = 6;
        private int recharge_time;


        public HealingRain(Seizonsha game, GameEntity user, int damage, int recharge_time, int duration)
            : base(game, user, 15, recharge_time, 5, 10)
        {
            this.damage = damage;
            this.duration = duration;
        }

        public override string getDescription()
        {
            return "Calls upon the heavens to rain health upon you";
        }

        public override string getName()
        {
            return Static.HEALING_RAIN_NAME;
        }

        public override void affect(GameEntity affected)
        {
            if (game.ShouldHeal(this.damageType, affected.getTargetType())) game.healEntity(user, affected, this.damage, this.damageType);
        }

        public void UpdateAnimation(GameTime gameTime)
        {

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed > delay)
            {
                if (currentFrame >= healingFrames - 1)
                {
                    currentFrame = 0;
                }

                else
                {
                    currentFrame++;
                }

                elapsed = 0;
            }


            healingSource = new Rectangle(256 * currentFrame, 0 * 128, 256, 128);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle hitbox)
        {
            spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_HEALING_RAIN], hitbox, healingSource, Color.White);
        }

        public override void Update()
        {
            base.Update();
        }


        protected override void UseSkill()
        {
            float dist = 100;
            int sW = 100;
            int sH = 100;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + this.bufferedVectorDirection.X * dist - sW / 2), (int)(user.getCenterY() + this.bufferedVectorDirection.Y * dist - sH / 2), sW, sH);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            AOEStatus rain = new AOEStatus(game, user, Static.PIXEL_THIN, this, slashBounds, this.duration, 1f, 30);
            rain.setTint(Color.White * .5f);
            game.Spawn(rain, slashBounds.Left, slashBounds.Top);
        }


        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
