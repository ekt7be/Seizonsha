using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class HealingRain : Skill, Unlockable
    {

        private int damage;
        private int duration;
        private int time;


        public HealingRain(Seizonsha game, GameEntity user, int damage, int recharge_time, int duration)
            : base(game, user, 15, recharge_time, 5, 10)
        {
            this.damage = damage;
            this.time = 0;
            this.duration = duration;
        }

        public override string getDescription()
        {
            return "Calls upon the heavens to rain health upon you";
        }

        public override string getName()
        {
            return Static.HEALING_RAIN_NAME;
        }

        public override void affect(GameEntity affected)
        {
            if (this.time % 30 == 0)
            {
                if (game.ShouldHeal(this.damageType, affected.getTargetType())) game.healEntity(user, affected, this.damage, this.damageType);
            }

        }

        public override void Update()
        {
            time++;
            base.Update();
        }


        protected override void UseSkill()
        {
            float dist = 100;
            int sW = 100;
            int sH = 100;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + this.bufferedVectorDirection.X * dist - sW / 2), (int)(user.getCenterY() + this.bufferedVectorDirection.Y * dist - sH / 2), sW, sH);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            AOEStatus rain = new AOEStatus(game, user, Static.PIXEL_THIN, this, slashBounds, this.duration, 1f);
            rain.setTint(Color.White * .5f);
            game.Spawn(rain, slashBounds.Left, slashBounds.Top);
        }


        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
