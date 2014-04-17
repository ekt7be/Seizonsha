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
    public class Sword : Weapon, Unlockable
    {

        private int damage;

        protected Rectangle? swordSource;

        protected Animation slashAnimation;

        protected Texture2D sprite;

        public Sword(Seizonsha game, GameEntity user,int damage, int recharge_time, int level, string name, Color tint) : base(game, user, recharge_time, recharge_time/2, level, name, tint)
        {
            this.damage = damage;
            slashAnimation = new SlashAnimation(this, user, recharge_time);
            this.sprite = Seizonsha.spriteMappings[Static.SPRITE_SWORD];


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
            spriteBatch.Draw(sprite, user.hitbox, swordSource, tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);

        }

        public override string getDescription()
        {
            return "A SWORD";
        }


        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, this.damage, this.damageType);
        }


        protected override void UseSkill()
        {

            if (slashAnimation is SlashAnimation)
            {
                ((SlashAnimation)slashAnimation).reset(user);
            }
            else if (slashAnimation is StabAnimation)
            {
                ((StabAnimation)slashAnimation).reset(user);
            }
            if (!user.HasAnimation(slashAnimation)){
                user.AddAnimation(slashAnimation);
            }
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * user.width / 2 - user.width / 4), (int)(user.getCenterY() + user.vectorDirection.Y * user.height / 2 - user.height / 4), user.width / 2, user.height / 2);
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
