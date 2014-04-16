using GameName1.Skills;
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
        protected Skill origin;
        protected Seizonsha game;
        protected Texture2D sprite;
        protected int duration;
        protected int time;

        public StatusEffect(Seizonsha game, GameEntity user, Skill origin, Texture2D sprite, GameEntity afflicted, int duration)
        {
            this.user = user;
            this.game = game;
            this.origin = origin;
            this.sprite = sprite;
            this.afflicted = afflicted;
            this.duration = duration;
            this.time = 0;
        }

        public Skill getOrigin()
        {
            return this.origin;
        }

        public virtual void onEnd()
        {
        }
        public virtual void onCreate()
        {
        }


        public virtual void Update()
        {
            time++;
            if (time >= duration)
            {
                afflicted.removeStatusEffect(this);
                this.onEnd();
            }
        }


        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
