using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills.Weapons
{
    class RustySword : Sword
    {
        public RustySword(Seizonsha game, GameEntity user)
            : base(game, user, Static.WEAPON_RUSTY_SWORD_DAMAGE, Static.WEAPON_RUSTY_SWORD_RECHARGE, Static.WEAPON_RUSTY_SWORD_LEVEL, Static.WEAPON_RUSTY_SWORD_NAME, Color.White)
        {

        }
    }
}
