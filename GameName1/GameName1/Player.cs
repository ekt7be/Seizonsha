using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public class Player : Animal
    {

        private Mate closestMate;

        private static readonly int MATE_REFRESH_FRAMES = 10;
        private static readonly int MATE_RADIUS = 4; //tiles to search

        private int mateRefresh;

        public override void Update()
        {
            closestMate = searchForMates();

            if (mateRefresh>0){
                mateRefresh--;
            }
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (closestMate != null)
            {
                spriteBatch.DrawString(game.GetSpriteFont(), "<3", new Vector2(x, y), Color.White);
            }
        }

        public Player(Game1 game, Texture2D sprite, int x, int y, Color skinColor) : base(game, sprite, x, y, skinColor)
        {
            this.mateRefresh = 0;
        
        }

        public void Mate()
        {
            if (closestMate != null && mateRefresh ==0 )
            {
                SkinColor = Game1.CombineColors(SkinColor, closestMate.SkinColor);
                mateRefresh = MATE_REFRESH_FRAMES;
                closestMate.setRemove(true);
                game.GenerateMate();
            }
        }

        public Mate searchForMates(){

            Mate returnMate = null;

            int leftRange = getLeftEdgeTileIndex() - MATE_RADIUS;
            if (leftRange < 0)
            {
                leftRange = 0;
            }

            int rightRange = getRightEdgeTileIndex() + MATE_RADIUS;
            if (rightRange > game.currLevel.GetTilesHorizontal() - 1)
            {
                rightRange = game.currLevel.GetTilesHorizontal() - 1;
            }

            Rectangle rangeRect = new Rectangle(leftRange * Game1.TileWidth, y, (rightRange - leftRange) * Game1.TileWidth, height);

            foreach (GameEntity entity in game.getEntities())
            {
                if (entity.GetType() == typeof(Mate)) //animal
                {
                    if (rangeRect.Intersects(entity.getHitbox()))
                    {
                        //within radius
                        if (returnMate == null)
                        {
                            returnMate = (Mate)entity;
                        }
                        else
                        {
                            int distanceToCurrent = Math.Abs(x - returnMate.x) + Math.Abs(x + width - (returnMate.x + returnMate.width));
                            int distanceToNew = Math.Abs(x - entity.x) + Math.Abs(x + width - (entity.x + entity.width));

                            if (distanceToNew < distanceToCurrent)
                            {
                                returnMate = (Mate)entity;
                            }
                        }
                    }
                }
            }

            return returnMate;

        }
    }
}
