using GameName1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
    class ManaRegenPlusUnlockable : Unlockable
    {
        private float amount;

        public ManaRegenPlusUnlockable(float amount)
        {
            this.amount = amount;
        }

        public void OnUnlock(Player player)
        {
            player.manaRegen += amount;
        }

        public string getDescription()
        {
            return "Increase Mana Regen";
        }

        public string getName()
        {
            return "Mana Regen + ";
        }
    }
}
