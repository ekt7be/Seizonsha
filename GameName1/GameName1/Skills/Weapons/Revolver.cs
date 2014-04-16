using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills.Weapons
{
    class Revolver : Gun
    {

        public Revolver(Seizonsha game, GameEntity user)
            : base(game, user, Static.WEAPON_REVOLVER_LEVEL, Static.WEAPON_REVOLVER_RECHARGE, 
            Static.WEAPON_REVOLVER_FREEZE, Static.WEAPON_REVOLVER_BULLET_SPEED, Static.WEAPON_REVOLVER_LEVEL, Static.WEAPON_REVOLVER_NAME,Static.WEAPON_REVOLVER_CLIP, Color.White)
        {

        }
    }
}
