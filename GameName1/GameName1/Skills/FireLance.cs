using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class FireLance : Skill
    {

        private int damage;
        private int damageType;


        public FireLance(Seizonsha game, GameEntity user, int damage, int recharge_time)
            : base(game, user, 5, recharge_time, 5, 5)
        {
            this.damage = damage;
            this.damageType = Static.DAMAGE_TYPE_NO_DAMAGE;
            if (user.getTargetType() == Static.TARGET_TYPE_FRIENDLY)
            {
                damageType = Static.DAMAGE_TYPE_FRIENDLY;
            }
            if (user.getTargetType() == Static.TARGET_TYPE_ENEMY)
            {
                damageType = Static.DAMAGE_TYPE_ENEMY;
            }
        }



        public override string getDescription()
        {
            return "Stab with a Lance made of Fire!";
        }

        public override string getName()
        {
            return "FireLance";
        }

        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, this.damage, this.damageType);
            
            affected.addStatusEffect(new Burning(game, user, this, null, affected, 1, this.damageType, 40));
        }


        protected override void UseSkill()
        {
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + this.bufferedVectorDirection.X * user.width / 2 - user.width / 4), (int)(user.getCenterY() + this.bufferedVectorDirection.Y * user.height / 2 - user.height / 4), 200, 10);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            AOECone attack = EntityFactory.getAOECone(game, user, Static.PIXEL_THIN, this, slashBounds, damage, damageType, 10);
            attack.rotateToAngle(this.bufferedDirection);
            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
        }

    }
}
