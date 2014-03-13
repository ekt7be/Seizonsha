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

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont spriteFont;


        public Player player;

        private static readonly Random randomGen = new Random();

        private ArrayList entities;
        private Queue<GameEntity> removalQueue;
        private Queue<GameEntity> spawnQueue;

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
        
        public ArrayList getEntities()
        {
            return entities;
        }


        protected override void Initialize()
        {
            entities = new ArrayList();
            removalQueue = new Queue<GameEntity>();
            spawnQueue = new Queue<GameEntity>();
            currLevel = new Level(this);
            graphics.PreferredBackBufferHeight = Static.SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = Static.SCREEN_WIDTH;

            Texture2D playerRect = new Texture2D(GraphicsDevice, Static.PLAYER_HEIGHT, Static.PLAYER_WIDTH);
           
            Color[] data = new Color[Static.PLAYER_HEIGHT * Static.PLAYER_WIDTH];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.White;
            }

            playerRect.SetData(data);
            player = new Player(this,playerRect,0,0);

            base.Initialize();
        }




        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Font");

        }

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
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .5 || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                player.MoveUp();
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -.5 || Keyboard.GetState().IsKeyDown(Keys.Left)){
                player.MoveLeft();
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > .5 || Keyboard.GetState().IsKeyDown(Keys.Right)){
                player.MoveRight();
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.5 || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                player.MoveDown();
            }

            //spawn all entities
            while (spawnQueue.Count > 0)
            {
                GameEntity spawnedEntity = spawnQueue.Dequeue();
                entities.Add(spawnedEntity);
                spawnedEntity.onSpawn();

            }

            //update all entities
            foreach (GameEntity entity in entities)
            {
                entity.Update();
                if (entity.remove)
                {
                    removalQueue.Enqueue(entity);
                }

            }
            //update player
            player.Update();


            //remove entities flagged for removal
            while (removalQueue.Count > 0)
            {
                entities.Remove(removalQueue.Dequeue());
            }

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


            currLevel.Draw(spriteBatch,0,0);
            foreach (GameEntity entity in entities)
            {
                entity.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);

            //print text
            //spriteBatch.DrawString(spriteFont, "TEXT", new Vector2(50, 50), Color.White);
            

            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}
