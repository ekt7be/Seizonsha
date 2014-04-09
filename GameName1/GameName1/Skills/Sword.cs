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
    class Sword : Skill, Unlockable
    {

        private int damage;
        private int damageType;

        protected Rectangle? swordSource;

        SlashAnimation slashAnimation;


        public Sword(Seizonsha game, GameEntity user,int damage, int recharge_time) : base(game, user, 0, recharge_time, 0, 10)
        {
            this.damage = damage;
            this.damageType = Static.DAMAGE_TYPE_NO_DAMAGE;
            if (user.getTargetType() == Static.TARGET_TYPE_FRIENDLY)
            {
                damageType = Static.DAMAGE_TYPE_FRIENDLY;
            }
            if (user.getTargetType() == Static.TARGET_TYPE_ENEMY)
            {
                damageType = Static.DAMAGE_TYPE_ENEMY;
            }

            slashAnimation = new SlashAnimation(this, user, recharge_time);


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
            return "A SWORD";
        }

        public override string getName()
        {
			return "Sword";
        }

        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, this.damage, this.damageType);
        }


        protected override void UseSkill()
        {

            slashAnimation.reset(user);
            if (!user.HasAnimation(slashAnimation)){
                user.AddAnimation(slashAnimation);
            }
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * user.width / 2 - user.width / 4), (int)(user.getCenterY() + user.vectorDirection.Y * user.height / 2 - user.height / 4), user.width / 2, user.height / 2);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            AOECone attack = EntityFactory.getAOECone(game, user, null, this, slashBounds, damage, damageType, 10);
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
