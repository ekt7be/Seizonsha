using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public class Level
    {
        private TileMap map;
        private Dictionary<int, Texture2D> tileSprites;
        private Seizonsha game;

        public Level(Seizonsha game)
        {
            this.game = game;
            this.map = new TileMap(this, Static.TILES_ON_SCREEN_X, Static.TILES_ON_SCREEN_Y);
            this.tileSprites = new Dictionary<int, Texture2D>();
            initialize();
        }

        private void initialize()
        {

            Texture2D tileRect = new Texture2D(game.GraphicsDevice, Static.TILE_HEIGHT, Static.TILE_WIDTH);
            Color[] tileData = new Color[Static.TILE_HEIGHT * Static.TILE_WIDTH];
            for (int i = 0; i < tileData.Length; ++i)
            {
                tileData[i] = Color.Blue;
            }

            tileRect.SetData(tileData);


            Texture2D obstacleRect = new Texture2D(game.GraphicsDevice, Static.TILE_HEIGHT, Static.TILE_WIDTH);

            Color[] obstacleData = new Color[Static.TILE_HEIGHT * Static.TILE_WIDTH];
            for (int i = 0; i < obstacleData.Length; ++i)
            {
                obstacleData[i] = Color.White;
            }

            obstacleRect.SetData(obstacleData);

            tileSprites.Add(Static.TILE_OBSTACLE, obstacleRect);
            tileSprites.Add(Static.TILE_NOT_OBSTACLE, tileRect);

        }

        public void Draw(SpriteBatch spriteBatch, int cameraX, int cameraY)
        {
            map.Draw(spriteBatch, cameraX, cameraY);
 
        }

        public Texture2D getTileSprite(int tileType)
        {
            if (tileSprites.ContainsKey(tileType)){
                return tileSprites[tileType];
            } else{
                return null;
            }
        }

        public Tile getTile(int horz, int vert)
        {
            return map.tiles[horz, vert];
        }

        public int GetTilesHorizontal()
        {
            return map.GetTilesHorizontal();
        }

        public int GetTilesVertical()
        {
            return map.GetTilesVertical();
        }
    }
}
