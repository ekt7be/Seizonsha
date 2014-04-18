using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class Blizzard : Skill, Unlockable
    {

        private int damage;
        private int duration;
        private int slowDuration;


        public Blizzard(Seizonsha game, GameEntity user, int damage, int recharge_time, int duration, int slowDuration)
            : base(game, user, 15, recharge_time, 5, 10)
        {
            this.damage = damage;
            this.duration = duration;
            this.slowDuration = slowDuration;
        }



        public override string getDescription()
        {
            return "Creates a horrific blizzard in front of you that slows and damages enemies.";
        }

        public override string getName()
        {
            return "Blizzard";
        }

        public override void affect(GameEntity affected)
        {

            game.damageEntity(user, affected, this.damage, damageType);
            foreach (StatusEffect statusEffect in affected.getStatusEffects())
            {
                if (statusEffect.getOrigin() is Blizzard) return;
                
            }
            
            if(game.ShouldDamage(this.damageType, affected.getTargetType())) affected.addStatusEffect(new Slow(game, user, this, null, affected, 0.3f, this.damageType, 100));

        }

        public override void Update()
        {

            base.Update();
        }


        protected override void UseSkill()
        {
            float dist = 100;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + this.bufferedVectorDirection.X * dist - 50), (int)(user.getCenterY() +this.bufferedVectorDirection.Y * dist - 50), 100, 100);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);

            AOEStatus blizzard = new AOEStatus(game, user, Static.PIXEL_THIN, this, slashBounds,this.duration, 1f, 1);
            blizzard.tint = Color.White * .4f;

            game.Spawn(blizzard, slashBounds.Left, slashBounds.Top);
        }


        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
