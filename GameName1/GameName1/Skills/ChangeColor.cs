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
        public bool Available(GameEntity user){
            return true;  //always true
        }
        public void Use(GameEntity user)
        {
            user.color = color;
        }
        public void OnEquip(GameEntity user)
        {
        }
        public void OnUnequip(GameEntity user)
        {
        }
        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }


    }
}
