using GameName1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
    class HealthPlusUnlockable : Unlockable
    {
        private int amount;

        public HealthPlusUnlockable(int amount)
        {
            this.amount = amount;
        }

        public void OnUnlock(Player player)
        {
            player.maxHealth += amount;
        }

        public string getDescription()
        {
            return "Increase Max Health by " + amount;
        }

        public string getName()
        {
            return "HP+ " + amount;
        }
    }
}
