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

            int bulletWidth = 15;
            int bulletHeight = 15;
         
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX()), (int)(user.getCenterY()), bulletWidth, bulletHeight);
            game.Spawn(new ExplodingBullet(game, user, game.getTestSprite(slashBounds, Color.Red), this,slashBounds, damage, damageType, 1, bulletSpeed, user.vectorDirection), slashBounds.Left, slashBounds.Top);

            // game sprite bounds amount dmgAmount dmgType duration bulletSpeed
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
