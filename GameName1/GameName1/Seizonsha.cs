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
using GameName1.AnimationTesting;
#endregion

namespace GameName1
{
    public class Seizonsha : Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        SpriteBatch spriteBatch;

        Texture2D playerRect;
        private SpriteFont spriteFont;
        private static readonly Random randomGen = new Random();
        private Player[] players;
        private List<GameEntity> entities;
        private Queue<GameEntity> removalQueue;
        private Queue<Spawnable> spawnQueue;
        private Queue<Vector2> spawnPointQueue;
        private HashSet<Collision> collisions;
        private List<AI> AIs;
        private Level currLevel;
        private PolygonIntersection.MainForm mainForm = new PolygonIntersection.MainForm();

        private bool paused;
		FPSCounterComponent fps;
		private bool showFPS = true; 

		private float sinceLastWaveCleared; 
	
        // By preloading any assets used by UI rendering, we avoid framerate glitches
        // when they suddenly need to be loaded in the middle of a menu transition.
        static readonly string[] preloadAssets =
        {
            "gradient",
        };

		bool waveCleared; 


        public static Dictionary<int, Texture2D> spriteMappings = new Dictionary<int, Texture2D>();

		// AlexAlpha
		Viewport defaultView, p1View, p2View, p3View, p4View; 
		Camera p1Camera, p2Camera, p3Camera, p4Camera; 
		List<Camera> cameras; 
		Rectangle yDivider, xDivider; 
		List<Rectangle> dividers; 

		Dictionary<int, PlayerIndex> playerToController;


        public int Wave { get; set; }
        public int numberEnemies;
        private int difficulty;



        public Seizonsha() : base()
        {

            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 853;
            graphics.PreferredBackBufferHeight = 480;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }
			
	
		// for pathfinding 
		public Tile getTileFromCoord(int x, int y) {
			return currLevel.getTileFromIndex(getTileIndexFromLeftEdgeX(x), getTileIndexFromTopEdgeY(y)); 
		}

		public Tile getTileFromIndex(int x, int y) {
			return currLevel.getTileFromIndex(x, y); 
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
            initializeVariables();

            graphics.PreferredBackBufferHeight = Static.SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = Static.SCREEN_WIDTH;

            //just for testing -- makes a rectangle
            //Texture2D playerRect = new Texture2D(GraphicsDevice, Static.PLAYER_HEIGHT, Static.PLAYER_WIDTH);

            playerRect = Content.Load<Texture2D>("Sprites/humanfullspritesheet");
            Texture2D npcRect = Content.Load<Texture2D>("Sprites/player");
            Texture2D basicEnemyRect = Content.Load<Texture2D>("Sprites/SkeletonWalkingSpritesheet");
            Texture2D nodeRect = Content.Load<Texture2D>("Sprites/SkillNode");

            Texture2D plateArmorHead = Content.Load<Texture2D>("Sprites/PlateArmorHeadSpritesheet");
            Texture2D plateArmorFeet = Content.Load<Texture2D>("Sprites/PlateArmorFeetSpritesheet");
            Texture2D plateArmorGloves = Content.Load<Texture2D>("Sprites/PlateArmorGlovesSpritesheet");
            Texture2D plateArmorPants = Content.Load<Texture2D>("Sprites/PlateArmorPantsSpritesheet");
            Texture2D plateArmorArmsShoulder = Content.Load<Texture2D>("Sprites/PlateArmorArmsShoulderSpriteSheet");
            Texture2D plateArmorTorso = Content.Load<Texture2D>("Sprites/PlateArmorTorsoSpritesheet");
            Texture2D fireball = Content.Load<Texture2D>("Sprites/fireballsprite");
            Texture2D heal = Content.Load<Texture2D>("Sprites/healsprite");
            Texture2D bullet = Content.Load<Texture2D>("Sprites/bulletsprite");
            Texture2D sword = Content.Load<Texture2D>("Sprites/swordspritesheetfull");

            SkillTree.SkillTree.nodeTextures.Add(Static.SKILL_TREE_NODE_ANY, nodeRect);

            initTileSprites();

            spriteMappings.Add(Static.SPRITE_BASIC_ENEMY_INT, basicEnemyRect);
            spriteMappings.Add(Static.SPRITE_PLAYER_INT, playerRect);
            spriteMappings.Add(Static.SPRITE_PLATE_ARMOR_HEAD, plateArmorHead);
            spriteMappings.Add(Static.SPRITE_PLATE_ARMOR_FEET, plateArmorFeet);
            spriteMappings.Add(Static.SPRITE_PLATE_ARMOR_GLOVES, plateArmorGloves);
            spriteMappings.Add(Static.SPRITE_PLATE_ARMOR_PANTS, plateArmorPants);
            spriteMappings.Add(Static.SPRITE_PLATE_ARMOR_ARMS_SHOULDER, plateArmorArmsShoulder);
            spriteMappings.Add(Static.SPRITE_PLATE_ARMOR_TORSO, plateArmorTorso);
            spriteMappings.Add(Static.SPRITE_FIREBALL, fireball);
            spriteMappings.Add(Static.SPRITE_HEAL, heal);
            spriteMappings.Add(Static.SPRITE_BULLET, bullet);
            spriteMappings.Add(Static.SPRITE_SWORD, sword);



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

            spawnInitialEntities();

            base.Initialize();
        }

