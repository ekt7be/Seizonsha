using GameName1.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills.Weapons
{
    class WeaponDrop : PickUp
    {


        private Weapon weapon;

        public WeaponDrop(Seizonsha game, Texture2D sprite, int width, int height, Weapon weapon) :base(game, sprite, width, height)
        {
            this.weapon = weapon;
        }

        public override void Interact(Player player)
        {
            if (player.weaponLevel < weapon.level)
            {
                return;
            }
            else
            {

                int weaponSkillSlot = player.weaponEquipped();

                //not equipped
                if (weaponSkillSlot == -1)
                {
                    player.removeEquipable(player.currentWeapon);
                    player.addEquipable(weapon);

                }
                else
                {
                    player.removeEquipable(player.currentWeapon);
                    player.addEquipable(weapon);
                    player.Equip(weapon, weaponSkillSlot);
  
                }

                    player.currentWeapon = weapon;
                    weapon.setUser(player);
                    if (weapon is Gun)
                    {
                        ((Gun)weapon).refillAmmo();
                    }

                    setRemove(true);
            }

        }

        public override string Message(Player player)
        {
            if (player.weaponLevel < weapon.level)
            {
                return "You cannot wield " + getName() + " (Weapon Level " + weapon.level +")";
            }
            else
            {
                if (player.currentWeapon == null)
                {
                    return "Press A(Enter) to pick up " + weapon.getName() + ".";
                }
                else
                {
                    return "Press A(Enter) to swap " + player.currentWeapon.getName() + " for " + weapon.getName() + ".";

                }
            }
        }

        public override bool Available(Player player)
        {
            return true;
        }


        protected override void OnDie()
        {
        }


        public override void collideWithWall()
        {
        }

        public override void collide(GameEntity entity)
        {
        }

        public override string getName()
        {
            return weapon.getName() + " DROP";
        }
    }


}
