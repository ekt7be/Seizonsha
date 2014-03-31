using GameName1.Effects;
using GameName1.NPCs;
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

        public static void createLists(String type)
        {
            if (!active.ContainsKey(type))
            {
                active[type] = new List<GameEntity>();
            }

            if (!dead.ContainsKey(type))
            {
                dead[type] = new List<GameEntity>();
            }
        }


        //call this to check if there is a recyclable instance.  returns null if not
        public static GameEntity tryRecycleInstance(String type)
        {

            createLists(type);

            if (dead[type].Count > 0)
            {
                GameEntity recycled = dead[type][0];
                active[type].Add(recycled);
                dead[type].Remove(recycled);
                Static.Debug("RECYCLING" + type + "DEAD COUNT: " + dead[type].Count);

                return recycled;

            }
            else
            {
                return null;
            }

        }

        //when there are no more to recycle, call this
        public static void addToActive(GameEntity entity)
        {

            createLists(entity.getName());
            active[entity.getName()].Add(entity);

            Static.Debug("ADDING " + entity.getName() + "TO ACTIVE.. ACTIVE COUNT: " + active[entity.getName()].Count);

        }


        //when an entity is removed from the game call this
        public static void removeFromActive(GameEntity entity)
        {
            createLists(entity.getName());
            active[entity.getName()].Remove(entity);
            Static.Debug("REmoving " +entity.getName() +"FROM ACTIVE.. ACTIVE COUNT: " + active[entity.getName()].Count);

            dead[entity.getName()].Add(entity);
            Static.Debug("ADDING " + entity.getName() + "TO DEAD.. DEAD COUNT: " + dead[entity.getName()].Count);


        }




        //instance generation



        public static Bullet getBullet(Seizonsha game, GameEntity user, Texture2D sprite, Rectangle bounds, int amount, int damageType, float bulletSpeed, float directionAngle)
        {
            GameEntity recycled = tryRecycleInstance(Static.TYPE_BULLET);
            if (recycled != null)
            {
                Bullet recycledBullet = (Bullet)recycled;
                recycledBullet.reset(user, sprite, bounds, amount, damageType, bulletSpeed, directionAngle);
                return recycledBullet;
            }

            Bullet newBullet = new Bullet(game, user, sprite, bounds, amount, damageType, bulletSpeed, directionAngle);
            addToActive(newBullet);
            return newBullet;
        }

        public static ExplodingBullet getExplodingBullet(Seizonsha game, GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, float bulletSpeed, float directionAngle)
        {
            GameEntity recycled = tryRecycleInstance(Static.TYPE_EXPLODING_BULLET);
            if (recycled != null)
            {
                ExplodingBullet recycledBullet = (ExplodingBullet)recycled;
                recycledBullet.reset(user, sprite, bounds, amount, damageType, bulletSpeed, directionAngle);
                return recycledBullet;
            }

            ExplodingBullet newBullet = new ExplodingBullet(game, user, sprite, origin, bounds, amount, damageType, bulletSpeed, directionAngle);
            addToActive(newBullet);
            return newBullet;
        }


        public static BasicEnemy getBasicEnemy(Seizonsha game)
        {
            GameEntity recycled = tryRecycleInstance(Static.TYPE_BASIC_ENEMY);
            if (recycled != null)
            {
                BasicEnemy recycledEnemy = (BasicEnemy)recycled;
                recycledEnemy.reset();
                return recycledEnemy;
            }

            BasicEnemy newEnemy = new BasicEnemy(game);
            addToActive(newEnemy);
            return newEnemy;
        }






        public static TextEffect getTextEffect(Seizonsha game, string text, int duration, Vector2 velocity, Color textColor)
        {
            GameEntity recycled = tryRecycleInstance(Static.TYPE_TEXT_EFFECT);
            if (recycled != null)
            {
                TextEffect recycledText = (TextEffect)recycled;
                recycledText.reset(text, textColor, velocity, duration);
                return recycledText;
            }

            TextEffect newText = new TextEffect(game, text, duration, velocity, textColor);
            addToActive(newText);
            return newText;
        }





        public static TextEffect getXPEffect(Seizonsha game, int amount)
        {
            return getTextEffect(game, "+" + amount + "XP", 30, new Vector2(0, -2), Static.UI_XP_COLOR);
        }






        public static AOECone getAOECone(Seizonsha game, GameEntity user, Texture2D sprite, Skill origin, Rectangle bounds, int amount, int damageType, int duration)
        {
            GameEntity recycled = tryRecycleInstance(Static.TYPE_AOE_CONE);
            if (recycled != null)
            {
                AOECone recycledAOE = (AOECone)recycled;
                recycledAOE.reset(user,sprite,origin,bounds,amount,damageType,duration);
                return recycledAOE;
            }

            AOECone newAOE = new AOECone(game, user, sprite, origin, bounds, amount, damageType, duration);
            addToActive(newAOE);
            return newAOE;
        }


    }
}
