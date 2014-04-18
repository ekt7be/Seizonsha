using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Food: PickUp
    {
        String name;
        Texture2D sprite;
        private int amount;

        public Food(Seizonsha game, String name, Texture2D sprite, int amount)
            : base(game, sprite, 20, 20, false)
        {
            this.tint = Color.Brown;
            this.sprite = sprite;
            setCollidable(false);
            this.name = name;
            this.amount = amount;
        }
        public override void Interact(Player player)
        {
            setRemove(true);
            game.healEntity(null, player, amount, Static.DAMAGE_TYPE_ALL);
        }

        public override string Message(Player player)
        {
            return "Press A(Enter) to eat " + getName();
        }

        public override bool Available(Player player)
        {
            return true;
        }

        protected override void OnDie()
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
            return name;
        }
    }
}
