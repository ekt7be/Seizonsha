using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Static
    {

        //player stuff
        public static readonly int PLAYER_HEIGHT = 45;
        public static readonly int PLAYER_WIDTH = 45;
		public static readonly int PLAYER_MOVE_SPEED = 6;
        public static readonly int PLAYER_MAX_HEALTH = 100;
        public static readonly int PLAYER_MAX_MANA = 100;
        public static readonly int PLAYER_L1_SKILL_INDEX = 0;
        public static readonly int PLAYER_L2_SKILL_INDEX = 1;
        public static readonly int PLAYER_R1_SKILL_INDEX = 2;
        public static readonly int PLAYER_R2_SKILL_INDEX = 3;
		public static readonly int PLAYER_LEFTCLICK_SKILL_INDEX = 0; 

        //enemy
        public static readonly int BASIC_ENEMY_HEIGHT = 45;
        public static readonly int BASIC_ENEMY_WIDTH = 45;


        //dimensions
		public static int SCREEN_WIDTH = 1280; // 640 x 480, 1280 x 640
		public static int SCREEN_HEIGHT = 720;
		// these values are read in through the map.txt file
		public static int TILES_ON_SCREEN_X = 0;
		public static int TILES_ON_SCREEN_Y = 0;//(int)((float)SCREEN_HEIGHT / (float)SCREEN_WIDTH * TILES_ON_SCREEN_X);
		public static int TILE_WIDTH = 0; //SCREEN_WIDTH / TILES_ON_SCREEN_X;
        public static int TILE_HEIGHT = TILE_WIDTH;

		public static readonly int SCREEN_WIDTH_FIX1 = 0;//SCREEN_WIDTH / 4;	// 640 / 4 = 160
		public static readonly int SCREEN_WIDTH_FIX2 = 0;//SCREEN_WIDTH / 8;	// 640 / 8 = 80 

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

        //UI
        public static readonly Color UI_XP_COLOR = Color.Yellow;
        public static readonly Color UI_DAMAGE_COLOR = Color.Red;
        public static readonly Color UI_HEAL_COLOR = Color.Green;

        //Int to Sprite Mappings
        public static readonly int BASIC_ENEMY_INT = 0;
        public static readonly int PLAYER_INT = 1;

        //Skilltree
        public static readonly int SKILL_TREE_WEIGHT_UNLOCKED = 1;
        public static readonly int SKILL_TREE_WEIGHT_LOCKED = 0;
        public static readonly int SKILL_TREE_NODE_WIDTH = 100;
        public static readonly int SKILL_TREE_NODE_HEIGHT = 100;
        public static readonly int SKILL_TREE_MOVEMENT_RECHARGE = 20;
        public static readonly int SKILL_TREE_CENTER_OFFSET_X = 1000;
        public static readonly int SKILL_TREE_CENTER_OFFSET_Y = 1000;



        //skilltree sprites
        public static readonly int SKILL_TREE_NODE_ANY = 4;

        //spritefont
        public static SpriteFont SPRITE_FONT = null;

        //static pixel for drawing rectangles
        public static Texture2D PIXEL_THIN = null;
        public static Texture2D PIXEL_THICK = null;

        public static void DrawLine(SpriteBatch spriteBatch, Texture2D sprite, Vector2 start, Vector2 end, Color color)
        {

            float distance = Vector2.Distance(start, end);
            float progress = 0f;

            Vector2 slope = new Vector2((end.X - start.X),(end.Y - start.Y));
            slope.Normalize();

            while (progress < distance)
            {

                spriteBatch.Draw(sprite, start + slope*progress, color);
                progress += 1f;
            }
            


        }


        public static void Debug(string line)
        {
            System.Diagnostics.Debug.Write(line +"\n");
        }

    }
}
