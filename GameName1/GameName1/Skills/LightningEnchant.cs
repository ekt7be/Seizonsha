using GameName1.AnimationTesting;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class LightningEnchant : Skill, Unlockable
    {

        private int damage;

        public LightningEnchant(Seizonsha game, GameEntity user, int damage, int recharge_time)
            : base(game, user, 0, recharge_time, recharge_time / 2, 0)
        {
            this.damage = damage;

        }

        public override string getDescription()
        {
            return "Makes your weapon attacks electrocute your opponents";
        }

        public override string getName()
        {
            return Static.LIGHTNINGENCHANT_NAME;
        }

        public override void affect(GameEntity affected)
        {
            int count = 0;
            game.damageEntity(user, affected, this.damage, this.damageType);
            GameEntity nextTarget = null;
            foreach(GameEntity e in game.getEntitiesInBounds(new Rectangle(affected.getCenterX() - 100, affected.getCenterY() - 100, 200, 200))){
                if(e.getTargetType() == Static.TARGET_TYPE_BAD && e != affected) nextTarget = e;
            }
            if (nextTarget == null) return;
            else
            {
                affect(nextTarget, count + 1);
            }
        }

        public void affect(GameEntity affected, int count)
        {
            if (count >= 3) return;
            GameEntity nextTarget = null;
            game.damageEntity(user, affected, this.damage, this.damageType);
            foreach(GameEntity e in game.getEntitiesInBounds(new Rectangle(affected.getCenterX() - 100, affected.getCenterY() - 100, 200, 200))){
                if(e.getTargetType() == Static.TARGET_TYPE_BAD && e != affected) nextTarget = e;
            }
            if (nextTarget == null) return;
            else
            {
                Effects.LightningEffect le = new Effects.LightningEffect(game, Static.PIXEL_THIN, 20, affected, nextTarget);
                game.Spawn(le, affected.getCenterX(), affected.getCenterY());
                affect(nextTarget, count + 1);
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


