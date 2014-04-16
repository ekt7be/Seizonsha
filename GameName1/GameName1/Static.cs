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


        //Animations
        public static readonly int ANIMATION_DAMAGE_DURATION = 20;


        //entity type names

        public static readonly string TYPE_BULLET = "Bullet";
        public static readonly string TYPE_EXPLODING_BULLET = "Exploding Bullet";
        public static readonly string TYPE_EFFECT = "Effect";
        public static readonly string TYPE_TEXT_EFFECT = "Text Effect";
        public static readonly string TYPE_BASIC_ENEMY = "Basic Enemy";
		public static readonly string TYPE_BOSS_ENEMY = "Boss Enemy";

        public static readonly string TYPE_AOE_CIRCLE = "AOE Circle";
        public static readonly string TYPE_AOE_CONE = "AOE Cone";
        public static readonly string TYPE_SWORD_SLASH= "Sword Slash";
        public static readonly string TYPE_PLAYER = "Player";
        public static readonly string TYPE_AOE_STATUS = "AOE Status";
        public static readonly string TYPE_AOE_POLYGON = "AOE Polygon";


		public static int NUM_PLAYERS = 1;

        public static int SECONDS_BETWEEN_WAVE = 30;

        public static float TIME_BETWEEN_SPAWN = 2000f;

        //player stuff
        public static readonly float PLAYER_SPRITE_SCALE = 1f;
        public static readonly int PLAYER_INTERACTION_RANGE = 4;
		public static readonly int PLAYER_HEIGHT = 50;
		public static readonly int PLAYER_WIDTH = 50;
		public static readonly int PLAYER_MOVE_SPEED = 5;
        public static readonly int PLAYER_MAX_HEALTH = 100;
		public static readonly int PLAYER_MAX_MANA = 100;
        public static readonly float PLAYER_START_MANA_REGEN = .08f;
		public static readonly int PLAYER_L1_SKILL_INDEX = 0; 
        public static readonly int PLAYER_L2_SKILL_INDEX = 1;
        public static readonly int PLAYER_R1_SKILL_INDEX = 2;
        public static readonly int PLAYER_R2_SKILL_INDEX = 3;
		public static readonly int PLAYER_LEFTCLICK_SKILL_INDEX = 0;
        public static readonly Color PLAYER_ARMOR_COLOR_0 = Color.White;
        public static readonly float PLAYER_SHIELD_1 = .2f;
        public static readonly Color PLAYER_ARMOR_COLOR_1 = Color.Cyan;
        public static readonly float PLAYER_SHIELD_2 = .4f;
        public static readonly Color PLAYER_ARMOR_COLOR_2 = Color.Gold;
        public static readonly float PLAYER_SHIELD_3 = .7f;
        public static readonly Color PLAYER_ARMOR_COLOR_3 = Color.Black;


        //drops
        public static readonly float DROP_DURATION = 20000;


        //basic enemy
		public static readonly int BASIC_ENEMY_HEIGHT = 50;
		public static readonly int BASIC_ENEMY_WIDTH = 50;
        public static readonly int BASIC_ENEMY_DAMAGE = 5;
		public static readonly float BASIC_ENEMY_SPEED = 2f;
        public static readonly int BASIC_ENEMY_HEALTH = 30;
        public static readonly float BASIC_ENEMY_SPRITE_SCALE = 1f;
        public static readonly int BASIC_ENEMY_XP = 100;
		public static readonly float BASIC_ENEMY_PATH_REFRESH = 500f;
        public static readonly float BASIC_ENEMY_EXTRA_ATTACK_RECHARGE = 2000f;


        //boss
        public static readonly int BOSS_ENEMY_XP = 1000;
        public static readonly int BOSS_ENEMY_HEIGHT = 32 * 3;
        public static readonly int BOSS_ENEMY_WIDTH = 32 * 3;
        public static readonly float BOSS_ENEMY_SPEED = 4f;


        //rusty shank
        public static readonly string WEAPON_RUSTY_SHANK_NAME = "Rusty Shank";
        public static readonly int WEAPON_RUSTY_SHANK_DAMAGE = 5;
        public static readonly int WEAPON_RUSTY_SHANK_RECHARGE = 20;
        public static readonly int WEAPON_RUSTY_SHANK_LEVEL = 1;

        //rusty sword
        public static readonly string WEAPON_RUSTY_SWORD_NAME = "Rusty Sword";
        public static readonly int WEAPON_RUSTY_SWORD_DAMAGE = 20;
        public static readonly int WEAPON_RUSTY_SWORD_RECHARGE = 70;
        public static readonly int WEAPON_RUSTY_SWORD_LEVEL = 1;

        //dank sword
        public static readonly string WEAPON_DANK_SWORD_NAME = "Shadow Blade";
        public static readonly int WEAPON_DANK_SWORD_DAMAGE = 40;
        public static readonly int WEAPON_DANK_SWORD_RECHARGE = 50;
        public static readonly int WEAPON_DANK_SWORD_LEVEL = 2;

        //revolver
        public static readonly string WEAPON_REVOLVER_NAME = "Revolver";
        public static readonly int WEAPON_REVOLVER_DAMAGE = 100;
        public static readonly int WEAPON_REVOLVER_RECHARGE = 150;
        public static readonly int WEAPON_REVOLVER_FREEZE = 20;
        public static readonly float WEAPON_REVOLVER_BULLET_SPEED = 10f;
        public static readonly int WEAPON_REVOLVER_CLIP = 6;
        public static readonly int WEAPON_REVOLVER_LEVEL = 1;

        //OKGUN
        public static readonly string WEAPON_OKGUN_NAME = "Glock";
        public static readonly int WEAPON_OKGUN_DAMAGE = 10;
        public static readonly int WEAPON_OKGUN_RECHARGE = 30;
        public static readonly int WEAPON_OKGUN_FREEZE = 10;
        public static readonly float WEAPON_OKGUN_BULLET_SPEED = 10f;
        public static readonly int WEAPON_OKGUN_CLIP = 30;
        public static readonly int WEAPON_OKGUN_LEVEL = 1;

        //dimensions
		public static int SCREEN_WIDTH = 1280; // 640 x 480, 1280 x 640
		public static int SCREEN_HEIGHT = 720;

		// these values are read in through the map.txt file
		public static int TILES_ON_SCREEN_X = 0;
		public static int TILES_ON_SCREEN_Y = 0;
		public static int TILE_WIDTH = 0;

        //tile stuff
        public static readonly int TILE_GRASS = 961;
        public static readonly int TILE_NERVES = 840;
        public static readonly int TILE_BRICK = 1033;
        public static readonly int TILE_STONE = 1157;

        //spawn directions
        public static readonly int SPAWN_POINT_UP = 1;
        public static readonly int SPAWN_POINT_RIGHT = 2;
        public static readonly int SPAWN_POINT_DOWN = 3;
        public static readonly int SPAWN_POINT_LEFT = 4;


        //Damage
        public static readonly int DAMAGE_TYPE_GOOD = 1;
        public static readonly int DAMAGE_TYPE_BAD = 2;
        public static readonly int DAMAGE_TYPE_ALL = 3;
        public static readonly int DAMAGE_TYPE_NO_DAMAGE = 0;

        //Target
        public static readonly int TARGET_TYPE_GOOD = 1;
        public static readonly int TARGET_TYPE_BAD = 2;
        public static readonly int TARGET_TYPE_NOT_DAMAGEABLE = 0;
        public static readonly int TARGET_TYPE_ALL = 3;

        //UI
        public static readonly Color UI_XP_COLOR = Color.Yellow;
        public static readonly Color UI_DAMAGE_COLOR = Color.Red;
        public static readonly Color UI_HEAL_COLOR = Color.Green;

        //Int to Sprite Mappings
        public static readonly int SPRITE_BASIC_ENEMY_INT = 0;
        public static readonly int SPRITE_PLAYER_INT = 1;

        public static readonly int SPRITE_PLATE_ARMOR_HEAD = 2;
        public static readonly int SPRITE_PLATE_ARMOR_FEET = 3;
        public static readonly int SPRITE_PLATE_ARMOR_GLOVES = 4;
        public static readonly int SPRITE_PLATE_ARMOR_PANTS = 5;
        public static readonly int SPRITE_PLATE_ARMOR_ARMS_SHOULDER = 6;
        public static readonly int SPRITE_PLATE_ARMOR_TORSO = 7;
        public static readonly int SPRITE_FIREBALL = 8;
        public static readonly int SPRITE_BULLET = 9;
        public static readonly int SPRITE_HEAL = 10;
        public static readonly int SPRITE_SWORD = 11;
        public static readonly int SPRITE_RETICLE = 12;

        //Skilltree
        public static readonly int SKILL_TREE_WEIGHT_UNLOCKED = 1;
        public static readonly int SKILL_TREE_WEIGHT_LOCKED = 0;
        public static readonly int SKILL_TREE_NODE_WIDTH = 70;
        public static readonly int SKILL_TREE_NODE_HEIGHT = 70;
        public static readonly int SKILL_TREE_MOVEMENT_RECHARGE = 20;

        //Firelance
        public static readonly int FIRELANCE_COST = 15;
        public static readonly int FIRELANCE_RECHARGE = 5;
        public static readonly int FIRELANCE_DAMAGE = 25;
        public static readonly string FIRELANCE_NAME = "Fire Lance";

        //Fireball
        public static readonly string FIREBALL_NAME = "Fireball";

        //Blizzard
        public static readonly string BLIZZARD_NAME = "Blizzard";

        //Kick
        public static readonly string KICK_NAME = "Kick";

        //Bash
        public static readonly string BASH_NAME = "Bash";

        //Healing Touch
        public static readonly string HEALING_TOUCH_NAME = "Healing Touch";

        //skilltree sprites
		public static readonly string SKILL_TREE_NODE_ANY = "Blank";


        //Life Drain
        public static readonly string LIFE_DRAIN_NAME = "Life Drain";

        //Teleport
        public static readonly string TELEPORT_NAME = "Teleport";

        //spritefont
        public static SpriteFont SPRITE_FONT = null;
		public static SpriteFont SPRITEFONT_Arial10 = null;
		public static SpriteFont SPRITEFONT_Calibri10 = null;
		public static SpriteFont SPRITEFONT_Calibri12 = null;
		public static SpriteFont SPRITEFONT_Calibri14 = null;





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
