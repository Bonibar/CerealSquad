using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class Game
    {
        private AWorld CurrentWorld = null;
        //private HUD HUD;
        private List<AWorld> Worlds = new List<AWorld>();
        //private List<IEntity> Players;
        //private IEntity WorldEntity;

        public Game(/*IRenderer Renderer*/)
        {
        }

        public void GameLoop()
        {
            /* test 
            World World = new World();
            for (int i = 0; i < 5; i++)
                World.AddRoom(0);
            AddWorld(World);
            World.AddRoom(0);
            CurrentWorld = World;
            CurrentWorld.DispRooms();
            Console.WriteLine("---------------");
            var firstWorld = Worlds.Take(1);
            foreach (AWorld w in firstWorld)
                w.DispRooms();
            //World.DispRooms();
            /* endTest */
        }

        public void AddWorld(AWorld World = null)
        {
            if (World == null)
                World = new World();
            Worlds.Add(World);
        }

        public AWorld getCurrentWorld()
        {
            return (CurrentWorld);
        }

        public List<AWorld> getWorlds()
        {
            return (Worlds);
        }

        public void setCurrentWorld(AWorld World)
        {
            CurrentWorld = World;
        }
    }
}
