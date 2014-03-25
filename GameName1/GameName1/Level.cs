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
        public static Dictionary<int, Texture2D> tileSprites = new Dictionary<int,Texture2D>();
        private Seizonsha game;

        public Level(Seizonsha game)
        {
            this.game = game;
			this.map = new TileMap(this, 80, 60);

            initialize();
        }

        private void initialize()
        {



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
            if (horz < 0 || horz > map.GetTilesHorizontal() - 1 || vert < 0 || vert > map.GetTilesVertical() - 1){
                return null;
            }
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

        public Rectangle getBounds()
        {
            return new Rectangle(0, 0, map.GetTilesHorizontal() * Static.TILE_WIDTH, map.GetTilesVertical() * Static.TILE_HEIGHT);
        }
    }
}
