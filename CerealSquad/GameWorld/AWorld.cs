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
        public static uint TILE_SIZE = 64;
        protected List<ARoom> Rooms = new List<ARoom>();

        public void AddRoom(ARoom room)
        {
            if (room != null)
                Rooms.Add(room);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Rooms.ForEach((ARoom room) => {
                RenderTexture roomTexture = new RenderTexture(room.Size.Width * TILE_SIZE, room.Size.Height * TILE_SIZE);
                room.Draw(roomTexture, states);
                Sprite roomSprite = new Sprite(roomTexture.Texture);

                roomSprite.Position = new SFML.System.Vector2f(room.Position.X * TILE_SIZE, room.Position.Y * TILE_SIZE);
                target.Draw(roomSprite, states);
                roomSprite.Dispose();
                roomTexture.Dispose();
            });
        }
    }
}
