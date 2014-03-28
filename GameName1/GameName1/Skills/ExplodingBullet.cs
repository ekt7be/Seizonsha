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


        public ExplodingBullet(Seizonsha game, GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, int duration, float bulletSpeed, Vector2 alexDirection)
            : base(game, user, sprite, bounds, amount, damageType, duration, bulletSpeed, alexDirection)
        {
            this.origin = origin;

        }




        public override void collide(GameEntity entity)
        {

            if (damageType == Static.TARGET_TYPE_FRIENDLY && entity.getTargetType() == Static.TARGET_TYPE_FRIENDLY) ;
            else
            {
                int explosionWidth = 80;
                int explosionHeight = 80;
                Rectangle slashBounds = new Rectangle((int)(entity.getCenterX() - explosionWidth / 2), (int)(entity.getCenterY() - explosionWidth / 2), explosionWidth, explosionHeight);
                game.Spawn(new AOECone(game, user, game.getTestSprite(slashBounds, Color.Green), this.origin, slashBounds, amount, this.damageType, 10, entity.vectorDirection));
                setRemove(true);
            }

        }

        public override void collideWithWall()
        {
            int explosionWidth = 80;
            int explosionHeight = 80;
            Rectangle slashBounds = new Rectangle((int)(getCenterX() - explosionWidth / 2), (int)(getCenterY() - explosionWidth / 2), explosionWidth, explosionHeight);
            game.Spawn(new AOECone(game, user, game.getTestSprite(slashBounds, Color.Green), this.origin, slashBounds, amount, this.damageType, 10, vectorDirection));
            setRemove(true);
        }


    }
}
