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
		List<Tile> wallTiles2; 
		Seizonsha game;

		public TileMap(Level level, Seizonsha game)
        {
            this.level = level;
			this.game = game; 

			wallTiles = new List<int>(new int[] {1033, 1157});	// look at map.txt and set the wall tile numbers
			wallTiles2 = new List<Tile>(); 

			using (StreamReader reader = new StreamReader(@"Content/maps/map5.txt"))
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
				Static.TILE_WIDTH = info[3];									// tile height

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

                            if (tileType == Static.SPAWN_POINT_DOWN || tileType == Static.SPAWN_POINT_UP || tileType == Static.SPAWN_POINT_RIGHT || tileType == Static.SPAWN_POINT_LEFT)
                            {
								SpawnTile spawnTile = new SpawnTile(tileType,i * Static.TILE_WIDTH, j * Static.TILE_WIDTH);
								spawnTile.capacity = 0; 
								level.AddSpawnPoint(spawnTile);
                                tiles[i, j] = spawnTile;
                            }
							else if (wallTiles.Contains(tileType)) {
                                tiles[i, j] = new Tile(i * Static.TILE_WIDTH, j * Static.TILE_WIDTH, true, tileType);
								tiles[i, j].capacity = 0; 
								wallTiles2.Add(tiles[i, j]);
								}
							else {
                                tiles[i, j] = new Tile(i * Static.TILE_WIDTH, j * Static.TILE_WIDTH, false, tileType);
								tiles[i, j].capacity = 0; 
								tiles[i, j].capacityBounds = tiles[i, j].bounds; 
							}
						}
                    }
					//System.Console.Write("\n");
                }
                readMap();
            }
        }

        private void readMap()
        {
			for (int j = 0; j < this.tilesVert; j++)
			{
				for (int i = 0; i < tilesHorz; i++) 
				{
					Tile tile = tiles[i,j]; 
					if (!tile.isObstacle()) {

						bool hitWall = false; 

						while(!hitWall) {
							tile.capacity += 1; 
							tile.capacityBounds.Width += 32; 
							tile.capacityBounds.Height += 32; 

							foreach(Tile wallTile in wallTiles2) {
								if (tile.capacityBounds.Intersects(wallTile.bounds)) {
									hitWall = true; 
								}
							}
						}

				
					}
				}

			}

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
