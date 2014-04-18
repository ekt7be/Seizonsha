using GameName1.AnimationTesting;
using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class DrainEnchant : Skill, Unlockable
    {

        public DrainEnchant(Seizonsha game, GameEntity user, int recharge_time)
            : base(game, user, 0, recharge_time, recharge_time / 2, 0)
        {

        }

        public override string getDescription()
        {
            return "Makes your weapon attacks drain life from your enemies";
        }

        public override string getName()
        {
            return Static.DRAINENCHANT_NAME;
        }

        public override void affect(GameEntity affected)
        {
            if (game.ShouldDamage(this.damageType, affected.getTargetType()))
            {
                int amount = 5;
                game.damageEntity(user, affected, amount, damageType);
                game.healEntity(user, affected, amount, damageType);
            }
        }

        public override bool Available()
        {
            return false;
        }

        protected override void UseSkill()
        {
            return;
        }


        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }

        public override void OnEquip()
        {

            ((Player)user).onHitEffects.Add(this);
            base.OnEquip();
        }

        public override void OnUnequip()
        {
            ((Player)user).onHitEffects.Remove(this);
            base.OnUnequip();
        }


    }
}


