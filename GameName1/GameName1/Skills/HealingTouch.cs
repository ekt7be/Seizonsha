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
        private int damage;

        public HealingTouch(Seizonsha game, GameEntity user, int damage, int recharge_time) : base(game, user, recharge_time, 30, 30)
        {

            this.damage = damage;
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
            return "Healing Touch";
        }

        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }


        protected override void UseSkill()
        {

            int damageType = Static.DAMAGE_TYPE_NO_DAMAGE;

            damageType = Static.DAMAGE_TYPE_ALL;

            Rectangle healBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * user.width / 2 - Static.PLAYER_WIDTH / 4), (int)(user.getCenterY() + user.vectorDirection.Y * user.height / 2 - Static.PLAYER_WIDTH / 4), Static.PLAYER_WIDTH / 2, Static.PLAYER_HEIGHT / 2);
            game.Spawn(new AOECone(game, user, game.getTestSprite(healBounds, Color.Green), healBounds, damage, damageType, 10, user.vectorDirection));
        }

    }
}
