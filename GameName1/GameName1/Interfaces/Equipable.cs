using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Interfaces
{
    public interface Equipable
    {
        void OnUnequip(GameEntity entity);
        void OnEquip(GameEntity entity);
        void Use(GameEntity entity);
        bool Available(GameEntity entity);
        string getDescription();
        string getName();
    }
}
