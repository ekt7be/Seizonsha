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
using GameName1.Skills;
using GameName1.Effects;
#endregion

namespace GameName1
{

    public class Seizonsha : Game
    {
		int numberOfPlayers = 1;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private static readonly Random randomGen = new Random();
        private Player[] players;
        private List<GameEntity> entities;
        private Queue<GameEntity> removalQueue;
        private Queue<Spawnable> spawnQueue;
        private HashSet<Collision> collisions;
        private List<AI> AIs;

        private Level currLevel;

        private Dictionary<int, Texture2D> spriteMappings;

		// AlexAlpha
		Viewport defaultView, p1View, p2View, p3View, p4View; 
		Camera p1Camera, p2Camera, p3Camera, p4Camera; 
		List<Camera> cameras; 
		Rectangle yDivider, xDivider; 
		List<Rectangle> dividers; 

		Dictionary<int, PlayerIndex> playerToController;
		//-

        public Seizonsha()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }
			
		void initTileSprites() {
			Texture2D floor_grass = Content.Load<Texture2D>("tiles/dc_tiles/dc-dngn/floor/grass/grass0");
			Texture2D floor_nerves = Content.Load<Texture2D>("tiles/dc_tiles/dc-dngn/floor/floor_nerves1");

			Texture2D wall_brick = Content.Load<Texture2D>("tiles/dc_tiles/dc-dngn/wall/brick_brown0");
			Texture2D wall_stone = Content.Load<Texture2D>("tiles/dc_tiles/dc-dngn/wall/stone_gray0");

			Level.tileSprites.Add(961, floor_grass);
			Level.tileSprites.Add(840, floor_nerves);

			Level.tileSprites.Add(1033, wall_brick);
			Level.tileSprites.Add(1157, wall_stone);
		}

        protected override void Initialize()
        {
            entities = new List<GameEntity>();
            AIs = new List<AI>();
            removalQueue = new Queue<GameEntity>();
            spawnQueue = new Queue<Spawnable>();
            collisions = new HashSet<Collision>();
            currLevel = new Level(this);

            this.players = new Player[4];
            this.spriteMappings = new Dictionary<int, Texture2D>();

			graphics.PreferredBackBufferHeight = Static.SCREEN_HEIGHT;
			graphics.PreferredBackBufferWidth = Static.SCREEN_WIDTH;

            //just for testing -- makes a rectangle
			//Texture2D playerRect = new Texture2D(GraphicsDevice, Static.PLAYER_HEIGHT, Static.PLAYER_WIDTH);

			Texture2D playerRect = Content.Load<Texture2D>("Sprites/BasicPlayerSpriteSheet");
            Texture2D npcRect = Content.Load<Texture2D>("Sprites/player");
			Texture2D basicEnemyRect = Content.Load<Texture2D>("Sprites/BasicEnemySprite");
            SkillTree.SkillTree.nodeTextures.Add(Static.SKILL_TREE_NODE_ANY, basicEnemyRect);

			initTileSprites();

            spriteMappings.Add(Static.BASIC_ENEMY_INT, basicEnemyRect);
            spriteMappings.Add(Static.PLAYER_INT, playerRect);
           
            Color[] data = new Color[Static.PLAYER_HEIGHT * Static.PLAYER_WIDTH];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Aquamarine;
            }

//		     playerRect.SetData(data);
            //will use real sprites eventually..
			// AlexAlpha

			initViewports(numberOfPlayers);

			Static.SCREEN_HEIGHT = defaultView.Height;
			Static.SCREEN_WIDTH = defaultView.Width;

			initSplitscreenDividers(); 

			cameras = new List<Camera>();
			cameras.Add(p1Camera);
			cameras.Add(p2Camera); 
			cameras.Add(p3Camera); 
			cameras.Add(p4Camera); 

			playerToController = new Dictionary<int, PlayerIndex>(); 
			playerToController.Add(1, PlayerIndex.One); 
			playerToController.Add(2, PlayerIndex.Two); 
			playerToController.Add(3, PlayerIndex.Three);
			playerToController.Add(4, PlayerIndex.Four); 

