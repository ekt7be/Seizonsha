﻿using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.AnimationTesting;

namespace GameName1.Skills
{
    public abstract class Skill : Equipable
    {
        protected GameEntity user;
        protected Seizonsha game;
		public int recharged;
        private int casting;
		public int rechargeTime;
        protected int freezeTime;
        protected int castingTime;
        private bool waitingForCast;
		public int manaCost;
        protected Vector2 bufferedVectorDirection;
        protected float bufferedDirection;
        protected int damageType;
        private CastAnimation castAnimation;
        

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
            castAnimation = new CastAnimation(user, castingTime);

            this.damageType = Static.DAMAGE_TYPE_NO_DAMAGE;
            if (user.getTargetType() == Static.TARGET_TYPE_GOOD)
            {
                damageType = Static.DAMAGE_TYPE_GOOD;
            }
            if (user.getTargetType() == Static.TARGET_TYPE_BAD)
            {
                damageType = Static.DAMAGE_TYPE_BAD;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void OnUnequip()
        {

        }

        public virtual void OnEquip()
        {
            this.recharged = rechargeTime;
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
                castAnimation.reset(user);
                if (!user.HasAnimation(castAnimation))
                {
                    user.AddAnimation(castAnimation);
                }
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
