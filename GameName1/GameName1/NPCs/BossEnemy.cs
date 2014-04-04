using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.NPCs
{
    abstract class BossEnemy : GameEntity
    {
        public BossEnemy(Seizonsha game, Texture2D sprite, int width, int height)
            : base(game, sprite, width, height, Static.DAMAGE_TYPE_ENEMY, 20)
        {
            setXPReward(1000);
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            base.Update(gameTime);
        }

        protected override void OnDie()
        {
            game.Spawn(new Food(game), x, y);
        }

        public override void OnSpawn()
        {
        }

        public override void collideWithWall()
        {
        }

        public override void collide(GameEntity entity)
        {
        }
    }
}
