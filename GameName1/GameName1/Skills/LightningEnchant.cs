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
                if(e.getTargetType() == Static.TARGET_TYPE_BAD) nextTarget = e;
            }
            if(nextTarget == null) return;
            else affect(game.getEntitiesInBounds(new Rectangle(affected.getCenterX() - 100, affected.getCenterY() - 100, 200, 200))[0], count + 1);
        }

        public void affect(GameEntity affected, int count)
        {
            if (count >= 3) return;
            GameEntity nextTarget = null;
            game.damageEntity(user, affected, this.damage, this.damageType);
            foreach(GameEntity e in game.getEntitiesInBounds(new Rectangle(affected.getCenterX() - 100, affected.getCenterY() - 100, 200, 200))){
                if(e.getTargetType() == Static.TARGET_TYPE_BAD) nextTarget = e;
            }
            if(nextTarget == null) return;
            else affect(game.getEntitiesInBounds(new Rectangle(affected.getCenterX() - 100, affected.getCenterY() - 100, 200, 200))[0], count + 1);
        }


        protected override void UseSkill()
        {
            int distance = 0;
            int sW = 100;
            int sH = 100;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * distance - sW / 2), (int)(user.getCenterY() + user.vectorDirection.Y * distance - sH / 2), sW, sH);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            AOECone attack = EntityFactory.getAOECone(game, Static.PIXEL_THIN, this, slashBounds, damage, damageType, 10);
            attack.setTint(Color.White * .5f);
            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
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


