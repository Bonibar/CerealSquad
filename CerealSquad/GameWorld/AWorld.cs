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
        protected List<ARoom> Rooms = new List<ARoom>();

        public List<ARoom> getRooms()
        {
            return (Rooms);
        }

        public void DispRooms()
        {
            foreach (ARoom Room in Rooms)
            {
                Console.WriteLine("RoomType = " + Room.RoomType);
            }
        }

        public void AddRoom(ARoom.e_RoomType Type = 0)
        {
            Rooms.Add(new ARoom(ARoom.s_MapRect.Zero, "Maps/TestRoom.txt", Type));
            Rooms.Add(new ARoom(new ARoom.s_MapRect(14, 0, 0, 0), "Maps/TestRoom.txt", Type));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Rooms.ForEach((ARoom x) => {
                SFML.Graphics.RenderTexture t = new RenderTexture(13 * 64, 13 * 64);
                x.Draw(t, states);
                Sprite te = new Sprite(t.Texture);
                te.Position = new SFML.System.Vector2f(x.MapRect.xStart * 64, x.MapRect.yStart * 64);
                target.Draw(te, states);
            });
        }
    }
}
