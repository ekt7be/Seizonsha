using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Squared.Tiled; 

namespace GameName1
{
    class TileMap
    {
        public Tile[,] tiles;
        public Level level;
		List<int> wallTiles; 
		HashSet<Tile> wallTiles2; 
		HashSet<Tile> groundTiles2; 

		Seizonsha game;
		Map map; 

		public TileMap(Level level, Seizonsha game)
        {
            this.level = level;
			this.game = game; 
			game.Content.RootDirectory = "Content";

			wallTiles = new List<int>(new int[] {1033, 1157});	// look at map.txt and set the wall tile numbers
			wallTiles2 = new HashSet<Tile>();
			groundTiles2 = new HashSet<Tile>();

			string mapname = "final"; 
		
			map = Map.Load(Path.Combine(game.Content.RootDirectory, "maps/"+mapname+".tmx"), game.Content);

			tiles = new Tile[map.Width, map.Height];

			//Console.WriteLine(map.Width + " x " + map.Height); 

			Static.TILES_ON_SCREEN_X = map.Width; 			
			Static.TILES_ON_SCREEN_Y = map.Height; 			
			Static.TILE_WIDTH = map.TileWidth; 				

			string line;
			System.IO.StreamReader file = new System.IO.StreamReader(Path.Combine(game.Content.RootDirectory, "maps/"+mapname+".txt"));

			//Console.WriteLine(Static.TILE_WIDTH); 

			while((line = file.ReadLine()) != null) {
				//Console.WriteLine(line);

				if (line.Split('=')[0] == "type") {
					string type = line.Split('=')[1]; 
					line = file.ReadLine(); 
					//Console.WriteLine("\n\n["+type+"]"); 

					for (int j = 0; j < map.Height; j++) {
						line = file.ReadLine();
						string[] nums = line.Trim().Split(',');
						//Console.WriteLine(); 

						for (int i = 0; i < map.Width; i++) {
							//Console.WriteLine(line); 

							int m;
							int tileNum = 0; 

							//Console.Write(nums[i] + " "); 
							//Console.WriteLine(i); 
							if (int.TryParse(nums[i], out m))
								tileNum = Convert.ToInt32(nums[i]);	// number that represents tile in .txt
					
							//Console.WriteLine(type);
							if (tileNum != 0) {

								if (type == "ground") {
									tiles[i, j] = new Tile(i * Static.TILE_WIDTH, j * Static.TILE_WIDTH, false, 11);	// not using tileType anymore
									tiles[i, j].capacity = 0; 
									tiles[i, j].capacityBounds = tiles[i, j].bounds; 
								}

								// decoration (trees and shit) is just any other higher non-collidable layer that isn't ground, wall, or special
								// this works because we can assume there will always be a ground tile underneath
									
								else if (type == "wall") {
									tiles[i, j] = new Tile(i * Static.TILE_WIDTH, j * Static.TILE_WIDTH, true, 11);
									tiles[i, j].capacity = 0; 
								}

								else if (type == "special") {
                                    if (tileNum == Static.SPAWN_POINT_LEFT)
                                    {
                                        SpawnTile spawnTile = new SpawnTile(Static.SPAWN_POINT_LEFT, i * Static.TILE_WIDTH, j * Static.TILE_WIDTH);
                                        spawnTile.capacity = 0;
                                        level.AddSpawnPoint(spawnTile);
                                        tiles[i, j] = spawnTile;
                                    }
                                    else if (tileNum == Static.SPAWN_POINT_RIGHT)
                                    {
                                        SpawnTile spawnTile = new SpawnTile(Static.SPAWN_POINT_RIGHT, i * Static.TILE_WIDTH, j * Static.TILE_WIDTH);
                                        spawnTile.capacity = 0;
                                        level.AddSpawnPoint(spawnTile);
                                        tiles[i, j] = spawnTile;
                                    }
                                    else if (tileNum == Static.SPAWN_POINT_UP)
                                    {
                                        SpawnTile spawnTile = new SpawnTile(Static.SPAWN_POINT_UP, i * Static.TILE_WIDTH, j * Static.TILE_WIDTH);
                                        spawnTile.capacity = 0;
                                        level.AddSpawnPoint(spawnTile);
                                        tiles[i, j] = spawnTile;
                                    }
                                    else if (tileNum == Static.SPAWN_POINT_DOWN)
                                    {
                                        SpawnTile spawnTile = new SpawnTile(Static.SPAWN_POINT_DOWN, i * Static.TILE_WIDTH, j * Static.TILE_WIDTH);
                                        spawnTile.capacity = 0;
                                        level.AddSpawnPoint(spawnTile);
                                        tiles[i, j] = spawnTile;
                                    }

								}
							}
						}
					}
				}
			}

			file.Close();

			for (int j = 0; j < map.Height; j++) {
				for (int i = 0; i < map.Width; i++) {
					if (!tiles[i,j].isObstacle()) {
						groundTiles2.Add(tiles[i,j]);
					}
					else {
						wallTiles2.Add(tiles[i, j]);
					}
				}
			}
				
			readMap();
		}

		private void readMap() {
			//Console.WriteLine("total tiles: " + (map.Width * map.Height)); 
			//Console.WriteLine("ground tiles: " + groundTiles2.Count); 
			//Console.WriteLine("wall tiles: " + wallTiles2.Count); 

			foreach(Tile groundTile in groundTiles2) {
				bool hitWall = false; 

				while(!hitWall) {
					groundTile.capacity += 1; 
					groundTile.capacityBounds.Width += Static.TILE_WIDTH; 
					groundTile.capacityBounds.Height += Static.TILE_WIDTH; 

					if (groundTile.capacity > (map.Width * map.Height)) 
						hitWall = true; 

					foreach(Tile wallTile in wallTiles2) {
						if (groundTile.capacityBounds.Intersects(wallTile.bounds))
							hitWall = true; 
						//Console.WriteLine("(" + groundTile.xIndex + ", " + groundTile.yIndex + ") " + groundTile.capacity + " | " + groundTile.capacityBounds + " -> " + wallTile.bounds + " : " + groundTile.capacityBounds.Intersects(wallTile.bounds)); 
					}
				}
			}
		}

        public void Draw(SpriteBatch spriteBatch, int cameraX, int cameraY)
        {

			//System.Console.WriteLine (tilesHorz);
			//System.Console.WriteLine (tilesVert);

			map.Draw(spriteBatch, new Rectangle(0, 0, map.Width * map.TileWidth, map.Height * map.TileHeight), new Vector2(0, 0));

			// draw clearance map 
			for (int i = 0; i < map.Width; i++) {
				for (int j = 0; j < map.Height; j++) {
					//tiles[i, j].Draw(spriteBatch, Static.PIXEL_THIN, cameraX, cameraY);
                }
            }

	
        }

        public int GetTilesHorizontal(){
			return map.Width;
        }

        public int GetTilesVertical()
        {
			return map.Height;
        }
    }



}
