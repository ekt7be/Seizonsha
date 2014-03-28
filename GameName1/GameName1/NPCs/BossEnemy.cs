using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.NPCs
{
    class BossEnemy : GameEntity
    {
        public BossEnemy(Seizonsha game, Texture2D sprite, int x, int y, int width, int height)
            : base(game, sprite, x, y, width, height, Static.DAMAGE_TYPE_ENEMY, 20)
        {
     
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        protected override void OnDie()
        {
            throw new NotImplementedException();
        }

        public override void OnSpawn()
        {
            throw new NotImplementedException();
        }

        public override void collideWithWall()
        {
            throw new NotImplementedException();
        }

        public override void collide(GameEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
