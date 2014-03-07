using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{

    public class Tile
    {

        private Game1 game;
        private Texture2D sprite;
        private bool obstacle;
        private Color color;
        public int x;
        public int y;

        public Tile (Game1 game, Texture2D sprite, Color color, int x, int y, bool obstacle){
            this.x = x;
            this.y = y;
            this.obstacle = obstacle;
            this.game = game;
            this.sprite = sprite;
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

               spriteBatch.Draw(sprite, new Rectangle(x - game.cameraX, y, Game1.TileWidth, Game1.TileHeight), color);

        }

        public bool isObstacle()
        {
            return obstacle;
        }

        public void setObstacle(bool obstacle)
        {
            this.obstacle = obstacle;
        }

        public Color getColor()
        {
            return color;
        }

    }
}
