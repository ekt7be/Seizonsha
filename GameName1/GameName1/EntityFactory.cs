using GameName1.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class EntityFactory
    {

        public static Dictionary<String, List<GameEntity>> active = new Dictionary<String, List<GameEntity>>();
        public static Dictionary<String, List<GameEntity>> dead = new Dictionary<String, List<GameEntity>>();
        private static Dictionary<Type, String> typeNames = new Dictionary<Type, String>();


        public static void createLists(String type)
        {
            if (active[type] == null)
            {
                active[type] = new List<GameEntity>();
            }

            if (dead[type] == null)
            {
                dead[type] = new List<GameEntity>();
            }
        }

        public static GameEntity tryRecycleInstance(String type)
        {

            createLists(type);

            if (dead[type].Count > 0)
            {
                GameEntity recycled = dead[type][0];
                //recycled.recycle();  //not implemented
                active[type].Add(recycled);
                return recycled;

            }
            else
            {
                return null;
            }

        }

        /*
        public static void removeFromActive(GameEntity entity)
        {
            createLists(entity.GetType());
            active[entity.GetType()].Remove(entity);
            dead[entity.GetType()].Add(entity);
        }


        public static Bullet getBullet(Seizonsha game, GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, int duration, float bulletSpeed, Vector2 alexDirection)
        {
            Bullet bullet = tryRecycleInstance(Bullet);
        }

        */
    }
}
