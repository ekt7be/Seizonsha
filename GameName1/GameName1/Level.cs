using GameName1.Interfaces;
using GameName1.NPCs;
using GameName1.SkillTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public class Level
    {
        private TileMap map;
		public static Dictionary<int, Texture2D> tileSprites = new Dictionary<int,Texture2D>();
        private Seizonsha game;
        private List<SpawnTile> spawnPoints;
        private Random rnd = new Random();

        public Queue<GameEntity> enemyQueue;


        public Level(Seizonsha game)
        {
            this.game = game;
            this.spawnPoints = new List<SpawnTile>();
			this.map = new TileMap(this, game);
            this.enemyQueue = new Queue<GameEntity>();
            initialize();
        }

        private void initialize()
        {

        }




        public void AddSpawnPoint(SpawnTile spawnPoint)
        {
            spawnPoints.Add(spawnPoint);
        }

        public List<SpawnTile> GetSpawnPoints()
        {
            return spawnPoints;
        }

        public void Update()
        {
            TrySpawnEnemy();
        }

        private void TrySpawnEnemy()
        {
            if (enemyQueue.Count < 1)
            {
                return;
            }
            SpawnTile spawn = getRandomSpawnPoint();
            GameEntity enemy = enemyQueue.Peek();
            Vector2 spawnPoint = spawn.getSpawnPosition(enemy);
            if (!game.willCollide(enemy, (int)spawnPoint.X, (int)spawnPoint.Y)){
                game.Spawn(enemy, (int)spawnPoint.X, (int)spawnPoint.Y);
                enemyQueue.Dequeue();
            }
        }

        /*
        public void spawnEnemies(int totalDifficulty)
        {

            for (int i = 0; i < totalDifficulty; i++)
            {
                SpawnTile spawn = getRandomSpawnPoint();
                BasicEnemy enemy = EntityFactory.getBasicEnemy(game);
                Vector2 spawnPoint = spawn.getSpawnPosition(enemy);
                game.Spawn(enemy, (int)spawnPoint.X, (int)spawnPoint.Y);
                game.increaseNumberEnemies();
            }

        }
         * */

        public void populateQueue(int totalDifficulty, List<GameEntity> list)
        {

            if (list != null)
            {
                foreach (GameEntity entity in list)
                {
                    enemyQueue.Enqueue(entity);
                    game.increaseNumberEnemies();
                }
            }

            for (int i = 0; i < totalDifficulty; i++)
            {
                SpawnTile spawn = getRandomSpawnPoint();
                enemyQueue.Enqueue(EntityFactory.getBasicEnemy(game));
                game.increaseNumberEnemies();
            }

        }

        public SpawnTile getRandomSpawnPoint()
        {
            if (spawnPoints.Count <= 0)
            {
                return null;
            }
            int index = rnd.Next(0, spawnPoints.Count);
            return spawnPoints[index];
        }
       


        public void Draw(SpriteBatch spriteBatch, int cameraX, int cameraY)
        {
            map.Draw(spriteBatch, cameraX, cameraY);
 
        }

		public Texture2D getTileSprite(int tileType)
        {
            if (tileSprites.ContainsKey(tileType)){
				return tileSprites[tileType];
            } else{
                return null;
            }
        }

		public Tile getTileFromIndex(int horz, int vert)
        {
            if (horz < 0 || horz > map.GetTilesHorizontal() - 1 || vert < 0 || vert > map.GetTilesVertical() - 1){
                return null;
            }
            return map.tiles[horz, vert];
        }

        public int GetTilesHorizontal()
        {
            return map.GetTilesHorizontal();
        }

        public int GetTilesVertical()
        {
            return map.GetTilesVertical();
        }

        public Rectangle getBounds()
        {
            return new Rectangle(0, 0, map.GetTilesHorizontal() * Static.TILE_WIDTH, map.GetTilesVertical() * Static.TILE_WIDTH);
        }
    }
}
