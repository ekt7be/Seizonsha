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


            //player.xp = 100000;
            BlankNode startNode = new BlankNode(this, nodeTextures[Static.SKILL_TREE_NODE_ANY]);
            nodes.Add(startNode);
            currNode = startNode;
                
            //fight path
            SkillTreeNode HpPlus50Node = new SkillTreeNode(this, nodeTextures[Static.HEALTH_INCREASE_NAME], new HealthPlusUnlockable(50), 300);
            nodes.Add(HpPlus50Node);
            SkillTreeNode HPplus100Node = new SkillTreeNode(this, nodeTextures[Static.HEALTH_INCREASE_NAME], new HealthPlusUnlockable(100), 1000);
            nodes.Add(HPplus100Node);
            SkillTreeNode HPplus50Node2 = new SkillTreeNode(this, nodeTextures[Static.HEALTH_INCREASE_NAME], new HealthPlusUnlockable(50), 500);
            nodes.Add(HPplus50Node2);


            SkillTreeNode WeaponPlusNode = new SkillTreeNode(this, nodeTextures[Static.WEAPON_PLUS_NAME], new WeaponPlusUnlockable(), 1200);
            nodes.Add(WeaponPlusNode);
            SkillTreeNode WeaponPlusNode2 = new SkillTreeNode(this, nodeTextures[Static.WEAPON_PLUS_NAME], new WeaponPlusUnlockable(), 1200);
            nodes.Add(WeaponPlusNode2);
            SkillTreeNode WeaponPlusNode3 = new SkillTreeNode(this, nodeTextures[Static.WEAPON_PLUS_NAME], new WeaponPlusUnlockable(), 1200);
            nodes.Add(WeaponPlusNode3);

           // SkillTreeNode AOETauntNode = new SkillTreeNode(this, nodeTextures[Static.BATTLECRY_NAME], new BattleCry(game, player, 0, 250), 2000);
            SkillTreeNode AOETauntNode = new SkillTreeNode(this, nodeTextures[Static.BATTLECRY_NAME], new BattleCry(game, player, 0, 250), 1500);
            nodes.Add(AOETauntNode);

            //SkillTreeNode EnchantTauntNode = new SkillTreeNode(this, nodeTextures[Static.TAUNTINGENCHANT_NAME], new TauntingEnchant(game, player, 250), 2000);
            SkillTreeNode EnchantTauntNode = new SkillTreeNode(this, nodeTextures[Static.TAUNTINGENCHANT_NAME], new TauntingEnchant(game, player, 250), 1000);
            nodes.Add(EnchantTauntNode);
 

            SkillTreeNode BashNode = new SkillTreeNode(this, nodeTextures[Static.BASH_NAME], new Bash(game, player, 0, 20), 700);
            nodes.Add(BashNode);

            SkillTreeNode armor1Node = new SkillTreeNode(this, nodeTextures[Static.ARMOR_INCREASE_NAME], new ArmorPlusUnlockable(), 500);
            nodes.Add(armor1Node);
            SkillTreeNode armor2Node = new SkillTreeNode(this, nodeTextures[Static.ARMOR_INCREASE_NAME], new ArmorPlusUnlockable(), 1000);
            nodes.Add(armor2Node);
            SkillTreeNode armor3Node = new SkillTreeNode(this, nodeTextures[Static.ARMOR_INCREASE_NAME], new ArmorPlusUnlockable(), 1500);
            nodes.Add(armor3Node);

            SkillTreeNode kickNode = new SkillTreeNode(this, nodeTextures[Static.KICK_NAME], new Kick(game, player, 5, 60), 1000);
            nodes.Add(kickNode);
            SkillTreeNode getBigNode = new SkillTreeNode(this, nodeTextures[Static.GET_BIG_NAME], new GetBigUnlockable(), 2000);
            nodes.Add(getBigNode);

            startNode.attachRight(HpPlus50Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH *4);
            HpPlus50Node.attachBottom(WeaponPlusNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH*2);
            HpPlus50Node.attachTop(armor1Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 5);
            armor1Node.attachLeft(HPplus100Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            armor1Node.attachRight(BashNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            HPplus100Node.attachBottom(WeaponPlusNode2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            HPplus100Node.attachTop(kickNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH *2);
            kickNode.attachTop(armor3Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            armor3Node.attachLeft(HPplus50Node2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            armor3Node.attachRight(WeaponPlusNode3, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            HPplus50Node2.attachTop(AOETauntNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            WeaponPlusNode3.attachRight(EnchantTauntNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            EnchantTauntNode.attachTop(armor2Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            WeaponPlusNode3.attachTop(getBigNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            getBigNode.attachRight(armor2Node, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);


            

            //magic path
            SkillTreeNode ManaPlusMagic1 = new SkillTreeNode(this, nodeTextures[Static.MANA_PLUS_NAME], new ManaPlusUnlockable(30), 500); // 500
            nodes.Add(ManaPlusMagic1);
            SkillTreeNode ManaPlusMagic2 = new SkillTreeNode(this, nodeTextures[Static.MANA_PLUS_NAME], new ManaPlusUnlockable(20), 1200);
            nodes.Add(ManaPlusMagic2);
            SkillTreeNode ManaPlusMagic3 = new SkillTreeNode(this, nodeTextures[Static.MANA_PLUS_NAME], new ManaPlusUnlockable(50), 1500);
            nodes.Add(ManaPlusMagic3);


            SkillTreeNode ManaRegenMagic1 = new SkillTreeNode(this, nodeTextures[Static.MANA_REGEN_NAME], new ManaRegenPlusUnlockable(Static.PLAYER_START_MANA_REGEN / 2), 1000);
            nodes.Add(ManaRegenMagic1);
            SkillTreeNode ManaRegenMagic2 = new SkillTreeNode(this, nodeTextures[Static.MANA_REGEN_NAME], new ManaRegenPlusUnlockable(Static.PLAYER_START_MANA_REGEN / 2), 1000);
            nodes.Add(ManaRegenMagic2);

            SkillTreeNode MissileNode = new SkillTreeNode(this, nodeTextures[Static.MAGIC_MISSILE_NAME], new MagicMissile(game, player, 15, 20), 300); 
            nodes.Add(MissileNode);

			SkillTreeNode FirelanceNode = new SkillTreeNode(this, nodeTextures[Static.FIRELANCE_NAME], new FireLance(game,player,Static.FIRELANCE_DAMAGE, 30), 1200); //1500
            nodes.Add(FirelanceNode);
            SkillTreeNode LifeDrainNode = new SkillTreeNode(this, nodeTextures[Static.LIFE_DRAIN_NAME], new LifeDrain(game, player, 5, 6, 4), 1200);
            nodes.Add(LifeDrainNode);
            SkillTreeNode FireballNode = new SkillTreeNode(this, nodeTextures[Static.FIREBALL_NAME], new Fireball(game, player, 40, 40, 10f), 600);
            nodes.Add(FireballNode);
            SkillTreeNode BlizzardNode = new SkillTreeNode(this, nodeTextures[Static.BLIZZARD_NAME], new Blizzard(game, player, 0, 200, 200, 100), 100);
            nodes.Add(BlizzardNode);
            SkillTreeNode LightningArrowNode = new SkillTreeNode(this, nodeTextures[Static.LIGHTNING_ARROW_NAME], new LightningArrow(game,player,0,50), 3000);
            nodes.Add(LightningArrowNode);
            SkillTreeNode TeleportNode = new SkillTreeNode(this, nodeTextures[Static.TELEPORT_NAME], new Teleport(game, player, 40, 40, 20), 2000);
            nodes.Add(TeleportNode);



            SkillTreeNode BurnEnchantNode = new SkillTreeNode(this, nodeTextures[Static.BURNINGENCHANT_NAME], new BurningEnchant(game, player, 2, 100), 1500);
            nodes.Add(BurnEnchantNode);

            SkillTreeNode LightningEnchantNode = new SkillTreeNode(this, nodeTextures[Static.LIGHTNINGENCHANT_NAME], new LightningEnchant(game, player, 7, 100), 1500);
            nodes.Add(LightningEnchantNode);

            startNode.attachBottom(MissileNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            MissileNode.attachBottom(ManaPlusMagic1, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaPlusMagic1.attachRight(FireballNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            FireballNode.attachBottom(ManaPlusMagic2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaPlusMagic1.attachBottom(ManaRegenMagic1, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaRegenMagic1.attachRight(ManaPlusMagic2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            //FirelanceNode.attachRight(FireballNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaRegenMagic1.attachLeft(BlizzardNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            BlizzardNode.attachLeft(TeleportNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            ManaRegenMagic1.attachBottom(LifeDrainNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            //BurnEnchantNode.attachBottom(ManaPlusMagic2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaPlusMagic2.attachRight(BurnEnchantNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            BurnEnchantNode.attachTop(BashNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 13);
            LifeDrainNode.attachBottom(ManaPlusMagic3, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            LifeDrainNode.attachLeft(LightningEnchantNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaPlusMagic3.attachRight(FirelanceNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaPlusMagic3.attachLeft(LightningArrowNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaPlusMagic3.attachBottom(ManaRegenMagic2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);








            //support
            SkillTreeNode ManaRegenSupport1 = new SkillTreeNode(this, nodeTextures[Static.MANA_PLUS_NAME], new ManaRegenPlusUnlockable(Static.PLAYER_START_MANA_REGEN / 3), 500);
            nodes.Add(ManaRegenSupport1);
            SkillTreeNode ManaRegenSupport2 = new SkillTreeNode(this, nodeTextures[Static.MANA_PLUS_NAME], new ManaRegenPlusUnlockable(Static.PLAYER_START_MANA_REGEN / 3), 500);
            nodes.Add(ManaRegenSupport2);

            SkillTreeNode ManaPlusSupport1 = new SkillTreeNode(this, nodeTextures[Static.MANA_REGEN_NAME], new ManaPlusUnlockable(30), 500); // 500
            nodes.Add(ManaPlusSupport1);
            SkillTreeNode ManaPlusSupport2 = new SkillTreeNode(this, nodeTextures[Static.MANA_REGEN_NAME], new ManaPlusUnlockable(20), 1200);
            nodes.Add(ManaPlusSupport2);


            SkillTreeNode DrainEnchantNode = new SkillTreeNode(this, nodeTextures[Static.DRAINENCHANT_NAME], new DrainEnchant(game, player, 100), 1500);
            nodes.Add(DrainEnchantNode);

            SkillTreeNode HealingTouchNode = new SkillTreeNode(this, nodeTextures[Static.HEALING_TOUCH_NAME], new HealingTouch(game,player,50,40), 300);
            nodes.Add(HealingTouchNode);

            SkillTreeNode HealingRainNode = new SkillTreeNode(this, nodeTextures[Static.HEALING_RAIN_NAME], new HealingRain(game,player,15,200, 300), 1500);
            nodes.Add(HealingRainNode);

            SkillTreeNode RegrowthNode = new SkillTreeNode(this, nodeTextures[Static.REGROWTH_NAME], new Regrowth(game,player,30,400), 1000);
            nodes.Add(RegrowthNode);


            startNode.attachLeft(HealingTouchNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            HealingTouchNode.attachTop(ManaRegenSupport1, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            ManaRegenSupport1.attachLeft(ManaPlusSupport1, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaPlusSupport1.attachTop(DrainEnchantNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 5);
            HealingTouchNode.attachLeft(RegrowthNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            RegrowthNode.attachTop(ManaPlusSupport1, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
           // ManaPlusSupport1.attachRight(HealingTouchNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            DrainEnchantNode.attachRight(HPplus50Node2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);
            RegrowthNode.attachLeft(ManaRegenSupport2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaRegenSupport2.attachLeft(HealingRainNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            ManaRegenSupport2.attachBottom(TeleportNode, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 8);
            HealingRainNode.attachLeft(ManaPlusSupport2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 2);
            //ManaRegenSupport2.attachLeft(HealingRain
            //ManaRegenSupport2.attachBottom(
            //DrainEnchantNode.attachRight(HPplus50Node2, Static.SKILL_TREE_WEIGHT_LOCKED, Static.SKILL_TREE_NODE_WIDTH * 4);


        
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


            spriteBatch.Draw(Static.PIXEL_THIN, new Rectangle(0, bounds.Height - 200, 450,200), Color.Black *.8f);

            if (currNode.unlockable!= null)
            {
                //spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, currNode.getEquipable().getDescription(), new Vector2(30, bounds.Height - 150), Color.White);
                spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, currNode.unlockable.getDescription(), new Vector2(30, bounds.Height - 150), Color.White);

            }
            
            if (!currNode.isUnlocked() && currNode.Available(player))
            {
				if (player.keyboard){
					spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, "Press E to Unlock", new Vector2(30, bounds.Height - 100), Color.White);

				} else {
					spriteBatch.DrawString(Static.SPRITEFONT_Calibri12, "Press A to Unlock", new Vector2(30, bounds.Height - 100), Color.White);

				}
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

            return; 

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
            return; 


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
            return; 


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
            return; 


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
