using GameName1.AnimationTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills.Weapons
{
    class RustyShank : Sword
    {
        public RustyShank(Seizonsha game, GameEntity user)
            : base(game, user, Static.WEAPON_RUSTY_SHANK_DAMAGE, Static.WEAPON_RUSTY_SHANK_RECHARGE, Static.WEAPON_RUSTY_SHANK_LEVEL, Static.WEAPON_RUSTY_SHANK_NAME, Color.White)
        {
            this.slashAnimation = new StabAnimation(this, user, Static.WEAPON_RUSTY_SHANK_RECHARGE);
           // this.sprite = Seizonsha.spriteMappings[Static.SPRITE_DAGGER];
        }
    }
}
