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
        private int damageType;
        private int duration;
        private int time;
        private TargetedAbility ability;
        private bool timerStarted;

        public LifeDrain(Seizonsha game, GameEntity user, int damage, int recharge_time, int duration)
            : base(game, user, 50, recharge_time, 20, 20)
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
            foreach (GameEntity entity in game.getEntitiesInBounds(game.getLevelBounds()))
            {
                if (entity.getTargetType() == Static.TARGET_TYPE_ENEMY)
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
                ability = new TargetedAbility(game, user, game.getTestSprite(slashBounds, Color.Red), t, damage, damageType, 5, user.vectorDirection);
                game.Spawn(ability);
            }
            
            // game sprite bounds amount dmgAmount dmgType duration bulletSpeed
        }


        public override string getDescription()
        {
            return "Drain life from your target while they are in range";
        }

        public override string getName()
        {
            return "Life Drain";
        }

        public override void affect(GameEntity affected)
        {
            throw new NotImplementedException();
        }

        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
