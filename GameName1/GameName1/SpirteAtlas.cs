
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameName1
{

    class SpriteAtlas
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


        public SpriteAtlas(Texture2D atlas, int rows, int columns)
        {
            this.texture = atlas;
            this.rows = rows;
            this.columns = columns;

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

        public void setSprite(int row, int col)
        {
            this.currRow = row;
            this.currColumn = col;
        }

        public void Update()
        {

        }


        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {

            sourceRectangle.X = width * currColumn;
            sourceRectangle.Y = height * currRow;
            destRectangle.X = (int)location.X;
            destRectangle.Y = (int)location.Y;

            spriteBatch.Draw(texture, destRectangle, sourceRectangle, Color.White);


        }




    }
}
