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
            AWorld World = new AWorld();
            for (int i = 0; i < 5; i++)
                World.AddRoom(0);
            AddWorld(World);
            AWorld World2 = new AWorld();
            for (int i = 0; i < 6; i++)
                World2.AddRoom(0);
            AddWorld(World2);
            CurrentWorld = World;
            CurrentWorld.DispRooms();
            Console.WriteLine("---------------");
            var firstWorld = Worlds.ElementAt(0);
            firstWorld.DispRooms();
            var index = Worlds.FindIndex(a => a == CurrentWorld);
            Console.WriteLine("Worlds : " + Worlds.Count);
            Console.WriteLine("Rooms : " + firstWorld.getRooms().Count);
            Console.WriteLine("Rooms 2 : " + Worlds.ElementAt(1).getRooms().Count);
            Console.WriteLine("Index : " + index);
            //World.DispRooms();
            /* endTest */
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
