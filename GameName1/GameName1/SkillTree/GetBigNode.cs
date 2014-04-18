using GameName1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
    class GetBigUnlockable : Unlockable
    {



            public GetBigUnlockable()
            {

            }

            public void OnUnlock(Player player)
            {
                player.setWidth(player.width+20);
                player.setHeight(player.height+20);

            }

            public string getDescription()
            {
                return "Get Big";
            }

            public string getName()
            {
                return "Get Big";
            }
    }
}
