using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using CerealSquad.EntitySystem;
using CerealSquad.Sounds;

namespace CerealSquad.GameWorld
{
    /// <summary>
    /// 
    /// </summary>
    class Game : Drawable
    {
        public static int STARTING_HP = 4;

        public enum GameState
        {
            Running,
            Exit
        }

        public GameState State { get; private set; }

        private int _HP;
        public int HP { get { return _HP; } protected set { _HP = value; updateHPAnimation(); } }
        private Graphics.AnimatedSprite _LifeBar;
        private Graphics.AnimatedSprite _LifeCounter;
        private Text _LifeCounterText;

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

        private void updateHPAnimation()
        {
            _LifeCounterText = new Text(HP.ToString(), Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));

            int cap = STARTING_HP / 4;

            if (HP <= 0)
            {
                if (_LifeBar.Animation != 4)
                    _LifeBar.PlayAnimation(4);
            }
            else if (HP <= cap)
            {
                if (_LifeBar.Animation != 3)
                    _LifeBar.PlayAnimation(3);
            }
            else if (HP <= 2 * cap)
            {
                if (_LifeBar.Animation != 2)
                    _LifeBar.PlayAnimation(2);
            }
            else if (HP <= 3 * cap)
            {
                if (_LifeBar.Animation != 1)
                    _LifeBar.PlayAnimation(1);
            }
        }

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

            Factories.TextureFactory.Instance.load("HUD_LifeBar", "Assets/HUD/LifeBar.png");
            _LifeBar = new Graphics.AnimatedSprite((uint)(256 * (int)renderer.Win.GetView().Size.X / 1980), (uint)(256 * (int)renderer.Win.GetView().Size.X / 1980));
            _LifeBar.addAnimation(0, "HUD_LifeBar", new List<uint> { 0 }, new SFML.System.Vector2u(64, 64));
            _LifeBar.addAnimation(1, "HUD_LifeBar", Enumerable.Range(1, 9).Select(i => (uint)i).ToList(), new SFML.System.Vector2u(64, 64));
            _LifeBar.addAnimation(2, "HUD_LifeBar", Enumerable.Range(10, 8).Select(i => (uint)i).ToList(), new SFML.System.Vector2u(64, 64));
            _LifeBar.addAnimation(3, "HUD_LifeBar", Enumerable.Range(18, 7).Select(i => (uint)i).ToList(), new SFML.System.Vector2u(64, 64));
            _LifeBar.addAnimation(4, "HUD_LifeBar", Enumerable.Range(25, 22).Select(i => (uint)i).ToList(), new SFML.System.Vector2u(64, 64), 100);
            _LifeBar.Loop = false;

            Factories.TextureFactory.Instance.load("HUD_LifeCounter", "Assets/HUD/LifeCounter.png");
            _LifeCounter = new Graphics.AnimatedSprite((uint)(64 * (int)renderer.Win.GetView().Size.X / 1980), (uint)(64 * (int)renderer.Win.GetView().Size.X / 1980));
            _LifeCounter.addAnimation(0, "HUD_LifeCounter", new List<uint> { 0 }, new SFML.System.Vector2u(32, 32));
            _LifeCounter.Loop = true;

            _LifeCounterText = new Text(HP.ToString(), Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));
            _LifeCounterText.CharacterSize = 30 * (uint)renderer.Win.GetView().Size.X / 1980;

#if !DEBUG
            Menus.IntroCutscene intro = new Menus.IntroCutscene(_renderer, manager);
            intro.Ended += Intro_Ended;
            Menus.MenuManager.Instance.AddMenu(intro);
#else
            characterSelection();
#endif
            JukeBox.Instance.loadMusic("Music_PlansinMotion", "Assets/Sound/PlansinMotion.ogg");
            JukeBox.Instance.SetVolumeMusic("Music_PlansinMotion", 7f);
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
            CurrentWorld = new AWorld("Maps/TutorialWorld.txt", worldEntity);
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
            JukeBox.Instance.PlayMusic("Music_PlansinMotion", true);
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
            SFML.System.Vector2f cameraOrigin = renderer.Win.MapPixelToCoords(new SFML.System.Vector2i(0, 0));
            _LifeBar.Position = new SFML.System.Vector2f(renderer.Win.GetView().Size.X / 2 + cameraOrigin.X, _LifeBar.Size.Y / 2 + cameraOrigin.Y);
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
                    float xToTranslate = (currentWorld.CurrentRoom.Position.X * 64 - screenSize.X / 2 + currentWorld.CurrentRoom.Size.Width * 32 - cameraOrigin.X) / (currentWorld.CurrentRoom.Size.Width * 5);
                    float yToTranslate = (currentWorld.CurrentRoom.Position.Y * 64 - screenSize.Y / 2 + currentWorld.CurrentRoom.Size.Height * 32 - cameraOrigin.Y) / (currentWorld.CurrentRoom.Size.Height * 5);
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
                if (_deathAnimations.Count(i => i.isFinished()) == _deathAnimations.Count && _LifeBar.Animation == 4 && _LifeBar.isFinished())
                {
                    State = GameState.Exit;
                    Menus.MenuManager.Instance.AddMenu(new Menus.GameOverMenu(renderer, _InputManager));
                }
            }
            cameraOrigin = renderer.Win.MapPixelToCoords(new SFML.System.Vector2i(0, 0));
            _LifeBar.Position = new SFML.System.Vector2f(renderer.Win.GetView().Size.X / 2 + cameraOrigin.X, 0 + cameraOrigin.Y + _LifeBar.Size.Y / 2);
            _LifeBar.Update(DeltaTime);
            _LifeCounter.Position = new SFML.System.Vector2f(renderer.Win.GetView().Size.X / 2 + cameraOrigin.X - _LifeBar.Size.X / 2, 0 + cameraOrigin.Y + _LifeCounter.Size.Y / 2);
            _LifeCounter.Update(DeltaTime);
            _LifeCounterText.Position = new SFML.System.Vector2f(renderer.Win.GetView().Size.X / 2 + cameraOrigin.X - _LifeBar.Size.X / 2 - _LifeCounter.Size.X / 4, 0 + cameraOrigin.Y + 2 * _LifeCounter.Size.Y / 5 - (_LifeCounterText.GetLocalBounds().Height + _LifeCounterText.GetLocalBounds().Top) / 2);
            if (State == GameState.Exit)
                JukeBox.Instance.StopMusic("Music_PlansinMotion");
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

            // Draw HP Bar
            target.Draw(_LifeBar, states);
            target.Draw(_LifeCounter, states);
            target.Draw(_LifeCounterText, states);
        }
    }
}
