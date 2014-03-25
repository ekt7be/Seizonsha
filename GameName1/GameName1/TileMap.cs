using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
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

            tiles = new Tile[tilesHorz, tilesVert];


            //string filename = "map1.txt";
            using (StreamReader reader = new StreamReader(@"Content/map1.txt"))
            {
                int count = 0;

                for (int j = 0; j < tilesVert; j++)
                {
                    string mapText = reader.ReadLine();
                    for (int i = 0; i < tilesHorz; i++)
                    {
                        char c = mapText[i]; 

                        count++;
                        if(c.Equals('0'))
                            tiles[i, j] = new Tile(Static.TILE_NOT_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, false);
                        else
                            tiles[i, j] = new Tile(Static.TILE_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, true);
                    }
                }
                readMap();
            }
        }

        private void readMap()
        {
            
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
