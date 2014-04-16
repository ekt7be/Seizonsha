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
    class Kick : Skill, Unlockable
    {

        private int damage;



        public Kick(Seizonsha game, GameEntity user, int damage, int recharge_time)
            : base(game, user, 0, recharge_time, recharge_time / 2, 0)
        {
            this.damage = damage;

        }

        public override string getDescription()
        {
            return "Kick enemies away from you";
        }

        public override string getName()
        {
            return Static.KICK_NAME;
        }

        public override void affect(GameEntity affected)
        {
            game.damageEntity(user, affected, this.damage, this.damageType);
            if (game.ShouldDamage(this.damageType, affected.getTargetType())) game.moveGameEntity(affected, 100*user.vectorDirection.X, 100*user.vectorDirection.Y);
        }


        protected override void UseSkill()
        {
            int distance = 40;
            int sW = 40;
            int sH = 40;
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


    }
}

