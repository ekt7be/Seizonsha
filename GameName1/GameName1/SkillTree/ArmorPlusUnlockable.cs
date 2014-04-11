using GameName1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
    class ArmorPlusUnlockable : Unlockable
    {

        public ArmorPlusUnlockable()
        {
        }

        public void OnUnlock(Player player)
        {
            player.incArmor();
        }

        public string getDescription()
        {
            return "Increase Armor";
        }

        public string getName()
        {
            return "Armor+";
        }
    }
}
