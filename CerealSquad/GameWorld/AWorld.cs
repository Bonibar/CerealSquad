using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.GameWorld
{
    class AWorld : Drawable
    {
        public static int TILE_SIZE = 64;
        protected List<ARoom> Rooms = new List<ARoom>();

        public void AddRoom(ARoom.e_RoomType Type = 0)
        {
            Rooms.Add(new ARoom(ARoom.s_MapRect.Zero, "Maps/TestRoom.txt", Type));
            Rooms.Add(new ARoom(new ARoom.s_MapRect(14, 0, 0, 0), "Maps/TestRoom.txt", Type));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Rooms.ForEach((ARoom room) => {
                RenderTexture roomTexture = new RenderTexture(room.Size.Width * TILE_SIZE, room.Size.Height * TILE_SIZE);
                room.Draw(roomTexture, states);
                Sprite roomSprite = new Sprite(roomTexture.Texture);
                roomSprite.Position = new SFML.System.Vector2f(room.Position.Width * TILE_SIZE, room.Position.Width * TILE_SIZE);
                target.Draw(roomSprite, states);
            });
        }
    }
}
