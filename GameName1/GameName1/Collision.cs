using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    class Collision
    {

        public GameEntity e1;
        public GameEntity e2;
        public Collision(GameEntity e1, GameEntity e2)
        {
            this.e1 = e1;
            this.e2 = e2;
        }

        public void execute()
        {
            e1.collide(e2);
            e2.collide(e1);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Collision))
            {
                return base.Equals(obj);
            }

            Collision c2 = (Collision)obj;

            return (this.e1.Equals(c2.e1) && this.e2.Equals(c2.e2)) || (this.e1.Equals(c2.e2) && this.e2.Equals(c2.e1));

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
