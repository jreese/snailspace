using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using SnailsPace.Graphics;
using SnailsPace.Input;

namespace SnailsPace.Screens.Menus
{
    class SettingsMenuScreen : MenuScreen
    {
        public SettingsMenuScreen(SnailsPace game)
            : base(game)
        {
        }

        protected override void SetupMenuItems()
        {
            float itemY = spriteFont.LineSpacing;
            float itemX = 25.0f;
            menuItems = new MenuItem[1];
            menuItems[0] = new MenuItem("Back", this, new Vector2(itemX, itemY), new ActionMapping.KeyAction(snailsPace.goToMainMenu));
            menuItemIndex = 0;
            ready = true;
        }
    }
}
