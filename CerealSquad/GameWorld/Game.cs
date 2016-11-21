using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.GameWorld
{
    /// <summary>
    /// 
    /// </summary>
    class Game : Drawable
    {
        private AWorld currentWorld = null;
        public AWorld CurrentWorld {
            get { return currentWorld; }
            set { setCurrentWorld(value); }
        }
        //private HUD HUD;
        private List<AWorld> Worlds = new List<AWorld>();
        private List<IEntity> Players = new List<IEntity>();
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

            Menus.IntroCutscene intro = new Menus.IntroCutscene(_renderer, manager);
            intro.Ended += Intro_Ended;
            Menus.MenuManager.Instance.AddMenu(intro);
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
                        _player = new Mike(worldEntity, new s_position(5, 6, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                    case 1:
                        _player = new Jack(worldEntity, new s_position(5, 6, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                    case 2:
                        _player = new Orangina(worldEntity, new s_position(5, 6, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                    case 3:
                        _player = new Tchong(worldEntity, new s_position(6, 6, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                    default:
                        _player = new Mike(worldEntity, new s_position(5, 6, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId);
                        break;
                }
                Players.Add(_player);
                _HUDs.Add(new Graphics.HUD((uint)Players.Count, (uint)e.Players.Where(i => i.Type != Menus.Players.Type.Undefined).Count(), ref _player, renderer));
            }

            _InputManager.KeyboardKeyPressed += Im_KeyboardKeyPressed;
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

        public void Update(SFML.System.Time DeltaTime)
        {
            if (currentWorld != null)
            {
                currentWorld.Update(DeltaTime);
                worldEntity.update(DeltaTime, currentWorld);
            }
            int NbPlayersDead = Players.FindAll(x => x.Die).Count;
            if (NbPlayersDead > 0 && NbPlayersDead == Players.Count)
            {
                System.Threading.Thread.Sleep(1000);
                
                Menus.MenuManager.Instance.AddMenu(new Menus.GameOverMenu(renderer, _InputManager));
            }
            _HUDs.ForEach(i => i.Update(DeltaTime));
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
