using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class FireLance : Skill, Unlockable
    {

        private int damage;


        public FireLance(Seizonsha game, GameEntity user, int damage, int recharge_time)
            : base(game, user, Static.FIRELANCE_COST, Static.FIRELANCE_RECHARGE, 5, 5)
        {
            this.damage = damage;
        }



        public override string getDescription()
        {
            return "Stab with a Lance made of Fire!";
        }

        public override string getName()
        {
            return Static.FIRELANCE_NAME;
        }

        public override void affect(GameEntity affected)
        {
            if(game.ShouldDamage(this.damageType, affected.getTargetType())){
                game.damageEntity(user, affected, this.damage, this.damageType);
            
             affected.addStatusEffect(new Burning(game, user, this, null, affected, Static.FIRELANCE_DOT_TICK, this.damageType, Static.FIRELANCE_DOT_DUR));
            }
        }

        protected override void UseSkill()
        {
            game.fireballSound.Play();
            int width = 10;
            int length = 200;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX()), (int)(user.getCenterY() -5), length, width);
            List<PolygonIntersection.Vector> points = new List<PolygonIntersection.Vector>();
            float theta = bufferedDirection;
            //if(theta < (float)(2.0*Math.PI)) theta += 2.0f*(float)Math.PI;
            float ox = user.getCenterX();
            float oy = user.getCenterY() - (int)(width / 2);
            float px1 = user.getCenterX();
            float py1 = user.getCenterY();
            float px2 = user.getCenterX();
            float py2 = user.getCenterY() + width;
            float px3 = user.getCenterX() + length;
            float py3 = user.getCenterY() + width;
            float px4 = user.getCenterX() + length;
            float py4 = user.getCenterY();
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px1-ox) - Math.Sin(theta) *(py1-oy) + ox),   (int)(Math.Sin(theta) * (px1-ox) + Math.Cos(theta) * (py1-oy) + oy)));
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px2-ox) - Math.Sin(theta) *(py2-oy) + ox),   (int)(Math.Sin(theta) * (px2-ox) + Math.Cos(theta) * (py2-oy) + oy)));
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px3-ox) - Math.Sin(theta) *(py3-oy) + ox),   (int)(Math.Sin(theta) * (px3-ox) + Math.Cos(theta) * (py3-oy) + oy)));
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px4-ox) - Math.Sin(theta) *(py4-oy) + ox),   (int)(Math.Sin(theta) * (px4-ox) + Math.Cos(theta) * (py4-oy) + oy)));
           /* p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
            p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy
            */

            PolygonIntersection.Polygon polygon = new PolygonIntersection.Polygon(points);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            AOEPolygon attack = new AOEPolygon(game, user, Seizonsha.spriteMappings[Static.SPRITE_FIREBALL], this, slashBounds, polygon, damage, damageType, 10, bufferedDirection);
            attack.rotateToAngle(this.bufferedDirection);
            
            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
        }


        public void OnUnlock(Player player)
        {
            player.addEquipable(this);
        }
    }
}
