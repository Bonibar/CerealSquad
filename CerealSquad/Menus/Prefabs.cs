using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus
{
    class Prefabs
    {
        #region Singleton
        private Prefabs() { }

        public static Prefabs Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested() { }

            internal static readonly Prefabs instance = new Prefabs();
        }
        #endregion

        public Menu MainMenu(Renderer renderer, InputManager.InputManager manager, GameWorld.GameManager gameManager)
        {
            Factories.FontFactory.FontFactory fontFactory = Factories.FontFactory.FontFactory.Instance;
            Menu mainMenu = new Menu(manager);

            Buttons.IButton btn_continue = new Buttons.OpenMenuButton("Continue", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 0, SettingsMenu(manager));
            MenuItem item_continue = new MenuItem(btn_continue, MenuItem.ItemType.Disabled);
            Buttons.IButton btn_newgame = new Buttons.NewGameButton("New Game", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 70, gameManager);
            MenuItem item_newgame = new MenuItem(btn_newgame);
            Buttons.IButton btn_settings = new Buttons.OpenMenuButton("Settings", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 140, SettingsMenu(manager));
            MenuItem item_settings = new MenuItem(btn_settings, MenuItem.ItemType.Disabled);
            Buttons.IButton btn_credits = new Buttons.OpenMenuButton("Credits", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 210, SettingsMenu(manager));
            MenuItem item_credits = new MenuItem(btn_credits, MenuItem.ItemType.Disabled);
            Buttons.IButton btn_exit = new Buttons.ExitButton("Exit", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 280, renderer);
            MenuItem item_exit = new MenuItem(btn_exit);

            mainMenu.AddItem(item_continue);
            mainMenu.AddItem(item_newgame);
            mainMenu.AddItem(item_settings);
            mainMenu.AddItem(item_credits);
            mainMenu.AddItem(item_exit);

            mainMenu.Initialize();

            return mainMenu;
        }

        public Menu SettingsMenu(InputManager.InputManager manager)
        {
            Factories.FontFactory.FontFactory fontFactory = Factories.FontFactory.FontFactory.Instance;
            Menu settingsMenu = new Menu(manager);

            Buttons.IButton btn_back = new Buttons.BackButton("Back", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular), 0, settingsMenu);
            MenuItem item_back = new MenuItem(btn_back);

            settingsMenu.AddItem(item_back);

            settingsMenu.Initialize();

            return settingsMenu;
        }

        public Menu CharacterSelectMenu(Renderer renderer, InputManager.InputManager manager)
        {
            Menu characterMenu = new CharacterSelectMenu(renderer, manager);

            return characterMenu;
        }
    }
}
