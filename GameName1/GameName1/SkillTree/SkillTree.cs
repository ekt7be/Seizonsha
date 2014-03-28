﻿using GameName1.Interfaces;
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
        private Vector2 cameraOffset;
        public static Dictionary<int, Texture2D> nodeTextures = new Dictionary<int, Texture2D>();


        private int movementRecharge;

        public SkillTree(Seizonsha game, Player player, Texture2D backgroundTexture)
        {
            this.player = player;
            this.backgroundTexture = backgroundTexture;
            this.nodes = new List<SkillTreeNode>();
            populateNodes();
            this.movementRecharge = Static.SKILL_TREE_MOVEMENT_RECHARGE;
            cameraOffset = new Vector2(0, 0);
        }

        public void populateNodes()
        {


            BlankNode startNode = new BlankNode(this, 0, 0, nodeTextures[Static.SKILL_TREE_NODE_ANY]);
            nodes.Add(startNode);
            currNode = startNode;
                
            SkillTreeNode cColorNode = new SkillTreeNode(this, startNode.getX()+Static.SKILL_TREE_NODE_WIDTH*2, startNode.getY(), nodeTextures[Static.SKILL_TREE_NODE_ANY], new ChangeColor(game, player, Color.Red));
            nodes.Add(cColorNode);
            startNode.attachRight(cColorNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode FireballNode = new SkillTreeNode(this, startNode.getX(), startNode.getY() + Static.SKILL_TREE_NODE_HEIGHT*2, nodeTextures[Static.SKILL_TREE_NODE_ANY], new Fireball(game, player, 300, 20, 12));
            nodes.Add(FireballNode);
            startNode.attachBottom(FireballNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode HealNode = new SkillTreeNode(this, startNode.getX()-Static.SKILL_TREE_NODE_WIDTH * 2, startNode.getY(), nodeTextures[Static.SKILL_TREE_NODE_ANY], new HealingTouch(game, player, 300, 12));
            nodes.Add(HealNode);
            startNode.attachLeft(HealNode, Static.SKILL_TREE_WEIGHT_LOCKED);


            
        
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
			Rectangle screenRectangle = new Rectangle(0,0,bounds.Width, bounds.Height);
			spriteBatch.Draw(backgroundTexture, screenRectangle, Color.Black);


            cameraOffset = new Vector2(currNode.getCenterX() - bounds.Width/2, currNode.getCenterY()- bounds.Height/2);
            foreach (SkillTreeNode node in nodes)
            {


                if (node.leftNode != null)
                {
                    Color lineColor = Color.White;
                    if (node.leftWeight != Static.SKILL_TREE_WEIGHT_UNLOCKED)
                    {
                        lineColor = Color.Red;
                    }
                    Static.DrawLine(spriteBatch, Static.PIXEL_THICK, new Vector2(node.getCenterX()-cameraOffset.X, node.getCenterY()-cameraOffset.Y), new Vector2(node.leftNode.getCenterX() - cameraOffset.X, node.leftNode.getCenterY() - cameraOffset.Y), lineColor);
                }


                if (node.topNode != null)
                {
                    Color lineColor = Color.White;
                    if (node.topWeight != Static.SKILL_TREE_WEIGHT_UNLOCKED)
                    {
                        lineColor = Color.Red;
                    }
                    Static.DrawLine(spriteBatch, Static.PIXEL_THICK, new Vector2(node.getCenterX() - cameraOffset.X, node.getCenterY() - cameraOffset.Y), new Vector2(node.topNode.getCenterX() - cameraOffset.X, node.topNode.getCenterY() - cameraOffset.Y), lineColor);
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

                node.Draw(spriteBatch, cameraOffset, tint);




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
