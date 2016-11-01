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
        //private List<IEntity> Players;
        //private IEntity WorldEntity;
        private Renderer renderer = null;

        public Game(Renderer _renderer)
        {
            renderer = _renderer;
        }

        public void GameLoop()
        {
            CurrentWorld = new AWorld();

            CurrentWorld.AddRoom(new ARoom(new ARoom.s_MapPos(0, 0), "Maps/TestRoom.txt", ARoom.e_RoomType.FightRoom));
            CurrentWorld.AddRoom(new ARoom(new ARoom.s_MapPos(13, 0), "Maps/TestRoom2.txt", ARoom.e_RoomType.FightRoom));
        }

        public void AddWorld(AWorld World = null)
        {
            if (World == null)
                World = new AWorld();
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
    }
}
