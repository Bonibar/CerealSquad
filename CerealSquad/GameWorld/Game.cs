using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using CerealSquad.EntitySystem;

namespace CerealSquad.GameWorld
{
    /// <summary>
    /// 
    /// </summary>
    class Game : Drawable
    {
        public static int STARTING_HP = 10;

        public enum GameState
        {
            Running,
            Exit
        }

        public GameState State { get; private set; }
        public int HP { get; protected set; }

        private AWorld currentWorld = null;
        public AWorld CurrentWorld {
            get { return currentWorld; }
            set { setCurrentWorld(value); }
        }
        //private HUD HUD;
        private List<AWorld> Worlds = new List<AWorld>();
        private List<APlayer> Players = new List<APlayer>();
        private List<Graphics.HUD> _HUDs = new List<Graphics.HUD>();
        private WorldEntity worldEntity = new WorldEntity();
        public WorldEntity WorldEntity
        {
            get { return worldEntity; }
        }
        private Renderer renderer = null;
        private InputManager.InputManager _InputManager = null;

        public Game(Renderer _renderer, InputManager.InputManager manager)
        {
            if (_renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");
            if (manager == null)
                throw new ArgumentNullException("Input Manager cannot be null");

            renderer = _renderer;
            _InputManager = manager;

            State = GameState.Running;
            HP = STARTING_HP;

#if !DEBUG
            Menus.IntroCutscene intro = new Menus.IntroCutscene(_renderer, manager);
            intro.Ended += Intro_Ended;
            Menus.MenuManager.Instance.AddMenu(intro);
#else
            characterSelection();
#endif
        }

        private void Intro_Ended(object source, Menus.IntroCutscene.CutsceneEventArgs e)
        {
            Menus.MenuManager.Instance.Clear();
            characterSelection();
        }

        private void characterSelection()
        {
            Menus.CharacterSelectMenu _current = new Menus.CharacterSelectMenu(renderer, _InputManager);
            Menus.MenuManager.Instance.AddMenu(_current);
            _current.GameStart += _current_GameStart;
            _current.MenuExit += _current_MenuExit;
        }

        private void _current_MenuExit(object source, Menus.CharacterSelectMenu.CharacterSelectionArgs e)
        {
            System.Diagnostics.Debug.WriteLine("RECIEVED FIRE");
            Menus.MenuManager.Instance.RemoveMenu((Menus.Menu)source);
            State = GameState.Exit;
        }
        private void _current_GameStart(object source, Menus.CharacterSelectMenu.CharacterSelectionArgs e)
        {
            CurrentWorld = new AWorld("Maps/TestWorld.txt", worldEntity);
            Worlds.Add(CurrentWorld);

            foreach (Menus.Players.Player player in e.Players.Where(i => i.Type != Menus.Players.Type.Undefined))
            {
                APlayer _player;
                switch (player.Selection)
                {
                    case 0:
                        _player = new Mike(worldEntity, new s_position(1, 5, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                    case 1:
                        _player = new Jack(worldEntity, new s_position(1, 5, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                    case 2:
                        _player = new Orangina(worldEntity, new s_position(1, 5, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                    case 3:
                        _player = new Tchong(worldEntity, new s_position(1, 5, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                    default:
                        _player = new Mike(worldEntity, new s_position(1, 5, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                }
                Players.Add(_player);
                _HUDs.Add(new Graphics.HUD((uint)Players.Count, (uint)e.Players.Where(i => i.Type != Menus.Players.Type.Undefined).Count(), ref _player, renderer));
            }

            _InputManager.KeyboardKeyPressed += Im_KeyboardKeyPressed;
            WorldEntity.PlayerNumber = Players.Count;
        }

        private void Im_KeyboardKeyPressed(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (!Menus.MenuManager.Instance.isDisplayed()) // Are we on game
            {
                if (e.KeyCode == InputManager.Keyboard.Key.Escape)
                    renderer.Win.Close();
            }
        }

        public void AddWorld(AWorld World)
        {
            if (World == null)
                throw new ArgumentNullException("World cannot be null");
            Worlds.Add(World);
        }

        public void goToNextWorld()
        {
            var index = Worlds.FindIndex(a => a == CurrentWorld);
            CurrentWorld = Worlds.ElementAt(index + 1);
        }

        public List<AWorld> getWorlds()
        {
            return (Worlds);
        }

        private void setCurrentWorld(AWorld World)
        {
            currentWorld = World;
        }

        private void checkPlayers()
        {
            foreach (APlayer player in Players)
            {
                if (player.State == APlayer.PlayerState.Respawn)
                {
                    HP--;
                    if (HP > 0)
                        player.Respawn();
                    else
                        player.GameOver();
                }
            }
        }

        public void Update(SFML.System.Time DeltaTime)
        {
            System.Diagnostics.Debug.WriteLine("HP: " + HP);
            if (currentWorld != null)
            {
                if (currentWorld.CurrentRoom != null && currentWorld.CurrentRoom.RoomType == ARoom.e_RoomType.VictoryRoom)
                {
                    State = GameState.Exit;
                    Menus.MenuManager.Instance.AddMenu(new Menus.VictoryMenu(renderer, _InputManager));
                }
                checkPlayers();
                currentWorld.Update(DeltaTime, Players);
                worldEntity.update(DeltaTime, currentWorld);
                if (currentWorld.CurrentRoom != null)
                {
                    APlayer _target = Players.First();
                    SFML.System.Vector2f screenSize = renderer.Win.GetView().Size;
                    SFML.System.Vector2f cameraOrigin = renderer.Win.MapPixelToCoords(new SFML.System.Vector2i(0, 0));
                    float xToTranslate = (currentWorld.CurrentRoom.Position.X * 64 - screenSize.X / 2 + currentWorld.CurrentRoom.Size.Width * 32 - cameraOrigin.X) / (currentWorld.CurrentRoom.Size.Width * 7);
                    float yToTranslate = (currentWorld.CurrentRoom.Position.Y * 64 - screenSize.Y / 2 + currentWorld.CurrentRoom.Size.Height * 32 - cameraOrigin.Y) / (currentWorld.CurrentRoom.Size.Height * 7);
                    if (xToTranslate != 0)
                        renderer.Move(xToTranslate, 0);
                    if (yToTranslate != 0)
                        renderer.Move(0, yToTranslate);
                }
            }
            _HUDs.ForEach(i => i.Update(DeltaTime));
            int NbPlayersDead = Players.Count(x => x.State == APlayer.PlayerState.Destroy);
            if (NbPlayersDead > 0 && NbPlayersDead == Players.Count)
            {
                List<Graphics.AnimatedSprite> _deathAnimations = Players.Where(i => i.ressourcesEntity.sprite.Type == Graphics.ETypeSprite.ANIMATED).
                    Select(i => (Graphics.AnimatedSprite)i.ressourcesEntity.sprite)
                    .ToList();
                if (_deathAnimations.Count(i => i.isFinished()) == _deathAnimations.Count)
                {
                    State = GameState.Exit;
                    Menus.MenuManager.Instance.AddMenu(new Menus.GameOverMenu(renderer, _InputManager));
                }
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            // Draw world
            if (CurrentWorld != null)
                target.Draw(CurrentWorld, states);

            // Draw entities
            if (worldEntity != null)
                target.Draw(worldEntity, states);

            // Draw HUD
            _HUDs.ForEach(i => target.Draw(i, states));
        }
    }
}
