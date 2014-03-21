using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class HealingTouch : Equipable, Unlockable
    {
        private int recharge_time;
        private int recharged;
        private int damage;

        public HealingTouch(int damage, int recharge_time)
        {
            this.recharge_time = recharge_time;
            this.recharged = recharge_time;
            this.damage = damage;
        }

        public void OnUnequip(Seizonsha game, GameEntity entity)
        {
            //throw new NotImplementedException();
        }

        public void OnEquip(Seizonsha game, GameEntity entity)
        {
            //throw new NotImplementedException();
        }

        public void Use(Seizonsha game, GameEntity entity)
        {
            if (entity.isFrozen())
            {
                return;
            }
            recharged = 0;
            entity.Freeze(recharge_time);
            int damageType = Static.DAMAGE_TYPE_NO_DAMAGE;
            if (entity.getTargetType() == Static.TARGET_TYPE_FRIENDLY)
            {
                damageType = Static.DAMAGE_TYPE_FRIENDLY;
            }
            if (entity.getTargetType() == Static.TARGET_TYPE_ENEMY)
            {
                damageType = Static.DAMAGE_TYPE_ENEMY;
            }
            damageType = Static.DAMAGE_TYPE_ALL;

            Rectangle slashBounds = new Rectangle((int)(entity.getCenterX() + entity.alexDirection.X * entity.width / 2 - Static.PLAYER_WIDTH / 4), (int)(entity.getCenterY() + entity.alexDirection.Y * entity.height / 2 - Static.PLAYER_WIDTH / 4), Static.PLAYER_WIDTH / 2, Static.PLAYER_HEIGHT / 2);
            game.Spawn(new AOECone(game, game.getTestSprite(slashBounds, Color.Green), slashBounds, damage, damageType, 10, entity.alexDirection));
        }

        public bool Available(Seizonsha game, GameEntity entity)
        {
            return recharged == recharge_time;
        }

        public void Update(Seizonsha game, GameEntity entity)
        {
            if (recharged < recharge_time) recharged++;
        }

        public string getDescription()
        {
            return "Heals a player directly in front of you.";
        }

        public string getName()
        {
            return "Healing Touch";
        }

        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
