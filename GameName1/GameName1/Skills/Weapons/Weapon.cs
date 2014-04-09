using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    public abstract class Weapon : Skill
    {




        public Weapon(Seizonsha game, GameEntity user, int recharge_time, int freezeTime)
            : base(game, user, 0, recharge_time, 0, freezeTime)
        {

        }


    }
}
