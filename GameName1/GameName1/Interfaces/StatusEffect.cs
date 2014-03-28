using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Interfaces
{
    public abstract class StatusEffect
    {
        protected GameEntity user;
        protected GameEntity afflicted;
        protected Seizonsha game;
        protected Texture2D sprite;
        protected int duration;
        protected int time;

        public StatusEffect(Seizonsha game, GameEntity user, Texture2D sprite, GameEntity afflicted, int duration)
        {
            this.user = user;
            this.game = game;
            this.sprite = sprite;
            this.afflicted = afflicted;
            this.duration = duration;
            this.time = 0;
        }



        public virtual void Update()
        {
            time++;
            if (time >= duration) afflicted.removeStatusEffect(this);
        }


        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
