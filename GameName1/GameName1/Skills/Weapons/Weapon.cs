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

        public int level;
        String name;
        protected Color tint;

        public Weapon(Seizonsha game, GameEntity user, int recharge_time, int freezeTime, int level, String name, Color tint)
            : base(game, user, 0, recharge_time, 0, freezeTime)
        {
            this.level = level;
            this.name = name;
            this.tint = tint;
        }

        public void setUser(GameEntity user)
        {
            this.user = user;
            this.damageType = Static.DAMAGE_TYPE_NO_DAMAGE;
            if (user.getTargetType() == Static.TARGET_TYPE_GOOD)
            {
                damageType = Static.DAMAGE_TYPE_GOOD;
            }
            if (user.getTargetType() == Static.TARGET_TYPE_BAD)
            {
                damageType = Static.DAMAGE_TYPE_BAD;
            }
        }

        public override string getName()
        {
            return name;
        }

        public void setTint(Color tint)
        {
            this.tint = tint;
        }
      
    }
}
