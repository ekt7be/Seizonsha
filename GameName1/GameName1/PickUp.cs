using GameName1.Effects;
using GameName1.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    abstract class PickUp : GameEntity, Interactable
    {


        public PickUp(Seizonsha game, Texture2D sprite, int width, int height)
            : base(game, sprite, width, height, Static.TARGET_TYPE_NOT_DAMAGEABLE, 0)
        {

        }

        public abstract void Interact(Player player);


        public abstract string Message(Player player);

        public abstract bool Available(Player player);

    }
}
