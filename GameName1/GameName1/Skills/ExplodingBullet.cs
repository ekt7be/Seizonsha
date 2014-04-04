using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class ExplodingBullet : Bullet
    {
        Skill origin;


        public ExplodingBullet(Seizonsha game, GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, float bulletSpeed, float directionAngle)
            : base(game, user, sprite, bounds, amount, damageType, bulletSpeed, directionAngle)
        {
            this.origin = origin;

        }




        public override void collide(GameEntity entity)
        {

            if (damageType == Static.TARGET_TYPE_FRIENDLY && entity.getTargetType() == Static.TARGET_TYPE_FRIENDLY)
            {

            }
            else
            {
                int explosionWidth = 80;
                int explosionHeight = 80;
                Rectangle slashBounds = new Rectangle((int)(entity.getCenterX() - explosionWidth / 2), (int)(entity.getCenterY() - explosionWidth / 2), explosionWidth, explosionHeight);
                game.Spawn(new AOECone(game, user, sprite, this.origin, slashBounds, amount, this.damageType, 10), slashBounds.Left, slashBounds.Top);
                setRemove(true);
            }

        }

        public override void collideWithWall()
        {
            int explosionWidth = 80;
            int explosionHeight = 80;
            Rectangle slashBounds = new Rectangle((int)(getCenterX() - explosionWidth / 2), (int)(getCenterY() - explosionWidth / 2), explosionWidth, explosionHeight);
            game.Spawn(EntityFactory.getAOECone(game, user, sprite, this.origin, slashBounds, amount, this.damageType, 10), slashBounds.Left, slashBounds.Top);
            setRemove(true);
        }



        public override string getName()
        {
            return Static.TYPE_EXPLODING_BULLET;
        }


        public void reset(GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, float bulletSpeed, float directionAngle)
        {
            base.reset(user, sprite, bounds, amount, damageType, bulletSpeed, directionAngle);
            this.origin = origin;
        }

    }
}
