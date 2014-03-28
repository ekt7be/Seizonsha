using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    public class ChangeColor : Skill, Unlockable
    {
        public Color color {get; set; }

        public ChangeColor(Seizonsha game, GameEntity user, Color color) : base(game, user,0, 0, 0, 0){
            this.color = color;
        }
        public override string getName()
        {
            return "Change Color";
        }
        public override string getDescription()
        {
            return "Changes user's color.";
        }

        protected override void UseSkill()
        {
            user.color = color;
        }

        public override void affect(GameEntity affected)
        {
            //throw new NotImplementedException();
        }

        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }

    }
}
