using GameName1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
    class WeaponPlusUnlockable : Unlockable
    {

        public WeaponPlusUnlockable()
        {
        }

        public void OnUnlock(Player player)
        {
            player.incWeaponSkill();
        }

        public string getDescription()
        {
            return "Increase Weapon Proficiency Level";
        }

        public string getName()
        {
            return "Weapon Skill +";
        }
    }
}
