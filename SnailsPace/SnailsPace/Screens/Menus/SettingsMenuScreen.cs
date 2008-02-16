using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using SnailsPace.Core;

namespace SnailsPace.Screens.Menus
{
    class SettingsMenuScreen : MenuScreen
    {
        public SettingsMenuScreen(SnailsPace game)
            : base(game)
        {
        }


		protected override string GetBackgroundImage()
		{
			return "Resources/Textures/SettingsScreen";
		}
		
		protected override void SetupMenuItems()
        {
			Input input = SnailsPace.inputManager;

            float itemY = spriteFont.LineSpacing;
            float itemX = 25.0f;
            menuItems = new MenuItem[7];
            menuItems[0] = new MenuItem("Back to Menu", this, new Vector2(itemX, itemY));
			menuItems[1] = new MenuItem("Move Up/Jetpack: " + input.getKeyBinding("Up"), this, new Vector2(itemX, itemY + spriteFont.LineSpacing * 2));
			menuItems[2] = new MenuItem("Move Left: " +input.getKeyBinding("Left"), this, new Vector2(itemX, itemY + spriteFont.LineSpacing * 4));
			menuItems[3] = new MenuItem("Move Down: " + input.getKeyBinding("Down"), this, new Vector2(itemX, itemY + spriteFont.LineSpacing * 6));
			menuItems[4] = new MenuItem("Move Right: " + input.getKeyBinding("Right"), this, new Vector2(itemX, itemY + spriteFont.LineSpacing * 8));
			menuItems[5] = new MenuItem("Fire: " + input.getKeyBinding("Fire"), this, new Vector2(itemX, itemY + spriteFont.LineSpacing * 10));
			menuItems[6] = new MenuItem("Pause: " + input.getKeyBinding("Pause"), this, new Vector2(itemX, itemY + spriteFont.LineSpacing * 12));
			menuItemIndex = 0;
            ready = true;
        }

        public override void Update(GameTime gameTime)
        {
            Input input = SnailsPace.inputManager;

            if (input.inputPressed("MenuToggle"))
            {
                snailsPace.changeState(SnailsPace.GameStates.MainMenu);
            }

            if (input.inputPressed("MenuSelect"))
            {
                switch (menuItemIndex)
                {
                    case 0:
                        snailsPace.changeState(SnailsPace.GameStates.MainMenu);
                        break;
                }
            }

            base.Update(gameTime);
        }
    }
}
