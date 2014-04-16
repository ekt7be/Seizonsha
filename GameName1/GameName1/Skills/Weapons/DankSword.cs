using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills.Weapons
{
    class DankSword : Sword
    {
        public DankSword(Seizonsha game, GameEntity user)
            : base(game, user, Static.WEAPON_DANK_SWORD_DAMAGE, Static.WEAPON_DANK_SWORD_RECHARGE, Static.WEAPON_DANK_SWORD_LEVEL, Static.WEAPON_DANK_SWORD_NAME, Color.Black)
        {

        }
    }
}
