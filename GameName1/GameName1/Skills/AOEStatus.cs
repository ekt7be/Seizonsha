using GameName1.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Skills
{
    class AOEStatus : GameName1.Effects.Effect
    {
        private GameEntity user;
        private Skill origin;
        private int time;
        private int frequency;



        public AOEStatus(Seizonsha game, GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int duration, float depth, int frequency)
            : base(game, sprite, bounds.Width, bounds.Height, duration)
        {
            this.user = user;
            this.origin = origin;
            this.depth = depth;
            this.frequency = frequency;
        }

        protected override void OnDie()
        {

        }

        public override void OnSpawn()
        {
            //game.affectArea(origin, this.hitbox);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Static.PIXEL_THIN, this.hitbox, null, tint, 0, new Vector2(0, 0), SpriteEffects.None, depth);
            base.Draw(spriteBatch);
        }

        


        public override void Update(GameTime gameTime)
        {
            if(time % frequency == 0){
                game.affectArea(origin, this.hitbox);
            }
            time++;

            base.Update(gameTime);
        }

        public override string getName()
        {
            return Static.TYPE_AOE_STATUS;
        }


        public void reset(GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int duration)
        {
            base.reset(duration);
            this.user = user;
            setSprite(sprite);
            this.origin = origin;
            this.width = bounds.Width;
            this.height = bounds.Height;


        }




    }
}
