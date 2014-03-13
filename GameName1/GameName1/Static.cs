using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Static
    {

        //player stuff
        public static readonly int PLAYER_HEIGHT = 100;
        public static readonly int PLAYER_WIDTH = 100;

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


        public static void Debug(string line)
        {
            System.Diagnostics.Debug.Write(line +"\n");
        }

    }
}
