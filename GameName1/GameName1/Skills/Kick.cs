using GameName1.AnimationTesting;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class Kick : Weapon, Unlockable
    {

        private int damage;

        protected Rectangle? swordSource;


        public Kick(Seizonsha game, GameEntity user, int damage, int recharge_time)
            : base(game, user, recharge_time, recharge_time / 2)
        {
            this.damage = damage;


            if (Math.Cos(user.direction) > .5)
            {
                //spriteSource = FramesToAnimation[RIGHT_ANIMATION];
                swordSource = new Rectangle(64 * 0, 3 * 64, 64, 64);

            }
            else if (Math.Sin(user.direction) > .5)
            {
                swordSource = new Rectangle(64 * 0, 2 * 64, 64, 64);

            }
            else if (Math.Sin(user.direction) < -.5)
            {
                //spriteSource = FramesToAnimation[UP_ANIMATION];
                swordSource = new Rectangle(64 * 0, 0 * 64, 64, 64);

            }
            else if (Math.Cos(user.direction) < -.5)
            {
                //spriteSource = FramesToAnimation[LEFT_ANIMATION];
                swordSource = new Rectangle(64 * 0, 1 * 64, 64, 64);
            }



        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_SWORD], user.hitbox, swordSource, user.tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);

        }

        public override string getDescription()
        {
            return "Kick enemies away from you";
        }

        public override string getName()
        {
            return Static.KICK_NAME;
        }

        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, this.damage, this.damageType);
            if (game.ShouldDamage(this.damageType, affected.getTargetType())) game.moveGameEntity(affected, 100*user.vectorDirection.X, 100*user.vectorDirection.Y);
        }


        protected override void UseSkill()
        {
            int distance = 40;
            int sW = 40;
            int sH = 40;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * distance - sW / 2), (int)(user.getCenterY() + user.vectorDirection.Y * distance - sH / 2), sW, sH);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            AOECone attack = EntityFactory.getAOECone(game, Static.PIXEL_THIN, this, slashBounds, damage, damageType, 10);
            attack.setTint(Color.White * .5f);
            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
        }

        public override void Update()
        {
            base.Update();

            if (Math.Cos(user.direction) > .5)
            {
                //spriteSource = FramesToAnimation[RIGHT_ANIMATION];
                swordSource = new Rectangle(64 * 0, 3 * 64, 64, 64);

            }
            else if (Math.Sin(user.direction) > .5)
            {
                swordSource = new Rectangle(64 * 0, 2 * 64, 64, 64);

            }
            else if (Math.Sin(user.direction) < -.5)
            {
                //spriteSource = FramesToAnimation[UP_ANIMATION];
                swordSource = new Rectangle(64 * 0, 0 * 64, 64, 64);

            }
            else if (Math.Cos(user.direction) < -.5)
            {
                //spriteSource = FramesToAnimation[LEFT_ANIMATION];
                swordSource = new Rectangle(64 * 0, 1 * 64, 64, 64);
            }


        }
        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }

        public Rectangle? getSwordSource()
        {
            return swordSource;
        }

        public void setSwordSource(Rectangle? updatedSource)
        {
            swordSource = updatedSource;
        }
    }
}

