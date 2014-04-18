using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    abstract class SimpleBow : Weapon
    {

        private float bulletSpeed;
        public int ammo;
        public int clipSize;

        //for enemies
        private Boolean unlimitedAmmo;


        public SimpleBow(Seizonsha game, GameEntity user, int damage, int recharge_time, int freezeTime, float bulletSpeed, int level, string name, int clipSize, Color tint)
            : base(game, user, recharge_time, freezeTime, level, damage, name, tint)
        {
            this.bulletSpeed = bulletSpeed;
            this.clipSize = clipSize;
            this.ammo = clipSize;
            this.unlimitedAmmo = false;
        }

        public void refillAmmo()
        {
            ammo = clipSize;
        }

        public void setUnlimitedAmmo(bool unlimited)
        {
            this.unlimitedAmmo = unlimited;
            if (unlimited)
            {
                this.ammo = clipSize;
            }
        }

        protected override void UseSkill()
        {


            int width = 5;
            int length = 15;
            Rectangle slashBounds = new Rectangle((int)(user.getCenterX()), (int)(user.getCenterY() - 5), length, width);
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
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px1 - ox) - Math.Sin(theta) * (py1 - oy) + ox), (int)(Math.Sin(theta) * (px1 - ox) + Math.Cos(theta) * (py1 - oy) + oy)));
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px2 - ox) - Math.Sin(theta) * (py2 - oy) + ox), (int)(Math.Sin(theta) * (px2 - ox) + Math.Cos(theta) * (py2 - oy) + oy)));
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px3 - ox) - Math.Sin(theta) * (py3 - oy) + ox), (int)(Math.Sin(theta) * (px3 - ox) + Math.Cos(theta) * (py3 - oy) + oy)));
            points.Add(new PolygonIntersection.Vector((int)(Math.Cos(theta) * (px4 - ox) - Math.Sin(theta) * (py4 - oy) + ox), (int)(Math.Sin(theta) * (px4 - ox) + Math.Cos(theta) * (py4 - oy) + oy)));
            /* p'x = cos(theta) * (px-ox) - sin(theta) * (py-oy) + ox
             p'y = sin(theta) * (px-ox) + cos(theta) * (py-oy) + oy
             */

            PolygonIntersection.Polygon polygon = new PolygonIntersection.Polygon(points);
            //game.Spawn(new SwordSlash(game, user, Static.PIXEL_THIN, slashBounds, damage, damageType, 10, user.vectorDirection), slashBounds.Left, slashBounds.Top);
            Arrow attack = new Arrow(game, user, Seizonsha.spriteMappings[Static.SPRITE_FIREBALL], this, slashBounds, polygon, damage, damageType, 20, new Vector2(bufferedVectorDirection.X * 50, bufferedVectorDirection.Y * 50), bufferedDirection, false);
            attack.rotateToAngle(this.bufferedDirection);

            game.Spawn(attack, slashBounds.Left, slashBounds.Top);
            if (!unlimitedAmmo)
            {
                this.ammo--;
            }

        }

        public override string getDescription()
        {
            return "A Bow";
        }

        public override bool Available()
        {
            if (ammo == 0)
            {
                return false;
            }
            return base.Available();
        }

    }
}
