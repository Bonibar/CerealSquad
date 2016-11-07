using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using CerealSquad.Global;

namespace CerealSquad.GameWorld
{
    class AWorld : Drawable
    {
        protected List<ARoom> Rooms = new List<ARoom>();

        public AWorld(string path)
        {
            if (path == null)
                throw new ArgumentNullException("Path cannot be null");

            Dictionary<s_Pos<int>, WorldParser.t_roomcontent> rooms = WorldParser.ParseWorld("Maps/TestWorld.txt");

            foreach (var room in rooms)
            {
                AddRoom(new ARoom(new s_Pos<int>(room.Key.X, room.Key.Y), room.Value.RoomPath, (ARoom.e_RoomType)room.Value.Type));
            }
        }

        public void AddRoom(ARoom room)
        {
            if (room != null)
                Rooms.Add(room);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Rooms.ForEach((ARoom room) => {
                target.Draw(room, states);
            });
        }

        public RoomParser.e_CellType getPosition(uint x, uint y)
        {
            foreach(ARoom room in Rooms)
            {
                if (x >= room.Position.X && x < room.Position.X + room.Size.Width &&
                y >= room.Position.Y && y < room.Position.Y + room.Size.Height)
                    return room.getPosition(x - room.Position.X, y - room.Position.Y);
            }
            return (RoomParser.e_CellType.Void);
        }
    }
}
