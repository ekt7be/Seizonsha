using GameName1.Interfaces;
using Microsoft.Xna.Framework;
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
        private int manaCost;
        protected Vector2 bufferedVectorDirection;
        protected float bufferedDirection;
        

        public Skill(Seizonsha game, GameEntity user, int manaCost, int rechargeTime, int castingTime, int freezeTime)
        {
            this.rechargeTime = rechargeTime + castingTime;
            this.recharged = rechargeTime;
            this.manaCost = manaCost;
            this.castingTime = castingTime;
            this.freezeTime = freezeTime;
            this.waitingForCast = false;
            this.user = user;
            this.game = game;
            bufferedDirection = 0;
        }

        public virtual void OnUnequip()
        {

        }

        public virtual void OnEquip()
        {

        }

        //INSTANTIATING IN HERE
        public virtual void Use()
        {
            this.bufferedDirection = user.direction;
            this.bufferedVectorDirection = new Vector2(user.vectorDirection.X, user.vectorDirection.Y);

            if (!(Available()) || user.isFrozen() || this.waitingForCast){
                return;
            }
            if (user is Player) ((Player)user).costMana(manaCost);
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
            bool available = true;

            if (recharged < rechargeTime){
                available = false;
            }

            if (user is Player && !(((Player)user).hasEnoughMana(this.manaCost))){
                available = false;
            }
            return available;
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
