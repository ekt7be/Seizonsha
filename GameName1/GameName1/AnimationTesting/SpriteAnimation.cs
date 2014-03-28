using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1
{
    class SpriteAnimation
    {

        Texture2D spriteSheet;

        // Is an animation being played?
        bool isAnimating = true;

        // Tint the sprite a color.
        Color colorTint = Color.White;

        // Screen position of the sprite
        Vector2 position = new Vector2(0, 0);
        Vector2 lastPosition = new Vector2(0, 0);

        // Dictionary holding all of the FrameAnimation objects associated w/ this sprite
        Dictionary<string, FrameAnimation> animations = new Dictionary<string, FrameAnimation>();

        // Whic FrameAnimation the dictionary above is playing
        string currentAnimation = null;

        // If true, the sprite will automatically rotate to align itself
        // with the angle difference between it's new position and
        // it's previous position.  In this case, the 0 rotation point
        // is to the right (so the sprite should start out facing to
        // the right.
        bool bRotateByPosition = false;

        // How much the sprite should be rotated by when drawn
        // Value is in Radians, and 0 indicates no rotation.
        float fRotation = 0f;

        // Calculated center of the sprite
        Vector2 center;

        // Calculated width and height of the sprite
        int spriteWidth;
        int spriteHeight;


        //Constructors
        public SpriteAnimation(Texture2D texture)
        {
            spriteSheet = texture;
        }

        // Methods

        public void Update(GameTime gameTime)
        {
            // Check if we're animating
            if (isAnimating)
            {
                // Make sure there isn't an active animation
                if (CurrentFrameAnimation == null)
                {
                    // Make sure we have an animation associated with this sprite
                    if (animations.Count > 0)
                    {
                        // Set the active animation to the first animation associated with this sprite
                        string[] keyArray = new string[animations.Count];
                        animations.Keys.CopyTo(keyArray, 0);
                        currentAnimation = keyArray[0];
                    }
                    else
                    {
                        return;
                    }
                }

                // Update the animation
                CurrentFrameAnimation.Update(gameTime);

                // Check to see if there is a "followup" animation named for this animation
                if (!String.IsNullOrEmpty(CurrentFrameAnimation.NextAnimation))
                {
                    // If there is, see if the currently playing animation has completed a full animation loop
                    if (CurrentFrameAnimation.PlayCount > 0)
                    {
                        // If it has, set up the next animation
                        currentAnimation = CurrentFrameAnimation.NextAnimation;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset)
        {
            if (isAnimating)
            {
                spriteBatch.Draw(spriteSheet, (position + new Vector2(xOffset, yOffset) + center),
                                CurrentFrameAnimation.FrameRectangle, colorTint,
                                fRotation, center, 1f, SpriteEffects.None, 0);
            }
        }

        public void AddAnimation(string name, int x, int y, int width, int height, int frames, float frameLength)
        {
            animations.Add(name, new FrameAnimation(x, y, width, height, frames, frameLength));
            spriteWidth = width;
            spriteHeight = height;
            center = new Vector2(spriteWidth / 2, spriteHeight / 2);
        }

        public void AddAnimation(string name, int x, int y, int width, int height, int frames, float frameLength, string nextAnimation)
        {
            animations.Add(name, new FrameAnimation(x, y, width, height, frames, frameLength, nextAnimation));
            spriteWidth = width;
            spriteHeight = height;
            center = new Vector2(spriteWidth / 2, spriteHeight / 2);
        }

        public FrameAnimation getAnimationByName(string name)
        {
            if (animations.ContainsKey(name))
            {
                return animations[name];
            }
            else
            {
                return null;
            }
        }

        public void MoveBy(int x, int y)
        {
            lastPosition = position;
            position.X += x;
            position.Y += y;
            UpdateRotation();
        }

        void UpdateRotation()
        {
            if (bRotateByPosition)
            {
                fRotation = (float)Math.Atan2(position.Y - lastPosition.Y, position.X - lastPosition.X);
            }
        }

        // Getters and setters
        public Vector2 Position
        {
            get { return position; }
            set
            {
                lastPosition = position;
                position = value;
                UpdateRotation();
            }
        }
 
        ///
        /// The X position of the sprite's upper left corner pixel.
        ///
        public int X
        {
            get { return (int)position.X; }
            set
            {
                lastPosition.X = position.X;
                position.X = value;
                UpdateRotation();
            }
        }
 
        ///
        /// The Y position of the sprite's upper left corner pixel.
        ///
        public int Y
        {
            get { return (int)position.Y; }
            set
            {
                lastPosition.Y = position.Y;
                position.Y = value;
                UpdateRotation();
            }
        }
 
        ///
        /// Width (in pixels) of the sprite animation frames
        ///
        public int Width
        {
            get { return spriteWidth; }
        }
 
        ///
        /// Height (in pixels) of the sprite animation frames
        ///
        public int Height
        {
            get { return spriteHeight; }
        }
 
        ///
        /// If true, the sprite will automatically rotate in the direction
        /// of motion whenever the sprite's Position changes.
        ///
        public bool AutoRotate
        {
            get { return bRotateByPosition; }
            set { bRotateByPosition = value; }
        }
 
        ///
        /// The degree of rotation (in radians) to be applied to the
        /// sprite when drawn.
        ///
        public float Rotation
        {
            get { return fRotation; }
            set { fRotation = value; }
        }
 
        ///
        /// Screen coordinates of the bounding box surrounding this sprite
        ///
        public Rectangle BoundingBox
        {
            get { return new Rectangle(X, Y, spriteWidth, spriteHeight); }
        }
 
        ///
        /// The texture associated with this sprite.  All FrameAnimations will be
        /// relative to this texture.
        ///
        public Texture2D Texture
        {
            get { return spriteSheet; }
        }
 
        ///
        /// Color value to tint the sprite with when drawing.  Color.White
        /// (the default) indicates no tinting.
        ///
        public Color Tint
        {
            get { return colorTint; }
            set { colorTint = value; }
        }
 
        ///
        /// True if the sprite is (or should be) playing animation frames.  If this value is set
        /// to false, the sprite will not be drawn (a sprite needs at least 1 single frame animation
        /// in order to be displayed.
        ///
        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }
 
        ///
        /// The FrameAnimation object of the currently playing animation
        ///
        public FrameAnimation CurrentFrameAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAnimation))
                    return animations[currentAnimation];
                else
                    return null;
            }
        }
 
        ///
        /// The string name of the currently playing animaton.  Setting the animation
        /// resets the CurrentFrame and PlayCount properties to zero.
        ///
        public string CurrentAnimation
        {
            get { return currentAnimation; }
            set
            {
                if (animations.ContainsKey(value))
                {
                    currentAnimation = value;
                    animations[currentAnimation].CurrentFrame = 0;
                    animations[currentAnimation].PlayCount = 0;
                }
            }
        }


    }
}
