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
        private Seizonsha game;
        private Texture2D backgroundTexture;
        public static Dictionary<int, Texture2D> nodeTextures = new Dictionary<int, Texture2D>();


        private int movementRecharge;

        public SkillTree(Seizonsha game, Player player, Texture2D backgroundTexture)
        {
            this.player = player;
            this.backgroundTexture = backgroundTexture;
            this.nodes = new List<SkillTreeNode>();
            populateNodes();
            this.movementRecharge = Static.SKILL_TREE_MOVEMENT_RECHARGE;
        }

        public void populateNodes()
        {


            BlankNode startNode = new BlankNode(this, Static.SKILL_TREE_CENTER_OFFSET_X, Static.SKILL_TREE_CENTER_OFFSET_Y, nodeTextures[Static.SKILL_TREE_NODE_ANY]);
            nodes.Add(startNode);
            currNode = startNode;
                
            SkillTreeNode cColorNode = new SkillTreeNode(this, 0, 0, nodeTextures[Static.SKILL_TREE_NODE_ANY], new ChangeColor(game, player, Color.Red));
            nodes.Add(cColorNode);
            cColorNode.attachLeft(startNode, Static.SKILL_TREE_WEIGHT_UNLOCKED);

            SkillTreeNode FireballNode = new SkillTreeNode(this, 100, 200, nodeTextures[Static.SKILL_TREE_NODE_ANY], new Fireball(game, player, 300, 20, 12));
            nodes.Add(FireballNode);
            FireballNode.attachLeft(cColorNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode HealNode = new SkillTreeNode(this, 500, 600, nodeTextures[Static.SKILL_TREE_NODE_ANY], new HealingTouch(game, player, 300, 12));
            nodes.Add(HealNode);
            HealNode.attachTop(FireballNode, Static.SKILL_TREE_WEIGHT_LOCKED);


            
        
        }

        public void Update()
        {
            if (movementRecharge < Static.SKILL_TREE_MOVEMENT_RECHARGE)
            {
                movementRecharge++;
            }
        }

		public void Draw(Rectangle bounds, SpriteBatch spriteBatch)
        {
            //draw background
			Rectangle screenRectangle = bounds;
			spriteBatch.Draw(backgroundTexture, screenRectangle, Color.Black);
            foreach (SkillTreeNode node in nodes)
            {


                if (node.leftNode != null)
                {
                    Color lineColor = Color.White;
                    if (node.leftWeight != Static.SKILL_TREE_WEIGHT_UNLOCKED)
                    {
                        lineColor = Color.Red;
                    }
                    Static.DrawLine(spriteBatch, Static.PIXEL_THICK, new Vector2(node.getCenterX(bounds), node.getCenterY(bounds)), new Vector2(node.leftNode.getCenterX(bounds), node.leftNode.getCenterY(bounds)), lineColor);
                }


                if (node.topNode != null)
                {
                    Color lineColor = Color.White;
                    if (node.topWeight != Static.SKILL_TREE_WEIGHT_UNLOCKED)
                    {
                        lineColor = Color.Red;
                    }
                    Static.DrawLine(spriteBatch, Static.PIXEL_THICK, new Vector2(node.getCenterX(bounds), node.getCenterY(bounds)), new Vector2(node.topNode.getCenterX(bounds), node.topNode.getCenterY(bounds)), lineColor);
                }

            }

            foreach (SkillTreeNode node in nodes)
            {


                Color tint = Color.White;
                if (currNode == node)
                {
                    tint = Color.Purple;
                }
                else if (node.isUnlocked())
                {
                    tint = Color.White;
                }

                node.Draw(spriteBatch, bounds, tint);




            }

        }

        private bool moveAvailable()
        {
            return movementRecharge >= Static.SKILL_TREE_MOVEMENT_RECHARGE;
        }

        public void Left()
        {
            if (currNode == null || !moveAvailable())
            {
                return;
            }

            if (currNode.leftNode != null && currNode.leftWeight == Static.SKILL_TREE_WEIGHT_UNLOCKED)
            {
                currNode = currNode.leftNode;
            }
            movementRecharge = 0;

        }

        public void Right()
        {
            if (currNode == null || !moveAvailable())
            {
                return;
            }

            if (currNode.rightNode != null && currNode.rightWeight == Static.SKILL_TREE_WEIGHT_UNLOCKED)
            {
                currNode = currNode.rightNode;
            }

            movementRecharge = 0;
        }


        public void Up()
        {
            if (currNode == null || !moveAvailable())
            {
                return;
            }

            if (currNode.topNode != null && currNode.topWeight == Static.SKILL_TREE_WEIGHT_UNLOCKED)
            {
                currNode = currNode.topNode;
            }
            movementRecharge = 0;

        }

        public void Down()
        {
            if (currNode == null || !moveAvailable())
            {
                return;
            }

            if (currNode.bottomNode != null && currNode.bottomWeight == Static.SKILL_TREE_WEIGHT_UNLOCKED)
            {
                currNode = currNode.bottomNode;
            }
            movementRecharge = 0;

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
