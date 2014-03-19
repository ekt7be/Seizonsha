using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    public class ChangeColor : Equipable, Unlockable
    {
        public Color color {get; set; }

        public ChangeColor(Color color){
            this.color = color;
        }
        public string getName()
        {
            return "Change Color";
        }
        public string getDescription()
        {
            return "Changes user's color.";
        }
        public bool Available(Seizonsha game, GameEntity user){
            return true;  //always true
        }
        public void Use(Seizonsha game, GameEntity user)
        {
            user.color = color;
        }
        public void OnEquip(Seizonsha game, GameEntity user)
        {
        }
        public void OnUnequip(Seizonsha game, GameEntity user)
        {
        }
        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }

        public void Update(Seizonsha game, GameEntity entity)
        {
        }
    }
}
