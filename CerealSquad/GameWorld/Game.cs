using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.GameWorld
{
    /// <summary>
    /// 
    /// </summary>
    class Game
    {
        private AWorld currentWorld = null;
        public AWorld CurrentWorld {
            get { return currentWorld; }
            set { setCurrentWorld(value); }
        }
        //private HUD HUD;
        private List<AWorld> Worlds = new List<AWorld>();
        private List<IEntity> Players = new List<IEntity>();
        private WorldEntity worldEntity = new WorldEntity();
        public WorldEntity WorldEntity
        {
            get { return worldEntity; }
        }
        private Renderer renderer = null;

        public Game(Renderer _renderer)
        {
            renderer = _renderer;
        }

        public void GameLoop(InputManager.InputManager im)
        {
            CurrentWorld = new AWorld("Maps/TestWorld.txt", worldEntity);
            //Players.Add(new Orangina(WorldEntity, new s_position(2, 2, 1), im));
            //Players.Add(new Jack(WorldEntity, new s_position(2, 4, 1), im));
            Players.Add(new Tchong(WorldEntity, new s_position(2, 4, 1), im));
            //new Ennemy(WorldEntity, new s_position(10, 10, 1));
            //new JackEnnemy(WorldEntity, new s_position(2, 10, 1));

            im.KeyboardKeyPressed += Im_KeyboardKeyPressed;
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

        public void Update(SFML.System.Time deltaTime)
        {
            worldEntity.update(deltaTime, currentWorld);
        }
    }
}
