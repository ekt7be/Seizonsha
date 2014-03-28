﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.Effects
{
    class XPEffect : TextEffect
    {

        public XPEffect(Seizonsha game, int amount, int duration, int x, int y)
            : base(game, "+" + amount + "XP", duration, x, y, new Vector2(0,-2), Static.UI_XP_COLOR)
        {

        }
    }
}