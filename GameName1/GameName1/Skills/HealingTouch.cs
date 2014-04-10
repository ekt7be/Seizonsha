using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            int damageType = Static.DAMAGE_TYPE_NO_DAMAGE;

            damageType = Static.DAMAGE_TYPE_ALL;

            Rectangle healBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * user.width / 2 - Static.PLAYER_WIDTH / 4), (int)(user.getCenterY() + user.vectorDirection.Y * user.height / 2 - Static.PLAYER_WIDTH / 4), Static.PLAYER_WIDTH / 2, Static.PLAYER_HEIGHT / 2);

            game.Spawn(EntityFactory.getAOECone(game, Static.PIXEL_THIN, this, healBounds, healing, damageType, 10), healBounds.Left, healBounds.Top);
        }

    }
}
