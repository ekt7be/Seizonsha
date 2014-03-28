using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Effects
{
    abstract public class Effect : GameEntity
    {

        protected int duration;

        public Effect(Seizonsha game, Texture2D sprite, int width, int height, int duration)
            : base(game, sprite,width, height, Static.TARGET_TYPE_NOT_DAMAGEABLE, 0)
        {
            this.duration = duration;
            setCollidable(false);
        }

        public override void Update(GameTime gameTime)
        {
            duration--;
            if (duration <= 0)
            {
                setRemove(true);
            }
        }

        abstract protected override void OnDie();

        abstract public override void OnSpawn();

        public override void collideWithWall()
        {
            //no collisions..
        }

        public override void collide(GameEntity entity)
        {
            //no collisions..
        }
    }
}
