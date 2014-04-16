using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills.Weapons
{
    class OKGun : Gun
    {
                public OKGun(Seizonsha game, GameEntity user)
            : base(game, user, Static.WEAPON_OKGUN_DAMAGE, Static.WEAPON_OKGUN_RECHARGE, 
            Static.WEAPON_OKGUN_FREEZE, Static.WEAPON_OKGUN_BULLET_SPEED, Static.WEAPON_OKGUN_LEVEL, Static.WEAPON_OKGUN_NAME,Static.WEAPON_OKGUN_CLIP, Color.White)
        {

        }
    }
}
