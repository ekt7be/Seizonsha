using GameName1.AnimationTesting;
using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class BattleCry : Skill, Unlockable
    {

        private int damage;

        public BattleCry(Seizonsha game, GameEntity user, int damage, int recharge_time)
            : base(game, user, 0, recharge_time, 10, 0)
        {
            this.damage = damage;

        }

        public override string getDescription()
        {
            return "Roar at enemies so they attack you";
        }

        public override string getName()
        {
            return Static.BATTLECRY_NAME;
        }

        public override void affect(GameEntity affected)
        {
            if (game.ShouldDamage(this.damageType, affected.getTargetType())) affected.addStatusEffect(new Taunt(game, user, this, Static.PIXEL_THIN, affected, this.damageType, 3*60));
        }

        protected override void UseSkill()
        {
            game.battleCrySound.Play();
            int distance = 0;
            int sW = 250;
            int sH = 250;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX() + user.vectorDirection.X * distance - sW / 2), (int)(user.getCenterY() + user.vectorDirection.Y * distance - sH / 2), sW, sH);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);

            AOECone attack = EntityFactory.getAOECone(game, Static.PIXEL_THIN, this, slashBounds, damage, damageType, 10, .6f);
            attack.setTint(Color.White * .5f);
            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
        }


        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }


    }
}


