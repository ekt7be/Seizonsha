using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class Fireball : Skill, Unlockable
    {

        private int damage;
        private int damageType;
        private float bulletSpeed;

        public Fireball(Seizonsha game, GameEntity user, int damage, int recharge_time, float bulletSpeed) : base(game, user, 40, recharge_time, 20, 20)
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
            this.bulletSpeed = bulletSpeed;
        }

        public override void affect(GameEntity affected)
        {
            affected.addStatusEffect(new GameName1.Effects.Burning(game, user, null, affected, 1, damageType, 40));
        }


        protected override void UseSkill()
        {

            int bulletWidth = 20;
            int bulletHeight = 20;
         
            Rectangle fireballBounds = new Rectangle((int)(user.getCenterX()), (int)(user.getCenterY()), bulletWidth, bulletHeight);
            ExplodingBullet fireball = EntityFactory.getExplodingBullet(game, user, Seizonsha.spriteMappings[Static.SPRITE_FIREBALL], this, fireballBounds, damage, damageType, bulletSpeed, user.direction);
            game.Spawn(fireball, fireballBounds.Left, fireballBounds.Top);

        }


        public override string getDescription()
        {
            return "Blast your target with Fire!";
        }

        public override string getName()
        {
            return "Fireball";
        }



        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
