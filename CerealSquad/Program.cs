using System;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace CerealSquad
{
    static class Program
    {
        public static Renderer renderer;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        /// 
        static void Main()
        {
            // File requirement
            System.Collections.Generic.List<System.Threading.Tasks.Task> tasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
            Downloaders.IDownloader ftpDownloader = new Downloaders.FTPDownloader();
            tasks.Add(ftpDownloader.RequireFile("testAsset", "Assets/Tiles/TestTile.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/alts.png"), false));
            tasks.Add(ftpDownloader.RequireFile("JackWalking", "Assets/Character/JackWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Characters/JackWalking.png"), false));
            tasks.Add(ftpDownloader.RequireFile("TchongWalking", "Assets/Character/ChongWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Characters/ChongWalking.png"), false));
            tasks.Add(ftpDownloader.RequireFile("jackHunter", "Assets/Character/JackHunter.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Characters/JackHunter.png"), false));
            tasks.Add(ftpDownloader.RequireFile("orangina", "Assets/Character/Orangina.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Characters/Orangina.png"), false));
            tasks.Add(ftpDownloader.RequireFile("basicEnnemy", "Assets/Character/BasicEnnemy.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Characters/BasicEnnemy.png"), false));
            tasks.Add(ftpDownloader.RequireFile("F_ReenieBeanie", "Fonts/ReenieBeanie.ttf", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Fonts/ReenieBeanie.ttf"), false));
            tasks.Add(ftpDownloader.RequireFile("F_XirodRegular", "Fonts/xirod.regular.ttf", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Fonts/xirod.regular.ttf"), false));
            tasks.Add(ftpDownloader.RequireFile("Bomb", "Assets/Trap/Bomb.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/GameplayElement/Bomb.png"), false));
            tasks.Add(ftpDownloader.RequireFile("BombExploding", "Assets/Trap/BombExploading.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/GameplayElement/BombExploading.png"), false));
            tasks.Add(ftpDownloader.RequireFile("BearTrap", "Assets/Trap/Beartrap.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/GameplayElement/Beartrap.png"), false));
            tasks.Add(ftpDownloader.RequireFile("Cursor", "Assets/Effects/Cursor.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Effects/Cursor.png"), false));
            tasks.Add(ftpDownloader.RequireFile("CS_UnselectedChar", "Assets/Debug/select_test.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Debug/select_test.png"), false));
            tasks.Add(ftpDownloader.RequireFile("CS_SelectedChar", "Assets/Debug/unselect_test.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Debug/unselect_test.png"), false));
            tasks.Add(ftpDownloader.RequireFile("Crates", "Assets/GameplayElement/Crates.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/GameplayElement/Crates.png"), false));

            try
            {
                System.Threading.Tasks.Task.WaitAll(tasks.ToArray());
            } catch (System.AggregateException e)
            {
                System.Diagnostics.Debug.WriteLine("Downloader error ! " + e.InnerException.Message);
                return;
            }

            renderer = new Renderer();
            renderer.Initialization();
            renderer.Resolution = Renderer.EResolution.R1920x1080;
            renderer.FrameRate = 350;

            Factories.TextureFactory.Instance.initTextures();

            InputManager.InputManager manager = new InputManager.InputManager(renderer);
            manager.KeyboardKeyPressed += Manager_KeyboardKeyPressed;

            GameWorld.GameManager gameManager = new GameWorld.GameManager(renderer, manager);

            Menus.MenuManager.Instance.AddMenu(Menus.Prefabs.Instance.MainMenu(renderer, manager, gameManager));

            FrameClock clock = new FrameClock();
            while (renderer.isOpen())
            {
                renderer.DispatchEvents();
                renderer.Clear(Color.Black);
                if (Menus.MenuManager.Instance.isDisplayed())
                    renderer.Draw(Menus.MenuManager.Instance.CurrentMenu);
                else if (gameManager.CurrentGame != null)
                {
                    gameManager.CurrentGame.Update(clock.Restart());
                    renderer.Draw(gameManager.CurrentGame.CurrentWorld);
                    gameManager.CurrentGame.WorldEntity.draw(renderer);
                }
                else
                    renderer.Win.Close();
                renderer.Display();
            }
        }
        
        private static void Manager_KeyboardKeyPressed(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (e.KeyCode.Equals(InputManager.Keyboard.Key.F))
                renderer.FullScreen = !renderer.FullScreen;
        }
    }
}
