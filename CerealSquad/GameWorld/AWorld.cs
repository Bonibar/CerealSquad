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
        public WorldEntity WorldEntity { get; protected set; }

        public AWorld(string path, WorldEntity worldentity)
        {
            if (path == null)
                throw new ArgumentNullException("Path cannot be null");

            Dictionary<s_Pos<int>, WorldParser.t_roomcontent> rooms = WorldParser.ParseWorld("Maps/TestWorld.txt");
            WorldEntity = worldentity;

            foreach (var room in rooms)
            {
                AddRoom(new ARoom(new s_Pos<int>(room.Key.X, room.Key.Y), room.Value.RoomPath, WorldEntity, (ARoom.e_RoomType)room.Value.Type));
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

        public RoomParser.e_CellType getPosition(int x, int y)
        {
            foreach(ARoom room in Rooms)
            {
                if (x >= room.Position.X && x < room.Position.X + room.Size.Width &&
                y >= room.Position.Y && y < room.Position.Y + room.Size.Height)
                    return room.getPosition((uint)(x - room.Position.X), (uint)(y - room.Position.Y));
            }
            return (RoomParser.e_CellType.Void);
        }

        /// <summary>
        /// Look at 4 points of CollisionBox of EntityResources and return true if one of them is inside a wall/void
        /// </summary>
        /// <param name="Res">Graphics.EntityResource</param>
        /// <returns>bool</returns>
        public bool IsCollidingWithWall(Graphics.EntityResources Res)
        {
            return false;
            SFML.System.Vector2f CollisionPointOne = new SFML.System.Vector2f(Res.CollisionBox.Width, -Res.CollisionBox.Top);
            SFML.System.Vector2f CollisionPointTwo = new SFML.System.Vector2f(-Res.CollisionBox.Left, -Res.CollisionBox.Top);
            SFML.System.Vector2f CollisionPointThree = new SFML.System.Vector2f(Res.CollisionBox.Width, Res.CollisionBox.Height);
            SFML.System.Vector2f CollisionPointFour = new SFML.System.Vector2f(-Res.CollisionBox.Left, Res.CollisionBox.Height);

            CollisionPointOne += Res.Position;
            CollisionPointTwo += Res.Position;
            CollisionPointThree += Res.Position;
            CollisionPointFour += Res.Position;

            CollisionPointOne /= 64.0f;
            CollisionPointTwo /= 64.0f;
            CollisionPointThree /= 64.0f;
            CollisionPointFour /= 64.0f;

            if (getPosition((int)(CollisionPointOne.X), (int)(CollisionPointOne.Y)) != RoomParser.e_CellType.Normal
                || getPosition((int)(CollisionPointTwo.X), (int)(CollisionPointTwo.Y)) != RoomParser.e_CellType.Normal
                || getPosition((int)(CollisionPointThree.X), (int)(CollisionPointThree.Y)) != RoomParser.e_CellType.Normal
                || getPosition((int)(CollisionPointFour.X), (int)(CollisionPointFour.Y)) != RoomParser.e_CellType.Normal)
                return true;
            return false;
        }
    }
}
