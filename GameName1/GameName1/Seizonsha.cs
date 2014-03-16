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
using GameName1.NPCs;
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
        private HashSet<Collision> collisions;

        private Level currLevel;

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
            collisions = new HashSet<Collision>();
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

            Spawn(new BasicNPC(this, playerRect, 300, 100, 10, 10));

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
                    if (((GameEntity)spawn).isCollidable())
                    {
                        BindEntityToTiles((GameEntity)spawn, true);
                    }
                }
                spawn.OnSpawn();

            }

            //update all entities
            foreach (GameEntity entity in entities)
            {
                entity.UpdateAll();
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
                player.UpdateAll();
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

            //TODO: run AI


            //execute all collisions

            foreach (Collision collision in collisions)
            {
                collision.execute();
            }
            collisions.Clear();

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


        public void BindEntityToTiles(GameEntity entity, bool bind)
        {
            for (int i = entity.getLeftEdgeTileIndex(); i <= entity.getRightEdgeTileIndex(); i++)
            {
                for (int j = entity.getTopEdgeTileIndex(); j <= entity.getBottomEdgeTileIndex(); j++)
                {
                    currLevel.getTile(i, j).BindEntity(entity, bind);
                }
            }
        }


        private void moveGameEntityWithoutCollision(GameEntity entity, int x, int y)
        {
            entity.x = x;
            entity.y = y;
            entity.hitbox.X = x;
            entity.hitbox.Y = y;
        }

        public void moveGameEntity(GameEntity entity, int dx, int dy)
        {

            if (!entity.isCollidable()) //skip collision detection
            {
                moveGameEntityWithoutCollision(entity, entity.x + dx, entity.y + dy);
                return;
            }

            BindEntityToTiles(entity, false);

            bool wallCollision = false;

            //calculate number of tiles distance covers
            int tilesX = (int)Math.Floor((float)Math.Abs(dx) / Static.TILE_WIDTH) + 1;
            int tilesY = (int)Math.Floor((float)Math.Abs(dy) / Static.TILE_HEIGHT) + 1;

            //get entity bounds
            int leftEdgeTile = entity.getLeftEdgeTileIndex();
            int rightEdgeTile = entity.getRightEdgeTileIndex();
            int topEdgeTile = entity.getTopEdgeTileIndex();
            int bottomEdgeTile = entity.getBottomEdgeTileIndex();

            int distanceToTravelX = dx;
            int distanceToTravelY = dy;

            //find distance to level boundary in movement direction and see if it is less move amount
            //figure out how many tiles your movement translates to in each direction
            //scan tiles in front of you to find closest obstacle in that direction
            //final movement is min of original movement and distance to obstacle


            if (dx > 0)
            {
                //right

                int distanceToBoundary = currLevel.GetTilesHorizontal() * Static.TILE_WIDTH - entity.getRightEdgeX();
                if (distanceToBoundary < distanceToTravelX)
                {
                    distanceToTravelX = distanceToBoundary;
                    //wallCollision = true;
                }

                int tilesToScanX = tilesX;

                if (rightEdgeTile + tilesToScanX > currLevel.GetTilesHorizontal() - 1)
                {
                    tilesToScanX = currLevel.GetTilesHorizontal() - 1 - rightEdgeTile;
                }


                for (int i = 1; i <= tilesToScanX; i++)
                {
                    for (int j = topEdgeTile; j <= bottomEdgeTile; j++)
                    {
                        Tile currTile = currLevel.getTile(rightEdgeTile + i, j);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = currTile.x - entity.getRightEdgeX();
                            if (distanceToTile < distanceToTravelX)
                            {
                                distanceToTravelX = distanceToTile;
                                wallCollision = true;
                                break;
                            }
                        }

                        GameEntity closest = null;
                        foreach (GameEntity tileEntity in currTile.touching)
                        {
                            if (tileEntity.getLeftEdgeX() - entity.getRightEdgeX() < distanceToTravelX)
                            {
                                if (tileEntity.OverlapsY(entity))
                                {
                                    distanceToTravelX = tileEntity.getLeftEdgeX() - entity.getRightEdgeX();
                                    closest = tileEntity;
                                }
                            }
                        }
                        if (closest != null)
                        {
                            collisions.Add(new Collision(entity, closest));
                        }
                    }
                }


            }
            else if (dx < 0)
            {
                //left


                int distanceToBoundary = -1 * entity.getLeftEdgeX();
                if (distanceToBoundary > distanceToTravelX)
                {
                    distanceToTravelX = distanceToBoundary;
                    //wallCollision = true;
                }

                int tilesToScanX = tilesX;

                if (leftEdgeTile - tilesToScanX < 0)
                {
                    tilesToScanX = leftEdgeTile;
                }


                for (int i = 1; i <= tilesToScanX; i++)
                {
                    for (int j = topEdgeTile; j <= bottomEdgeTile; j++)
                    {
                        Tile currTile = currLevel.getTile(leftEdgeTile - i, j);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = (currTile.x + Static.TILE_WIDTH) - entity.getLeftEdgeX();
                            if (distanceToTile > distanceToTravelX)
                            {
                                distanceToTravelX = distanceToTile;
                                wallCollision = true;
                                break;
                            }
                        }

                        GameEntity closest = null;
                        foreach (GameEntity tileEntity in currTile.touching)
                        {
                            if (tileEntity.getRightEdgeX() - entity.getLeftEdgeX() > distanceToTravelX)
                            {
                                if (tileEntity.OverlapsY(entity))
                                {
                                    distanceToTravelX = tileEntity.getRightEdgeX() - entity.getLeftEdgeX();
                                    closest = tileEntity;
                                }
                            }
                        }
                        if (closest != null)
                        {
                            collisions.Add(new Collision(entity, closest));
                        }

                    }

                }
            }

            entity.x = entity.x + distanceToTravelX;
            entity.hitbox.Offset(distanceToTravelX, 0);
            leftEdgeTile = entity.getLeftEdgeTileIndex();
            rightEdgeTile = entity.getRightEdgeTileIndex();


            if (dy > 0)
            { //down

                int distanceToBoundary = currLevel.GetTilesVertical() * Static.TILE_HEIGHT - entity.getBottomEdgeY();
                if (distanceToBoundary < distanceToTravelY)
                {
                    distanceToTravelY = distanceToBoundary;
                    //wallCollision = true;
                }


                int tilesToScanY = tilesY;

                if (bottomEdgeTile + tilesToScanY > currLevel.GetTilesVertical() - 1)
                {
                    tilesToScanY = currLevel.GetTilesVertical() - 1 - bottomEdgeTile;
                }

                for (int i = 1; i <= tilesToScanY; i++)
                {
                    for (int j = leftEdgeTile; j <= rightEdgeTile; j++)
                    {
                        Tile currTile = currLevel.getTile(j, bottomEdgeTile + i);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = currTile.y - entity.getBottomEdgeY();
                            if (distanceToTile < distanceToTravelY)
                            {
                                distanceToTravelY = distanceToTile;
                                wallCollision = true;
                                break;
                            }
                        }

                        GameEntity closest = null;
                        foreach (GameEntity tileEntity in currTile.touching)
                        {
                            if (tileEntity.getTopEdgeY() - entity.getBottomEdgeY() < distanceToTravelY)
                            {
                                if (tileEntity.OverlapsX(entity))
                                {
                                    distanceToTravelY = tileEntity.getTopEdgeY() - entity.getBottomEdgeY();
                                    closest = tileEntity;
                                }
                            }
                        }
                        if (closest != null)
                        {
                            
                            collisions.Add(new Collision(entity, closest));
                        }
                    }
                }

            }
            else if (dy < 0)
            { //up


                int distanceToBoundary = -1 * entity.getTopEdgeY();
                if (distanceToBoundary > distanceToTravelY)
                {
                    distanceToTravelY = distanceToBoundary;
                    //wallCollision = true;
                }

                int tilesToScanY = tilesY;

                if (topEdgeTile - tilesToScanY < 0)
                {
                    tilesToScanY = topEdgeTile;
                }


                for (int i = 1; i <= tilesToScanY; i++)
                {
                    for (int j = leftEdgeTile; j <= rightEdgeTile; j++)
                    {
                        Tile currTile = currLevel.getTile(j, topEdgeTile - i);
                        if (currTile.isObstacle())
                        {
                            int distanceToTile = (currTile.y + Static.TILE_HEIGHT) - entity.getTopEdgeY();
                            if (distanceToTile > distanceToTravelY)
                            {
                                    distanceToTravelY = distanceToTile;
                                    wallCollision = true;
                                    break;
                            }
                        }

                        GameEntity closest = null;
                        foreach (GameEntity tileEntity in currTile.touching)
                        {
                            if (tileEntity.getBottomEdgeY() - entity.getTopEdgeY() > distanceToTravelY)
                            {
                                if (tileEntity.OverlapsX(entity))
                                {
                                    distanceToTravelY = tileEntity.getBottomEdgeY() - entity.getTopEdgeY();
                                    closest = tileEntity;
                                }
                            }
                        }
                        if (closest != null)
                        {
                            collisions.Add(new Collision(entity, closest));
                        }
                    }

                }

            }

            entity.y = entity.y + distanceToTravelY;
            entity.hitbox.Offset(0, distanceToTravelY);

            if (wallCollision)
            {
                entity.collideWithWall();
            }

            BindEntityToTiles(entity, true);

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

        public SpriteFont getSpriteFont()
        {
            return spriteFont;
        }


    }
}
