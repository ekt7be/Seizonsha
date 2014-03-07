#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Collections;
#endregion

namespace GameName1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //private AnimatedSprite lizardSpriteAtlas;
        private SpriteFont spriteFont;
        public static readonly int ScreenWidth = 640;
        public static readonly int ScreenHeight = 480;
        public static readonly int TilesOnScreenX = 80;
        public static readonly int TilesOnScreenY  = (int)((float)ScreenHeight / (float)ScreenWidth * TilesOnScreenX);
        public static readonly int TileWidth = ScreenWidth / TilesOnScreenX;
        public static readonly int TileHeight = TileWidth;

        public int cameraX;
        public Player player;
        public static readonly int gravity = 1;

        private static readonly Random randomGen = new Random();

        private ArrayList entities;
        private ArrayList entityRemoval;

        public Level currLevel;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        public static void Debug(String message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public static Color randomColor()
        {
            Color randomColor = Color.Black;

            randomColor.R = (byte)randomGen.Next(255);
            randomColor.G = (byte)randomGen.Next(255);
            randomColor.B = (byte)randomGen.Next(255);

            return randomColor;
        }

        public static Color CombineColors(Color color1, Color color2)
        {
            Color returnColor = Color.Black;
            returnColor.R = (byte)((color1.R + color2.R) / 2);
            returnColor.G = (byte)((color1.G + color2.G) / 2);
            returnColor.B = (byte)((color1.B + color2.B) / 2);

            return returnColor;
        }

        public static double ColorDistance(Color color1, Color color2)
        {
            long rmean = ((long)color1.R + (long)color2.R) / 2;
            long r = (long)color1.R - (long)color2.R;
            long g = (long)color1.G - (long)color2.G;
            long b = (long)color1.B - (long)color2.B;
            return Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));
        }

        public ArrayList getEntities()
        {
            return entities;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            entities = new ArrayList();
            entityRemoval = new ArrayList();
            cameraX = 0;
            currLevel = new Level(this);
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;

            Texture2D playerRect = new Texture2D(GraphicsDevice, Animal.ANIMAL_WIDTH, Animal.ANIMAL_HEIGHT);
           
            Color[] data = new Color[Animal.ANIMAL_HEIGHT * Animal.ANIMAL_WIDTH];
            for (int i = 0; i < data.Length; ++i)
            {

                data[i] = Color.White;

            }
            playerRect.SetData(data);
            player = new Player(this,playerRect,0,0, Game1.randomColor());

            GenerateMate();
            GenerateMate();

            base.Initialize();
        }

        public void GenerateMate()
        {
            Texture2D mateRect = new Texture2D(GraphicsDevice, Animal.ANIMAL_WIDTH, Animal.ANIMAL_HEIGHT);

            Color[] data = new Color[Animal.ANIMAL_HEIGHT * Animal.ANIMAL_WIDTH];
            for (int i = 0; i < data.Length; ++i)
            {

                data[i] = Color.White;

            }
            mateRect.SetData(data);
            entities.Add(new Mate(this, mateRect, (int)((Game1.ScreenWidth -Animal.ANIMAL_WIDTH*2) * randomGen.NextDouble()), 0, Game1.randomColor()));

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Font");
            //lizardSpriteAtlas = new AnimatedSprite(Content.Load<Texture2D>("lizardatlas"), 1, 4);
            //shroom = Content.Load<Texture2D>("shroom1");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                player.Jump();
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -.5 || Keyboard.GetState().IsKeyDown(Keys.Left)){
                player.MoveLeft();
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > .5 || Keyboard.GetState().IsKeyDown(Keys.Right)){
                player.MoveRight();
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.5 || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                player.Crouch();
            }
            else if (player.crouched)
            {
                player.Uncrouch();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                player.Mate();
            }

            foreach (GameEntity entity in entities)
            {
                entity.Update();
                if (entity.remove)
                {
                    entityRemoval.Add(entity);
                }

            }

            foreach (GameEntity removeEntity in entityRemoval)
            {
                entities.Remove(removeEntity);
            }

            player.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            /*
            spriteBatch.Draw(snailSprite, snailLocation, new Rectangle(0, 0, 800, 480), Color.White, snailAngle, new Vector2(snailSprite.Width/2, snailSprite.Height/2), SpriteEffects.None, 1);
            creatureSpriteAtlas.Draw(spriteBatch, new Vector2(0,0));
             * */
            currLevel.Draw(spriteBatch);
            foreach (GameEntity entity in entities)
            {
                entity.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);

            spriteBatch.DrawString(spriteFont, "Camouflage: " + player.stealth * 100 + "%", new Vector2(50, 50), Color.White);

            spriteBatch.DrawString(spriteFont, "Press X (GamePad) or Enter (Key) to mate", new Vector2(50, Game1.ScreenHeight*5/6), Color.White);
            spriteBatch.DrawString(spriteFont, "Press B (GamePad) or Space (Key) to jump", new Vector2(50, Game1.ScreenHeight * 5 / 6 + 50), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public SpriteFont GetSpriteFont()
        {
            return spriteFont;
        }
    }
}
