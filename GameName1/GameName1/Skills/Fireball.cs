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
        private float bulletSpeed;

        public Fireball(Seizonsha game, GameEntity user, int damage, int recharge_time, float bulletSpeed) : base(game, user, 40, recharge_time, 20, 20)
        {

            this.damage = damage;

            this.bulletSpeed = bulletSpeed;
        }

        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, this.damage, this.damageType);
           if (game.ShouldDamage(this.damageType,affected.getTargetType())){
                affected.addStatusEffect(new GameName1.Effects.Burning(game, user, this, null, affected, 1, damageType, 40));
           }
        }


        protected override void UseSkill()
        {

            int bulletWidth = 20;
            int bulletHeight = 20;
         
            Rectangle fireballBounds = new Rectangle((int)(user.getCenterX() - bulletWidth/2), (int)(user.getCenterY() - bulletHeight/2), bulletWidth, bulletHeight);
            ExplodingBullet fireball = EntityFactory.getExplodingBullet(game, user, Seizonsha.spriteMappings[Static.SPRITE_FIREBALL], this, fireballBounds, damage, damageType, bulletSpeed, this.bufferedDirection);
            game.Spawn(fireball, fireballBounds.Left, fireballBounds.Top);

        }


        public override string getDescription()
        {
            return "Blast your target with Fire!";
        }

        public override string getName()
        {
            return Static.FIREBALL_NAME;
        }



        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
