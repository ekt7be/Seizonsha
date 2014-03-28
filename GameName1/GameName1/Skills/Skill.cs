using GameName1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    public abstract class Skill : Equipable
    {
        protected GameEntity user;
        protected Seizonsha game;
        private int recharged;
        private int casting;
        protected int rechargeTime;
        protected int freezeTime;
        protected int castingTime;
        private bool waitingForCast;

        public Skill(Seizonsha game, GameEntity user, int rechargeTime, int castingTime, int freezeTime)
        {
            this.rechargeTime = rechargeTime;
            this.castingTime = castingTime;
            this.freezeTime = freezeTime;
            this.waitingForCast = false;
            this.user = user;
            this.game = game;
        }

        public virtual void OnUnequip()
        {

        }

        public virtual void OnEquip()
        {

        }


        public virtual void Use()
        {
            if (!(Available()) || user.isFrozen()){
                return;
            }

            user.Freeze(freezeTime);
            if (castingTime > 0)
            {
                casting = 0;
                waitingForCast = true;
            }
            else
            {
                UseSkill();
            }
            recharged = 0;

        }

        protected abstract void UseSkill();


        public virtual bool Available()
        {
            return recharged >= rechargeTime;
        }

        public double percentCasted()
        {
            return (double)casting / (double) castingTime;
        }

        public abstract void affect(GameEntity affected);

        public virtual void Update()
        {

            //check if casting and if casting is complete
            if (waitingForCast)
            {
                casting++;
                if (casting >= castingTime)
                {
                    UseSkill();
                    waitingForCast = false;
                }
            }


            //check if skill is recharging
            if (recharged < rechargeTime && !waitingForCast) 
            {
                recharged++;
            }
        }

        public abstract string getDescription();

        public abstract string getName();

        public GameEntity getUser()
        {
            return user;
        }


    }
}
