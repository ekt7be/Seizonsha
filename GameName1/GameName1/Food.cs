using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Food: PickUp
    {

        public Food(Seizonsha game)
            : base(game, Static.PIXEL_THIN, 20, 20)
        {
            this.tint = Color.Brown;
            setCollidable(false);
        }
        public override void Interact(Player player)
        {
            setRemove(true);
            game.healEntity(null, player, 100, Static.DAMAGE_TYPE_ALL);
        }

        public override string Message(Player player)
        {
            return "EAT FOOD";
        }

        public override bool Available(Player player)
        {
            return true;
        }

        protected override void OnDie()
        {
            
        }

        public override void OnSpawn()
        {
        }

        public override void collideWithWall()
        {
        }

        public override void collide(GameEntity entity)
        {
        }

        public override string getName()
        {
            return "FOOD";
        }
    }
}
