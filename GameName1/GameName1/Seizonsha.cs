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
using GameName1.Interfaces;
#endregion

namespace GameName1
{

    public class Seizonsha : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private static readonly Random randomGen = new Random();

        private Player[] players;
        private List<GameEntity> entities;
        private Queue<GameEntity> removalQueue;
        private Queue<Spawnable> spawnQueue;

        public Level currLevel;

        public Seizonsha()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        



        protected override void Initialize()
        {
            entities = new List<GameEntity>();
            removalQueue = new Queue<GameEntity>();
            spawnQueue = new Queue<Spawnable>();
            currLevel = new Level(this);
            graphics.PreferredBackBufferHeight = Static.SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = Static.SCREEN_WIDTH;
            this.players = new Player[4];

            //just for testing -- makes a rectangle
            Texture2D playerRect = new Texture2D(GraphicsDevice, Static.PLAYER_HEIGHT, Static.PLAYER_WIDTH);
           
            Color[] data = new Color[Static.PLAYER_HEIGHT * Static.PLAYER_WIDTH];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Aquamarine;
            }

            playerRect.SetData(data);
            //will use real sprites eventually..

            players[0] = new Player(this,PlayerIndex.One,playerRect,0,0);

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
            

            //spawn Spawnables
            while (spawnQueue.Count > 0)
            {
                Spawnable spawn = spawnQueue.Dequeue();
                if (spawn is GameEntity)
                {
 
                    entities.Add((GameEntity)spawn);
                }
                spawn.OnSpawn();

            }

            //update all entities
            foreach (GameEntity entity in entities)
            {
                entity.Update();
                if (entity.shouldRemove())
                {
                    removalQueue.Enqueue(entity);
                }

            }
            //update all players
            foreach (Player player in players)
            {
                if (player == null)
                {
                    continue;
                }
                player.Update();
            }


            //remove entities flagged for removal
            while (removalQueue.Count > 0)
            {
                entities.Remove(removalQueue.Dequeue());
            }


            //handle all player input
            foreach (Player player in players)
            {
                if (player == null)
                {
                    continue;
                }
                handlePlayerInput(player);
            }

            base.Update(gameTime);
        }



        protected void handlePlayerInput(Player player)
        {
            if (GamePad.GetState(player.playerIndex).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y > .5 || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                player.MoveUp();
            }
            if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X < -.5 || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                player.MoveLeft();
            }
            if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X > .5 || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                player.MoveRight();
            }
            if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y < -.5 || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                player.MoveDown();
            }


            if (GamePad.GetState(player.playerIndex).Buttons.LeftShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                player.L1Button();
            }
            if (GamePad.GetState(player.playerIndex).Triggers.Left > .5f || Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                player.L2Button();
            }
            if (GamePad.GetState(player.playerIndex).Buttons.RightShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                player.R1Button();
            }
            if (GamePad.GetState(player.playerIndex).Triggers.Right > .5f || Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                player.R2Button();
            }
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

            foreach (Player player in players)
            {
                if (player == null)
                {
                    continue;
                }
                player.Draw(spriteBatch);
            
                string displaySkills = "L1: " + player.getSkill(Static.PLAYER_L1_SKILL_INDEX).getName() + "\n" +
                    "L2: " + player.getSkill(Static.PLAYER_L2_SKILL_INDEX).getName() + "\n" +
                    "R1: " + player.getSkill(Static.PLAYER_R1_SKILL_INDEX).getName() + "\n" +
                    "R2: " + player.getSkill(Static.PLAYER_R2_SKILL_INDEX).getName() + "\n";
                spriteBatch.DrawString(spriteFont, displaySkills, new Vector2(50, 50), Color.White);
            }

            //print text
            //spriteBatch.DrawString(spriteFont, "TEXT", new Vector2(50, 50), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Spawn(Spawnable spawn)
        {
            spawnQueue.Enqueue(spawn);
        }


    }
}
