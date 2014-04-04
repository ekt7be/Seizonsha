using GameName1.NPCs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public class SpawnTile : Tile
    {
        int direction;

        public SpawnTile(int tileType, int x, int y)
            : base(x, y, true, tileType)
        {
            this.direction = tileType;
        }

        public Vector2 getSpawnPosition(GameEntity entity)
        {
            if (direction == Static.SPAWN_POINT_DOWN)
            {
                return new Vector2(x,y + Static.TILE_WIDTH);
            }
            else if (direction == Static.SPAWN_POINT_RIGHT)
            {
                return new Vector2(x + Static.TILE_WIDTH + entity.width*2, y);
            }
            else if (direction == Static.SPAWN_POINT_LEFT)
            {
                return new Vector2(x - entity.width, y);
            } else {
                return new Vector2(x, y - entity.height*2);
            }
        }



    }




}
