using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameName1
{
    class FrameAnimation : ICloneable
    {

        // The first frame of the animation.
        private Rectangle rectInitialFrame;

        // Number of frames in the Animation
        private int frames = 1;

        // The current frame. Ranged from 0 to frames-1
        private int currentFrame = 0;

        // Time between each frame
        private float delay = 200f;

        // Elapsed time since the last animation
        private float elapsed = 0.0f;

        // Number of times the frame has been player
        private int playCount = 0;

        // The animation that should be player after this animation
        private string nextAnimation;

        // Constructors
        public FrameAnimation(Rectangle FirstFrame, int numberOfFrames)
        {
            rectInitialFrame = FirstFrame;
            frames = numberOfFrames;
        }

        public FrameAnimation(int x, int y, int width, int height, int numberOfFrames)
        {
            rectInitialFrame = new Rectangle(x, y, width, height);
            frames = numberOfFrames;
        }

        public FrameAnimation(int x, int y, int width, int height, int numberOfFrames, float frameLength)
        {
            rectInitialFrame = new Rectangle(x, y, width, height);
            frames = numberOfFrames;
            delay = frameLength;
        }

        public FrameAnimation(int x, int y, int width, int height, int numberOfFrames, float frameLength, string NextAnimation)
        {
            rectInitialFrame = new Rectangle(x, y, width, height);
            frames = numberOfFrames;
            delay = frameLength;
            nextAnimation = NextAnimation;
        }


        // Methods
        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed > delay)
            {
                elapsed = 0.0f;
                currentFrame = (currentFrame + 1) % frames;
                if (currentFrame == 0)
                    playCount = (int)MathHelper.Min(playCount + 1, int.MaxValue);
            }
        }

        object ICloneable.Clone()
        {
            return new FrameAnimation(this.rectInitialFrame.X, this.rectInitialFrame.Y,
                                      this.rectInitialFrame.Width, this.rectInitialFrame.Height,
                                      this.frames, this.delay, nextAnimation);
        }

        // Number of frames in the animation
        public int FrameCount
        {
            get { return frames; }
            set { frames = value; }
        }

        // Time to display each frame
        public float FrameLength
        {
            get { return delay; }
            set { delay = value; }
        }

        // The frame currently being displayed
        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public int FrameWidth
        {
            get { return rectInitialFrame.Width; }
        }

        public int FrameHeight
        {
            get { return rectInitialFrame.Height; }
        }

        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(
                                    rectInitialFrame.X + (rectInitialFrame.Width * currentFrame),
                                    rectInitialFrame.Y, rectInitialFrame.Width, rectInitialFrame.Height);
            }
        }

        public int PlayCount
        {
            get { return playCount; }
            set { playCount = value; }
        }

        public string NextAnimation
        {
            get { return nextAnimation; }
            set { nextAnimation = value; }
        }




    }
}
