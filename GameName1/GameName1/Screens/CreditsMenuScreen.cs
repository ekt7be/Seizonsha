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
    class CreditsMenuScreen : MenuScreen
    {
        #region Fields
        #endregion
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public CreditsMenuScreen()
            : base("Credits")
        {
            // Create our menu entries.
            MenuEntry soundMenuEntry = new MenuEntry("Sound: PlayOnLoop.com");
            MenuEntry mentorMenuEntry = new MenuEntry("Mentor: Mark Sherriff");
            MenuEntry back = new MenuEntry("Back");
 
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(soundMenuEntry);
            MenuEntries.Add(mentorMenuEntry);
            MenuEntries.Add(back);
        }

        #endregion

        #region Handle Input

        #endregion
    }
}
