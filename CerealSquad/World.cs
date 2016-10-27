using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class World : AWorld
    {
        public World()
        {
        }

        public void AddRoom(ARoom.e_RoomType Type = 0)
        {
            Rooms.Add(new ARoom(Type));
        }
    }
}