        public void initializeVariables()
        {
            entities = new List<GameEntity>();
            AIs = new List<AI>();
            removalQueue = new Queue<GameEntity>();
            spawnQueue = new Queue<Spawnable>();
            spawnPointQueue = new Queue<Vector2>();
            collisions = new HashSet<Collision>();
            currLevel = new Level(this);

            paused = false;
            this.players = new Player[4];

            initViewports(Static.NUM_PLAYERS);

			fps = new FPSCounterComponent(this, spriteBatch, spriteFont); 

        }

        public void spawnInitialEntities()
        {
            for (int i = 0; i < Static.NUM_PLAYERS; i++)
            {
                cameras[i] = new Camera();
                players[i] = new Player(this, playerToController[i + 1], playerRect, cameras[i]);
                Spawn(players[i], 500, 100 + (i * 40));
            }



            this.difficulty = 5;
            this.numberEnemies = 0;
            this.Wave = 0;

			//Spawn(new Food(this), 500, 600);

			waveCleared = false; 

			//Spawn(new BasicEnemy(this), 500, 800);


			//Spawn(new BossEnemy(this), 500, 800);

			//Spawn(new BasicEnemy(this), 500, 800);

			//Spawn(new BasicEnemy(this), 500, 800);

			//Spawn(new BasicEnemy(this), 600, 800);


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

			yDivider = new Rectangle(Static.SCREEN_WIDTH/2-(divWidth/2), 0, divWidth, Static.SCREEN_HEIGHT*2);
			xDivider = new Rectangle(0, Static.SCREEN_HEIGHT/2-(divWidth/2), Static.SCREEN_WIDTH*2, divWidth);

			dividers.Add(yDivider); 
			dividers.Add(xDivider);
		}

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Font");
            Static.SPRITE_FONT = spriteFont;
            Static.PIXEL_THIN = new Texture2D(GraphicsDevice, 1, 1);
            Static.PIXEL_THIN.SetData(new[] { Color.White });
            Static.PIXEL_THICK = new Texture2D(GraphicsDevice, 3, 3);
            Static.PIXEL_THICK.SetData(new[] { Color.White });
            foreach (string asset in preloadAssets)
            {
                Content.Load<Texture2D>(asset);
            }
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
        public void UpdateGame(GameTime gameTime)
        {


			sinceLastWaveCleared += (float)gameTime.ElapsedGameTime.TotalMilliseconds; 

            //flag entities to be removed
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

                //this causes memory leak if factory is not recycling particular class
                if (EntityFactory.isRecycling(remEntity.getName())){
                    EntityFactory.removeFromActive(remEntity);
                }

            }

            //remove animations flagged for removal
            foreach (GameEntity entity in entities)
            {
                entity.ClearAnimations();
            }



