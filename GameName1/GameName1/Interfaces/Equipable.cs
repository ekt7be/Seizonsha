using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Interfaces
{
    public interface Equipable
    {
        void OnUnequip();
        void OnEquip();
        void Use();
        bool Available();
        void Update();
        string getDescription();
        string getName();
    }
}
