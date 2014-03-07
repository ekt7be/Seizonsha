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
        private int tilesHorz;
        private int tilesVert;

        public TileMap(Game1 game, int tilesHorz, int tilesVert){
            this.tilesHorz = tilesHorz;
            this.tilesVert = tilesVert;

            tiles = new Tile[tilesHorz,tilesVert];

            Texture2D rect = new Texture2D(game.GraphicsDevice, Game1.TileWidth, Game1.TileHeight);

            Color[] data = new Color[Game1.TileHeight*Game1.TileWidth];
            for (int i = 0; i < data.Length; ++i)
            {

                data[i] = Color.White;
                
            }
            rect.SetData(data);


            for (int i = 0; i < tilesHorz; i++)
            {
                for (int j = 0; j < tilesVert; j++)
                {
                    if (j > tilesVert/2 && i < tilesHorz / 4)
                    {
                        tiles[i, j] = new Tile(game, rect, Color.BurlyWood, i * Game1.TileWidth, j * Game1.TileHeight, true);
                    }
                    else if (j > tilesVert * 3 / 4 || (i < tilesHorz / 2 && j > tilesVert / 2))
                    {
                        tiles[i, j] = new Tile(game, rect, Color.Green, i * Game1.TileWidth, j * Game1.TileHeight, true);
                    }
                    else
                    {
                        tiles[i, j] = new Tile(game, rect, Color.Blue, i * Game1.TileWidth, j * Game1.TileHeight, false);

                    }


                    
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tilesHorz; i++)
            {
                for (int j = 0; j < tilesVert; j++)
                {
                    tiles[i, j].Draw(spriteBatch);
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
