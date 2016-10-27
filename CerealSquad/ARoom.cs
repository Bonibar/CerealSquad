using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class ARoom
    {
        public struct s_MapRect
        {
            public s_MapRect(int xStart, int xEnd, int yStart, int yEnd)
            {
                this.xStart = xStart;
                this.xEnd = xEnd;
                this.yStart = yStart;
                this.yEnd = yEnd;
            }

            public int xStart { get; }
            public int xEnd { get; }
            public int yStart { get; }
            public int yEnd { get; }
        }

        public enum e_RoomType { FightRoom, TransitionRoom };

        protected e_RoomType RoomType = 0;
        //protected List<IEntity> Ennemies;
        protected s_MapRect MapRect;

        public ARoom(e_RoomType Type = 0)
        {
            RoomType = Type;
        }
        
        public e_RoomType getRoomType()
        {
            return (RoomType);
        }

        public void setRoomType(e_RoomType Type)
        {
            RoomType = Type;
        }

        public s_MapRect getMapRect()
        {
            return (MapRect);
        }

        public void setMapRect(s_MapRect Rect)
        {
            MapRect = Rect;
        }

        public void setMapRect(int xStart, int xEnd, int yStart, int yEnd)
        {
            MapRect = new s_MapRect(xStart, xEnd, yStart, yEnd);
        }
    }
}
