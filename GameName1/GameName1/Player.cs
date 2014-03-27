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
        public int health { get; set; } // should add to GameEntity or make interface?  Will be redundent to code health for all enemies
        public int mana { get; set; }
        public int xp { get; set; }
        public int skillPoints { get; set; }
        public PlayerIndex playerIndex { get; set; }
        private bool dead;
        private Equipable[] skillSlots;
        private List<Equipable> inventory;
        private SkillTree.SkillTree skilltree;
        private bool skilltreescreen;

        private static readonly int UP_ANIMATION = 0;
        private static readonly int DOWN_ANIMATION = 2;
        private static readonly int LEFT_ANIMATION = 1;
        private static readonly int RIGHT_ANIMATION = 3;




		public Camera camera; 



        public override void Update()
        {
            foreach (Equipable skill in skillSlots){
                if (skill != null)
                {
                    skill.Update();
                }
            }
            //base.source = new Rectangle(sprite.Width / 4 * currentAnimationFrame, 0, sprite.Width / 4, sprite.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //draw armor and weapons equipped etc

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

		public Player(Seizonsha game, PlayerIndex playerIndex, Texture2D sprite, int x, int y, Camera camera) : base(game, sprite, x, y, Static.PLAYER_WIDTH, Static.PLAYER_HEIGHT, Static.TARGET_TYPE_FRIENDLY, Static.PLAYER_MAX_HEALTH)
        {
            this.cameraX = 0;
            this.cameraY = 0;
            this.skillPoints = 0;
            this.xp = 0;
            this.mana = Static.PLAYER_MAX_MANA;
            this.health = Static.PLAYER_MAX_HEALTH;
            this.dead = false;
            this.playerIndex = playerIndex;

			this.skillSlots = new Equipable[4]; //each slot is different skill, weapon, or item
            this.inventory = new List<Equipable>();
            this.skilltree = new SkillTree.SkillTree(game, this, game.getTestSprite(new Rectangle(0, 0, Static.SCREEN_WIDTH, Static.SCREEN_HEIGHT), Color.Black));
            Equip(new Gun(game, this, 30, 10, 10f), Static.PLAYER_L1_SKILL_INDEX);
            Equip(new Fireball(game, this, 120, 100, 5f), Static.PLAYER_L2_SKILL_INDEX);
            Equip(new HealingTouch(game, this, -50, 100), Static.PLAYER_R1_SKILL_INDEX);
            Equip(new Sword(game, this, 300, 10), Static.PLAYER_R2_SKILL_INDEX);


            this.skilltreescreen = false;

			this.camera = camera;


            base.scale = 1.0f;
            FramesToAnimation.Add(UP_ANIMATION, new Rectangle(sprite.Width / 4 * UP_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            FramesToAnimation.Add(DOWN_ANIMATION, new Rectangle(sprite.Width / 4 * DOWN_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            FramesToAnimation.Add(LEFT_ANIMATION, new Rectangle(sprite.Width / 4 * LEFT_ANIMATION, 0, sprite.Width / 4, sprite.Height));
            FramesToAnimation.Add(RIGHT_ANIMATION, new Rectangle(sprite.Width / 4 * RIGHT_ANIMATION, 0, sprite.Width / 4, sprite.Height));

            this.rotateToAngle(0f); //sets correct animation




        }

        public void DrawScreen(Rectangle screenPortion, SpriteBatch spriteBatch)
        {
            //we will render the entire game world and send it to each player who will use their camera coordinates
            //and the dimensions of their portion of the screen to draw their screen.
            //Interface will be drawn on top along with any menus including skill tree

			if (skilltreescreen) {
				skilltree.Draw (screenPortion, spriteBatch);
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

        public void OpenSkillTree(bool open)
        {
            skilltreescreen = open;
        }

		public void LeftClick()
		{
			UseSkill(Static.PLAYER_LEFTCLICK_SKILL_INDEX);
		}

        public void AButton()
        {
            //whatever A Button does
        }

        public void BButton()
        {
            //whatever B Button does
        }

        public void L1Button()
        {
            UseSkill(Static.PLAYER_L1_SKILL_INDEX);
        }
        public void L2Button()
        {
            UseSkill(Static.PLAYER_L2_SKILL_INDEX);
        }
        public void R1Button()
        {
            UseSkill(Static.PLAYER_R1_SKILL_INDEX);
        }
        public void R2Button()
        {
            UseSkill(Static.PLAYER_R2_SKILL_INDEX);
        }

        public void MoveUp()
        {
            this.move(0, -Static.PLAYER_MOVE_SPEED);
        }
        public void MoveDown()
        {
            this.move(0, Static.PLAYER_MOVE_SPEED);
        }
        public void MoveLeft()
        {
            this.move(-Static.PLAYER_MOVE_SPEED, 0);
        }
        public void MoveRight()
        {
            this.move(Static.PLAYER_MOVE_SPEED, 0);
        }

        protected override void OnDie()
        {
            dead = true;
        }

        public bool isDead()
        {
            return dead;
        }

        public override void OnKillOther(GameEntity entity)
        {
            XPEffect xpEffect = new XPEffect(game,entity.getXPReward(), 30, getCenterX(), getCenterY());
            game.Spawn(xpEffect);
            incXP(entity.getXPReward());
        }

        public override void OnDamageOther(GameEntity entity, int amount)
        {
        }

        public override void rotateToAngle(float angle) //animation is based on rotation which is used by both movement and aiming
        {
            base.rotateToAngle(angle);

            if (FramesToAnimation == null) //gameentity class calls this during initialization too
            {
                return;
            }

            if (Math.Cos(angle) > .5)
            {
                spriteSource = FramesToAnimation[RIGHT_ANIMATION];
            }
            else if (Math.Sin(angle) > .5)
            {
                spriteSource = FramesToAnimation[DOWN_ANIMATION];
            }
            else if (Math.Sin(angle) < -.5)
            {
                spriteSource = FramesToAnimation[UP_ANIMATION];
            }
            else if (Math.Cos(angle) < -.5)
            {
                spriteSource = FramesToAnimation[LEFT_ANIMATION];
            }
        }
    }
}
