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
			using (StreamReader reader = new StreamReader(@"Content/map2.txt"))
            {
                int count = 0;

				string mapText;
				List<int> info = new List<int>();
				// burn useless info
				for (int x = 0; x < 12; x++) {

					mapText = reader.ReadLine();

					string[] words = mapText.Split('=');

					foreach (string word in words) {
						int n; 
						if (int.TryParse (word, out n)) {
							System.Console.WriteLine (word);
							info.Add(n); 
						}
					}
				}

				this.tilesVert = info [1];
				this.tilesHorz = info [0];
					
				for (int j = 0; j < this.tilesVert; j++)
                {
					System.Console.WriteLine(this.tilesVert);

                    mapText = reader.ReadLine();
					System.Console.WriteLine(mapText);

					mapText = mapText.Trim (); 

					string[] nums = mapText.Split(',');
					System.Console.WriteLine(nums.Length + "\n");


					for (int i = 0; i < nums.Length; i++) 
                    {
						System.Console.Write(i + " : " + nums[i] + "\n");

                        count++;

						int m; 
						if (int.TryParse (nums[i], out m)) {
							if(nums[i][0].Equals('0'))
								tiles[i, j] = new Tile(Static.TILE_NOT_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, false);
							else
								tiles[i, j] = new Tile(Static.TILE_OBSTACLE, i * Static.TILE_WIDTH, j * Static.TILE_HEIGHT, true);
						}


                    }
					System.Console.Write("\n");

                }
                readMap();
            }
        }

        private void readMap()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, int cameraX, int cameraY)
        {

			System.Console.WriteLine (tilesHorz);
			System.Console.WriteLine (tilesVert);


			for (int i = 0; i < this.tilesHorz; i++)
            {
				for (int j = 0; j < this.tilesVert; j++)
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
