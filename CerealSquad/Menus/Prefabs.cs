using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus
{
    public static class Prefabs
    {
        public static Menu MainMenu(SFML.Graphics.RenderWindow win, InputManager.InputManager manager)
        {
            Factories.FontFactory.FontFactory fontFactory = Factories.FontFactory.FontFactory.Instance;
            Menu mainMenu = new Menu(manager);

            Buttons.IButton btn_continue = new Buttons.OpenMenuButton("Continue", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 0, SettingsMenu(win, manager));
            MenuItem item_continue = new MenuItem(btn_continue);
            Buttons.IButton btn_newgame = new Buttons.BackButton("New Game", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 70, mainMenu);
            MenuItem item_newgame = new MenuItem(btn_newgame);
            Buttons.IButton btn_settings = new Buttons.OpenMenuButton("Settings", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 140, SettingsMenu(win, manager));
            MenuItem item_settings = new MenuItem(btn_settings);
            Buttons.IButton btn_credits = new Buttons.OpenMenuButton("Credits", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 210, SettingsMenu(win, manager));
            MenuItem item_credits = new MenuItem(btn_credits);
            Buttons.IButton btn_exit = new Buttons.ExitButton("Exit", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 280, win);
            MenuItem item_exit = new MenuItem(btn_exit);

            mainMenu.AddItem(item_continue);
            mainMenu.AddItem(item_newgame);
            mainMenu.AddItem(item_settings);
            mainMenu.AddItem(item_credits);
            mainMenu.AddItem(item_exit);

            mainMenu.Initialize();

            return mainMenu;
        }

        public static Menu SettingsMenu(SFML.Graphics.RenderWindow win, InputManager.InputManager manager)
        {
            Factories.FontFactory.FontFactory fontFactory = Factories.FontFactory.FontFactory.Instance;
            Menu settingsMenu = new Menu(manager);

            Buttons.IButton btn_back = new Buttons.BackButton("Back", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular), 0, settingsMenu);
            MenuItem item_back = new MenuItem(btn_back);

            settingsMenu.AddItem(item_back);

            settingsMenu.Initialize();

            return settingsMenu;
        }
    }
}
