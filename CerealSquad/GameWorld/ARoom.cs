using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.GameWorld
{
    class ARoom : Drawable
    {
        /// <summary>
        /// Contains the globale position in the world.
        /// </summary>
        public struct s_MapRect
        {
            public static s_MapRect Zero { get { return new s_MapRect(0, 0, 0, 0); } }
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

        public e_RoomType RoomType { get; private set; }
        //protected List<IEntity> Ennemies;
        public s_MapRect MapRect { get; private set; }
        private Dictionary<RoomParser.t_cellpos, RoomParser.t_cellcontent> Cells;
        private Graphics.EnvironmentResources er = new Graphics.EnvironmentResources();

        public ARoom(s_MapRect Position, string MapFile, e_RoomType Type = 0)
        {
            RoomType = Type;
            MapRect = Position;
            Cells = RoomParser.ParseRoom(MapFile);
            parseRoom();
        }

        private void parseRoom()
        {
            Graphics.TextureFactory.Instance.initTextures();
            foreach (var cell in Cells)
            {
                if (!Graphics.TextureFactory.Instance.exists(cell.Value.TexturePath))
                {
                    Graphics.TextureFactory.Instance.load(cell.Value.TexturePath, cell.Value.TexturePath);
                    Graphics.PaletteManager.Instance.AddPaletteInformations(cell.Value.TexturePath);
                }
                er.AddSprite(cell.Key.Row, cell.Key.Column, cell.Value.TexturePath, uint.Parse(cell.Value.Texture.ToString()));
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
           target.Draw(er);
           // er.Draw(target, states);
        }
    }
}