			for (int i = 0; i < numberOfPlayers; i++) {
				cameras[i] = new Camera(); 
				players[i] = new Player(this, playerToController[i+1], playerRect, 500, 100+(i*40), cameras[i]);

				//players[i] = new Player(this, playerToController[i+1], playerRect, 0, 0+(i*20), cameras[i]);
				Spawn(players[i]);
			}
			//-
           // Spawn(new BasicNPC(this, npcRect, 300, 100, 10, 10));
			Spawn(new BasicEnemy(this, basicEnemyRect, 200, 200));
            Spawn(new SpawnEntity(this, 2, 250, 250));
            Spawn(new SpawnEntity(this, 2, 0, 250));

            base.Initialize();
        }

		void initViewports(int numberOfPlayers) {
			switch (numberOfPlayers) {
				case 1: 
					defaultView = GraphicsDevice.Viewport;
					defaultView.Width = Static.SCREEN_WIDTH;
					defaultView.Height = Static.SCREEN_HEIGHT;
					p1View = defaultView; 

					break;
			case 2: 
					defaultView = GraphicsDevice.Viewport;
					defaultView.Width = Static.SCREEN_WIDTH;
					defaultView.Height = Static.SCREEN_HEIGHT;
					p1View = p2View = defaultView;

					p1View.Width = p1View.Width/2; 
					p2View.Width = p2View.Width/2;

					p2View.X = p1View.Width;

					break;
				case 3: 
					defaultView = GraphicsDevice.Viewport;
					defaultView.Width = Static.SCREEN_WIDTH;
					defaultView.Height = Static.SCREEN_HEIGHT;
					p1View = p2View = p3View = defaultView;

					p1View.Width = p1View.Width/2;
					p2View.Width = p2View.Width/2;
					p3View.Width = p3View.Width/2; 

					p1View.Height = p1View.Height/2; 
					p2View.Height = p2View.Height/2;
					p3View.Height = p3View.Height/2;

					p2View.X = p1View.Width;
					p3View.Y = p1View.Height; 

					break;
				case 4: 
					defaultView = GraphicsDevice.Viewport;
					defaultView.Width = Static.SCREEN_WIDTH;
					defaultView.Height = Static.SCREEN_HEIGHT;
					p1View = p2View = p3View = p4View = defaultView;

					p1View.Width = p1View.Width/2;
					p2View.Width = p2View.Width/2;
					p3View.Width = p3View.Width/2; 
					p4View.Width = p4View.Width/2; 

					p1View.Height = p1View.Height/2; 
					p2View.Height = p2View.Height/2;
					p3View.Height = p3View.Height/2;
					p4View.Height = p4View.Height/2; 

					p2View.X = p1View.Width;
					p3View.Y = p1View.Height;
					p4View.X = p1View.Width; p4View.Y = p1View.Height; 

					break;
				default:
					break;
			}

		}

		void initSplitscreenDividers() {
			dividers = new List<Rectangle>();
			int divWidth = 3;	// width of dividing lines

			yDivider = new Rectangle(Static.SCREEN_WIDTH/2+Static.SCREEN_WIDTH_FIX2-(divWidth/2), 0, divWidth, Static.SCREEN_HEIGHT*2);
			xDivider = new Rectangle(0, Static.SCREEN_HEIGHT/2-(divWidth/2), Static.SCREEN_WIDTH*2, divWidth);

			dividers.Add(yDivider); 
			dividers.Add(xDivider);
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
            
			// ALEX
			IsMouseVisible = true; 
			//-ALEX


            //update all entities including players
            foreach (GameEntity entity in entities)
            {
                if (entity.shouldRemove())
                {
                    if (entity is GameEntity)
                    {
                        BindEntityToTiles((GameEntity)entity, false);
                    }
                    removalQueue.Enqueue(entity);
                }

            }


            //remove entities flagged for removal
            while (removalQueue.Count > 0)
            {
                GameEntity remEntity = removalQueue.Dequeue();
                entities.Remove(remEntity);
                if (remEntity is AI)
                {
                    AIs.Remove((AI)remEntity);
                }
            }



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

                    if (spawn is AI)
                    {
                        AIs.Add((AI)spawn);
                    }
                }



                spawn.OnSpawn();

            }



            //update all entities including players
            foreach (GameEntity entity in entities)
            {
                entity.UpdateAll();
            }

            //handle all player input
            foreach (Player player in players)
            {
                if (player == null || player.isDead())
                {
                    continue;
                }

				handlePlayerInput(player);

				// AlexAlpha
				player.camera.Update(this.getPlayers().Count, player, this.getLevelBounds()); 
				//-

            }

            //run AI
            foreach (AI ai in AIs)
            {
                ai.AI();
            }

            //execute all collisions
            foreach (Collision collision in collisions)
            {
                collision.execute();
            }
            collisions.Clear();

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (Player player in players)
            {
                if (player == null)
                {
                    continue;
                }

				// AlexAlpha
				switch (Array.IndexOf(players, player) + 1) {
					case 1: 
						GraphicsDevice.Viewport = p1View; 
						break;
					case 2: 
						GraphicsDevice.Viewport = p2View; 
						break;
					case 3: 
						GraphicsDevice.Viewport = p3View; 
						break;
					case 4: 
						GraphicsDevice.Viewport = p4View; 
						break;
					default:
						break;
				}

				spriteBatch.Begin(
					SpriteSortMode.Deferred, 
					BlendState.AlphaBlend, 
					null, 
					null, 
					null, 
					null, 
					player.camera.transform
				);

					currLevel.Draw(spriteBatch, 0, 0);

					foreach (GameEntity entity in entities)
					{
						entity.Draw(spriteBatch);
					}

					// DISPLAY TEXT FOR LIST OF SKILLS 


				spriteBatch.End();
            }

            //print text
            //spriteBatch.DrawString(spriteFont, "TEXT", new Vector2(50, 50), Color.White);


			GraphicsDevice.Viewport = defaultView; 


			spriteBatch.Begin();

			drawSplitscreenDividers();

			spriteBatch.End();




			foreach (Player player in players) 
			{

				if (player == null)
				{
					continue;
				}

				spriteBatch.Begin();


				switch (Array.IndexOf(players, player) + 1) {
				case 1: 
					GraphicsDevice.Viewport = p1View; 
					break;
				case 2: 
					GraphicsDevice.Viewport = p2View; 
					break;
				case 3: 
					GraphicsDevice.Viewport = p3View; 
					break;
				case 4: 
					GraphicsDevice.Viewport = p4View; 
					break;
				default:
					break;
				}

				//draw player interface
				player.DrawScreen (GraphicsDevice.Viewport.Bounds, spriteBatch);
				spriteBatch.End(); 

			}




            base.Draw(gameTime);
        }

		void drawSplitscreenDividers() {

				Texture2D rect = new Texture2D(GraphicsDevice, 1, 1);
				rect.SetData(new[] { Color.White });

				if (numberOfPlayers == 2) {
					spriteBatch.Draw(rect, yDivider, Color.White); 
				}
				else if (numberOfPlayers > 2) {
					spriteBatch.Draw(rect, yDivider, Color.White); 
					spriteBatch.Draw(rect, xDivider, Color.White); 
				}

		}

        protected void handlePlayerInput(Player player)
        
        {

            if (player.playerIndex == PlayerIndex.One)
            {

                if (GamePad.GetState(player.playerIndex).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    player.OpenSkillTree(!player.SkillTreeOpen());
                }

                if (GamePad.GetState(player.playerIndex).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y > .5 || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    player.MoveUp();
                    //player.rotateToAngle((float)(3 * Math.PI / 2));

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X < -.5 || Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    player.MoveLeft();
                    //player.rotateToAngle((float)Math.PI);

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X > .5 || Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    player.MoveRight();
                    // player.rotateToAngle((float)0);

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y < -.5 || Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    player.MoveDown();
                    // player.rotateToAngle((float)Math.PI / 2);
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

                if (Math.Abs(GamePad.GetState(player.playerIndex).ThumbSticks.Right.Y) > .1 || Math.Abs(GamePad.GetState(player.playerIndex).ThumbSticks.Right.X) > .1)
                {
                    player.rotateToAngle((float)Math.Atan2(GamePad.GetState(player.playerIndex).ThumbSticks.Right.Y * -1, GamePad.GetState(player.playerIndex).ThumbSticks.Right.X)); // angle to point		
                }

				// calculates mouse aim angle and rotation 
                MouseState mouse = Mouse.GetState();

                Vector2 playerMouseDistance; // distance between player and mouse

				playerMouseDistance.X = player.camera.getWorldPositionX(mouse.X) - player.x;
				playerMouseDistance.Y = player.camera.getWorldPositionY(mouse.Y) - player.y;

				player.rotateToAngle((float)Math.Atan2(playerMouseDistance.Y, playerMouseDistance.X)); // angle to point at					

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    player.LeftClick();
                }
            }
            else
            {

                if (GamePad.GetState(player.playerIndex).Buttons.Start == ButtonState.Pressed)
                {
                    player.OpenSkillTree(!player.SkillTreeOpen());
                }
                if (GamePad.GetState(player.playerIndex).Buttons.Back == ButtonState.Pressed)
                {
                    Exit();
                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y > .5)
                {
                    player.MoveUp();
                    //player.rotateToAngle((float)(3 * Math.PI / 2));

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X < -.5)
                {
                    player.MoveLeft();
                    //player.rotateToAngle((float)Math.PI);

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X > .5)
                {
                    player.MoveRight();
                    // player.rotateToAngle((float)0);

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y < -.5)
                {
                    player.MoveDown();
                    // player.rotateToAngle((float)Math.PI / 2);


                }


                if (GamePad.GetState(player.playerIndex).Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    player.L1Button();
                }
                if (GamePad.GetState(player.playerIndex).Triggers.Left > .5f)
                {
                    player.L2Button();
                }
                if (GamePad.GetState(player.playerIndex).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    player.R1Button();
                }
                if (GamePad.GetState(player.playerIndex).Triggers.Right > .5f)
                {
                    player.R2Button();
                }


                if (Math.Abs(GamePad.GetState(player.playerIndex).ThumbSticks.Right.Y) > .1 || Math.Abs(GamePad.GetState(player.playerIndex).ThumbSticks.Right.X) > .1)
                {
                    player.rotateToAngle((float)Math.Atan2(GamePad.GetState(player.playerIndex).ThumbSticks.Right.Y * -1, GamePad.GetState(player.playerIndex).ThumbSticks.Right.X)); // angle to point		
                }


            }

        }


        public void BindEntityToTiles(GameEntity entity, bool bind)
        {
            for (int i = getTileIndexFromLeftEdgeX(entity.getLeftEdgeX()); i <= getTileIndexFromRightEdgeX(entity.getRightEdgeX()); i++)
            {
                for (int j = getTileIndexFromTopEdgeY(entity.getTopEdgeY()); j <= getTileIndexFromBottomEdgeY(entity.getBottomEdgeY()); j++)
                {
                    Tile currTile = currLevel.getTile(i, j);
                    if (currTile != null) //if within bounds of level
                    {
                        currLevel.getTile(i, j).BindEntity(entity, bind);
                    }
                }
            }
        }

        public Texture2D getTestSprite(Rectangle bounds, Color color)
        {
            Texture2D testRect = new Texture2D(GraphicsDevice, bounds.Height, bounds.Width);

            Color[] data = new Color[bounds.Height * bounds.Width];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = color;
            }

            testRect.SetData(data);
            return testRect;
        }


        public void healEntity(GameEntity user, GameEntity target, int amount, int damageType)
        {
            if (ShouldHeal(damageType, target.getTargetType())){
                incEntityHealth(target,amount);
                TextEffect text = new TextEffect(this, amount + "", 10, target.getCenterX(), target.getCenterY() - 60, Color.Green);
                Spawn(text);
            }
        }

        public void damageEntity(GameEntity user, GameEntity target, int amount, int damageType)
        {

            if (ShouldDamage(damageType, target.getTargetType()))
            {

                TextEffect text = new TextEffect(this, amount + "", 10, target.getCenterX(), user.getCenterY() - 60, Color.Red);
                Spawn(text);

                if (user == null)
                {
                    incEntityHealth(target, -1 * amount);
                    return;
                }

                user.OnDamageOther(target, amount); // if damage goes through
                if (incEntityHealth(target, -1 * amount))
                {
                    user.OnKillOther(target); //if kills target
                }
            }

        }

        public bool incEntityHealth(GameEntity target, int amount)
        { //true if dead
            target.health += amount;
            if (target.health > target.maxHealth)
            {
                target.health = target.maxHealth;
            }
            if (target.health < 0)
            {
                target.health = 0;
                target.die();
                return true;
            }
            return false;
        }


        public void damageArea(GameEntity user, Rectangle bounds, int amount, int damageType){ //damage not caused by an entity
            foreach (GameEntity entity in getEntitiesInBounds(bounds)){
                damageEntity(user, entity, amount, damageType);
            }
        }


        public void healArea(GameEntity user, Rectangle bounds, int amount, int damageType)
        {
            foreach (GameEntity entity in getEntitiesInBounds(bounds))
            {
                healEntity(user,entity,amount,damageType);
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

            BindEntityToTiles(entity, false);

            if (!entity.isCollidable()) //skip collision detection
            {
                moveGameEntityWithoutCollision(entity, entity.x + dx, entity.y + dy);
                BindEntityToTiles(entity, true);
                return;
            }


            bool wallCollision = false;

            //calculate number of tiles distance covers
            int tilesX = (int)Math.Floor((float)Math.Abs(dx) / Static.TILE_WIDTH) + 1;
            int tilesY = (int)Math.Floor((float)Math.Abs(dy) / Static.TILE_HEIGHT) + 1;

            //get entity bounds
            int leftEdgeTile = getTileIndexFromLeftEdgeX(entity.getLeftEdgeX());
            int rightEdgeTile = getTileIndexFromRightEdgeX(entity.getRightEdgeX());
            int topEdgeTile = getTileIndexFromTopEdgeY(entity.getTopEdgeY());
            int bottomEdgeTile = getTileIndexFromBottomEdgeY(entity.getBottomEdgeY());

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
                    wallCollision = true;
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
                        if (currTile == null)
                        {
                            continue;
                        }
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
                        foreach (GameEntity tileEntity in currTile.getEntities()) 
                        {
                            if (tileEntity.getLeftEdgeX() - entity.getRightEdgeX() < distanceToTravelX)
                            {
                                if (tileEntity.OverlapsY(entity) && entity.shouldCollide(tileEntity) && tileEntity.shouldCollide(entity))
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
                    wallCollision = true;
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
                        if (currTile == null)
                        {
                            continue;
                        }
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
                        foreach (GameEntity tileEntity in currTile.getEntities())
                        {
                            if (tileEntity.getRightEdgeX() - entity.getLeftEdgeX() > distanceToTravelX)
                            {
                                if (tileEntity.OverlapsY(entity) && entity.shouldCollide(tileEntity) && tileEntity.shouldCollide(entity))
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
            leftEdgeTile = getTileIndexFromLeftEdgeX(entity.getLeftEdgeX());
            rightEdgeTile = getTileIndexFromRightEdgeX(entity.getRightEdgeX());


            if (dy > 0)
            { //down

                int distanceToBoundary = currLevel.GetTilesVertical() * Static.TILE_HEIGHT - entity.getBottomEdgeY();
                if (distanceToBoundary < distanceToTravelY)
                {
                    distanceToTravelY = distanceToBoundary;
                    wallCollision = true;
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
                        if (currTile == null)
                        {
                            continue;
                        }
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
                        foreach (GameEntity tileEntity in currTile.getEntities())
                        {
                            if (tileEntity.getTopEdgeY() - entity.getBottomEdgeY() < distanceToTravelY)
                            {
                                if (tileEntity.OverlapsX(entity) && entity.shouldCollide(tileEntity) && tileEntity.shouldCollide(entity))
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
                    wallCollision = true;
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
                        if (currTile == null)
                        {
                            continue;
                        }
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
                        foreach (GameEntity tileEntity in currTile.getEntities())
                        {
                            if (tileEntity.getBottomEdgeY() - entity.getTopEdgeY() > distanceToTravelY)
                            {
                                if (tileEntity.OverlapsX(entity) && entity.shouldCollide(tileEntity) && tileEntity.shouldCollide(entity))
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




        public List<GameEntity> getEntitiesInBounds(Rectangle bounds)
        {

            //hash set so there will be no duplicates
            HashSet<GameEntity> returnSet = new HashSet<GameEntity>();
            for (int i = getTileIndexFromLeftEdgeX(bounds.Left); i <= getTileIndexFromRightEdgeX(bounds.Right); i++)
            {
                for (int j = getTileIndexFromTopEdgeY(bounds.Top); j <= getTileIndexFromBottomEdgeY(bounds.Bottom); j++)
                {
                    Tile currTile = currLevel.getTile(i, j);
                    if (currTile != null)
                    {
                        foreach (GameEntity entity in currTile.getEntities())
                        {
                            returnSet.Add(entity);
                        }
                    }
                }
            }

            List<GameEntity> returnList = new List<GameEntity>();
            foreach (GameEntity entity in returnSet)
            {
                returnList.Add(entity);
            }

            return returnList;
        }

        public int getTileIndexFromLeftEdgeX(int x)
        {
            return (int)Math.Floor((float)x / Static.TILE_WIDTH);
        }

        public int getTileIndexFromRightEdgeX(int x)
        {
            return (int)Math.Ceiling(((float)x / Static.TILE_WIDTH)) - 1;
        }

        public int getTileIndexFromTopEdgeY(int y)
        {
            return (int)Math.Floor((float)y / Static.TILE_HEIGHT);
        }

        public int getTileIndexFromBottomEdgeY(int y)
        {
            return (int)Math.Ceiling(((float)y / Static.TILE_HEIGHT)) - 1;
        }

        public void Spawn(Spawnable spawn)
        {
            spawnQueue.Enqueue(spawn);
        }

        public bool ShouldDamage(int damageType, int targetType)
        {
            return (targetType == Static.TARGET_TYPE_ENEMY && damageType == Static.TARGET_TYPE_FRIENDLY)
                || (targetType == Static.TARGET_TYPE_FRIENDLY && damageType == Static.TARGET_TYPE_ENEMY)
                || (targetType != Static.TARGET_TYPE_NOT_DAMAGEABLE && damageType == Static.DAMAGE_TYPE_ALL)
                || (targetType == Static.TARGET_TYPE_ALL);
        }

        public bool ShouldHeal(int damageType, int targetType)
        {
            return (targetType == Static.TARGET_TYPE_ENEMY && damageType == Static.TARGET_TYPE_ENEMY)
                || (targetType == Static.TARGET_TYPE_FRIENDLY && damageType == Static.TARGET_TYPE_FRIENDLY)
                || (targetType != Static.TARGET_TYPE_NOT_DAMAGEABLE && damageType == Static.DAMAGE_TYPE_ALL)
                || (targetType == Static.TARGET_TYPE_ALL);
        }

        public SpriteFont getSpriteFont()
        {
            return spriteFont;
        }

        public Rectangle getLevelBounds()
        {
            return currLevel.getBounds();
        }

        public List<Player> getPlayers()
        {
            List<Player> list = new List<Player>();
            foreach (Player p in players)
            {
                if (p == null)
                {
                    continue;
                }
                list.Add(p);
            }

            return list;
        }

        public int getEnemyCount()
        {
            int count = 0;
            foreach (GameEntity entity in entities)
            {
                if (entity is BasicEnemy)
                {
                    count++;
                }
            }
            return count;
        }

        public Texture2D getSpriteTexture(int x)
        {
            return spriteMappings[x];
        }

    }
}
