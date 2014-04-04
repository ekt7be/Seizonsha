using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
    class BlankNode : SkillTreeNode
    {

        public BlankNode(SkillTree skilltree, int x,int  y, Texture2D sprite) : base(skilltree,x,y, sprite, null, 0)
        {

        }
    }
}
