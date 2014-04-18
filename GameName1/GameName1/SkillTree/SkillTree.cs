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


            player.xp = 100000;
            BlankNode startNode = new BlankNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY]);
            nodes.Add(startNode);
            currNode = startNode;
                
            //fight path
            SkillTreeNode HpPlus2Node = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new HealthPlusUnlockable(50), 300);
            nodes.Add(HpPlus2Node);

            SkillTreeNode WeaponPlusNode = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new WeaponPlusUnlockable(), 1200);
            nodes.Add(WeaponPlusNode);
            SkillTreeNode WeaponPlusNode2 = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new WeaponPlusUnlockable(), 2000);
            nodes.Add(WeaponPlusNode2);


            SkillTreeNode HPplus100Node = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new HealthPlusUnlockable(100), 1000);
            nodes.Add(HPplus100Node);

            SkillTreeNode BashNode = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new Bash(game, player, 0, 20), 7000);
            nodes.Add(BashNode);

            SkillTreeNode armor1Node = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new ArmorPlusUnlockable(), 500);
            nodes.Add(armor1Node);

            SkillTreeNode kickNode = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new Kick(game, player, 5, 60), 1000);
            nodes.Add(kickNode);

            startNode.attachRight(HpPlus2Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH *4);
            HpPlus2Node.attachBottom(WeaponPlusNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH*2);
            HpPlus2Node.attachTop(armor1Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 3);
            armor1Node.attachLeft(HPplus100Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            armor1Node.attachRight(BashNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            HPplus100Node.attachBottom(WeaponPlusNode2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            HPplus100Node.attachTop(kickNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH *2);


            /*
            HPplus100Node.attachLeft(armor1Node, Static.SKILL_TREE_WEIGHT_LOCKED);
            */
            

            //magic path
			SkillTreeNode ManaPlus2 = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new ManaPlusUnlockable(30), 500); // 500
            nodes.Add(ManaPlus2);

            SkillTreeNode ManaRegenPlusNode = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new ManaRegenPlusUnlockable(Static.PLAYER_START_MANA_REGEN / 2), 1500);
            nodes.Add(ManaRegenPlusNode);

			SkillTreeNode FirelanceNode = new SkillTreeNode(this, nodeTextures[Static.FIRELANCE_NAME], new FireLance(game,player,Static.FIRELANCE_DAMAGE, 30), 1500); //1500
            nodes.Add(FirelanceNode);

            SkillTreeNode LifeDrainNode = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new LifeDrain(game, player, 5, 6, 4), 5000);
            nodes.Add(LifeDrainNode);


            startNode.attachBottom(ManaPlus2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            ManaPlus2.attachLeft(ManaRegenPlusNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaPlus2.attachRight(FirelanceNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaRegenPlusNode.attachBottom(LifeDrainNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);


            //support
			SkillTreeNode ManaRegen2Node = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new ManaRegenPlusUnlockable(Static.PLAYER_START_MANA_REGEN/3), 500);
            nodes.Add(ManaRegen2Node);

            SkillTreeNode BlizzardNode = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new Blizzard(game, player, 0, 200, 200, 100), 2000);
            nodes.Add(BlizzardNode);

            SkillTreeNode ManaPlusNode = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new ManaPlusUnlockable(50), 2000);
            nodes.Add(ManaPlusNode);

            SkillTreeNode TeleportNode = new SkillTreeNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY], new Teleport(game,player,40,40,20), 2000);
            nodes.Add(TeleportNode);


            startNode.attachLeft(ManaRegen2Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            ManaRegen2Node.attachTop(BlizzardNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaRegen2Node.attachBottom(ManaPlusNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaPlusNode.attachLeft(TeleportNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);

        
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
                    Rectangle highlight = node.getBounds();
                    highlight.Inflate(5, 5);
                    spriteBatch.Draw(Static.PIXEL_THIN, highlight, Color.Yellow);
                    tint = Color.Pink;
                }
                else if (node.isUnlocked())
                {
					tint = Color.Pink;
                }
                else
                {
					tint = Color.Gray;
                }
                node.Draw(spriteBatch, cameraOffset, tint);
           }



            /*
            if (currNode.getEquipable() != null && currNode.isUnlocked())
            {
                spriteBatch.DrawString(game.getSpriteFont(), "Press L1(1), L2(2), R1(3), or R2(4) to Equip", new Vector2(30, bounds.Height - 100), Color.White);
            }
             * */

            spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, "XP Available: "+ player.xp, new Vector2(10, 10), Color.White);


            if (currNode.getEquipable() != null)
            {
                spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, currNode.getEquipable().getDescription(), new Vector2(30, bounds.Height - 150), Color.White);
            }

            if (!currNode.isUnlocked() && currNode.Available(player))
            {
                spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, "Press A(Enter) to Unlock", new Vector2(30, bounds.Height - 100), Color.White);
            }

            if (!currNode.isUnlocked() && !currNode.Available(player))
            {
                spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, "Not enough XP!", new Vector2(30, bounds.Height - 100), Color.White);
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
