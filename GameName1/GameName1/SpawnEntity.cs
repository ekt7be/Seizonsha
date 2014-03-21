﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.Interfaces;
using GameName1.NPCs;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1
{
    //a spawn point for enemies

    public class SpawnEntity : GameEntity
    {
        Random random = new Random();
        int spawnChance;

        public SpawnEntity(Seizonsha game, int spawnChance, int x, int y)
            : base(game, null, x, y, 0, 0, Static.TARGET_TYPE_NOT_DAMAGEABLE, 0)
        {
            this.spawnChance = spawnChance;
        }
        public override void Update()
        {
            int rand = random.Next(1, 1000);

            //10% chance to spawn an enemy every game-second    
            if (rand < spawnChance)
            {
                Texture2D basicEnemyRect = game.Content.Load<Texture2D>("Sprites/basicEnemy");
                game.Spawn(new BasicEnemy(game, basicEnemyRect, 200, 200, 25, 25));
            }
        }
        public override void collide(GameEntity entity)
        {
            //game.Spawn(new TextEffect(game, "collision", 30, x, y));
            // game.damageArea(game.getLevelBounds(), 300, Static.DAMAGE_TYPE_ALL);
            //setCollidable(false);
        }

        public override void collideWithWall()
        {
        }

        protected override void OnDie()
        {
        }

        public override void OnSpawn()
        {
            //nothing happens
        }
    }


}