            //spawn Spawnables
            while (spawnQueue.Count > 0)
            {
                Spawnable spawn = spawnQueue.Dequeue();
                Vector2 spawnPoint = spawnPointQueue.Dequeue();
                if (spawn is GameEntity)
                {


                    entities.Add((GameEntity)spawn);

                    if (spawn is AI)
                    {
                        AIs.Add((AI)spawn);
                    }

                    ((GameEntity)spawn).OnSpawnAll((int)spawnPoint.X, (int)spawnPoint.Y);
                    if (((GameEntity)spawn).isCollidable())
                    {
                        BindEntityToTiles((GameEntity)spawn, true);
                    }

                }
                else
                {
                    spawn.OnSpawn();
                }

			


            }

            //check for wave completion
			if (numberEnemies <= 0 && !waveCleared)
            {
				WaveCleared(gameTime);
				//pause and do other stuff, maybe set time

            }
			else if (waveCleared) {
				if (sinceLastWaveCleared >= 20000) {
					WaveBegin();
				}
			}


            currLevel.Update();

            //update all entities including players
            foreach (GameEntity entity in entities)
            {

                entity.UpdateAll(gameTime);
            }

            //handle all player input
            foreach (Player player in players)
            {
                if (player == null || player.isDead())
                {
                    continue;
                }

				handlePlayerInput(player);

				player.camera.Update(this.getPlayers().Count, player, this.getLevelBounds()); 
            }

            //run AI
            foreach (AI ai in AIs)
            {
				ai.AI(gameTime);
            }

            //execute buffered movement
            foreach (GameEntity entity in entities)
            {
                entity.executeMovement();
            }

            //execute all collisions
            foreach (Collision collision in collisions)
            {
                collision.execute();
            }
            collisions.Clear();


            //update animations
            foreach (GameEntity entity in entities)
            {
                entity.UpdateAnimation(gameTime);
            }

