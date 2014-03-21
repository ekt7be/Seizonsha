using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class TileMap
    {

        public Tile[,] tiles;
        public Level level;
        private int tilesHorz;
        private int tilesVert;

        public TileMap(Level level, int tilesHorz, int tilesVert)
        {
            this.tilesHorz = tilesHorz;
            this.tilesVert = tilesVert;
            this.level = level;

            tiles = new Tile[tilesHorz,tilesVert];

            for (int i = 0; i < tilesHorz; i++)
            {
                for (int j = 0; j < tilesVert; j++)
                {
                    if (j > tilesVert * 3 / 4 || (i < tilesHorz / 2 && j > tilesVert / 2))
                    {
                        //tiles[i, j] = new Tile(Static.TILE_NOT_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, false);

                        tiles[i, j] = new Tile(Static.TILE_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, true);
                    }
                    else
                    {
                        tiles[i, j] = new Tile(Static.TILE_NOT_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, false);

                    }                    
                }
            }
             

           // readMap();
        }

        private void readMap()
        {
            int j = 0;
            int i = 0;
            string[] lines = System.IO.File.ReadAllLines(@"c:\windows\desktop\WriteLines2.txt");
            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    if(c.Equals("0"))
                        tiles[i, j] = new Tile(Static.TILE_NOT_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, true);
                    else
                        tiles[i, j] = new Tile(Static.TILE_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, true);
                    j++;
                }
                i++;
            }

        }

        public void Draw(SpriteBatch spriteBatch, int cameraX, int cameraY)
        {
            for (int i = 0; i < tilesHorz; i++)
            {
                for (int j = 0; j < tilesVert; j++)
                {
                    tiles[i, j].Draw(spriteBatch, level.getTileSprite(tiles[i,j].getType()), cameraX, cameraY);
                }
            }
        }

        public int GetTilesHorizontal(){
            return tilesHorz;
        }

        public int GetTilesVertical()
        {
            return tilesVert;
        }
    }



}
