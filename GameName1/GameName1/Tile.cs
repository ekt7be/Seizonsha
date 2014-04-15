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

		private bool drawCapacity = false;
		private bool drawIndex = false;

		// used in pathfinding
		public int xIndex;
		public int yIndex; 
		public Tile parent; 
		public int F = 0; 
		public int G = 0; 
		public int H = 0; 
		public int capacity; 
		public Rectangle capacityBounds; 

		public Tile (int x, int y, bool obstacle, int tileType){
			this.x = x;
			this.y = y;
			this.obstacle = obstacle;
			this.touching = new List<GameEntity>();
			this.bounds = new Rectangle(x, y, Static.TILE_WIDTH, Static.TILE_WIDTH);
			this.tileType = tileType; 

			this.xIndex = this.x / Static.TILE_WIDTH;
			this.yIndex = this.y / Static.TILE_WIDTH;
			this.parent = null; 
		}

		public void Draw(SpriteBatch spriteBatch, Texture2D sprite, int cameraX, int cameraY)
		{
			if (sprite == null)
			{
				return;
			}
			if (!this.isObstacle()) 
				spriteBatch.Draw(sprite, new Rectangle(x - cameraX, y - cameraY, Static.TILE_WIDTH, Static.TILE_WIDTH), new Color (Color.Black, 0.1f));
			else 
				spriteBatch.Draw(sprite, new Rectangle(x - cameraX, y - cameraY, Static.TILE_WIDTH, Static.TILE_WIDTH), new Color (Color.Red, 0.1f));

			if (drawCapacity) {
				spriteBatch.DrawString(
					Static.SPRITEFONT_Calibri14, 
					this.capacity + "", 
					new Vector2(this.x+2, this.y), 
					new Color (Color.Pink, 0.5f)
				);
			}

			if (drawIndex) {
				spriteBatch.DrawString(
					Static.SPRITEFONT_Calibri10, 
					xIndex + " " + yIndex, 
					new Vector2(this.x+2, this.y+16), 
					new Color (Color.Pink, 0.1f)
				);
			}
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

		public int getCenterX()
		{
			return x + Static.TILE_WIDTH / 2;
		}

		public int getCenterY()
		{
			return y + Static.TILE_WIDTH / 2;
		}


        public bool isTouching(GameEntity entity)
        {
            return touching.Contains(entity);
        }
	}
}
