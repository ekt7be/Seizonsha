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
    class Bash : Skill, Unlockable
    {

        private int damage;



        public Bash(Seizonsha game, GameEntity user, int damage, int recharge_time)
            : base(game, user, recharge_time, 0, recharge_time / 2, 0)
        {
            this.damage = damage;

        }


        public override string getDescription()
        {
            return "Bash your target and stun them";
        }

        public override string getName()
        {
            return Static.BASH_NAME;
        }

        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, this.damage, this.damageType);
            if(game.ShouldDamage(this.damageType, affected.getTargetType())) affected.addStatusEffect(new GameName1.Effects.Stun(game, user, this, Static.PIXEL_THIN, affected, this.damageType, 100));
        }


        protected override void UseSkill()
        {
            int distance = 40;
            int sW = 40;
            int sH = 40;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * distance - sW/2), (int)(user.getCenterY() + user.vectorDirection.Y * distance - sH/2), sW, sH);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            AOECone attack = EntityFactory.getAOECone(game, Static.PIXEL_THIN, this, slashBounds, damage, damageType, 10);
            attack.setTint(Color.White * .5f);
            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
        }

     
        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }

    }
}

