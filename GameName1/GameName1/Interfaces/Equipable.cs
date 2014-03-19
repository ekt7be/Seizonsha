using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Interfaces
{
    public interface Equipable
    {
        void OnUnequip(Seizonsha game, GameEntity entity);
        void OnEquip(Seizonsha game, GameEntity entity);
        void Use(Seizonsha game, GameEntity entity);
        bool Available(Seizonsha game, GameEntity entity);
        void Update(Seizonsha game, GameEntity entity);
        string getDescription();
        string getName();
    }
}
