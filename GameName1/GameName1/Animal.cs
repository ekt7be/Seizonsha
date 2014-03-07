using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public class Animal : GameEntity
    {

        private int health;
        public Color SkinColor;
        public double stealth;
        public Boolean crouched;
        public static readonly int ANIMAL_WIDTH = Game1.TileWidth*6;
        public static readonly int ANIMAL_HEIGHT = Game1.TileHeight*4;
        public static readonly int ANIMAL_CROUCH_WIDTH = ANIMAL_WIDTH;
        public static readonly int ANIMAL_CROUCH_HEIGHT = Game1.TileHeight*2;

        public static readonly double ANIMAL_MAX_COLOR_DIFFERENCE = Math.Sqrt(Math.Pow(255, 2) + Math.Pow(255, 2) + Math.Pow(255, 2));

        public Animal(Game1 game, Texture2D sprite, int x, int y, Color skinColor) : base(game, sprite, x, y, ANIMAL_WIDTH, ANIMAL_HEIGHT)
        {
            this.SkinColor = skinColor;
            this.health = 100;
            crouched = false;
            stealth = 0;
            
        }

        public override void Update()
        {


            stealth = calculateStealth();

            base.Update();

        }


        public void Crouch()
        {
            if (!crouched && !falling)
            {
                crouched = true;
                this.width = ANIMAL_CROUCH_WIDTH;
                this.height = ANIMAL_CROUCH_HEIGHT;
                this.y = y + (ANIMAL_HEIGHT-ANIMAL_CROUCH_HEIGHT);
                hitbox = new Rectangle(x,y,width,height);
            }
        }

        public void Uncrouch()
        {
            crouched = false;
            this.width = ANIMAL_WIDTH;
            this.height = ANIMAL_HEIGHT;
            this.y = y - (ANIMAL_HEIGHT - ANIMAL_CROUCH_HEIGHT);
            hitbox = new Rectangle(x,y,width,height);
        }

        public void Jump()
        {
            if (!falling)
            {
                if (crouched)
                {
                    Uncrouch();
                }
                velocityY = -18;
                falling = true;
            }
        }

        public void MoveLeft()
        {
            base.move(-5, 0);
        }

        public void MoveRight()
        {
            base.move(5, 0);
        }



        private double calculateStealth()
        {
            //find tiles underneath
            double sumStealth = 0.0;
            int numTiles = 0;

            int bottomTileIndex = this.getBottomEdgeTileIndex();

            for (int i = this.getLeftEdgeTileIndex(); i <= this.getRightEdgeTileIndex(); i++)
            {
                numTiles++;
                double tileStealth = 0.0;

                if (!(bottomTileIndex + 1 > game.currLevel.GetTilesVertical())){ //not bottom boundary

     
                    Tile currTile = game.currLevel.getTile(i, bottomTileIndex + 1);

                    if (!currTile.isObstacle())
                    { //not obstacle
                        tileStealth = 0.0;
                    }
                    else
                    {
                        
                        int tileR = currTile.getColor().R;
                        int tileG = currTile.getColor().G;
                        int tileB = currTile.getColor().B;
                         

                        double tileColorDiff = Math.Sqrt(Math.Pow(tileR - SkinColor.R, 2) + Math.Pow(tileG - SkinColor.G, 2) + Math.Pow(tileB - SkinColor.B, 2));
                        tileStealth = 1 - tileColorDiff / ANIMAL_MAX_COLOR_DIFFERENCE;
                        //tileStealth = Game1.ColorDistance(SkinColor, currTile.getColor());

             
                    }
                }

                sumStealth += tileStealth;
            }

            double stealthPercent = sumStealth / numTiles;


            return stealthPercent;


        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(sprite, hitbox, SkinColor);
        }
         

    }
}
