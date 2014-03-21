using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
    class SkillTreeNode
    {

        private SkillTree skilltree; 
        public SkillTreeNode leftNode;
        public int leftWeight;
        public SkillTreeNode rightNode;
        public int rightWeight;
        public SkillTreeNode bottomNode;
        public int bottomWeight;
        public SkillTreeNode topNode;
        public int topWeight;
        private Rectangle bounds;
        private Unlockable unlockable;
        private Texture2D sprite;

        private bool unlocked;


        public SkillTreeNode(SkillTree skilltree, Rectangle bounds, Texture2D sprite, Unlockable unlockable)
        {
            this.unlockable = unlockable;
            this.unlocked = false;
            this.bounds = bounds;
            this.sprite = sprite;
            this.skilltree =skilltree;
        }


        public void Unlock(Player player)
        {
            unlockable.OnUnlock(player);
            this.unlocked = true;

            setLeftWeight(Static.SKILL_TREE_WEIGHT_UNLOCKED);
            setRightWeight(Static.SKILL_TREE_WEIGHT_UNLOCKED);
            setBottomWeight(Static.SKILL_TREE_WEIGHT_UNLOCKED);
            setTopWeight(Static.SKILL_TREE_WEIGHT_UNLOCKED);



        }

        public bool isUnlocked()
        {
            return unlocked;
        }


        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(sprite, bounds, color);
        }

        public void attachLeft(SkillTreeNode node, int weight)
        {
            this.leftNode = node;
            setLeftWeight(weight);
        }
        public void setLeftWeight(int weight)
        {
            if (this.leftNode == null)
            {
                return;
            }
            this.leftNode.rightWeight = weight;
            this.leftWeight = weight;
        }

        public void attachRight(SkillTreeNode node, int weight)
        {
            this.rightNode = node;
            setRightWeight(weight);
        }
        public void setRightWeight(int weight)
        {
            if (this.rightNode == null)
            {
                return;
            }
            this.rightNode.leftWeight = weight;
            this.rightWeight = weight;
        }

        public void attachTop(SkillTreeNode node, int weight)
        {
            this.topNode = node;
            setTopWeight(weight);
        }
        public void setTopWeight(int weight)
        {
            if (this.topNode == null)
            {
                return;
            }
            this.topNode.bottomWeight = weight;
            this.topWeight = weight;
        }

        public void attachBottom(SkillTreeNode node, int weight)
        {
            this.bottomNode = node;
            setBottomWeight(weight);
        }
        public void setBottomWeight(int weight)
        {
            if (this.bottomNode == null)
            {
                return;
            }
            this.bottomNode.topWeight = weight;
            this.bottomWeight = weight;
        }
    }
}
