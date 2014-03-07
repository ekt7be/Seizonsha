using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public abstract class GameObject
    {
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update();


        public int x;
        public int y;
        public int width;
        public int height;

        protected Rectangle hitbox;

        public bool remove;




        public GameObject(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.remove = false;
            hitbox = new Rectangle(x, y, width, height);
        }


        public void setRemove(bool remove)
        {
            this.remove = remove;
        }

    }
}
