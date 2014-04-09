using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class LifeDrain : Skill, Unlockable
    {

        private int damage;
        private int duration;
        private int time;
        private TargetedAbility ability;
        private bool timerStarted;

        public LifeDrain(Seizonsha game, GameEntity user, int damage, int recharge_time, int duration)
            : base(game, user, 20, recharge_time, 20, 20)
        {

            this.damage = damage;
            this.duration = duration;
            this.time = 0;
            this.timerStarted = false;
        }

        public override void Update()
        {
           // Static.Debug("updating");
            if(this.timerStarted == true)
            {
                time++;
                if (time >= duration)
                {
                    this.ability.endAbility();
                    timerStarted = false;
                }
            }
            base.Update();
        }

        protected GameEntity getTarget()
        {
            GameEntity enemy = null;
            int rangeX = 300;
            int rangeY = 300;
            foreach (GameEntity entity in game.getEntitiesInBounds(new Rectangle(user.getCenterX() - rangeX/2, user.getCenterY() - rangeY/2, rangeX, rangeY)))
            {
                if (entity.getTargetType() == Static.TARGET_TYPE_BAD)
                {
                    enemy = entity;
                    break;
                }
            }
            
            return enemy;
        }

        protected override void UseSkill()
        {
            GameEntity t = this.getTarget();
            if (t != null)
            {
                this.time = 0;
                this.timerStarted = true;
                int bulletWidth = 15;
                int bulletHeight = 15;
                Rectangle slashBounds = new Rectangle((int)(user.getCenterX()), (int)(user.getCenterY()), bulletWidth, bulletHeight);
                ability = new TargetedAbility(game, user, this, Static.PIXEL_THIN, t, damage, damageType, this.duration, user.vectorDirection);
                ability.setTint(Color.Aquamarine);
                game.Spawn(ability, slashBounds.Left, slashBounds.Top);
            }
            
            // game sprite bounds amount dmgAmount dmgType duration bulletSpeed
        }


        public override string getDescription()
        {
            return "Drain life from your target while they are in range.";
        }

        public override string getName()
        {
            return Static.LIFE_DRAIN_NAME;
        }

        public override void affect(GameEntity affected)
        {
            game.healEntity(user, user, this.damage, this.damageType);
            game.damageEntity(user, affected, this.damage, this.damageType);
            foreach (StatusEffect statusEffect in affected.getStatusEffects())
            {
                if (statusEffect.getOrigin() is LifeDrain) return;

            }
            affected.addStatusEffect(new Slow(game, user, this, null, affected, 0.3f, this.damageType, this.duration));
        }

        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
