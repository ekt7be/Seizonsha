
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameName1
{

    class AnimatedSprite
    {
        public Texture2D texture { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private Rectangle sourceRectangle;
        private Rectangle destRectangle;
        private int width;
        private int height;
        private int currColumn;
        private int currRow;

        public AnimatedSprite(Texture2D atlas, int rows, int columns)
        {
            this.texture = atlas;
            this.rows = rows;
            this.columns = columns;
            this.currentFrame = 0;
            this.totalFrames = rows * columns;

            this.width = texture.Width / columns;
            this.height = texture.Height / rows;

            this.sourceRectangle = new Rectangle();
            sourceRectangle.Width = width;
            sourceRectangle.Height = height;
            this.destRectangle = new Rectangle();
            destRectangle.Width = width;
            destRectangle.Height = height;
                    
            this.currColumn = 0;
            this.currRow = 0;


        }

        public void Update()
        {
            currentFrame++;
            if (currentFrame > totalFrames)
            {
                currentFrame = 0;
            }
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            currRow = (int)((float)currentFrame / (float)columns);
            currColumn = currentFrame % columns;

            sourceRectangle.X = width * currColumn;
            sourceRectangle.Y = height * currRow;
            destRectangle.X = (int)location.X;
            destRectangle.Y = (int)location.Y;

            spriteBatch.Draw(texture, destRectangle, sourceRectangle, Color.White);


        }




    }
}
