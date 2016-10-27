using System;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace CerealSquad
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        static void Main()
        {
            /*
            Renderer renderer = new Renderer();

            renderer.initialization();
            renderer.loop();
            */
            RenderWindow win = new RenderWindow(new VideoMode(800, 800), "Cereal Menu");
            InputManager.InputManager manager = new InputManager.InputManager(win);

            Menus.Menu mainMenu = Menus.Prefabs.MainMenu(win, manager);
            Menus.MenuManager.Instance.AddMenu(mainMenu);

            manager.KeyboardKeyPressed += Manager_KeyboardKeyPressed;

            mainMenu.Show();

            while (win.IsOpen)
            {
                win.DispatchEvents();
                win.Clear(Color.Magenta);
                if (Menus.MenuManager.Instance.isDisplayed())
                {
                    // Menus prior on game
                    Menus.MenuManager.Instance.CurrentMenu.Draw();
                }
                else
                {
                    // GameLogic
                    // game.update();
                }
                win.Display();
            }
        }

        private static void Manager_KeyboardKeyPressed(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (e.KeyCode.Equals(InputManager.Keyboard.Key.Escape))
                ((RenderWindow)source).Close();
        }
    }
}
