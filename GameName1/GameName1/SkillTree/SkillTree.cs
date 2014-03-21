using GameName1.Interfaces;
using GameName1.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
    public class SkillTree
    {
        private List<SkillTreeNode> nodes;
        private SkillTreeNode currNode;
        private Player player;
        private Texture2D backgroundTexture;
        public static Dictionary<int, Texture2D> nodeTextures = new Dictionary<int, Texture2D>();

        public SkillTree(Player player, Texture2D backgroundTexture)
        {
            this.player = player;
            this.backgroundTexture = backgroundTexture;
            this.nodes = new List<SkillTreeNode>();
            populateNodes();
        }

        public void populateNodes()
        {
            nodes.Add(new SkillTreeNode(this, new Rectangle(0,0, Static.SKILL_TREE_NODE_WIDTH, Static.SKILL_TREE_NODE_HEIGHT), nodeTextures[Static.SKILL_TREE_NODE_ANY], new ChangeColor(Color.Red)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw background
            Rectangle screenRectangle = new Rectangle(0, 0, Static.SCREEN_WIDTH, Static.SCREEN_HEIGHT);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.Black);
            foreach (SkillTreeNode node in nodes)
            {
                Color tint = Color.White;
                if (currNode == node)
                {
                    tint = Color.Yellow;
                }
                else if (node.isUnlocked())
                {
                    tint = Color.Gray;
                }



                node.Draw(spriteBatch, tint);

            }

        }

        public void Left()
        {
            if (currNode == null)
            {
                return;
            }

            if (currNode.leftNode != null && currNode.leftWeight == Static.SKILL_TREE_WEIGHT_UNLOCKED)
            {
                currNode = currNode.leftNode;
            }
        }

        public void Right()
        {
            if (currNode == null)
            {
                return;
            }

            if (currNode.rightNode != null && currNode.rightWeight == Static.SKILL_TREE_WEIGHT_UNLOCKED)
            {
                currNode = currNode.rightNode;
            }
        }


        public void Up()
        {
            if (currNode == null)
            {
                return;
            }

            if (currNode.topNode != null && currNode.topWeight == Static.SKILL_TREE_WEIGHT_UNLOCKED)
            {
                currNode = currNode.topNode;
            }
        }

        public void Down()
        {
            if (currNode == null)
            {
                return;
            }

            if (currNode.bottomNode != null && currNode.bottomWeight == Static.SKILL_TREE_WEIGHT_UNLOCKED)
            {
                currNode = currNode.bottomNode;
            }
        }

        public void Unlock()
        {
            if (currNode == null)
            {
                return;
            }
            currNode.Unlock(player);
        }

     }

    
}
