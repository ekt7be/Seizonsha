﻿using GameName1.Effects;
using GameName1.Interfaces;
using GameName1.PickUps;
using GameName1.Skills;
using GameName1.Skills.Weapons;
using GameName1.SkillTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public class Player : GameEntity
    {
        #region Fields
        public int cameraX { get; set; }
        public int cameraY { get; set; }
        public int xp { get; set; }
        public int skillPoints { get; set; }
        public PlayerIndex playerIndex { get; set; }
        private bool dead;
        private Equipable[] skillSlots;
		private List<Equipable> inventory;	// list of all usable weapons, skills, and items 
        private List<Equipable> unEquippedSkills;
		public SkillTree.SkillTree skilltree;
        public float manaRegen;
        public float mana;
        public float maxMana;
        public List<Skill> onHitEffects;

        private int armorLevel;
        private Color armorColor;

        public int weaponLevel;
        public Weapon currentWeapon;

        public bool playerReady;

        public bool keyboard = false;
        public KeyboardState oldKeyboardState;
        public GamePadState oldGamepadState;

        private bool skilltreebuttondown;
        private bool skilltreescreen;

        private Interactable currentInteractable;

		List<Skill> unlockedSkills = new List<Skill>(); 


        private static float elapsed;
        private static float swordElapsed;
        private static readonly float delay = 200f;
        private static int walkFrame = 0;

        private static readonly int UP_ANIMATION = 8;
        private static readonly int DOWN_ANIMATION = 10;
        private static readonly int LEFT_ANIMATION = 9;
        private static readonly int RIGHT_ANIMATION = 11;
        private static readonly int WALK_ANIMATION_FRAMES = 9;

		public int skillbarIndex = 0;
		public int skillbarIndex2 = 0; 
		public Rectangle highlightRect; 
		bool selectingSkill, selectingSkill2; 
		public Rectangle viewportBounds;

        public bool drawWeapon = true;

		public Camera camera;
        #endregion

        public override void Update(GameTime gameTime)
        {

            foreach (Equipable skill in skillSlots){
                if (skill != null)
                {
                    skill.Update();
                }
            }

            if (SkillTreeOpen())
            {
                skilltree.Update();
            }
            this.mana += manaRegen;
            if (this.mana > this.maxMana) this.mana = maxMana;
            base.Update(gameTime);
            //base.source = new Rectangle(sprite.Width / 4 * currentAnimationFrame, 0, sprite.Width / 4, sprite.Height);

            Interactable foundInteraction = findInteraction();

            if (foundInteraction!=null){
                if (foundInteraction.Available(this))
                {
                    this.currentInteractable = foundInteraction;
                }
            } else {
                this.currentInteractable = null;
            }

         
        }



        public bool hasEnoughMana(int manaCost)
        {
            if ((int)this.mana >= manaCost) return true;
            else return false;
        }

        public void costMana(int manaCost)
        {
            this.mana -= (float)manaCost;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            /*
            //draw hitbox
            Rectangle rect = new Rectangle(this.x,this.y,this.hitbox.Width,this.hitbox.Height);
            spriteBatch.Draw(Static.PIXEL_THIN, rect, Color.Blue);
            */
            
            base.Draw(spriteBatch);
            foreach(Equipable equip in this.skillSlots)
            {
                if (equip is Skill)
                {
                    Skill skill = (Skill)equip;
                    if (skill.percentCasted() < .99 && skill.percentCasted() > .01)
                    {
                        Static.DrawBorderedText(spriteBatch, game.getSpriteFont(), skill.getName(), this.getCenterX()-50, this.hitbox.Y + this.hitbox.Height, Color.Black, Color.White);
                        //spriteBatch.DrawString(game.getSpriteFont(), skill.getName(), new Vector2(this.getCenterX() - 50, this.hitbox.Y + this.hitbox.Height), Color.White);
                        Rectangle cBounds = new Rectangle(this.getCenterX() - 30, this.hitbox.Y + this.hitbox.Height + 30, (int)(skill.percentCasted()*60), 5);
                        Rectangle tBounds = new Rectangle(this.getCenterX() - 30, this.hitbox.Y + this.hitbox.Height + 30, 60, 5);
                        
                        spriteBatch.Draw(Static.PIXEL_THIN, tBounds, Color.White);
                        spriteBatch.Draw(Static.PIXEL_THIN, cBounds, Color.Blue);
                    }
                }
            }


            //draw armor and weapons equipped etc


            Color equipColor = tint;
            if (tint == Color.White)
            {
                equipColor = armorColor;
            }

            if (armorLevel == 1)
            {
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_FEET], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_PANTS], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_GLOVES], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_ARMS_SHOULDER], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_TORSO], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_REG_ARMOR_HEAD], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
            }
            else if (armorLevel == 2)
            {
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_PLATE_ARMOR_FEET], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_PLATE_ARMOR_PANTS], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_PLATE_ARMOR_GLOVES], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_PLATE_ARMOR_ARMS_SHOULDER], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_PLATE_ARMOR_TORSO], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_PLATE_ARMOR_HEAD], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
            }
            else if (armorLevel == 3)
            {
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_FEET], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_PANTS], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_GLOVES], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_ARMS_SHOULDER], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_TORSO], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
                spriteBatch.Draw(Seizonsha.spriteMappings[Static.SPRITE_GOLD_ARMOR_HEAD], this.spriteBox, base.spriteSource, equipColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, depth);
            }


            /*
            //draw cursor
            int dist = 50;
            int width = 30;
            int height = 30;
            Rectangle rBounds = new Rectangle((int)((float)this.getCenterX() + dist * this.vectorDirection.X - width / 2), (int)((float)this.getCenterY() + dist * this.vectorDirection.Y - height / 2), width, height);

            spriteBatch.Draw(game.getSpriteTexture(Static.SPRITE_RETICLE), rBounds, Color.Red);
             * */

            foreach (Skill skill in skillSlots)
            {
                if (skill != null && drawWeapon)
                {
                    skill.Draw(spriteBatch);
                }
            }
        }

        public override void OnSpawn()
        {
        }

        public override void collide(GameEntity entity)
        {
        }

        public override void collideWithWall()
        {
        }

		public Player(Seizonsha game, PlayerIndex playerIndex, Texture2D sprite, Camera camera) : base(game, sprite, Static.PLAYER_WIDTH, Static.PLAYER_HEIGHT, Static.TARGET_TYPE_GOOD, Static.PLAYER_MAX_HEALTH)
        {
            this.cameraX = 0;
            this.cameraY = 0;
            this.skillPoints = 0;
			this.xp = 0;
            this.health = Static.PLAYER_MAX_HEALTH;
            this.dead = false;
            this.playerIndex = playerIndex;
            this.armorLevel = 1;
            this.armorColor = Static.PLAYER_ARMOR_COLOR_0;
            this.onHitEffects = new List<Skill>();


            this.weaponLevel = 1;

			this.skillSlots = new Equipable[4]; //each slot is different skill, weapon, or item
            this.inventory = new List<Equipable>();
            this.unEquippedSkills = new List<Equipable>();
            this.unEquippedSkills.Add(null);
            this.skilltree = new SkillTree.SkillTree(game, this, Static.PIXEL_THIN);




            /*
            Gun gun = new Gun(game, this, 10, 30,0, 15f);
            Equip(gun, Static.PLAYER_L1_SKILL_INDEX);
            addEquipable(gun);
             * */


           // addEquipable(new Blizzard(game, this, 0, 200, 200, 100));

            /*
            Kick kick = new Kick(game, this, 5, 60);
            addEquipable(kick);
            Equip(kick, Static.PLAYER_R1_SKILL_INDEX);



            Fireball fireball = new Fireball(game, this, 40, 30, 10);
            addEquipable(fireball);
            Equip(fireball, Static.PLAYER_R2_SKILL_INDEX);
             * */

            /*
            Blizzard blizzard = new Blizzard(game, this, 0, 200, 200, 100);
            addEquipable(blizzard);
            Equip(blizzard, Static.PLAYER_R2_SKILL_INDEX);
             * */

            
            /*
            LightningArrow la = new LightningArrow(game, this, 20, 120);
            addEquipable(la);
            Equip(la, Static.PLAYER_L1_SKILL_INDEX);

            HealingRain healingrain = new HealingRain(game, this, 15, 60, 5*60);
            addEquipable(healingrain);
            Equip(healingrain, Static.PLAYER_R1_SKILL_INDEX);

            MagicMissile mm = new MagicMissile(game, this, 10, 5 * 60);
            addEquipable(mm);
            Equip(mm, Static.PLAYER_R1_SKILL_INDEX);

            LightningEnchant lightningenchant = new LightningEnchant(game, this, 5, 0);
            addEquipable(lightningenchant);
            Equip(lightningenchant, Static.PLAYER_R2_SKILL_INDEX);
            */

            /*
            HealingTouch healingtouch = new HealingTouch(game, this, 50, 60);
            addEquipable(healingtouch);
            Equip(healingtouch, Static.PLAYER_L2_SKILL_INDEX);
             * */
            /*
            Regrowth regrowth = new Regrowth(game, this, 30, 120);
            addEquipable(regrowth);
            Equip(regrowth, Static.PLAYER_L2_SKILL_INDEX);
            */
         

            Sword sword = new RustySword(game, this);
            addEquipable(sword);
            Equip(sword, Static.PLAYER_L1_SKILL_INDEX);
            this.currentWeapon = sword;



            this.maxMana = Static.PLAYER_MAX_MANA;
            this.mana = maxMana;

            this.manaRegen = Static.PLAYER_START_MANA_REGEN;
            this.skilltreescreen = false;
            this.skilltreebuttondown = false;
            this.currentInteractable = null;

			this.camera = camera;

            base.scale = Static.PLAYER_SPRITE_SCALE;

            this.rotateToAngle(0f); //sets correct animation
           //incArmor(); //incArmor();
   }

        public void incArmor()
        {
            if (armorLevel == 3)
            {
                return;
            }
            armorLevel++;
            defaultShield();

        }

        public void incWeaponSkill()
        {
            if (weaponLevel == 3)
            {
                return;
            }
            weaponLevel++;
        }

        private void defaultShield()
        {

            if (armorLevel == 1)
            {
                shield = Static.PLAYER_SHIELD_1;
               // armorColor = Static.PLAYER_ARMOR_COLOR_1;


            } else if (armorLevel == 2){
                shield = Static.PLAYER_SHIELD_2;
               // armorColor = Static.PLAYER_ARMOR_COLOR_2;

            }
            else if (armorLevel == 3)
            {
                shield = Static.PLAYER_SHIELD_3;
               // armorColor = Static.PLAYER_ARMOR_COLOR_3;
            }
        }

        public Interactable findInteraction()
        {
            if (Math.Cos(this.direction) > .5) // right
            {
                for (int i = game.getTileIndexFromTopEdgeY(getTopEdgeY()); i <= game.getTileIndexFromBottomEdgeY(getBottomEdgeY()); i++){
                    for (int j = 0; j <= Static.PLAYER_INTERACTION_RANGE; j++)
                    {
                        Tile currTile = game.getTileFromIndex(game.getTileIndexFromRightEdgeX(getRightEdgeX()) + j, i);

                        if (currTile == null)
                        {
                            continue;
                        }

                        if (currTile is Interactable)
                        {
                            return (Interactable)currTile;
                        }
                        else
                        {
                            foreach (GameEntity entity in currTile.getEntities())
                            {
                                if (entity is Interactable)
                                {
                                    return (Interactable)entity;
                                }
                            }
                        }
                    }
                }
            }
            else if (Math.Sin(this.direction) > .5) // down
            {
                for (int i = game.getTileIndexFromLeftEdgeX(getLeftEdgeX()); i <= game.getTileIndexFromRightEdgeX(getRightEdgeX()); i++)
                {
                    for (int j = 0; j <= Static.PLAYER_INTERACTION_RANGE; j++)
                    {
                        Tile currTile = game.getTileFromIndex(i,game.getTileIndexFromBottomEdgeY(getBottomEdgeY())+j);

                        if (currTile == null)
                        {
                            continue;
                        }

                        if (currTile is Interactable)
                        {
                            return (Interactable)currTile;
                        }
                        else
                        {
                            foreach (GameEntity entity in currTile.getEntities())
                            {
                                if (entity is Interactable)
                                {
                                    return (Interactable)entity;
                                }
                            }
                        }
                    }
                }

            }
            else if (Math.Sin(direction) < -.5) //up
            {
                for (int i = game.getTileIndexFromLeftEdgeX(getLeftEdgeX()); i <= game.getTileIndexFromRightEdgeX(getRightEdgeX()); i++)
                {
                    for (int j = 0; j <= Static.PLAYER_INTERACTION_RANGE; j++)
                    {
                        Tile currTile = game.getTileFromIndex(i, game.getTileIndexFromTopEdgeY(getTopEdgeY())-j);

                        if (currTile == null)
                        {
                            continue;
                        }
                        if (currTile is Interactable)
                        {
                            return (Interactable)currTile;
                        }
                        else
                        {
                            foreach (GameEntity entity in currTile.getEntities())
                            {
                                if (entity is Interactable)
                                {
                                    return (Interactable)entity;
                                }
                            }
                        }
                    }
                }

            }
            else if (Math.Cos(direction) < -.5) //left
            {
                for (int i = game.getTileIndexFromTopEdgeY(getTopEdgeY()); i <= game.getTileIndexFromBottomEdgeY(getBottomEdgeY()); i++)
                {
                    for (int j = 0; j <= Static.PLAYER_INTERACTION_RANGE; j++)
                    {
                        Tile currTile = game.getTileFromIndex(game.getTileIndexFromLeftEdgeX(getLeftEdgeX()) - j, i);

                        if (currTile == null)
                        {
                            continue;
                        }

                        if (currTile is Interactable)
                        {
                            return (Interactable)currTile;
                        }
                        else
                        {
                            foreach (GameEntity entity in currTile.getEntities())
                            {
                                if (entity is Interactable)
                                {
                                    return (Interactable)entity;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public void DrawScreen(Rectangle screenPortion, SpriteBatch spriteBatch)
        {
            //we will render the entire game world and send it to each player who will use their camera coordinates
            //and the dimensions of their portion of the screen to draw their screen.
            //Interface will be drawn on top along with any menus including skill tree

            this.viewportBounds = screenPortion;

            if (!dead)
            {
                //draw cursor
                int dist = 50;
                int width = 30;
                int height = 30;
                Rectangle rBounds = new Rectangle((int)camera.getScreenPositionX(((float)this.getCenterX() + dist * this.vectorDirection.X - width / 2)), (int)camera.getScreenPositionY(((float)this.getCenterY() + dist * this.vectorDirection.Y - height / 2)), width, height);

                spriteBatch.Draw(game.getSpriteTexture(Static.SPRITE_RETICLE), rBounds, Color.Red);


                //interaction message
                if (currentInteractable != null)
                {
                    if (currentInteractable is GameEntity)
                    {
                        Static.DrawBorderedText(spriteBatch, game.getSpriteFont(),currentInteractable.Message(this), screenPortion.Width / 2, screenPortion.Height / 2, Color.Black, Color.White);
                        //spriteBatch.DrawString(game.getSpriteFont(), currentInteractable.Message(this), new Vector2(screenPortion.Width / 2, screenPortion.Height / 2), Color.White);
                    }
                }
            }

            if (SkillTreeOpen())
            {
                DrawSkillTree(screenPortion, spriteBatch);
                return;
            }

            //draw Wave number
            if (game.Wave != 0)
            {
                if (game.waveCleared)
                {
                    Static.DrawBorderedText(spriteBatch, game.getSpriteFont(), "Wave " + game.Wave + " Complete", 20, screenPortion.Height - 100, Color.Black, Color.White);

                }
                else
                {
                    Static.DrawBorderedText(spriteBatch, game.getSpriteFont(), "Wave: " + game.Wave + " (" + (game.totalEnemies - game.numberEnemies) + " / " + game.totalEnemies + ")", 20, screenPortion.Height - 100, Color.Black, Color.White);

                }
            }
            //spriteBatch.DrawString(game.getSpriteFont(), "WAVE: " + game.Wave, new Vector2(20, screenPortion.Height - 100), Color.White);
      
            Texture2D texture = Static.PIXEL_THIN;

            int barLength = screenPortion.Width / 3;
            int barHeight = screenPortion.Height / 32;

            int hpHeight = screenPortion.Height - barHeight * 3;
            int manaHeight = screenPortion.Height - barHeight * 2;
            int xpHeight = screenPortion.Height - barHeight;
            int offsetFromLeft = 20;

            double green = ((double)this.health / (double)this.maxHealth) * barLength;
            double blue = ((double)this.mana / (double)this.maxMana) * barLength;
            Rectangle hpMax = new Rectangle(offsetFromLeft, hpHeight, barLength, barHeight);
            Rectangle hpRemaining = new Rectangle(offsetFromLeft, hpHeight, (int)green, barHeight);
            Rectangle manaMax = new Rectangle(offsetFromLeft, manaHeight, barLength, barHeight);
            Rectangle manaRemaining = new Rectangle(offsetFromLeft, manaHeight, (int)blue, barHeight);
            Rectangle xp = new Rectangle(offsetFromLeft, xpHeight, barLength, barHeight);

            // draw HP bar
            spriteBatch.Draw(texture, hpMax, Color.Red);
            spriteBatch.Draw(texture, hpRemaining, Color.Green);
            // draw Mana bar
            spriteBatch.Draw(texture, manaMax, Color.LightBlue);
            spriteBatch.Draw(texture, manaRemaining, Color.Blue);
            // draw XP bar
            spriteBatch.Draw(texture, xp, Color.Yellow);

            // draw HP text
            Static.DrawBorderedText(spriteBatch, Static.SPRITEFONT_Calibri14, "HP : " + this.health + "/" + this.maxHealth,
                offsetFromLeft, hpHeight, Color.Black, Color.White);
          /*  spriteBatch.DrawString(
                Static.SPRITEFONT_Calibri14,
                "HP : " + this.health + "/" + this.maxHealth,
                new Vector2(offsetFromLeft, hpHeight),
                Color.White
            );*/

            // draw Mana text
            Static.DrawBorderedText(spriteBatch, Static.SPRITEFONT_Calibri14, "Mana : " + (int)this.mana + "/" + this.maxMana,
                offsetFromLeft, manaHeight, Color.Black, Color.White);
          /*
            spriteBatch.DrawString(
                Static.SPRITEFONT_Calibri14,
                "Mana : " + (int)this.mana + "/" + this.maxMana,
                new Vector2(offsetFromLeft, manaHeight),
                Color.White
            );*/

            // draw XP text
            Static.DrawBorderedText(spriteBatch, Static.SPRITEFONT_Calibri14, "XP : " + this.xp,
                offsetFromLeft, xpHeight, Color.Black, Color.White);
            /*
            spriteBatch.DrawString(
                Static.SPRITEFONT_Calibri14,
                "XP : " + this.xp,
                new Vector2(offsetFromLeft, xpHeight),
                Color.Black
            );*/

            if (game.waveCleared)
            {

                string nextWaveMessage;
                /*
                string nextWaveMessage = Static.SECONDS_BETWEEN_WAVE - (int)game.sinceLastWaveCleared / 1000 +
                " seconds until next wave...\n" +
                "press space(start) to open skill tree! \n" +
                "use arrow keys (DPad) to equip skills and weapons";
                */

                if (keyboard)
                {
                    if (!playerReady)
                    {
                        nextWaveMessage = "F: Start Next Wave \n" +
                    "Space: Skill Tree \n";
                    }
                    else
                    {
                        nextWaveMessage = "Waiting for other players";
                    }
                }
                else
                {
                    if (!playerReady)
                    {
                     nextWaveMessage = "Y: Start Next Wave \n" +
                    "Back: Skill Tree \n"; ;
                    }
                    else
                    {
                        nextWaveMessage = "Waiting for other players";
                    }
                }

                //spriteBatch.Draw(Static.PIXEL_THIN, new Rectangle(0,screenPortion.Height - 170, viewportBounds.Width, 210), Color.Black*.3f);
                Static.DrawBorderedText(spriteBatch, Static.SPRITE_FONT, nextWaveMessage,offsetFromLeft, screenPortion.Height - 150, Color.Black, Color.White);
            }

            #region SKILL BAR
			int iconSize = screenPortion.Width/18;

            for (int s = 0; s < skillSlots.Length; s++)
            {
                Rectangle skillBox = new Rectangle(screenPortion.Width / 2 + (s * (iconSize + 3)) - 2 *iconSize - 6, screenPortion.Height - iconSize, iconSize, iconSize);

                Skill skill = (Skill)skillSlots[s];
                Texture2D icon = SkillTree.SkillTree.nodeTextures[Static.SKILL_TREE_NODE_ANY];
                Color iconTint = Color.White;

                if (skill == null)
                {
                    iconTint = Color.Black * .5f;
                }
                else if (SkillTree.SkillTree.nodeTextures.ContainsKey(skill.getName()))
                {
                    icon = SkillTree.SkillTree.nodeTextures[skill.getName()];
                }

                spriteBatch.Draw(icon, skillBox, iconTint);

                String Button;

                if (keyboard)
                {
                    if (s == 0)
                    {
                        Button = "1";
                    }
                    else if (s == 1)
                    {
                        Button = "2";
                    }
                    else if (s == 2)
                    {
                        Button = "3";
                    }
                    else
                    {
                        Button = "4";
                    }
                }
                else
                {
                    if (s == 0)
                    {
                        Button = "LB)";
                    }
                    else if (s == 1)
                    {
                        Button = "LT";
                    }
                    else if (s == 2)
                    {
                        Button = "3 RT";
                    }
                    else
                    {
                        Button = "4 RB";
                    }
                }
                spriteBatch.DrawString(Static.SPRITEFONT_Calibri10, Button, new Vector2(skillBox.Left, skillBox.Top), Color.White);

                if (skill is Gun)
                {
                    spriteBatch.DrawString(Static.SPRITEFONT_Calibri10, ((Gun)skill).ammo + "/"+((Gun)skill).clipSize , new Vector2(skillBox.Left, skillBox.Bottom-20), Color.White);
                }
                if (skill is SimpleBow)
                {
                    spriteBatch.DrawString(Static.SPRITEFONT_Calibri10, ((SimpleBow)skill).ammo + "/" + ((SimpleBow)skill).clipSize, new Vector2(skillBox.Left, skillBox.Bottom - 20), Color.White);
                }

                if (skill == null)
                {

                } else if (!skill.Available())
                {
                    if (skill.recharged < skill.rechargeTime)
                    {
                        double coolDownPercentage = (double)skill.recharged / skill.rechargeTime;

                        //System.Console.WriteLine(skill.recharged + " " + skill.rechargeTime + " " + (double)skill.recharged/skill.rechargeTime );

                        Rectangle cooldown = new Rectangle(screenPortion.Width / 2 + (s * (iconSize + 3)) - 2 * iconSize - 6, screenPortion.Height - iconSize, iconSize, iconSize - (int)(iconSize * coolDownPercentage));

                        //spriteBatch.Draw(icon, skillBox, Color.White);

                        spriteBatch.Draw(Static.PIXEL_THIN, cooldown, new Color(Color.Black, 0.5f));

                        //to draw cooldown time
                        /*
                        spriteBatch.DrawString(
                            game.getSpriteFont(),
                            skill.rechargeTime - skill.recharged + "",
                            new Vector2(screenPortion.Width / 2 + (s * (iconSize + 3)) - 2 * iconSize - 6, screenPortion.Height - iconSize),
                            Color.White
                        );
                         * */
                    }
                    if (!skill.Available())
                        spriteBatch.Draw(Static.PIXEL_THIN, skillBox, new Color(Color.DarkBlue, 0.10f));
                }

                if (selectingSkill)
                {
                    highlightRect = new Rectangle(viewportBounds.Width / 2 + (skillbarIndex * (iconSize + 3)) - 2 * iconSize - 6, viewportBounds.Height - iconSize, iconSize, iconSize);

                    spriteBatch.Draw(Static.PIXEL_THIN, highlightRect, new Color(Color.DarkOrchid, 0.3f));
                    if (!selectingSkill2)
                    {
                        Static.DrawBorderedText(spriteBatch, game.getSpriteFont(), "^", highlightRect.X, highlightRect.Y, Color.Black, this.tint);
                    }
                    else
                    {
                        Static.DrawBorderedText(spriteBatch, game.getSpriteFont(), "v", highlightRect.X, highlightRect.Y, Color.Black, this.tint);

                    }


                    if (selectingSkill2)
                    {

                        int j = 0;

                        foreach (Skill unlockedSkill in unEquippedSkills)
                        {
                                Rectangle unlockedSkillRect = new Rectangle(viewportBounds.Width / 2 + (skillbarIndex * (iconSize + 3) + (j * (iconSize / 2)) + (j * 3)) - 2 * iconSize - 6, viewportBounds.Height - iconSize - (iconSize / 2 + 3), iconSize / 2, iconSize / 2);

                                if (unlockedSkill == null)
                                {
                                    spriteBatch.Draw(Static.PIXEL_THIN, unlockedSkillRect, Color.Black*.5f);

                                } else if (SkillTree.SkillTree.nodeTextures.ContainsKey(unlockedSkill.getName()))
                                {
                                    spriteBatch.Draw(SkillTree.SkillTree.nodeTextures[unlockedSkill.getName()], unlockedSkillRect, Color.White);
                                } else 
                                {
                                    spriteBatch.Draw(Static.PIXEL_THIN, unlockedSkillRect, Color.White);
                                }
                                j++;
                        }
                        Rectangle unlockedSkillRect2 = new Rectangle(viewportBounds.Width / 2 + (skillbarIndex * (iconSize + 3) + (skillbarIndex2 * (iconSize / 2)) + (skillbarIndex2 * 3)) - 2 * iconSize - 6, viewportBounds.Height - iconSize - (iconSize / 2 + 3), iconSize / 2, iconSize / 2);
                        spriteBatch.Draw(Static.PIXEL_THIN, unlockedSkillRect2, new Color(Color.DarkOrchid, 0.3f));

                        if (unEquippedSkills[skillbarIndex2] == null)
                        {
                            Static.DrawBorderedText(spriteBatch, game.getSpriteFont(), "None", viewportBounds.Width / 2 + (skillbarIndex * (iconSize + 3) + (skillbarIndex2 * (iconSize / 2)) + (skillbarIndex2 * 3)) - 2 * iconSize - 6, viewportBounds.Height - iconSize - (iconSize / 2 + 3) - 50, Color.Black, Color.White);
                        }
                        else
                        {
                            Static.DrawBorderedText(spriteBatch, game.getSpriteFont(), unEquippedSkills[skillbarIndex2].getName(), viewportBounds.Width / 2 + (skillbarIndex * (iconSize + 3) + (skillbarIndex2 * (iconSize / 2)) + (skillbarIndex2 * 3)) - 2 * iconSize - 6, viewportBounds.Height - iconSize - (iconSize / 2 + 3) - 50, Color.Black, Color.White);

                        }
                    }
                }
            #endregion
            }
        }


        public Equipable getSkill(int skillIndex)
        {
            return skillSlots[skillIndex];
        }

        private void UseSkill(int skillIndex)
        {
			if (skillIndex > 4 || skillIndex < 0)
            {
                return;
            }
            //make sure slot is not empty
            if (skillSlots[skillIndex] == null)
            {
                return;
            }
            //make sure skill can be used
            else if (!skillSlots[skillIndex].Available())
            {
                return;
            }
            //use skill
            skillSlots[skillIndex].Use();
			//System.Console.WriteLine(skillSlots[skillIndex].getName() + " used!");
        }

        public void addEquipable(Equipable equip)
        {
            inventory.Add(equip);
            unEquippedSkills.Add(equip);
        }

        public void removeEquipable(Equipable equip)
        {
            if (equip == null)
            {
                return;
            }
            for (int i = 0; i < skillSlots.Length; i++ )
            {
                if (skillSlots[i] == equip)
                {
                    skillSlots[i].OnUnequip();
                    skillSlots[i] = null;
                }
            }
            inventory.Remove(equip);
            unEquippedSkills.Remove(equip);

        }

        //returns skillIndex of currentWeapon
        //-1 if not equipped
        public int weaponEquipped()
        {

            for (int i = 0; i < skillSlots.Count(); i++)
            {
                if (skillSlots[i] == currentWeapon)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Equip(Equipable equip, int skillIndex)
        {
			if (skillIndex > 4 || skillIndex < 0)
            {
                return;
            }
            //unequip previous skill if there was one
            if (skillSlots[skillIndex] != null)
            {
                skillSlots[skillIndex].OnUnequip();
                unEquippedSkills.Add(skillSlots[skillIndex]);

            }
            //equip new skill
            skillSlots[skillIndex] = equip;
            if (equip != null)
            {
                equip.OnEquip();
                unEquippedSkills.Remove(equip);
            }

        }

        public void incXP(int amount)
        {
            xp += amount;
        }
        public bool SkillTreeOpen()
        {
            return skilltreescreen;
        }

        public void SkillTreeButton()
        {
            if (!game.waveCleared)
            {
                skilltreescreen = false;
                return;
            }

            OpenSkillTree();
        }

        /*
        public void SkillTreeButtonDown()
        {
            if (!skilltreebuttondown)
            {
                skilltreebuttondown = true;
                OpenSkillTree();
            }
            else
            {
                return;
            }
        }

        public void SkillTreeButtonRelease()
        {
            if (skilltreebuttondown)
            {
                skilltreebuttondown = false;
            }
            else
            {
                return;
            }
        }
         * */

        public void DrawSkillTree(Rectangle screenPortion, SpriteBatch spriteBatch)
        {
            skilltree.Draw(screenPortion, spriteBatch);
        }

        private void OpenSkillTree()
        {
            skilltreescreen = !skilltreescreen;
        }

		public void DownArrow()
		{

            if (SkillTreeOpen())
            {
                skilltree.Down();

                return;
            }

			if (selectingSkill2) {

                Equip(unEquippedSkills[skillbarIndex2], skillbarIndex);
				selectingSkill2 = false; 
				skillbarIndex2 = 0; 
				return;
			}
			/*

			if (selectingSkill2) {
				selectingSkill2 = false; 
				return; 
			}
			*/

			//selectingSkill = false; 
		}

		public void UpArrow()
		{

            if (SkillTreeOpen())
            {
                skilltree.Up();

                return;
            }

            /*
			if (!selectingSkill) {
				selectingSkill = true; 
				skillbarIndex = 0; 
				return;
			}
             * */

			if (selectingSkill && unEquippedSkills.Count() > 0) {
				selectingSkill2 = true; 
				skillbarIndex2 = 0;
			}
		}

		public void RightArrow()
		{

            if (SkillTreeOpen())
            {
                skilltree.Right();

                return;
            }
			if (selectingSkill2) {
				if (skillbarIndex2 < unEquippedSkills.Count() -1)
					skillbarIndex2++;
				else
					skillbarIndex2 = 0;
				return; 
			}

			if (skillbarIndex < skillSlots.Length-1) 
				skillbarIndex++; 
			else 
				skillbarIndex = 0; 
		}

		public void LeftArrow()
		{

            if (SkillTreeOpen())
            {
                skilltree.Left();

                return;
            }

			if (selectingSkill2) {
				if (skillbarIndex2 > 0)
					skillbarIndex2--;
				else
					skillbarIndex2 = unEquippedSkills.Count() -1;
				return; 
			}

			if (skillbarIndex > 0) 
				skillbarIndex--; 
			else 
				skillbarIndex = skillSlots.Length-1; 
		}
			
		public void LeftClick()
		{
            if (SkillTreeOpen())
            {
                return;
            }
			UseSkill(Static.PLAYER_LEFTCLICK_SKILL_INDEX);
		}

        public void Ybutton()
        {
            if (SkillTreeOpen())
            {
                return;
            }

            if (game.waveCleared)
            {
                playerReady = !playerReady;
            }
            
        }

        public void AButton()
        {
			/*
			if (selectingSkill) {
				selectingSkill = false; 
				return; 
			}
			*/

            if (SkillTreeOpen())
            {
                skilltree.Unlock();
                return;
            }

			if (selectingSkill2) {
                DownButton();
				return;
			}


				
            if (currentInteractable != null)
            {
                if (currentInteractable.Available(this)){
                    currentInteractable.Interact(this);
                }
            }
        }

        public void BButton()
        {
            if (SkillTreeOpen())
            {
                return;
            }
            //whatever B Button does
        }

        public void L1Button()
        {
            if (SkillTreeOpen())
            {
                skilltree.L1();
                return;
            }
            UseSkill(Static.PLAYER_L1_SKILL_INDEX);
        }
        public void L2Button()
        {
            if (SkillTreeOpen())
            {
                skilltree.L2();
                return;
            }
            UseSkill(Static.PLAYER_L2_SKILL_INDEX);
        }
        public void R1Button()
        {
            if (SkillTreeOpen())
            {
                skilltree.R1();
                return;
            }
            UseSkill(Static.PLAYER_R1_SKILL_INDEX);
        }
        public void R2Button()
        {
            if (SkillTreeOpen())
            {
                skilltree.R2();
                return;
            }
            UseSkill(Static.PLAYER_R2_SKILL_INDEX);
        }

        public void UpButton()
        {
            if (SkillTreeOpen())
            {
                skilltree.Up();

                return;
            }
            MoveUp();
        }

        public void DownButton()
        {
            if (SkillTreeOpen())
            {
                skilltree.Down();
                return;
            }
            MoveDown();
        }

        public void LeftButton()
        {
            if (SkillTreeOpen())
            {
                skilltree.Left();

                return;
            }
            MoveLeft();
        }

        public void RightButton()
        {

            if (SkillTreeOpen())
            {
                skilltree.Right();

                return;
            }
            MoveRight();
        }

        private void MoveUp()
        {
            this.move(0, -Static.PLAYER_MOVE_SPEED);
        }
        private void MoveDown()
        {
            this.move(0, Static.PLAYER_MOVE_SPEED);
        }
        private void MoveLeft()
        {
            this.move(-Static.PLAYER_MOVE_SPEED, 0);
        }
        private void MoveRight()
        {
            this.move(Static.PLAYER_MOVE_SPEED, 0);
        }

        public void noMovement()
        {
        }

        protected override void OnDie()
        {
            this.skilltreescreen = false;
            dead = true;
            game.Spawn(new DeadPlayer(game, this), x, y);
        }

        public bool isDead()
        {
            return dead;
        }

        public override void OnKillOther(GameEntity entity)
        {
            int totalPlayers = 0;
            foreach (Player player in game.players)
            {

                if (player == null || player.isDead())
                {
                    continue;
                }
                totalPlayers++;
            }
            int reward = entity.getXPReward();//  totalPlayers;
            foreach (Player player in game.players)
            {

                if (player == null || player.isDead())
                {
                    continue;
                }
                TextEffect xpEffect = EntityFactory.getXPEffect(game, reward);
                game.Spawn(xpEffect, player.getCenterX(), player.getCenterY());
                player.incXP(reward);
            }

        }

        public override void OnDamageOther(GameEntity entity, int amount)
        {
        }

        public override void rotateToAngle(float angle) //animation is based on rotation which is used by both movement and aiming
        {
            if (SkillTreeOpen())
            {
                return;
            }
            base.rotateToAngle(angle);
        }

        public override void UpdateAnimation(GameTime gameTime)
        {
            // Animation stuff
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            swordElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed > delay)
            {
                if (walkFrame >= WALK_ANIMATION_FRAMES - 1)
                {
                    walkFrame = 0;
                }

                else
                {
                    walkFrame++;
                }
                elapsed = 0;
            }

            if (this.getLastMovement().X != 0 || this.getLastMovement().Y != 0)
            {
                if (Math.Cos(this.direction) > .5)
                {
                    base.spriteSource = new Rectangle(64 * walkFrame, RIGHT_ANIMATION * 64, 64, 64);

                }
                else if (Math.Sin(this.direction) > .5)
                {
                    base.spriteSource = new Rectangle(64 * walkFrame, DOWN_ANIMATION * 64, 64, 64);

                }
                else if (Math.Sin(direction) < -.5)
                {
                    //spriteSource = FramesToAnimation[UP_ANIMATION];
                    base.spriteSource = new Rectangle(64 * walkFrame, UP_ANIMATION * 64, 64, 64);

                }
                else if (Math.Cos(direction) < -.5)
                {
                    //spriteSource = FramesToAnimation[LEFT_ANIMATION];
                    base.spriteSource = new Rectangle(64 * walkFrame, LEFT_ANIMATION * 64, 64, 64);
                }
            }
            else
            {
                if (Math.Cos(this.direction) > .5)
                {
                    //spriteSource = FramesToAnimation[RIGHT_ANIMATION];
                    base.spriteSource = new Rectangle(64 * 0, RIGHT_ANIMATION * 64, 64, 64);

                }
                else if (Math.Sin(this.direction) > .5)
                {
                    base.spriteSource = new Rectangle(64 * 0, DOWN_ANIMATION * 64, 64, 64);

                }
                else if (Math.Sin(direction) < -.5)
                {
                    //spriteSource = FramesToAnimation[UP_ANIMATION];
                    base.spriteSource = new Rectangle(64 * 0, UP_ANIMATION * 64, 64, 64);

                }
                else if (Math.Cos(direction) < -.5)
                {
                    //spriteSource = FramesToAnimation[LEFT_ANIMATION];
                    base.spriteSource = new Rectangle(64 * 0, LEFT_ANIMATION * 64, 64, 64);
                }

            }


            base.UpdateAnimation(gameTime);

        }

        public override string getName()
        {
            return Static.TYPE_PLAYER;
        }


        public void revive()
        {
            this.drawWeapon = true;
            this.health = maxHealth;
            this.dead = false;   
            this.mana = maxMana;
            this.skilltreescreen = false;
            this.skilltreebuttondown = false;
            this.currentInteractable = null;
            this.remove = false;

        }

        public void waveClear()
        {
           // revive();
            if (!dead)
            {
                health = maxHealth;
            }
            selectingSkill = true;
            skillbarIndex = 0;
            playerReady = false;
        }

        public void waveBegin(){
            selectingSkill = false;
        }

    }
}
