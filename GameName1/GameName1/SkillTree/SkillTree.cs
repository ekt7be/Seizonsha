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
		public List<SkillTreeNode> nodes;
        private SkillTreeNode currNode;
        private Player player;
        private Seizonsha game;
        private Texture2D backgroundTexture;
        private Vector2 cameraOffset;
		public static Dictionary<string, Texture2D> nodeTextures = new Dictionary<string, Texture2D>();

        private int movementRecharge;

        public SkillTree(Seizonsha game, Player player, Texture2D backgroundTexture)
        {
            this.player = player;
            this.game = game;
            this.backgroundTexture = backgroundTexture;
            this.nodes = new List<SkillTreeNode>();
            populateNodes();
            currNode.Unlock(player); //unlock first node
            this.movementRecharge = Static.SKILL_TREE_MOVEMENT_RECHARGE;
            cameraOffset = new Vector2(0, 0);
        }

		public List<Skill> allUnlockedSkills() {
			List<Skill> allUnlockedSkills = new List<Skill>(); 

			foreach(SkillTreeNode skn in this.nodes) {
				if (skn.isUnlocked() && (!(skn is BlankNode))) {
					allUnlockedSkills.Add((Skill)skn.unlockable); 
					//unlockedSkills.Add((Skill)sk.unlockable); 
					//System.Console.WriteLine(((Skill)skn.unlockable).getName()); 
				}
			}

			return allUnlockedSkills;
		}


        public void populateNodes()
        {
		


            BlankNode startNode = new BlankNode(this, 0, 0, nodeTextures[Static.SKILL_TREE_NODE_ANY]);
            nodes.Add(startNode);
            currNode = startNode;
                
            //fight path
            SkillTreeNode goodSwordNode = new SkillTreeNode(this, startNode.getX()+Static.SKILL_TREE_NODE_WIDTH*2, startNode.getY(), nodeTextures[Static.SKILL_TREE_NODE_ANY], new Sword(game,player,60,20), 300);
            nodes.Add(goodSwordNode);
            startNode.attachRight(goodSwordNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode HPplusNode = new SkillTreeNode(this, goodSwordNode.getX() + Static.SKILL_TREE_NODE_WIDTH, startNode.getY()- Static.SKILL_TREE_NODE_WIDTH*2, nodeTextures[Static.SKILL_TREE_NODE_ANY], new HealthPlusUnlockable(100), 1000);
            nodes.Add(HPplusNode);
            goodSwordNode.attachTop(HPplusNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode dankSwordNode = new SkillTreeNode(this, HPplusNode.getX() + Static.SKILL_TREE_NODE_WIDTH * 2, HPplusNode.getY(), nodeTextures[Static.SKILL_TREE_NODE_ANY], new Sword(game, player, 300, 10), 7000);
            nodes.Add(dankSwordNode);
            HPplusNode.attachRight(dankSwordNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            

            //magic path
			SkillTreeNode FireballNode = new SkillTreeNode(this, startNode.getX(), startNode.getY() + Static.SKILL_TREE_NODE_HEIGHT*2, nodeTextures["Fireball"], new Fireball(game, player, 50, 20, 100), 0); // 500
            nodes.Add(FireballNode);
            startNode.attachBottom(FireballNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode ManaRegenPlusNode = new SkillTreeNode(this, FireballNode.getX() - Static.SKILL_TREE_NODE_WIDTH * 2, FireballNode.getY() + Static.SKILL_TREE_NODE_WIDTH * 2, nodeTextures[Static.SKILL_TREE_NODE_ANY], new ManaRegenPlusUnlockable(Static.PLAYER_START_MANA_REGEN / 2), 1500);
            nodes.Add(ManaRegenPlusNode);
            FireballNode.attachLeft(ManaRegenPlusNode, Static.SKILL_TREE_WEIGHT_LOCKED);

			SkillTreeNode FirelanceNode = new SkillTreeNode(this, FireballNode.getX() + Static.SKILL_TREE_NODE_WIDTH * 2, FireballNode.getY() + Static.SKILL_TREE_NODE_WIDTH * 2, nodeTextures["FireLance"], new FireLance(game,player,Static.FIRELANCE_DAMAGE, 30), 0); //1500
            nodes.Add(FirelanceNode);
            FireballNode.attachRight(FirelanceNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode LifeDrainNode = new SkillTreeNode(this, ManaRegenPlusNode.getX() + Static.SKILL_TREE_NODE_WIDTH, ManaRegenPlusNode.getY() + Static.SKILL_TREE_NODE_WIDTH * 2, nodeTextures[Static.SKILL_TREE_NODE_ANY], new LifeDrain(game, player, 5, 6, 4), 5000);
            nodes.Add(LifeDrainNode);
            ManaRegenPlusNode.attachBottom(LifeDrainNode, Static.SKILL_TREE_WEIGHT_LOCKED);


            //support
			SkillTreeNode HealNode = new SkillTreeNode(this, startNode.getX()-Static.SKILL_TREE_NODE_WIDTH * 2, startNode.getY(), nodeTextures["Healing Touch"], new HealingTouch(game, player, 100, 12), 500);
            nodes.Add(HealNode);
            startNode.attachLeft(HealNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode BlizzardNode = new SkillTreeNode(this, HealNode.getX() - Static.SKILL_TREE_NODE_WIDTH, HealNode.getY() - Static.SKILL_TREE_NODE_WIDTH*2, nodeTextures[Static.SKILL_TREE_NODE_ANY], new Blizzard(game, player, 0, 200, 200, 100), 2000);
            nodes.Add(BlizzardNode);
            HealNode.attachTop(BlizzardNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode ManaPlusNode = new SkillTreeNode(this, HealNode.getX() - Static.SKILL_TREE_NODE_WIDTH, HealNode.getY() + Static.SKILL_TREE_NODE_WIDTH*2, nodeTextures[Static.SKILL_TREE_NODE_ANY], new ManaPlusUnlockable(50), 2000);
            nodes.Add(ManaPlusNode);
            HealNode.attachBottom(ManaPlusNode, Static.SKILL_TREE_WEIGHT_LOCKED);

            SkillTreeNode TeleportNode = new SkillTreeNode(this, ManaPlusNode.getX() - Static.SKILL_TREE_NODE_WIDTH*2, ManaPlusNode.getY(), nodeTextures[Static.SKILL_TREE_NODE_ANY], new Teleport(game,player,40,40,20), 2000);
            nodes.Add(TeleportNode);
            ManaPlusNode.attachLeft(TeleportNode, Static.SKILL_TREE_WEIGHT_LOCKED);



        
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
			spriteBatch.Draw(backgroundTexture, screenRectangle, new Color(new Vector4(0, 0, 0, 0.7f)));


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
					tint = Color.Pink;

                }
                else
                {
					tint = Color.LightGray;
                }

                node.Draw(spriteBatch, cameraOffset, tint);


            }

            if (currNode.getEquipable()!= null)
            {
                spriteBatch.DrawString(game.getSpriteFont(),currNode.getEquipable().getDescription(), new Vector2(30, bounds.Height - 300), Color.White);
            }


            if (currNode.getEquipable() != null && currNode.isUnlocked())
            {
                spriteBatch.DrawString(game.getSpriteFont(), "Press L1(1), L2(2), R1(3), or R2(4) to Equip", new Vector2(30, bounds.Height - 100), Color.White);
            }

            if (!currNode.isUnlocked() && currNode.Available(player))
            {
                spriteBatch.DrawString(game.getSpriteFont(), "Press A(Enter) to Unlock", new Vector2(30, bounds.Height - 100), Color.White);
            }

            if (!currNode.isUnlocked() && !currNode.Available(player))
            {
                spriteBatch.DrawString(game.getSpriteFont(), "Not enough XP", new Vector2(30, bounds.Height - 100), Color.White);
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


        public void L1()
        {
            if (currNode == null || !moveAvailable() || !currNode.isUnlocked())
            {
                return;
            }

            if (currNode.getEquipable() != null)
            {
                player.Equip(currNode.getEquipable(),Static.PLAYER_L1_SKILL_INDEX);
            }

            movementRecharge = 0;
        }

        public void L2()
        {
            if (currNode == null || !moveAvailable() || !currNode.isUnlocked())
            {
                return;
            }

            if (currNode.getEquipable() != null)
            {
                player.Equip(currNode.getEquipable(), Static.PLAYER_L2_SKILL_INDEX);
            }

            movementRecharge = 0;
        }

        public void R1()
        {
            if (currNode == null || !moveAvailable() || !currNode.isUnlocked())
            {
                return;
            }

            if (currNode.getEquipable() != null)
            {
                player.Equip(currNode.getEquipable(), Static.PLAYER_R1_SKILL_INDEX);
            }

            movementRecharge = 0;
        }

        public void R2()
        {
            if (currNode == null || !moveAvailable() || !currNode.isUnlocked())
            {
                return;
            }

            if (currNode.getEquipable() != null)
            {
                player.Equip(currNode.getEquipable(), Static.PLAYER_R2_SKILL_INDEX);
            }

            movementRecharge = 0;
        }

        public void Unlock()
        {
            currNode.Unlock(player);
        }

     }

    
}
