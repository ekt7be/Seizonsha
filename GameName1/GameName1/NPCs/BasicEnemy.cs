using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.NPCs
{
    class BasicEnemy : GameEntity, AI
    {

        int count = 0;

        public BasicEnemy(Seizonsha game, Texture2D sprite, int x, int y, int width, int height)
            : base(game, sprite, x, y, width, height, Static.DAMAGE_TYPE_ENEMY, 200)
        {
     
        }

        public void AI()
        {
            //just sits there
        }

        public override void collide(GameEntity entity)
        {
          //  Static.Debug("NPC collision with entity");
			if(entity.getTargetType() == Static.TARGET_TYPE_FRIENDLY){
				entity.damage(2, Static.DAMAGE_TYPE_ENEMY);
			}
        }

        public override void collideWithWall()
        {
        }

        public override void OnSpawn()
        {
            this.velocityX = 0;
            this.velocityY = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //draw armor and weapons equipped etc

        }

        public override void Update()
        {

            List<Player> players = game.getPlayers();

            Player closest = null;
            double closestDistance = Double.PositiveInfinity;


            foreach (Player p in players)
            {
                if (p == null)
                    continue;
                if (Math.Sqrt(Math.Pow(p.x - this.x, 2) + Math.Pow(p.y - this.y, 2)) < closestDistance)
                    closest = p;
            }

            if (closest == null)
                return;

            this.hitbox = new Rectangle(this.x, this.y, this.width, this.height);
            if (closest.x < this.x)
                this.velocityX = -1;
            if (closest.x > this.x)
                this.velocityX = 1;
            if (closest.y < this.y)
                this.velocityY = -1;
            if (closest.y > this.y)
                this.velocityY = 1;
          

        }

        protected override void OnDie()
        {
            count++;
        }

    }
}
