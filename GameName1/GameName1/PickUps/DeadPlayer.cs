using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1.PickUps
{
    class DeadPlayer : PickUp
    {
        private Player deadPlayer;

        public DeadPlayer(Seizonsha game, Player deadPlayer) : base(game, Static.PIXEL_THIN, Static.PLAYER_WIDTH,Static.PLAYER_HEIGHT, true)
        {
            this.deadPlayer = deadPlayer;
            this.setHeight(deadPlayer.height + 20);
            this.setWidth(deadPlayer.width + 20);
            setCollidable(true);
            deadPlayer.drawWeapon = false;

        }
        public override void Interact(Player player)
        {
            deadPlayer.revive();
            game.Spawn(deadPlayer, x, y);
            setRemove(true);
        }

        public override string Message(Player player)
        {
           return "Press A(Enter) to Revive Player " + player.playerIndex.ToString();
        }

        public override bool Available(Player player)
        {
            return player != deadPlayer;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            deadPlayer.spriteSource = new Rectangle(5 * 64, 20 * 64, 64, 64);              
            deadPlayer.Draw(spriteBatch);
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
            return "Dead Player Drop";
        }
    }
}
