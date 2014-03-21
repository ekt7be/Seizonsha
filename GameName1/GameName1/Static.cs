using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Static
    {

        //player stuff
        public static readonly int PLAYER_HEIGHT = 20;
        public static readonly int PLAYER_WIDTH = 20;
        public static readonly int PLAYER_MOVE_SPEED = 3;
        public static readonly int PLAYER_MAX_HEALTH = 100;
        public static readonly int PLAYER_MAX_MANA = 100;
        public static readonly int PLAYER_L1_SKILL_INDEX = 0;
        public static readonly int PLAYER_L2_SKILL_INDEX = 1;
        public static readonly int PLAYER_R1_SKILL_INDEX = 2;
        public static readonly int PLAYER_R2_SKILL_INDEX = 3;
		public static readonly int PLAYER_LEFTCLICK_SKILL_INDEX = 0; 


        //dimensions
        public static readonly int SCREEN_WIDTH = 640;
        public static readonly int SCREEN_HEIGHT = 480;
        public static readonly int TILES_ON_SCREEN_X = 80;
        public static readonly int TILES_ON_SCREEN_Y = (int)((float)SCREEN_HEIGHT / (float)SCREEN_WIDTH * TILES_ON_SCREEN_X);
        public static readonly int TILE_WIDTH = SCREEN_WIDTH / TILES_ON_SCREEN_X;
        public static readonly int TILE_HEIGHT = TILE_WIDTH;

        //tile stuff
        public static readonly int TILE_OBSTACLE = 1;
        public static readonly int TILE_NOT_OBSTACLE = 0;

        //Damage
        public static readonly int DAMAGE_TYPE_FRIENDLY = 1;
        public static readonly int DAMAGE_TYPE_ENEMY = 2;
        public static readonly int DAMAGE_TYPE_ALL = 3;
        public static readonly int DAMAGE_TYPE_NO_DAMAGE = 0;

        //Target
        public static readonly int TARGET_TYPE_FRIENDLY = 1;
        public static readonly int TARGET_TYPE_ENEMY = 2;
        public static readonly int TARGET_TYPE_NOT_DAMAGEABLE = 0;
        public static readonly int TARGET_TYPE_ALL = 3;

        //Int to Sprite Mappings
        public static readonly int BASIC_ENEMY_INT = 0;
        public static readonly int PLAYER_INT = 1;

        public static void Debug(string line)
        {
            System.Diagnostics.Debug.Write(line +"\n");
        }

    }
}
