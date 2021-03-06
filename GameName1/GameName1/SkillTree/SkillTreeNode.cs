﻿using GameName1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1.SkillTree
{
	public class SkillTreeNode
    {

		public SkillTree skilltree; 
        public SkillTreeNode leftNode;
        public int leftWeight;
        public SkillTreeNode rightNode;
        public int rightWeight;
        public SkillTreeNode bottomNode;
        public int bottomWeight;
        public SkillTreeNode topNode;
        public int topWeight;
        private Rectangle bounds;
		public Unlockable unlockable;	// the skill
        private Texture2D sprite;
        private int x; //offset from origin
        private int y;
        private int cost;


        private bool unlocked;


        public SkillTreeNode(SkillTree skilltree, Texture2D sprite, Unlockable unlockable, int cost)
        {
            this.unlockable = unlockable;
            this.unlocked = false;
            this.x = 0;
            this.y = 0;
            this.sprite = sprite;
            this.skilltree =skilltree;
            this.cost = cost;
        }

        public Equipable getEquipable()
        {
            if (unlockable is Equipable)
            {
                return (Equipable)unlockable;
            }
            else
            {
                return null;
            }
        }

        public void Unlock(Player player)
        {
            if (!Available(player)){
                return;
            }

            if (isUnlocked())
            {
                return;
            }

            player.incXP(-cost);

            setLeftWeight(Static.SKILL_TREE_WEIGHT_UNLOCKED);
            setRightWeight(Static.SKILL_TREE_WEIGHT_UNLOCKED);
            setBottomWeight(Static.SKILL_TREE_WEIGHT_UNLOCKED);
            setTopWeight(Static.SKILL_TREE_WEIGHT_UNLOCKED);

            this.unlocked = true;

            if (unlockable == null)
            {
                return;
            }
            unlockable.OnUnlock(player);

        }

        public bool Available(Player player)
        {
            if (player.xp >= cost)
            {
                return true;
            }
            return false;
        }

        public bool isUnlocked()
        {
            return unlocked;
        }

        public int getCenterX()
        {
            return  x+Static.SKILL_TREE_NODE_WIDTH / 2;
        }

        public int getCenterY()
        {
            return y + Static.SKILL_TREE_NODE_HEIGHT/2;
        }

        public int getX()
        {
            return x;

        }

        public int getY()
        {
            return y;
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 cameraOffset, Color color)
        {
            bounds = new Rectangle(x - (int)cameraOffset.X, y - (int)cameraOffset.Y, Static.SKILL_TREE_NODE_WIDTH, Static.SKILL_TREE_NODE_HEIGHT);
            spriteBatch.Draw(sprite, bounds, color);
            if (unlockable == null)
            {
                return;
            }
            if (unlocked)
            {
                Static.DrawBorderedText(spriteBatch, Static.SPRITEFONT_Calibri12, unlockable.getName(), bounds.Left, bounds.Top, Color.Black, Color.Ivory);
                //spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, unlockable.getName(), new Vector2(bounds.Left, bounds.Top), Color.Green);
            }
            else
            {
                Static.DrawBorderedText(spriteBatch, Static.SPRITEFONT_Calibri12, unlockable.getName() + "\n" + cost + " XP", bounds.Left, bounds.Top, Color.Black, Color.Ivory);
                
                //spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, unlockable.getName() + "\n" + cost + " XP", new Vector2(bounds.Left, bounds.Top), Color.Green);
            }

        }

        public void attachLeft(SkillTreeNode node, int weight, int distance)
        {
            this.leftNode = node;
            node.rightNode = this;
            node.x = this.x - distance;
            node.y = this.y;
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

        public void attachRight(SkillTreeNode node, int weight, int distance)
        {
            this.rightNode = node;
            node.leftNode = this;
            node.x = this.x + distance;
            node.y = this.y;
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

        public void attachTop(SkillTreeNode node, int weight, int distance)
        {
            this.topNode = node;
            node.x = this.x;
            node.y = this.y - distance;
            node.bottomNode = this;
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

        public void attachBottom(SkillTreeNode node, int weight, int distance)
        {
            this.bottomNode = node;
            node.topNode = this;
            node.x = this.x;
            node.y = this.y + distance;
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

        public Rectangle getBounds()
        {
            return bounds;
        }
    }
}
