using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.AnimationTesting
{
    public class DamageAnimation : Animation
    {


        public DamageAnimation(GameEntity target)
            : base(target, 20)
        {

        }
        protected override void UpdateAnimation(GameEntity target)
        {
            target.setTint(Color.Red);
        }

        public override void OnRemove(GameEntity target)
        {
            target.setDefaultTint();
        }
    }
}
