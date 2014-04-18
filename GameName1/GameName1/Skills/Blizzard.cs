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
    class Blizzard : Skill, Unlockable
    {

        private int damage;
        private int duration;
        private int slowDuration;

        Rectangle? blizzardSource;

        private float elapsed;
        private float delay = 70f;
        private int currentFrame = 0;
        private static readonly int blizzardFrames = 6;
        private int recharge_time;


        public Blizzard(Seizonsha game, GameEntity user, int damage, int recharge_time, int duration, int slowDuration)
            : base(game, user, 15, recharge_time, 5, 10)
        {
            this.damage = damage;
            this.duration = duration;
            this.slowDuration = slowDuration;
            blizzardSource = new Rectangle(256 * 0, 0 * 128, 256, 128);
        }



        public override string getDescription()
        {
            return "Creates a horrific blizzard in front of you that slows and damages enemies.";
        }

        public override string getName()
        {
            return "Blizzard";
        }

        public override void affect(GameEntity affected)
        {

            game.damageEntity(user, affected, this.damage, damageType);
            foreach (StatusEffect statusEffect in affected.getStatusEffects())
            {
                if (statusEffect.getOrigin() is Blizzard) return;
                
            }
            
            if(game.ShouldDamage(this.damageType, affected.getTargetType())) affected.addStatusEffect(new Slow(game, user, this, null, affected, 0.3f, this.damageType, 100));

        }

        public void UpdateAnimation(GameTime gameTime)
        {

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed > delay)
            {
                if (currentFrame >= blizzardFrames - 1)
                {
                    currentFrame = 0;
                }

                else
                {
                    currentFrame++;
                }

                elapsed = 0;
            }


            blizzardSource = new Rectangle(256 * currentFrame, 0 * 128, 256, 128);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle hitbox)
        {
            spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_BLIZZARD], hitbox, blizzardSource, Color.White);
        }

        public override void Update()
        {

            base.Update();
        }


        protected override void UseSkill()
        {
            game.blizzardSound.Play();
            float dist = 100;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + this.bufferedVectorDirection.X * dist - 50), (int)(user.getCenterY() +this.bufferedVectorDirection.Y * dist - 50), 100, 100);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);

            AOEStatus blizzard = new AOEStatus(game, user,  null, this, slashBounds,this.duration, 1f, 1);
            blizzard.tint = Color.White * .8f;

            game.Spawn(blizzard, slashBounds.Left, slashBounds.Top);
        }


        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
