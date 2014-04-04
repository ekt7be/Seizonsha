using GameName1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
    class ManaPlusUnlockable : Unlockable
    {
        private int amount;

        public ManaPlusUnlockable(int amount)
        {
            this.amount = amount;
        }

        public void OnUnlock(Player player)
        {
            player.maxMana += amount;
        }

        public string getDescription()
        {
            return "Increase Max Mana by " + amount;
        }

        public string getName()
        {
            return "Mana+ " + amount;
        }
    }
}