			fps.Update(gameTime); 
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }

        public void DrawGame(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (Player player in players)
            {
                if (player == null)
                {
                    continue;
                }

                // AlexAlpha
                switch (Array.IndexOf(players, player) + 1)
                {
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
                    //was Deferred
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

            GraphicsDevice.Viewport = defaultView;

            spriteBatch.Begin();

            	drawSplitscreenDividers(Static.NUM_PLAYERS);

            spriteBatch.End();

			if (showFPS)
				fps.Draw(gameTime); 

			spriteBatch.Begin();

				if (waveCleared)
					spriteBatch.DrawString(Static.SPRITE_FONT, 20-(int)sinceLastWaveCleared/1000+
					" seconds until next wave...\n" +
					"press space(start) to open skill tree!",
					new Vector2(defaultView.Width-390, defaultView.Height-50), Color.White); 
			spriteBatch.End();

        }

		void drawSplitscreenDividers(int numberOfPlayers) {

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
                    player.SkillTreeButtonDown();
                } else if (GamePad.GetState(player.playerIndex).Buttons.Start == ButtonState.Released || Keyboard.GetState().IsKeyUp(Keys.Space)){
                    player.SkillTreeButtonRelease();
                }

                if (GamePad.GetState(player.playerIndex).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    player.AButton();
                }

                if (GamePad.GetState(player.playerIndex).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y > .5 || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    player.UpButton();

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X < -.5 || Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    player.LeftButton();

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X > .5 || Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    player.RightButton();

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y < -.5 || Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    player.DownButton();
                }
                
                if(Math.Abs(GamePad.GetState(player.playerIndex).ThumbSticks.Left.X) <= .5 && Math.Abs(GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y) <= .5 && Keyboard.GetState().GetPressedKeys().Length == 0)
                {
                    player.noMovement();
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
                    player.SkillTreeButtonDown();
                }
                else if (GamePad.GetState(player.playerIndex).Buttons.Start == ButtonState.Released)
                {
                    player.SkillTreeButtonRelease();

                }
                 
                if (GamePad.GetState(player.playerIndex).Buttons.A == ButtonState.Pressed)
                {
                    player.AButton();
                }

                if (GamePad.GetState(player.playerIndex).Buttons.Back == ButtonState.Pressed)
                {
                    Exit();
                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y > .5)
                {
                    player.UpButton();
                    //player.rotateToAngle((float)(3 * Math.PI / 2));

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X < -.5)
                {
                    player.LeftButton();
                    //player.rotateToAngle((float)Math.PI);

                }
                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.X > .5)
                {
                    player.RightButton();
                    // player.rotateToAngle((float)0);

                }

                if (GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y < -.5)
                {
                    player.DownButton();
                    // player.rotateToAngle((float)Math.PI / 2);
                }

                if (Math.Abs(GamePad.GetState(player.playerIndex).ThumbSticks.Left.X) <= .5 && Math.Abs(GamePad.GetState(player.playerIndex).ThumbSticks.Left.Y) <= .5)
                {
                    player.noMovement();
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
					Tile currTile = currLevel.getTileFromIndex(i, j);
                    if (currTile != null) //if within bounds of level
                    {
						currLevel.getTileFromIndex(i, j).BindEntity(entity, bind);
                    }
                }
            }
        }



        public void healEntity(GameEntity user, GameEntity target, int amount, int damageType)
        {
            if (ShouldHeal(damageType, target.getTargetType())){
                incEntityHealth(target,amount);

                TextEffect text = EntityFactory.getTextEffect(this, amount + "", 10, new Vector2(0, -2), Color.Green); //new TextEffect(this, amount + "", 10, new Vector2(0, -2), Color.Green);
                Spawn(text,target.getCenterX(), target.getCenterY() - 60);
            }
        }


        public void damageEntity(GameEntity user, GameEntity target, int amount, int damageType)
        {

            if (amount == 0)
            {
                return;
            }

            if (ShouldDamage(damageType, target.getTargetType()))
            {

                    TextEffect text = EntityFactory.getTextEffect(this, amount + "", 10, new Vector2(0, -2), Color.Red);
                    Spawn(text, target.getCenterX(), target.getCenterY() - 60);

                if (user == null)
                {
                    incEntityHealth(target, -1 * amount);
                    //target.AddAnimation(new DamageAnimation(target));
                    target.makeDamageAnimation();
                    return;
                }

                user.OnDamageOther(target, amount); // if damage goes through
                //target.AddAnimation(new DamageAnimation(target));
                target.makeDamageAnimation();
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
            if (target.health <= 0)
            {
                target.health = 0;
                if (!target.shouldRemove())
                {
                    target.die();
                    return true;

                }
                return false;
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


        public bool willCollide(GameEntity entity, int x, int y)
        {

            Rectangle bounds = new Rectangle(x, y, entity.width, entity.height);
            bool collide = false;

            for (int i = getTileIndexFromLeftEdgeX(bounds.Left); i <= getTileIndexFromRightEdgeX(bounds.Right); i++)
            {
                for (int j = getTileIndexFromTopEdgeY(bounds.Top); j <= getTileIndexFromBottomEdgeY(bounds.Bottom); j++)
                {
					Tile tile = currLevel.getTileFromIndex(i, j);
                    if (tile == null)
                    {
                        collide = true;
                        continue;
                    }
                    if (tile.isObstacle())
                    {
                        collide = true;
                    }
                }
            }

            foreach (GameEntity tileEntity in getEntitiesInBounds(bounds)){
                if (entity.shouldCollide(tileEntity) && tileEntity.shouldCollide(tileEntity) && entity!=tileEntity){
                    collide = true;
                }
            }

            return collide;
        }

		public void moveGameEntityWithoutCollision(GameEntity entity, float x, float y)
        {
            entity.floatx = x;
            entity.floaty = y;
            entity.x = (int)entity.floatx;
            entity.y = (int)entity.floaty;
            entity.hitbox.X = entity.x;
            entity.hitbox.Y = entity.y;
        }

        public void moveGameEntity(GameEntity entity, float dx, float dy)
        {

            BindEntityToTiles(entity, false);

            if (!entity.isCollidable()) //skip collision detection
            {
                moveGameEntityWithoutCollision(entity, entity.floatx + dx, entity.floaty + dy);
                BindEntityToTiles(entity, true);
                return;
            }


            bool wallCollision = false;

            //calculate number of tiles distance covers
            int tilesX = (int)Math.Floor((float)Math.Abs(dx) / Static.TILE_WIDTH) + 1;
            int tilesY = (int)Math.Floor((float)Math.Abs(dy) / Static.TILE_WIDTH) + 1;

            //get entity bounds
            int leftEdgeTile = getTileIndexFromLeftEdgeX(entity.getLeftEdgeX());
            int rightEdgeTile = getTileIndexFromRightEdgeX(entity.getRightEdgeX());
            int topEdgeTile = getTileIndexFromTopEdgeY(entity.getTopEdgeY());
            int bottomEdgeTile = getTileIndexFromBottomEdgeY(entity.getBottomEdgeY());

            float distanceToTravelX = dx;
            float distanceToTravelY = dy;

            //find distance to level boundary in movement direction and see if it is less move amount
            //figure out how many tiles your movement translates to in each direction
            //scan tiles in front of you to find closest obstacle in that direction
            //final movement is min of original movement and distance to obstacle


            if (dx > 0)
            {
                //right

                float distanceToBoundary = currLevel.GetTilesHorizontal() * Static.TILE_WIDTH - entity.getRightEdgeFloatX();
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
						Tile currTile = currLevel.getTileFromIndex(rightEdgeTile + i, j);
                        if (currTile == null)
                        {
                            continue;
                        }
                        if (currTile.isObstacle())
                        {
                            float distanceToTile = currTile.x - entity.getRightEdgeFloatX();
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
                            if (tileEntity.getLeftEdgeFloatX() - entity.getRightEdgeFloatX() < distanceToTravelX)
                            {
                                if (tileEntity.OverlapsY(entity) && entity.shouldCollide(tileEntity) && tileEntity.shouldCollide(entity))
                                {
                                    distanceToTravelX = tileEntity.getLeftEdgeFloatX() - entity.getRightEdgeFloatX();
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


                float distanceToBoundary = -1 * entity.getLeftEdgeFloatX();
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
						Tile currTile = currLevel.getTileFromIndex(leftEdgeTile - i, j);
                        if (currTile == null)
                        {
                            continue;
                        }
                        if (currTile.isObstacle())
                        {
                            float distanceToTile = (currTile.x + Static.TILE_WIDTH) - entity.getLeftEdgeFloatX();
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
                            if (tileEntity.getRightEdgeFloatX() - entity.getLeftEdgeFloatX() > distanceToTravelX)
                            {
                                if (tileEntity.OverlapsY(entity) && entity.shouldCollide(tileEntity) && tileEntity.shouldCollide(entity))
                                {
                                    distanceToTravelX = tileEntity.getRightEdgeFloatX() - entity.getLeftEdgeFloatX();
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

            entity.floatx = entity.floatx + distanceToTravelX;
            entity.lastMovement.X = distanceToTravelX;
            //entity.hitbox.Offset(distanceToTravelX, 0);
            //entity.hitbox.Offset((int)entity.floatx - entity.x, 0);
            entity.x = (int)entity.floatx;
            entity.hitbox.X = entity.x;
            leftEdgeTile = getTileIndexFromLeftEdgeX(entity.getLeftEdgeX());
            rightEdgeTile = getTileIndexFromRightEdgeX(entity.getRightEdgeX());


            if (dy > 0)
            { //down

                float distanceToBoundary = currLevel.GetTilesVertical() * Static.TILE_WIDTH - entity.getBottomEdgeFloatY();
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
						Tile currTile = currLevel.getTileFromIndex(j, bottomEdgeTile + i);
                        if (currTile == null)
                        {
                            continue;
                        }
                        if (currTile.isObstacle())
                        {
                            float distanceToTile = currTile.y - entity.getBottomEdgeFloatY();
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
                            if (tileEntity.getTopEdgeFloatY() - entity.getBottomEdgeFloatY() < distanceToTravelY)
                            {
                                if (tileEntity.OverlapsX(entity) && entity.shouldCollide(tileEntity) && tileEntity.shouldCollide(entity))
                                {
                                    distanceToTravelY = tileEntity.getTopEdgeFloatY() - entity.getBottomEdgeFloatY();
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


                float distanceToBoundary = -1 * entity.getTopEdgeFloatY();
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
						Tile currTile = currLevel.getTileFromIndex(j, topEdgeTile - i);
                        if (currTile == null)
                        {
                            continue;
                        }
                        if (currTile.isObstacle())
                        {
                            float distanceToTile = (currTile.y + Static.TILE_WIDTH) - entity.getTopEdgeFloatY();
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
                            if (tileEntity.getBottomEdgeFloatY() - entity.getTopEdgeFloatY() > distanceToTravelY)
                            {
                                if (tileEntity.OverlapsX(entity) && entity.shouldCollide(tileEntity) && tileEntity.shouldCollide(entity))
                                {
                                    distanceToTravelY = tileEntity.getBottomEdgeFloatY() - entity.getTopEdgeFloatY();
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


            entity.floaty = entity.floaty + distanceToTravelY;
            entity.lastMovement.Y = distanceToTravelY;
            //entity.hitbox.Offset(0, (int)entity.floaty - entity.y);
            entity.y = (int)entity.floaty;
            entity.hitbox.Y = entity.y;

            if (wallCollision)
            {
                entity.collideWithWall();
            }

            BindEntityToTiles(entity, true);

        }


        public void increaseNumberEnemies()
        {
            numberEnemies++;
        }

        public void decreaseNumberEnemies()
        {
            numberEnemies--;

        }

        public List<Tile> getTilesInBounds(Rectangle bounds)
        {
            List<Tile> returnList = new List<Tile>();

            for (int i = getTileIndexFromLeftEdgeX(bounds.Left); i <= getTileIndexFromRightEdgeX(bounds.Right); i++)
            {
                for (int j = getTileIndexFromTopEdgeY(bounds.Top); j <= getTileIndexFromBottomEdgeY(bounds.Bottom); j++)
                {
                    returnList.Add(currLevel.getTileFromIndex(i, j));
                }
            }

            return returnList;
        }

       public List<GameEntity> getEntitiesInBounds(PolygonIntersection.Polygon p)
        {
            List<GameEntity> ret = new List<GameEntity>();
            foreach (GameEntity entity in this.entities)
            {
                if((mainForm.PolygonCollision(p, entity.getPolygonFromHitbox(), new PolygonIntersection.Vector(0,0)).Intersect)) ret.Add(entity);
            }
            return ret;
        }

        public List<GameEntity> getEntitiesInBounds(Rectangle bounds)
        {

            //hash set so there will be no duplicates
            HashSet<GameEntity> returnSet = new HashSet<GameEntity>();
            for (int i = getTileIndexFromLeftEdgeX(bounds.Left); i <= getTileIndexFromRightEdgeX(bounds.Right); i++)
            {
                for (int j = getTileIndexFromTopEdgeY(bounds.Top); j <= getTileIndexFromBottomEdgeY(bounds.Bottom); j++)
                {
					Tile currTile = currLevel.getTileFromIndex(i, j);
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
            return (int)Math.Floor((float)y / Static.TILE_WIDTH);
        }

        public int getTileIndexFromBottomEdgeY(int y)
        {
            return (int)Math.Ceiling(((float)y / Static.TILE_WIDTH)) - 1;
        }

        public Tile getTileFromCoordinates(int x, int y)
        {
			return currLevel.getTileFromIndex(x, y);
        }

        public void Spawn(Spawnable spawn, int x, int y)
        {
            spawnQueue.Enqueue(spawn);
            spawnPointQueue.Enqueue(new Vector2(x, y));
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


		public void WaveCleared(GameTime gameTime)
        {
			sinceLastWaveCleared = 0; 

			waveCleared = true; 

            difficulty++;
            difficulty++;

        }

        public void WaveBegin()
        {
			waveCleared = false; 
            Wave++;
            //currLevel.spawnEnemies(difficulty);

            if (this.Wave % 3 == 0)
            {
                List<GameEntity> enemyList = new List<GameEntity>();
                enemyList.Add(new BossEnemy(this));
                //Spawn(new BossEnemy(this), 500, 800);
                currLevel.populateQueue(difficulty, enemyList);
            }
            else
            {
                currLevel.populateQueue(difficulty, null);
            }

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
