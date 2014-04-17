using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.PickUps
{
    class DeadPlayer : PickUp
    {
        private Player deadPlayer;

        public DeadPlayer(Seizonsha game, Player deadPlayer) : base(game, Static.PIXEL_THIN, Static.PLAYER_WIDTH,Static.PLAYER_HEIGHT)
        {
            this.deadPlayer = deadPlayer; 
            setCollidable(true);

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
