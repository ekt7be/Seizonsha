using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameName1
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PlayerMenuScreen : MenuScreen
    {
        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlayerMenuScreen()
            : base("Wave Completed!")
        {
            // Create our menu entries.
            MenuEntry equippableGameMenuEntry = new MenuEntry("Change Equippables");
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            equippableGameMenuEntry.Selected += EquippableGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(equippableGameMenuEntry);
        }
        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void EquippableGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            ScreenManager.SpriteBatch.Begin();
            graphics.Viewport = game.defaultView;
            List<Player> players = game.getPlayers();
            
            switch ((int)ControllingPlayer+1)
            {
                case 1:
                    graphics.Viewport = game.p1View;
                    //players[0].SkillTreeOpen();
                    players[0].DrawSkillTree(graphics.Viewport.Bounds, ScreenManager.SpriteBatch);
                    break;
                case 2:
                    graphics.Viewport = game.p2View;
                    players[0].DrawSkillTree(graphics.Viewport.Bounds, ScreenManager.SpriteBatch);
                    break;
                case 3:
                    graphics.Viewport = game.p3View;
                    break;
                case 4:
                    graphics.Viewport = game.p4View;
                    break;
                default:
                    graphics.Viewport = game.defaultView;
                    break;
            }
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion
    }
}
