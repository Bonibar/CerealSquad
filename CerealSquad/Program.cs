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
            tasks.Add(ftpDownloader.RequireFile("IMG_LoadingBackground", "Assets/Loading/loading.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Loading/loading.png"), false));

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

            awaiter.Add(ftpDownloader.RequireFile("MainMenuBackground", "Assets/Background/MainMenuBackground.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Background/MainMenuBackground.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CutScene", "Assets/Background/cutscene_1024.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Background/cutscene_1024.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("testAsset", "Assets/Tiles/TestTile.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/TestTile.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_CastleWall", "Assets/Tiles/CastleWall.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/CastleWall.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("JackWalking", "Assets/Character/JackWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/JackWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("MikeWalking", "Assets/Character/MikeWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/MikeWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("OrangeHinaWalking", "Assets/Character/HinaWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/HinaWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("TchongWalking", "Assets/Character/ChongWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/ChongWalking.png"), false));
           
            awaiter.Add(ftpDownloader.RequireFile("EggWalking", "Assets/Enemies/Normal/EggyWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/EggyWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("EggBreaking", "Assets/Enemies/Normal/EggyBreaking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/EggyBreaking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("HalfEggBreaking", "Assets/Enemies/Normal/HalfEggyWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/HalfEggyWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("MilkyGhost", "Assets/Enemies/Normal/MilkyGhost.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/MilkyGhost.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("RiceBowlWalking", "Assets/Enemies/Normal/RiceBowlWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/RiceBowlWalking.png"), false));

            awaiter.Add(ftpDownloader.RequireFile("CoffeeMachineWalking", "Assets/Enemies/Boss/CoffeeMachineWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeMachineWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CoffeeMachineThrowing", "Assets/Enemies/Boss/CoffeeMachineThrowingCoffee.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeMachineThrowingCoffee.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Rice", "Assets/Enemies/Normal/Rice.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/Rice.png"), false));

            awaiter.Add(ftpDownloader.RequireFile("Death_Hina", "Assets/Character/Death/HinaDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/Death/HinaDying.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Death_Jack", "Assets/Character/Death/JackDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/Death/JackDying.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Death_Mike", "Assets/Character/Death/MikeDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/Death/MikeDying.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Death_Tchong", "Assets/Character/Death/TchongDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/Death/TchongDying.png"), false));

            awaiter.Add(ftpDownloader.RequireFile("jackHunter", "Assets/Character/JackHunter.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/JackHunter.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("orangina", "Assets/Character/Orangina.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/Orangina.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("basicEnnemy", "Assets/Character/BasicEnnemy.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/BasicEnnemy.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("F_ReenieBeanie", "Fonts/ReenieBeanie.ttf", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Fonts/ReenieBeanie.ttf"), false));
            awaiter.Add(ftpDownloader.RequireFile("F_XirodRegular", "Fonts/xirod.regular.ttf", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Fonts/xirod.regular.ttf"), false));
            awaiter.Add(ftpDownloader.RequireFile("Bomb", "Assets/Trap/Bomb.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Trap/Bomb.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BombExploding", "Assets/Trap/BombExploading.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Trap/BombExploading.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("SugarWall", "Assets/Trap/SugarWall.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Trap/SugarWall.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BearTrap", "Assets/Trap/Beartrap.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Trap/Beartrap.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Cursor", "Assets/Effects/Cursor.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Effects/Cursor.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CS_LockedChar", "Assets/Debug/select_test.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Debug/select_test.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CS_SelectedChar", "Assets/Debug/unselect_test.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Debug/unselect_test.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Crates", "Assets/GameplayElement/Crates.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/GameplayElement/Crates.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CratesOpening", "Assets/GameplayElement/CratesOpening.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/GameplayElement/CratesOpening.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("ConstructionCloud", "Assets/GameplayElement/ConstructionCloud.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/GameplayElement/ConstructionCloud.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_BackgroundMusic", "Assets/Music/CharacterSelection.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Music/CharacterSelection.ogg"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_BackgroundImage", "Assets/Background/CharacterSelection.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Background/CharacterSelectionBackground.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_Mike", "Assets/Character/Selection/MikeSelection.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/Selection/MikeSelection.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_Jack", "Assets/Character/Selection/JackSelection.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/Selection/JackSelection.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_Hina", "Assets/Character/Selection/HinaSelection.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/Selection/HinaSelection.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_Tchong", "Assets/Character/Selection/TchongSelection.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/Selection/TchongSelection.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_Fork", "Assets/HUD/Fork.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/HUD/Fork.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_Knife", "Assets/HUD/Knife.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/HUD/Knife.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_PlayerOverlay", "Assets/HUD/SelectionPlayerOverlay.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/HUD/SelectionPlayerOverlay.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("S_CS_PlayerCursor", "Assets/HUD/SelectionPlayerCursor.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/HUD/SelectionPlayerCursor.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("HUD_PlayerCharacter", "Assets/HUD/PlayerCharacterHUD.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/HUD/PlayerCharacterHUD.png"), false));

            // Initialisation
            renderer = new Renderer();
            renderer.Initialization();
            renderer.Resolution = Renderer.EResolution.R854x480;
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
                {
                    Menus.MenuManager.Instance.Update(clock.Restart());
                    renderer.Draw(Menus.MenuManager.Instance.CurrentMenu);
                }
                else if (gameManager.CurrentGame != null)
                {
                    gameManager.CurrentGame.Update(clock.Restart());
                    renderer.Draw(gameManager.CurrentGame);
                }
                else if (awaiter.Status == Downloaders.TaskAwaiter.TaskStatus.Completed)
                {
                    System.Diagnostics.Debug.WriteLine("DOWNLOAD ENDED");
                    Debug.Time.Instance.StartTimer("DISPLAYING MAIN MENU", Debug.Type.Critical, true);
                    Menus.MenuManager.Instance.AddMenu(new Menus.MainMenu(renderer, manager, gameManager));
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
