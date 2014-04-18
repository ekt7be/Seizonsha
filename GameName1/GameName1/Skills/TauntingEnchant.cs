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
    class TauntingEnchant : Skill, Unlockable
    {

        public TauntingEnchant(Seizonsha game, GameEntity user, int recharge_time)
            : base(game, user, 0, recharge_time, recharge_time / 2, 0)
        {

        }

        public override string getDescription()
        {
            return "Makes your weapon attacks taunt your opponent";
        }

        public override string getName()
        {
            return Static.TAUNTINGENCHANT_NAME;
        }

        public override void affect(GameEntity affected)
        {
            if (game.ShouldDamage(this.damageType, affected.getTargetType()))
            {
                affected.addStatusEffect(new Taunt(game, user, this, Static.PIXEL_THIN, affected, this.damageType, 2 * 60));
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


