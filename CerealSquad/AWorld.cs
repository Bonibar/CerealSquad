using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class AWorld
    {
        protected List<ARoom> Rooms = new List<ARoom>();

        public List<ARoom> getRooms()
        {
            return (Rooms);
        }

        public void DispRooms()
        {
            foreach (ARoom Room in Rooms)
            {
                Console.WriteLine("RoomType = " + Room.getRoomType());
            }
        }
    }
}
