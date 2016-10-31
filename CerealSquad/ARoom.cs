using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class ARoom
    {
        /// <summary>
        /// Contains the globale position in the world.
        /// </summary>
        public struct s_MapRect
        {
            /// <summary>
            /// Constructor of s_MapRect.
            /// </summary>
            /// <param name="xStart">uint</param>
            /// <param name="xEnd">uint</param>
            /// <param name="yStart">uint</param>
            /// <param name="yEnd">uint</param>
            public s_MapRect(uint xStart, uint xEnd, uint yStart, uint yEnd)
            {
                this.xStart = xStart;
                this.xEnd = xEnd;
                this.yStart = yStart;
                this.yEnd = yEnd;
            }

            public uint xStart { get; }
            public uint xEnd { get; }
            public uint yStart { get; }
            public uint yEnd { get; }
        }

        public enum e_RoomType { FightRoom, TransitionRoom };

        protected Dictionary<RoomParser.t_cellpos, RoomParser.t_cellcontent> Cells;
        protected e_RoomType RoomType = 0;
        //protected List<IEntity> Ennemies;
        protected s_MapRect MapRect;
        
        public ARoom(e_RoomType Type = 0)
        {
            RoomType = Type;
        }

        /// <summary>
        /// Loop that calls Environment Ressources functions to add sprites.
        /// </summary>
        public void loopAddSprites()
        {
            /*foreach (var cell in Cells)
            {
                EnvironmentRessources.AddSprite(cell.Key.Row, cell.Key.Column, cell.Value.TexturePath, cell.Value.Texture);
            }*/
        }

        /// <summary>
        /// Return the type of the room.
        /// </summary>
        /// <returns>e_RoomType</returns>
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

        public void setMapRect(uint xStart, uint xEnd, uint yStart, uint yEnd)
        {
            MapRect = new s_MapRect(xStart, xEnd, yStart, yEnd);
        }

        public Dictionary<RoomParser.t_cellpos, RoomParser.t_cellcontent> getCells()
        {
            return (Cells);
        }
    }
}
