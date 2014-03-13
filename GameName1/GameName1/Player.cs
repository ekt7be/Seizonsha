using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public class Player : GameEntity
    {

        public Rectangle screen;

        public override void Update()
        {

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

        }

        public Player(Game1 game, Texture2D sprite, int x, int y) : base(game, sprite, x, y, Static.PLAYER_WIDTH, Static.PLAYER_HEIGHT)
        {
            this.screen = new Rectangle(0, 0, Static.SCREEN_WIDTH, Static.SCREEN_HEIGHT); // this will change depending on number of players
        }

        public void DrawScreen(SpriteBatch spriteBatch)
        {
            //TODO:  Everything will be drawn through this so we can have split screen
            //We need to use the spritebatch.draw method with scaling, and we will probably need to implement that 
            //for every drawable class we have so that drawing can adjust for different screen sizes from splitscreen
        }

        public void MoveUp()
        {
            this.move(0, -10);
        }
        public void MoveDown()
        {
            this.move(0, 10);
        }
        public void MoveLeft()
        {
            this.move(-10, 0);
        }
        public void MoveRight()
        {
            this.move(10, 0);
        }
    }
}
