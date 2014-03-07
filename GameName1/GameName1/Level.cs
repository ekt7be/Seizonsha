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
        private Game1 game;

        public Level(Game1 game)
        {
            this.map = new TileMap(game, Game1.TilesOnScreenX, Game1.TilesOnScreenY);
            this.game = game;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
 
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
