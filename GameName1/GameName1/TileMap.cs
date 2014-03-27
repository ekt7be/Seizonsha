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
		List<int> wallTiles; 

        public TileMap(Level level)
        {
            this.level = level;

			wallTiles = new List<int>(new int[] {1033, 1157});	// look at map.txt and set the wall tile numbers

			using (StreamReader reader = new StreamReader(@"Content/maps/map4.txt"))
            {
				string mapText;
				List<int> info = new List<int>();

				// non-grid lines
				for (int x = 0; x < 12; x++) {
					mapText = reader.ReadLine();

					string[] words = mapText.Split('=');

					foreach (string word in words) {
						int n; 
						if (int.TryParse(word, out n)) {
							// System.Console.WriteLine (word);
							info.Add(n); 
						}
					}
				}
																		// see map.txt file
				this.tilesHorz = Static.TILES_ON_SCREEN_X = info[0]; 			// number of y tiles
				this.tilesVert = Static.TILES_ON_SCREEN_Y = info[1]; 			// number of x tiles
				Static.TILE_WIDTH = info[2]; 									// tile width
				Static.TILE_HEIGHT = info[3];									// tile height

				tiles = new Tile[tilesHorz, tilesVert];

				// start reading grid of numbers	
				for (int j = 0; j < this.tilesVert; j++)
                {
					//System.Console.WriteLine(this.tilesVert);
                    mapText = reader.ReadLine();
					//System.Console.WriteLine(mapText);

					string[] nums = mapText.Trim().Split(',');
					//System.Console.WriteLine(nums.Length + "\n");

					for (int i = 0; i < tilesHorz; i++) 
                    {
						//System.Console.Write(i + " : " + nums[i] + "\n");
						int m; 
						if (int.TryParse(nums[i], out m)) {
							int tileType = Convert.ToInt32(nums[i]); 

							if (wallTiles.Contains(tileType))
								tiles[i, j] = new Tile(Static.TILE_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, true, tileType);
							else
								tiles[i, j] = new Tile(Static.TILE_NOT_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, false, tileType);
						}
                    }
					//System.Console.Write("\n");
                }
                readMap();
            }
        }

        private void readMap()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, int cameraX, int cameraY)
        {

			//System.Console.WriteLine (tilesHorz);
			//System.Console.WriteLine (tilesVert);

			for (int i = 0; i < this.tilesHorz; i++)
            {
				for (int j = 0; j < this.tilesVert; j++)
                {
					// System.Console.WriteLine(tiles[i,j].tileID); 
					tiles[i, j].Draw(spriteBatch, level.getTileSprite(tiles[i,j].tileType), cameraX, cameraY);
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
