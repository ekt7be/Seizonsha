using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.AnimationTesting;

namespace GameName1.Skills
{
    class HealingTouch : Skill, Unlockable
    {
        private int healing;


        public HealingTouch(Seizonsha game, GameEntity user, int damage, int recharge_time) : base(game, user, 20,recharge_time, 30, 30)
        {
            this.healing = damage;

        }

        public override void OnUnequip()
        {
        }

        public override void OnEquip()
        {
        }


        public override string getDescription()
        {
            return "Heals a player directly in front of you.";
        }

        public override string getName()
        {
            return Static.HEALING_TOUCH_NAME;
        }

        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }

        public override void affect(GameEntity affected)
        {
            game.healEntity(user, affected, healing, damageType);
        }


        protected override void UseSkill()
        {
            game.healingRainSound.Play();
            int damageType = Static.DAMAGE_TYPE_NO_DAMAGE;

            damageType = Static.DAMAGE_TYPE_ALL;
            int boundsWidth = 200;
            int boundsHeight = 200;
            Rectangle healBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * user.width / 2 - boundsWidth / 2), (int)(user.getCenterY() + user.vectorDirection.Y * user.height / 2 - boundsHeight / 2), boundsWidth, boundsHeight);

            game.Spawn(new HealAnimation(game, Seizonsha.spriteMappings[Static.SPRITE_HEAL], this, healBounds, healing, damageType, 30), healBounds.Left, healBounds.Top);
        }

    }
}
