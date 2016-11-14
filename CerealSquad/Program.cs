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
            // Debug Clock Watcher
            Debug.Time.Instance.DebugMode(Debug.Type.Info, true);
            Debug.Time.Instance.DebugMode(Debug.Type.Warning, true);
            Debug.Time.Instance.DebugMode(Debug.Type.Critical, true);
            Debug.Time.Instance.DebugMode(Debug.Type.Debug, true);

            Debug.Time.Instance.StartTimer("Main", Debug.Type.Debug, false);

            // Downloading Files
            Downloaders.IDownloader ftpDownloader = new Downloaders.FTPDownloader();
            // File requirement (Downloaded before start)
            System.Collections.Generic.List<System.Threading.Tasks.Task> tasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
            tasks.Add(ftpDownloader.RequireFile("IMG_LoadingBackground", "Assets/Loading/loading.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Loading/loading.png"), false));

            try
            {
                System.Threading.Tasks.Task.WaitAll(tasks.ToArray());
            }
            catch (System.AggregateException e)
            {
                System.Diagnostics.Debug.WriteLine("Downloader error ! " + e.InnerException.Message);
                return;
            }

            // File downloaded after start
            Downloaders.TaskAwaiter awaiter = new Downloaders.TaskAwaiter();

            awaiter.Add(ftpDownloader.RequireFile("testAsset", "Assets/Tiles/TestTile.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Tiles/TestTile.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("JackWalking", "Assets/Character/JackWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Character/JackWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("TchongWalking", "Assets/Character/ChongWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Character/ChongWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("jackHunter", "Assets/Character/JackHunter.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Character/JackHunter.png"), true));
            awaiter.Add(ftpDownloader.RequireFile("orangina", "Assets/Character/Orangina.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Character/Orangina.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("basicEnnemy", "Assets/Character/BasicEnnemy.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Character/BasicEnnemy.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("F_ReenieBeanie", "Fonts/ReenieBeanie.ttf", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Fonts/ReenieBeanie.ttf"), false));
            awaiter.Add(ftpDownloader.RequireFile("F_XirodRegular", "Fonts/xirod.regular.ttf", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Fonts/xirod.regular.ttf"), false));
            awaiter.Add(ftpDownloader.RequireFile("Bomb", "Assets/Trap/Bomb.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Trap/Bomb.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BombExploding", "Assets/Trap/BombExploading.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Trap/BombExploading.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BearTrap", "Assets/Trap/Beartrap.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Trap/Beartrap.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Cursor", "Assets/Effects/Cursor.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Effects/Cursor.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CS_LockedChar", "Assets/Debug/select_test.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Debug/select_test.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CS_SelectedChar", "Assets/Debug/unselect_test.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Debug/unselect_test.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Crates", "Assets/GameplayElement/Crates.png", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/GameplayElement/Crates.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CharacterSelect", "Assets/Music/CharacterSelection.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH_BACKUP + "Assets/Music/CharacterSelection.ogg"), false));

            // Initialisation
            renderer = new Renderer();
            renderer.Initialization();
            renderer.Resolution = Renderer.EResolution.R1920x1080;
            renderer.FrameRate = 60;

            InputManager.InputManager manager = new InputManager.InputManager(renderer);
            manager.KeyboardKeyPressed += Manager_KeyboardKeyPressed;

            GameWorld.GameManager gameManager = new GameWorld.GameManager(renderer, manager);

            Downloaders.LoadingScreen _loadingScreen = new Downloaders.LoadingScreen(renderer);

            FrameClock clock = new FrameClock();
            while (renderer.isOpen())
            {
                renderer.DispatchEvents();
                renderer.Clear(Color.Black);
                if (awaiter != null && awaiter.Status == Downloaders.TaskAwaiter.TaskStatus.Running)
                {
                    _loadingScreen.update(clock.Restart());
                    renderer.Draw(_loadingScreen);
                }
                else if (Menus.MenuManager.Instance.isDisplayed())
                    renderer.Draw(Menus.MenuManager.Instance.CurrentMenu);
                else if (gameManager.CurrentGame != null)
                {
                    gameManager.CurrentGame.Update(clock.Restart());
                    renderer.Draw(gameManager.CurrentGame.CurrentWorld);
                    gameManager.CurrentGame.WorldEntity.draw(renderer);
                }
                else if (awaiter.Status == Downloaders.TaskAwaiter.TaskStatus.Completed)
                {
                    System.Diagnostics.Debug.WriteLine("DOWNLOAD ENDED");
                    Debug.Time.Instance.StartTimer("DISPLAYING MAIN MENU", Debug.Type.Critical, true);
                    Menus.MenuManager.Instance.AddMenu(Menus.Prefabs.Instance.MainMenu(renderer, manager, gameManager));
                    Debug.Time.Instance.StopTimer("DISPLAYING MAIN MENU");
                    awaiter.Reset();
                }
                else
                    renderer.Win.Close();
                if (awaiter.Status == Downloaders.TaskAwaiter.TaskStatus.Faulted)
                {
                    System.Diagnostics.Debug.Fail("Error: " + awaiter.Exception.InnerException.Message);
                    renderer.Win.Close();
                }
                renderer.Display();
            }

            Debug.Time.Instance.StopTimer("Main");
        }
        
        private static void Manager_KeyboardKeyPressed(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (e.KeyCode.Equals(InputManager.Keyboard.Key.F))
                renderer.FullScreen = !renderer.FullScreen;
        }
    }
}
