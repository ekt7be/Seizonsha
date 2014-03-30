#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace GameName1
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry numPlayersMenuEntry;
        MenuEntry languageMenuEntry;
        MenuEntry elfMenuEntry;

        
        static int currentNumPlayers = 0;
        static int[] players = { 1, 2, 3, 4 };
        
        static string[] languages = { "English", "French"};
        static int currentLanguage = 0;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            numPlayersMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            numPlayersMenuEntry.Selected += NumPlayersMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;

            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(numPlayersMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            numPlayersMenuEntry.Text = "Number of Players: " + (currentNumPlayers+1);
            Static.NUM_PLAYERS = currentNumPlayers + 1;
            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void NumPlayersMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
/*            currentNumPlayers++;

            if (currentNumPlayers > 4)
                currentNumPlayers = 0;
            */

            currentNumPlayers = (currentNumPlayers + 1) %  players.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        /*void FrobnicateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            frobnicate = !frobnicate;

            SetMenuEntryText();
        }*/




        #endregion
    }
}
