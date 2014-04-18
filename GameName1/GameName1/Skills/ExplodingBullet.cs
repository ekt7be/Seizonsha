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


        public ExplodingBullet(Seizonsha game, Skill origin, Texture2D sprite, Rectangle bounds, int amount, int damageType, float bulletSpeed, float directionAngle)
            : base(game, origin, sprite, bounds, amount, damageType, bulletSpeed, directionAngle)
        {

        }




        public override void collide(GameEntity entity)
        {
            
            if (damageType == Static.TARGET_TYPE_GOOD && entity.getTargetType() == Static.TARGET_TYPE_GOOD)
            {

            }
            else
            {
                int explosionWidth = 80;
                int explosionHeight = 80;
                Rectangle slashBounds = new Rectangle((int)(entity.getCenterX() - explosionWidth / 2), (int)(entity.getCenterY() - explosionWidth / 2), explosionWidth, explosionHeight);
                game.Spawn(EntityFactory.getAOECone(game, sprite, this.origin, slashBounds, amount, this.damageType, 10, 1f), slashBounds.Left, slashBounds.Top);
                setRemove(true);
                if (origin is Fireball)
                {
                    game.fireballHitSound.Play();//playsound
                }
            }
        }

        public override void collideWithWall()
        {
            int explosionWidth = 80;
            int explosionHeight = 80;
            Rectangle slashBounds = new Rectangle((int)(getCenterX() - explosionWidth / 2), (int)(getCenterY() - explosionWidth / 2), explosionWidth, explosionHeight);
            game.Spawn(EntityFactory.getAOECone(game, sprite, this.origin, slashBounds, amount, this.damageType, 10, 1f), slashBounds.Left, slashBounds.Top);
            setRemove(true);
            if (origin is Fireball)
            {
                game.fireballHitSound.Play();//playsound
            }
        }



        public override string getName()
        {
            return Static.TYPE_EXPLODING_BULLET;
        }



    }
}
