using GameName1.Effects;
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
        private float manaRegen;
        private float mana;
        private int maxMana;

        private bool skilltreebuttondown;
        private bool skilltreescreen;

        private static float elapsed;
        private static readonly float delay = 200f;
        private static int currentFrame = 0;

        private static readonly int UP_ANIMATION = 0;
        private static readonly int DOWN_ANIMATION = 2;
        private static readonly int LEFT_ANIMATION = 1;
        private static readonly int RIGHT_ANIMATION = 3;
        private static readonly int WALK_ANIMATION_FRAMES = 9;

        // This is messy. Make this more efficient.
        private bool isWalking = false;
        private bool up;
        private bool down;
        private bool left;
        private bool right;

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

            // Animation stuff
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed > delay)
            {
                if (currentFrame >= WALK_ANIMATION_FRAMES - 1)
                {
                    currentFrame = 0;
                }
                else
                {
                    currentFrame++;
                }
                elapsed = 0;
            }

            if (isWalking)
            {
                if (up)
                    base.spriteSource = new Rectangle(64 * currentFrame, UP_ANIMATION * 64, 64, 64);
                else if (left)
                    base.spriteSource = new Rectangle(64 * currentFrame, LEFT_ANIMATION * 64, 64, 64);
                else if (down)
                    base.spriteSource = new Rectangle(64 * currentFrame, DOWN_ANIMATION * 64, 64, 64);
                else if (right)
                    base.spriteSource = new Rectangle(64 * currentFrame, RIGHT_ANIMATION * 64, 64, 64);
            }
            else
            {
                if (up)
                    base.spriteSource = new Rectangle(64 * 0, UP_ANIMATION * 64, 64, 64);
                else if (left)
                    base.spriteSource = new Rectangle(64 * 0, LEFT_ANIMATION * 64, 64, 64);
                else if (down)
                    base.spriteSource = new Rectangle(64 * 0, DOWN_ANIMATION * 64, 64, 64);
                else if (right)
                    base.spriteSource = new Rectangle(64 * 0, RIGHT_ANIMATION * 64, 64, 64);
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
                        
                        spriteBatch.Draw(game.getTestSprite(tBounds, Color.White), tBounds, Color.White);
                        spriteBatch.Draw(game.getTestSprite(cBounds, Color.Blue), cBounds, Color.Blue);
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
            this.skilltree = new SkillTree.SkillTree(game, this, game.getTestSprite(new Rectangle(0, 0, Static.SCREEN_WIDTH, Static.SCREEN_HEIGHT), Color.Black));
            Equip(new Gun(game, this, 30, 10, 10f), Static.PLAYER_L1_SKILL_INDEX);
            Equip(new Fireball(game, this, 120, 100, 5f), Static.PLAYER_L2_SKILL_INDEX);
            //Equip(new LifeDrain(game, this, 2, 2, 40), Static.PLAYER_L2_SKILL_INDEX);
            Equip(new HealingTouch(game, this, -50, 100), Static.PLAYER_R1_SKILL_INDEX);
            Equip(new Sword(game, this, 300, 10), Static.PLAYER_R2_SKILL_INDEX);

            this.maxMana = Static.PLAYER_MAX_MANA;
            this.mana = maxMana;

            this.manaRegen = 0.2f;
            this.skilltreescreen = false;
            this.skilltreebuttondown = false;

			this.camera = camera;


            base.scale = 1.0f;
            FramesToAnimation.Add(UP_ANIMATION, new Rectangle(0, 64 * UP_ANIMATION, 64, 64));
            FramesToAnimation.Add(DOWN_ANIMATION, new Rectangle(0, 64 * DOWN_ANIMATION, 64, 64));
            FramesToAnimation.Add(LEFT_ANIMATION, new Rectangle(0, 64 * LEFT_ANIMATION, 64, 64));
            FramesToAnimation.Add(RIGHT_ANIMATION, new Rectangle(0, 64 * RIGHT_ANIMATION, 64, 64));

            this.rotateToAngle(0f); //sets correct animation




        }

        public void DrawScreen(Rectangle screenPortion, SpriteBatch spriteBatch)
        {
            //we will render the entire game world and send it to each player who will use their camera coordinates
            //and the dimensions of their portion of the screen to draw their screen.
            //Interface will be drawn on top along with any menus including skill tree

            if (SkillTreeOpen())
            {
                skilltree.Draw(screenPortion, spriteBatch);
                return;
            }
			Texture2D texture = new Texture2D(game.GraphicsDevice, 1, 1);
			texture.SetData(new[] { Color.White });

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
			string displaySkills = "L1: " + this.getSkill(Static.PLAYER_L1_SKILL_INDEX).getName() + "\n" +
				"L2: " + this.getSkill(Static.PLAYER_L2_SKILL_INDEX).getName() + "\n" +
				"R1: " + this.getSkill(Static.PLAYER_R1_SKILL_INDEX).getName() + "\n" +
				"R2: " + this.getSkill(Static.PLAYER_R2_SKILL_INDEX).getName() + "\n" +
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
            //whatever A Button does
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
                return;
            }
            UseSkill(Static.PLAYER_L1_SKILL_INDEX);
        }
        public void L2Button()
        {
            if (SkillTreeOpen())
            {
                return;
            }
            UseSkill(Static.PLAYER_L2_SKILL_INDEX);
        }
        public void R1Button()
        {
            if (SkillTreeOpen())
            {
                return;
            }
            UseSkill(Static.PLAYER_R1_SKILL_INDEX);
        }
        public void R2Button()
        {
            if (SkillTreeOpen())
            {
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
            isWalking = true;
            this.move(0, -Static.PLAYER_MOVE_SPEED);
        }
        private void MoveDown()
        {
            isWalking = true;
            this.move(0, Static.PLAYER_MOVE_SPEED);
        }
        private void MoveLeft()
        {
            isWalking = true;
            this.move(-Static.PLAYER_MOVE_SPEED, 0);
        }
        private void MoveRight()
        {
            isWalking = true;
            this.move(Static.PLAYER_MOVE_SPEED, 0);
        }

        public void noMovement()
        {
            isWalking = false;
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

            if (FramesToAnimation == null) //gameentity class calls this during initialization too
            {
                return;
            }

            if (Math.Cos(angle) > .5)
            {
                //spriteSource = FramesToAnimation[RIGHT_ANIMATION];
                right = true;
                up = left = down = false;
            }
            else if (Math.Sin(angle) > .5)
            {
                //spriteSource = FramesToAnimation[DOWN_ANIMATION];
                down = true;
                up = left = right = false;
            }
            else if (Math.Sin(angle) < -.5)
            {
                //spriteSource = FramesToAnimation[UP_ANIMATION];
                up = true;
                right = left = down = false;
            }
            else if (Math.Cos(angle) < -.5)
            {
                //spriteSource = FramesToAnimation[LEFT_ANIMATION];
                left = true;
                up = right = down = false;
            }
        }

        public override string getName()
        {
            return Static.TYPE_PLAYER;
        }
    }
}
