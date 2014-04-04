﻿using GameName1.Effects;
using GameName1.Interfaces;
using GameName1.Skills;
using GameName1.SkillTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName1
{
    public class Player : GameEntity
    {
        public int cameraX { get; set; }
        public int cameraY { get; set; }
        public int xp { get; set; }
        public int skillPoints { get; set; }
        public PlayerIndex playerIndex { get; set; }
        private bool dead;
        private Equipable[] skillSlots;
        private List<Equipable> inventory;
        private SkillTree.SkillTree skilltree;
        public float manaRegen;
        private float mana;
        public float maxMana;

        private bool skilltreebuttondown;
        private bool skilltreescreen;

        private Interactable currentInteractable;

        private static float elapsed;
        private static float swordElapsed;
        private static readonly float delay = 200f;
        private static readonly float swordDelay = 50f;
        private static int walkFrame = 0;
        private static int swordFrame = 0;

        private static readonly int UP_ANIMATION = 0;
        private static readonly int DOWN_ANIMATION = 2;
        private static readonly int LEFT_ANIMATION = 1;
        private static readonly int RIGHT_ANIMATION = 3;
        private static readonly int WALK_ANIMATION_FRAMES = 9;
        private static readonly int SLASH_ANIMATION_FRAMES = 6;


		public Camera camera;


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

            this.currentInteractable = findInteraction();

         
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
            base.Draw(spriteBatch);
            foreach(Equipable equip in this.skillSlots)
            {
                if (equip is Skill)
                {
                    Skill skill = (Skill)equip;
                    if (skill.percentCasted() < .99 && skill.percentCasted() > .01)
                    {

                        spriteBatch.DrawString(game.getSpriteFont(), skill.getName(), new Vector2(this.getCenterX() - 50, this.hitbox.Y + this.hitbox.Height), Color.White);
                        Rectangle cBounds = new Rectangle(this.getCenterX() - 30, this.hitbox.Y + this.hitbox.Height + 30, (int)(skill.percentCasted()*60), 5);
                        Rectangle tBounds = new Rectangle(this.getCenterX() - 30, this.hitbox.Y + this.hitbox.Height + 30, 60, 5);
                        
                        spriteBatch.Draw(Static.PIXEL_THIN, tBounds, Color.White);
                        spriteBatch.Draw(Static.PIXEL_THIN, cBounds, Color.Blue);
                    }
                }
            }

            //draw armor and weapons equipped etc
            spriteBatch.Draw(Seizonsha.spriteMappings[3], this.hitbox, base.spriteSource, tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
            spriteBatch.Draw(Seizonsha.spriteMappings[5], this.hitbox, base.spriteSource, tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
            spriteBatch.Draw(Seizonsha.spriteMappings[4], this.hitbox, base.spriteSource, tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
            spriteBatch.Draw(Seizonsha.spriteMappings[6], this.hitbox, base.spriteSource, tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
            spriteBatch.Draw(Seizonsha.spriteMappings[7], this.hitbox, base.spriteSource, tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);
            spriteBatch.Draw(Seizonsha.spriteMappings[2], this.hitbox, base.spriteSource, tint, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1);

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

		public Player(Seizonsha game, PlayerIndex playerIndex, Texture2D sprite, Camera camera) : base(game, sprite, Static.PLAYER_WIDTH, Static.PLAYER_HEIGHT, Static.TARGET_TYPE_FRIENDLY, Static.PLAYER_MAX_HEALTH)
        {
            this.cameraX = 0;
            this.cameraY = 0;
            this.skillPoints = 0;
            this.xp = 0;
            this.health = Static.PLAYER_MAX_HEALTH;
            this.dead = false;
            this.playerIndex = playerIndex;

			this.skillSlots = new Equipable[4]; //each slot is different skill, weapon, or item
            this.inventory = new List<Equipable>();
            this.skilltree = new SkillTree.SkillTree(game, this, Static.PIXEL_THIN);

			//Equip(new Gun(game, this, 30, 10, 10f), Static.PLAYER_L1_SKILL_INDEX);
            //Equip(new LifeDrain(game, this, 3, 40, 40), Static.PLAYER_L1_SKILL_INDEX);
            //Equip(new FireLance(game, this, 10, 5), Static.PLAYER_L1_SKILL_INDEX);
            //Equip(new Fireball(game, this, 120, 100, 5f), Static.PLAYER_R1_SKILL_INDEX);
            //Equip(new LifeDrain(game, this, 2, 2, 40), Static.PLAYER_L2_SKILL_INDEX);
            //Equip(new Teleport(game, this, 180, 3, 1), Static.PLAYER_L2_SKILL_INDEX);
            //Equip(new HealingTouch(game, this, -50, 100), Static.PLAYER_R1_SKILL_INDEX);
            //Equip(new Sword(game, this, 300, 10), Static.PLAYER_R2_SKILL_INDEX);
            //Equip(new Blizzard(game, this, 0, 200, 200, 100), Static.PLAYER_R2_SKILL_INDEX);

            Equip(new Gun(game, this, 10, 30, 15), Static.PLAYER_L1_SKILL_INDEX);
            Equip(new Sword(game, this, 30, 40), Static.PLAYER_R1_SKILL_INDEX);
            Equip(new Fireball(game, this, 40, 30, 10), Static.PLAYER_R2_SKILL_INDEX);
            Equip(new HealingTouch(game, this, -50, 60), Static.PLAYER_L2_SKILL_INDEX);

            this.maxMana = Static.PLAYER_MAX_MANA;
            this.mana = maxMana;

            this.manaRegen = Static.PLAYER_START_MANA_REGEN;
            this.skilltreescreen = false;
            this.skilltreebuttondown = false;
            this.currentInteractable = null;

			this.camera = camera;


            base.scale = 1.0f;

            this.rotateToAngle(0f); //sets correct animation




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



            if (currentInteractable != null)
            {
                if (currentInteractable is GameEntity)
                {
                    spriteBatch.DrawString(game.getSpriteFont(), "Press A(Enter) to " + currentInteractable.Message(this), new Vector2(screenPortion.Width/2, screenPortion.Height/2), Color.White);
                    //(GameEntity)currentInteractable
                }
            }

            if (SkillTreeOpen())
            {
                DrawSkillTree(screenPortion, spriteBatch);
                return;
            }
            Texture2D texture = Static.PIXEL_THIN;

			int barLength = screenPortion.Width / 2; 
			int barHeight = screenPortion.Height / 32; 

			double green = ((double)this.health/(double)this.maxHealth) * barLength;
            double blue = ((double)this.mana / (double)this.maxMana) * barLength;
			Rectangle hpMax = new Rectangle(20, 20, barLength, barHeight);
			Rectangle hpRemaining = new Rectangle(20, 20, (int)green, barHeight);
			Rectangle manaMax = new Rectangle (20, 20+(barHeight), barLength, barHeight);
            Rectangle manaRemaining = new Rectangle(20, 20 + (barHeight), (int) blue, barHeight);
			Rectangle xp = new Rectangle (20, 20+(barHeight*2), barLength, barHeight); 

			// draw HP bar
			spriteBatch.Draw(texture, hpMax, Color.Red); 
			spriteBatch.Draw(texture, hpRemaining, Color.Green); 
			// draw Mana bar
			spriteBatch.Draw(texture, manaMax, Color.LightBlue);
			spriteBatch.Draw(texture, manaRemaining, Color.Blue);
			// draw XP bar
			spriteBatch.Draw(texture, xp, Color.Yellow); 
            
			// draw HP text
			spriteBatch.DrawString(
				game.getSpriteFont(), 
				"HP : " + this.health + "/" + this.maxHealth, 
				new Vector2(20, 20), 
				Color.White
			);
			// draw Mana text
			spriteBatch.DrawString(
				game.getSpriteFont(), 
				"Mana : " + (int)this.mana + "/" + this.maxMana, 
				new Vector2(20, 20+(barHeight)), 
				Color.White
			);
			// draw XP text
			spriteBatch.DrawString(
				game.getSpriteFont(), 
				"XP : " + this.xp, 
				new Vector2(20, 20+(barHeight*2)), 
				Color.Black
			);

			// draw skills text
			string displaySkills = "L1(1 key): " + this.getSkill(Static.PLAYER_L1_SKILL_INDEX).getName() + "\n" +
				"L2(2 key): " + this.getSkill(Static.PLAYER_L2_SKILL_INDEX).getName() + "\n" +
				"R1(3 key): " + this.getSkill(Static.PLAYER_R1_SKILL_INDEX).getName() + "\n" +
				"R2(4 key): " + this.getSkill(Static.PLAYER_R2_SKILL_INDEX).getName() + "\n" +
                "P:  Pause/Quit Menu (temp)" ;
			spriteBatch.DrawString(game.getSpriteFont(), displaySkills, new Vector2(20, 100), Color.White);


            //draw Wave number
            spriteBatch.DrawString(game.getSpriteFont(), "WAVE: " + game.Wave, new Vector2(20, screenPortion.Height-100), Color.White);
           
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
        }

        public void addEquipable(Equipable equip)
        {
            inventory.Add(equip);
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
            }
            //equip new skill
            skillSlots[skillIndex] = equip;
            equip.OnEquip();


        }

        public void incXP(int amount)
        {
            xp += amount;
        }
        public bool SkillTreeOpen()
        {
            return skilltreescreen;
        }

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

        public void DrawSkillTree(Rectangle screenPortion, SpriteBatch spriteBatch)
        {
            skilltree.Draw(screenPortion, spriteBatch);
        }

        private void OpenSkillTree()
        {
            skilltreescreen = !skilltreescreen;
        }

		public void LeftClick()
		{
            if (SkillTreeOpen())
            {
                return;
            }
			UseSkill(Static.PLAYER_LEFTCLICK_SKILL_INDEX);
		}

        public void AButton()
        {
            if (SkillTreeOpen())
            {
                skilltree.Unlock();
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
        }

        public bool isDead()
        {
            return dead;
        }

        public override void OnKillOther(GameEntity entity)
        {
            TextEffect xpEffect = EntityFactory.getXPEffect(game,entity.getXPReward());
            game.Spawn(xpEffect,getCenterX(), getCenterY());
            incXP(entity.getXPReward());
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

            base.UpdateAnimation(gameTime);
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
        }

        public override string getName()
        {
            return Static.TYPE_PLAYER;
        }
    }
}
