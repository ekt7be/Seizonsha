using GameName1.Interfaces;
using GameName1.Skills;
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


        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //draw armor and weapons equipped etc

        }

        public override void OnSpawn()
        {
            //nothing happens
        }

        public override void collide(GameEntity entity)
        {
            //nothing happens
        }

        public override void collideWithWall()
        {
            //nothing happens
        }

        public Player(Seizonsha game, PlayerIndex playerIndex, Texture2D sprite, int x, int y) : base(game, sprite, x, y, Static.PLAYER_WIDTH, Static.PLAYER_HEIGHT)
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

            Equip(new ChangeColor(Color.Red), Static.PLAYER_L1_SKILL_INDEX);
            Equip(new ChangeColor(Color.Purple), Static.PLAYER_L2_SKILL_INDEX);
            Equip(new ChangeColor(Color.Green), Static.PLAYER_R1_SKILL_INDEX);
            Equip(new ChangeColor(Color.Black), Static.PLAYER_R2_SKILL_INDEX);



        }

        public void DrawScreen(Texture2D renderedGame, Rectangle screenPortion, SpriteBatch spriteBatch)
        {
            //we will render the entire game world and send it to each player who will use their camera coordinates
            //and the dimensions of their portion of the screen to draw their screen.
            //Interface will be drawn on top along with any menus including skill tree
        }

        public Equipable getSkill(int skillIndex)
        {
            return skillSlots[skillIndex];
        }

        private void UseSkill(int skillIndex)
        {
            if (skillIndex > 3 || skillIndex < 0)
            {
                return;
            }
            //make sure slot is not empty
            if (skillSlots[skillIndex] == null)
            {
                return;
            }
            //make sure skill can be used
            else if (!skillSlots[skillIndex].Available(this))
            {
                return;
            }
            //use skill
            skillSlots[skillIndex].Use(this);
        }

        public void addEquipable(Equipable equip)
        {
            inventory.Add(equip);
        }
        public void Equip(Equipable equip, int skillIndex)
        {
            if (skillIndex > 3 || skillIndex < 0)
            {
                return;
            }
            //unequip previous skill if there was one
            if (skillSlots[skillIndex] != null)
            {
                skillSlots[skillIndex].OnUnequip(this);
            }
            //equip new skill
            skillSlots[skillIndex] = equip;
            equip.OnEquip(this);


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
            this.move(0, -10);
        }
        public void MoveDown()
        {
            this.move(0, 10);
        }
        public void MoveLeft()
        {
            this.move(-10, 0);
        }
        public void MoveRight()
        {
            this.move(10, 0);
        }
    }
}
