using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class Teleport : Skill, Unlockable
    {

        private int damage;
        private int damageType;
        private int duration;
        private AOECone ability;

        public Teleport(Seizonsha game, GameEntity user, int damage, int recharge_time, int duration)
            : base(game, user, 20, recharge_time, 10, 20)
        {

            this.damage = damage;
            this.damageType = Static.DAMAGE_TYPE_NO_DAMAGE;
            if (user.getTargetType() == Static.TARGET_TYPE_GOOD)
            {
                damageType = Static.DAMAGE_TYPE_GOOD;
            }
            if (user.getTargetType() == Static.TARGET_TYPE_BAD)
            {
                damageType = Static.DAMAGE_TYPE_BAD;
            }
            this.duration = duration;
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void UseSkill()
        {
                Vector2 unitV = Vector2.Normalize(user.vectorDirection);
                float dist = 200f;
                game.moveGameEntity(user,unitV.X*dist, unitV.Y*dist);
                int width = 100;
                int height = 100;
                Rectangle slashBounds = new Rectangle((int)(user.getCenterX())-(int)((double)width/2.0), (int)(user.getCenterY())-(int)((double)height/2.0), width, height);
                ability = EntityFactory.getAOECone(game, Static.PIXEL_THIN, this, slashBounds, damage, damageType, 1, .8f);
                game.Spawn(ability, slashBounds.Left, slashBounds.Top);

            // game sprite bounds amount dmgAmount dmgType duration bulletSpeed
        }


        public override string getDescription()
        {
            return "Drain life from your target while they are in range";
        }

        public override string getName()
        {
            return Static.TELEPORT_NAME;
        }

        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, this.damage, this.damageType);
        }

        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
