using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using CerealSquad.Global;
using CerealSquad.EntitySystem;

namespace CerealSquad.GameWorld
{
    class AWorld : Drawable
    {
        public ARoom CurrentRoom { get; protected set; }
        protected List<ARoom> Rooms = new List<ARoom>();
        public WorldEntity WorldEntity { get; protected set; }

        private bool _PositionsUpToDate = false;

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

        public void ChangeRoom(ARoom room)
        {
            List<APlayer> _players = WorldEntity.GetAllEntities().Where(i => i.getEntityType() == e_EntityType.Player).Select(i => (APlayer)i).ToList();
            if (CurrentRoom != room && _players.Count(i => i.FinishedMovement == false) == 0)
            {
                _players.ForEach(i => i.CancelTrapDelivery());
                room.Start(_players);
            }
            CurrentRoom = room;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Rooms.ForEach((ARoom room) => {
                target.Draw(room, states);
            });
        }

        public void InvalidatePlayersPosition()
        {
            _PositionsUpToDate = false;
        }

        private void CheckPlayerPosition()
        {
            if (!_PositionsUpToDate)
            {
                foreach (ARoom room in Rooms)
                {
                    foreach (APlayer player in WorldEntity.getChildren().Where(i => i.Type == e_EntityType.Player))
                    {
                        if ((int)player.Pos.X >= room.Position.X && (int)player.Pos.X < room.Position.X + room.Size.Width &&
                            (int)player.Pos.Y >= room.Position.Y && (int)player.Pos.Y < room.Position.Y + room.Size.Height)
                        {
                            if (room != CurrentRoom)
                                ChangeRoom(room);
                        }
                    }
                }
            }
        }

        public RoomParser.e_CellType getCellType(int x, int y)
        {
            foreach(ARoom room in Rooms)
            {
                if (x >= room.Position.X && x < room.Position.X + room.Size.Width &&
                y >= room.Position.Y && y < room.Position.Y + room.Size.Height)
                {
                    //if (room != CurrentRoom)
                    //    ChangeRoom(room);
                    return room.getPosition((uint)(x - room.Position.X), (uint)(y - room.Position.Y));
                }
            }
            return (RoomParser.e_CellType.Void);
        }

        /// <summary>
        /// Check 4 points from position and collision box return true if one of them is inside a considered wall
        /// </summary>
        /// <param name="Res">Graphics.EntityResource</param>
        /// <returns>bool</returns>
        public bool IsCollidingWithWall(SFML.System.Vector2f Position, FloatRect CollisionBox)
        {
            List<RoomParser.e_CellType> wallTypes = new List<RoomParser.e_CellType> {
                RoomParser.e_CellType.Wall,
                RoomParser.e_CellType.Void
            };

            SFML.System.Vector2f CollisionPointOne = new SFML.System.Vector2f(CollisionBox.Left, CollisionBox.Top);
            SFML.System.Vector2f CollisionPointTwo = new SFML.System.Vector2f(CollisionBox.Left, CollisionBox.Top + CollisionBox.Height);
            SFML.System.Vector2f CollisionPointThree = new SFML.System.Vector2f(CollisionBox.Left + CollisionBox.Width, CollisionBox.Top + CollisionBox.Height);
            SFML.System.Vector2f CollisionPointFour = new SFML.System.Vector2f(CollisionBox.Left + CollisionBox.Width, CollisionBox.Top);

            CollisionPointOne /= 64.0f;
            CollisionPointTwo /= 64.0f;
            CollisionPointThree /= 64.0f;
            CollisionPointFour /= 64.0f;

            if (wallTypes.Contains(getCellType((int)(CollisionPointOne.X), (int)(CollisionPointOne.Y)))
                || wallTypes.Contains(getCellType((int)(CollisionPointTwo.X), (int)(CollisionPointTwo.Y)))
                || wallTypes.Contains(getCellType((int)(CollisionPointThree.X), (int)(CollisionPointThree.Y)))
                || wallTypes.Contains(getCellType((int)(CollisionPointFour.X), (int)(CollisionPointFour.Y))))
                return true;
            return false;
        }

        /// <summary>
        /// Look at 4 points of CollisionBox of EntityResources and return true if one of them is inside a wall/void
        /// </summary>
        /// <param name="Res">Graphics.EntityResource</param>
        /// <returns>bool</returns>
        public bool IsCollidingWithWall(Graphics.EntityResources Res)
        {
            return IsCollidingWithWall(Res.Position, Res.CollisionBox);
        }

        public RoomParser.e_CellType getPosition(int x, int y)
        {
            RoomParser.e_CellType ret = RoomParser.e_CellType.Void;
            Rooms.ForEach(r =>
            {
                if (r.Position.X <= x && r.Position.X + r.Size.Width > x
                && r.Position.Y <= y && r.Position.Y + r.Size.Height > y)
                    ret = r.getPosition((uint)(x - r.Position.X), (uint)(y - r.Position.Y));
            });
            return (ret);
        }

        public void Update(SFML.System.Time DeltaTime, List<APlayer> players)
        {
            CheckPlayerPosition();
            Rooms.ForEach(x => x.Update(DeltaTime, players));
        }
    }
}
