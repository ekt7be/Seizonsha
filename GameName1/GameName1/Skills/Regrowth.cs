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
    class Regrowth : Skill, Unlockable
    {

        private int damage;

        public Regrowth(Seizonsha game, GameEntity user, int damage, int recharge_time)
            : base(game, user, 0, recharge_time, recharge_time / 2, 0)
        {
            this.damage = damage;

        }

        public override string getDescription()
        {
            return "Heals surrounding players over time";
        }

        public override string getName()
        {
            return Static.REGROWTH_NAME;
        }

        public override void affect(GameEntity affected)
        {
            if (game.ShouldHeal(this.damageType, affected.getTargetType())) affected.addStatusEffect(new GameName1.Effects.HealOverTime(game, user, this, Static.PIXEL_THIN, affected, 3, this.damageType, 4*60));
        }


        protected override void UseSkill()
        {
            int distance = 0;
            int sW = 300;
            int sH = 300;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * distance - sW / 2), (int)(user.getCenterY() + user.vectorDirection.Y * distance - sH / 2), sW, sH);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);

            AOECone attack = EntityFactory.getAOECone(game, null, this, slashBounds, damage, damageType, 10, .6f);
            attack.setTint(Color.White * .5f);
            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
        }


        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }


    }
}


