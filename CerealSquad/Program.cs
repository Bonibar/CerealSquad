using System;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace CerealSquad
{
    static class Program
    {
        public static Renderer renderer;

        private static bool pause = false;
        private static double step = -1;
        private const double stepTime = 0.1;

        private static FrameClock clock = new FrameClock();

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        /// 
        static void Main()
        {
            // Debug Clock Watcher
#if DEBUG
            Debug.Time.Instance.DebugMode(Debug.Type.Info, true);
            Debug.Time.Instance.DebugMode(Debug.Type.Warning, true);
            Debug.Time.Instance.DebugMode(Debug.Type.Critical, true);
            Debug.Time.Instance.DebugMode(Debug.Type.Debug, true);
#endif

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
            awaiter.Add(ftpDownloader.RequireFile("MainMenuRafiki", "Assets/Background/RafikiBlack.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Background/RafikiBlack.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CutScene", "Assets/Background/cutscene_1024.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Background/cutscene_1024.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CutScene_Soundclip", "Assets/Music/intro_soundclip.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Music/intro_soundclip.ogg"), false));
            awaiter.Add(ftpDownloader.RequireFile("JackWalking", "Assets/Character/JackWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/JackWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("MikeWalking", "Assets/Character/MikeWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/MikeWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("OrangeHinaWalking", "Assets/Character/HinaWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/HinaWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("TchongWalking", "Assets/Character/ChongWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Character/ChongWalking.png"), false));

            // Tiles
            awaiter.Add(ftpDownloader.RequireFile("Tiles_Black", "Assets/Tiles/black.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/black.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("testAsset", "Assets/Tiles/TestTile.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/TestTile.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("testAsset2", "Assets/Tiles/TestTile2.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/TestTile2.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_CastleWall", "Assets/Tiles/CastleWall.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/CastleWall.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_Carpet", "Assets/Tiles/Carpet.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/Carpet.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_CastleGround", "Assets/Tiles/CastleGround.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/CastleGround.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_DefaultWall", "Assets/Tiles/DefaultWall.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/DefaultWall.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_KitchenTilesV1", "Assets/Tiles/KitchenTilesV1.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/KitchenTilesV1.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_KitchenTilesV2", "Assets/Tiles/KitchenTilesV2.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/KitchenTilesV2.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_KitchenTopWalls", "Assets/Tiles/KitchenTopWalls.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/KitchenTopWalls.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_KitchenWall", "Assets/Tiles/KitchenWall.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/KitchenWall.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_KitchenWindow", "Assets/Tiles/KitchenWindow.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/KitchenWindow.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_Paintings", "Assets/Tiles/Paintings.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/Paintings.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_SimpleWalls", "Assets/Tiles/SimpleWalls.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/SimpleWalls.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_TopWals", "Assets/Tiles/TopWalls.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/TopWalls.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Tiles_WallsWithWindow", "Assets/Tiles/WallsWithWindow.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Tiles/WallsWithWindow.png"), false));

            awaiter.Add(ftpDownloader.RequireFile("EggWalking", "Assets/Enemies/Normal/EggyWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/EggyWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("EggBreaking", "Assets/Enemies/Normal/EggyBreaking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/EggyBreaking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("HalfEggBreaking", "Assets/Enemies/Normal/HalfEggyWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/HalfEggyWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("MilkyGhost", "Assets/Enemies/Normal/MilkyGhost.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/MilkyGhost.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("RiceBowlWalking", "Assets/Enemies/Normal/RiceBowlWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/RiceBowlWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("RiceBowlFiring", "Assets/Enemies/Normal/RiceBowlFiring.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/RiceBowlFiring.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("Rice", "Assets/Enemies/Normal/Rice.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/Rice.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("GrilledToastWalking", "Assets/Enemies/Normal/GrilledToastWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/GrilledToastWalking.png"), false));

            awaiter.Add(ftpDownloader.RequireFile("HalfEggDying", "Assets/Enemies/Normal/Death/HalfEggyDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/Death/HalfEggyDying.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("MilkyGhostDying", "Assets/Enemies/Normal/Death/MilkyGhostDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/Death/MilkyGhostDying.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("RiceBowlDying", "Assets/Enemies/Normal/Death/RiceBowlDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/Death/RiceBowlDying.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("GrilledToastDying", "Assets/Enemies/Normal/Death/GrilledToastDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Normal/Death/GrilledToastDying.png"), false));

            awaiter.Add(ftpDownloader.RequireFile("CoffeeStaying", "Assets/Enemies/Boss/CoffeeStaying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeStaying.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CoffeeSpreading", "Assets/Enemies/Boss/CoffeeSpreading.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeSpreading.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CoffeeThrowed", "Assets/Enemies/Boss/CoffeeThrowed.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeThrowed.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CoffeeMachineWalking", "Assets/Enemies/Boss/CoffeeMachineWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeMachineWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CoffeeMachineMidWalking", "Assets/Enemies/Boss/CoffeeMachineMidWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeMachineMidWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CoffeeMachineEmptyWalking", "Assets/Enemies/Boss/CoffeeMachineEmptyWalking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeMachineEmptyWalking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CoffeeMachineDying", "Assets/Enemies/Boss/CoffeeMachineDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeMachineDying.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CoffeeMachineToMid", "Assets/Enemies/Boss/CoffeeMachineToMid.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeMachineToMid.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("CoffeeMachineToEmpty", "Assets/Enemies/Boss/CoffeeMachineToEmpty.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeMachineToEmpty.png"), false));           
            awaiter.Add(ftpDownloader.RequireFile("CoffeeMachineThrowing", "Assets/Enemies/Boss/CoffeeMachineThrowingCoffee.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/CoffeeThrowed.png"), false));

            awaiter.Add(ftpDownloader.RequireFile("BaggyHiding", "Assets/Enemies/Boss/BaggyHiding.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/BaggyHiding.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BaggyPhase1toPhase2", "Assets/Enemies/Boss/BaggyPhase1toPhase2.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/BaggyPhase1toPhase2.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BaggyPhase1Walking", "Assets/Enemies/Boss/BaggyPhase1Walking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/BaggyPhase1Walking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BaggyPhase2Walking", "Assets/Enemies/Boss/BaggyPhase2Walking.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/BaggyPhase2Walking.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BaggySummoning", "Assets/Enemies/Boss/BaggySummoning.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/BaggySummoning.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BaggyDying", "Assets/Enemies/Boss/BaggyDying.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/BaggyDying.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("BaggyProjectile", "Assets/Enemies/Boss/BaggyProjectile.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Enemies/Boss/BaggyProjectile.png"), false));

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
            awaiter.Add(ftpDownloader.RequireFile("BlockRoom", "Assets/GameplayElement/Fire.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/GameplayElement/Fire.png"), false));
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
            awaiter.Add(ftpDownloader.RequireFile("HUD_LifeBar", "Assets/HUD/LifeBar.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/HUD/LifeBar.png"), false));
            awaiter.Add(ftpDownloader.RequireFile("HUD_LifeBarCounter", "Assets/HUD/LifeCounter.png", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/HUD/LifeCounter.png"), false));

            //Sounds
            awaiter.Add(ftpDownloader.RequireFile("Sound_StoryBegin", "Assets/Sound/And_so_the_story_begins.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Sound/And_so_the_story_begins.ogg"), false));
            awaiter.Add(ftpDownloader.RequireFile("Sound_CerealsHelp", "Assets/Sound/Cereals_might_help_you.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Sound/Cereals_might_help_you.ogg"), false));
            
            awaiter.Add(ftpDownloader.RequireFile("Sound_BearTrap", "Assets/Sound/BearTrap.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Sound/BearTrap.ogg"), false));
            awaiter.Add(ftpDownloader.RequireFile("Sound_Construction", "Assets/Sound/Construction.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Sound/Construction.ogg"), false));
            awaiter.Add(ftpDownloader.RequireFile("Sound_Explosion", "Assets/Sound/Explosion.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Sound/Explosion.ogg"), false));

            awaiter.Add(ftpDownloader.RequireFile("Sound_CrackingEggs", "Assets/Sound/CrackingEggs.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Sound/CrackingEggs.ogg"), false));
            awaiter.Add(ftpDownloader.RequireFile("Sound_ghost", "Assets/Sound/ghost.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Sound/ghost.ogg"), false));
            awaiter.Add(ftpDownloader.RequireFile("Sound_SugarWallSound", "Assets/Sound/SugarWallLowSound.ogg", new Uri(Downloaders.FTPDownloader.FTP_PATH + "Assets/Sound/SugarWallLowSound.ogg"), false));

            // Initialisation
            renderer = new Renderer();
            renderer.Initialization();
            renderer.Resolution = Renderer.EResolution.R854x480;
            renderer.FrameRate = 60;

            InputManager.InputManager manager = new InputManager.InputManager(renderer);
            manager.ResetKeyMaps();
            manager.KeyboardKeyPressed += Manager_KeyboardKeyPressed;

            GameWorld.GameManager gameManager = new GameWorld.GameManager(renderer, manager);

            Downloaders.LoadingScreen _loadingScreen = new Downloaders.LoadingScreen(renderer);
            
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
                    SFML.System.Time time = clock.Restart();
                    if (!pause)
                        gameManager.Update(time);
                    if (step > 0)
                    {
                        step -= time.AsSeconds();
                        if (step < 0)
                        {
                            pause = true;
                            step = -1;
                        }
                    }
                    if (gameManager.CurrentGame != null)
                        renderer.Draw(gameManager.CurrentGame);
                }
                else if (awaiter.Status == Downloaders.TaskAwaiter.TaskStatus.Completed || awaiter.Status == Downloaders.TaskAwaiter.TaskStatus.Empty)
                {
                    if (awaiter.Status == Downloaders.TaskAwaiter.TaskStatus.Completed)
                        Factories.SoundBufferFactory.Instance.initSoundBuffer();
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
            if (e.KeyCode.Equals(InputManager.Keyboard.Key.P))
            {
                clock.Restart();
                pause = !pause;
            }
            if (e.KeyCode.Equals(InputManager.Keyboard.Key.O) && pause)
            {
                clock.Restart();
                pause = false;
                step = stepTime;
            }
        }
    }
}
