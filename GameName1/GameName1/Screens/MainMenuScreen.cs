
#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GameName1
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization
        ContentManager Content;
        int currentNumPlayers = 0;
        int keyboardPlayer = 4;
        MenuEntry numPlayersMenuEntry;
        MenuEntry keyboardPlayerMenuEntry;
        SoundEffect menuSound;
        SoundEffectInstance menuSoundLoop;
        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            numPlayersMenuEntry = new MenuEntry(string.Empty);
            keyboardPlayerMenuEntry = new MenuEntry(string.Empty);
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            SetMenuEntryText();

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;
            numPlayersMenuEntry.Selected += NumPlayersMenuEntrySelected;
            keyboardPlayerMenuEntry.Selected += KeyboardPlayerMenuEntrySelected;
            creditsMenuEntry.Selected += CreditsMenuEntrySelected;
            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(numPlayersMenuEntry);
            MenuEntries.Add(keyboardPlayerMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion

        public override void LoadContent()
        {
            if (Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");

            menuSound = Content.Load<SoundEffect>("sound/menu sound");
            menuSoundLoop = menuSound.CreateInstance();
            menuSoundLoop.IsLooped = true;
            menuSoundLoop.Play();
        }

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
			game.initializeVariables();
            game.spawnInitialEntities();
            menuSoundLoop.Stop();
            game.gameSoundLoop.Play();
            if (keyboardPlayer != 4)
            {
                game.players[keyboardPlayer].keyboard = true;
            }
        }

        void SetMenuEntryText()
        {
            numPlayersMenuEntry.Text = "Number of Players: \"" + (currentNumPlayers + 1) + "\"";
            if (keyboardPlayer == 4)
            {
                keyboardPlayerMenuEntry.Text = "Using Keyboard: \"Nobody" + "\"";
            }
            else
            {
                keyboardPlayerMenuEntry.Text = "Using Keyboard: \"Player " + (keyboardPlayer+1) + "\"";
            }
            Static.NUM_PLAYERS = currentNumPlayers + 1;
            Static.KEYBOARD_PLAYER = keyboardPlayer;
        }

        void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CreditsMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the numplayers menu entry is selected.
        /// </summary>
        void NumPlayersMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentNumPlayers = (currentNumPlayers + 1) % 4;
            SetMenuEntryText();
        }

        void KeyboardPlayerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            keyboardPlayer = (keyboardPlayer + 1) % 5; //0,1,2,3,4
            SetMenuEntryText();
        }
        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
