using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Interfaces
{
    public interface Unlockable
    {
        void OnUnlock(Player player);
        string getDescription();
        string getName();
    }
}
