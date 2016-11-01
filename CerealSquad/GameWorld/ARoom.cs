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
        public struct s_MapPos
        {
            public static s_MapPos Zero { get { return new s_MapPos(0, 0); } }

            public uint X { get; }
            public uint Y { get; }

            public s_MapPos(uint x, uint y)
            {
                X = x;
                Y = y;
            }
        }

        public struct s_MapSize
        {
            public uint Width { get; }
            public uint Height { get; }

            public s_MapSize(uint width, uint height)
            {
                Width = width;
                Height = height;
            }
        }

        public enum e_RoomType { FightRoom, TransitionRoom };

        public e_RoomType RoomType { get; private set; }
        //protected List<IEntity> Ennemies;
        public s_MapPos Position { get; private set; }
        public s_MapSize Size { get; private set; }
        private Dictionary<RoomParser.t_cellpos, RoomParser.t_cellcontent> Cells;
        private Graphics.EnvironmentResources er = new Graphics.EnvironmentResources();

        public ARoom(s_MapPos Pos, string MapFile, e_RoomType Type = 0)
        {
            RoomType = Type;
            Position = Pos;
            Cells = RoomParser.ParseRoom(MapFile);
            Size = new s_MapSize(Cells.Keys.OrderBy(x => x.Row).Last().Row + 1, Cells.Keys.OrderBy(x => x.Column).Last().Column + 1);
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
