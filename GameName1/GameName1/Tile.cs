using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{

    public class Tile
    {

        private bool obstacle;
        public int x;
        public int y;
        private List<GameEntity> touching;
        public Rectangle bounds;
		public int tileType;

		public Tile (int x, int y, bool obstacle, int tileType){
            this.x = x;
            this.y = y;
            this.obstacle = obstacle;
            this.touching = new List<GameEntity>();
            this.bounds = new Rectangle(x, y, Static.TILE_WIDTH, Static.TILE_HEIGHT);
			this.tileType = tileType; 
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D sprite, int cameraX, int cameraY)
		{
            if (sprite == null)
            {
                return;
            }
                spriteBatch.Draw(sprite, new Rectangle(x - cameraX, y - cameraY, Static.TILE_WIDTH, Static.TILE_HEIGHT), Color.White);
        }

        public void BindEntity(GameEntity entity, bool bind)
        {
            if (bind)
            {
                touching.Add(entity);
            }
            else
            {
                touching.Remove(entity);
            }
        }

        public bool isObstacle()
        {
            return obstacle;
        }

        public void setObstacle(bool obstacle)
        {
            this.obstacle = obstacle;
        }

        public int getType()
        {
            return tileType;
        }
        public Rectangle getBounds()
        {
            return bounds;
        }

        public List<GameEntity> getEntities()
        {
            return touching;
        }

    }
}
