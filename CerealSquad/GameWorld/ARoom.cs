using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using CerealSquad.Graphics;
using CerealSquad.Global;

namespace CerealSquad.GameWorld
{
    class ARoom : Drawable
    {
        public static readonly uint TILE_SIZE = 64;

        private static readonly SFML.System.Vector2f GROUND_TRANSFORM = new SFML.System.Vector2f(1f, 1f);

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
        public s_Pos<int> Position { get; private set; }
        public s_MapSize Size { get; private set; }
        public RegularSprite _RenderSprite { get; }
        private RenderTexture _RenderTexture = null;
        private Dictionary<s_Pos<uint>, RoomParser.t_cellcontent> Cells = null;
        private EnvironmentResources er = new Graphics.EnvironmentResources();

        public ARoom(s_Pos<int> Pos, string MapFile, e_RoomType Type = 0)
        {
            RoomType = Type;
            Position = Pos;
            Cells = RoomParser.ParseRoom(MapFile);
            Size = new s_MapSize(Cells.Keys.OrderBy(x => x.X).Last().X + 1, Cells.Keys.OrderBy(x => x.Y).Last().Y + 1);
            _RenderTexture = new RenderTexture(Size.Width * TILE_SIZE, Size.Height * TILE_SIZE);
            IntRect rect = new IntRect(0, 0, (int)_RenderTexture.Size.X, (int)_RenderTexture.Size.Y);
            _RenderSprite = new RegularSprite(_RenderTexture.Texture, new SFML.System.Vector2i((int)_RenderTexture.Size.X, (int)_RenderTexture.Size.Y), rect);
            _RenderSprite.Position = new SFML.System.Vector2f(Position.X * TILE_SIZE * GROUND_TRANSFORM.X, Position.Y * TILE_SIZE * GROUND_TRANSFORM.Y);
            parseRoom();
        }

        private void parseRoom()
        {
            foreach (var cell in Cells)
            {
                if (!Factories.TextureFactory.Instance.exists(cell.Value.TexturePath))
                {
                    Factories.TextureFactory.Instance.load(cell.Value.TexturePath, cell.Value.TexturePath);
                    PaletteManager.Instance.AddPaletteInformations(cell.Value.TexturePath);
                }
                er.AddSprite(cell.Key.X, cell.Key.Y, cell.Value.TexturePath, uint.Parse(cell.Value.Texture.ToString()));
            }
        }

        public RoomParser.e_CellType getPosition(uint x, uint y)
        {
            RoomParser.e_CellType cel = RoomParser.e_CellType.Void;
            if (x < Size.Width && y <= Size.Height)
            {
                cel = Cells.First(z => z.Key.Y.Equals(y) && z.Key.X.Equals(x)).Value.Type;
            }
            return (cel);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _RenderTexture.Clear();
            _RenderTexture.Draw(er, states);
            _RenderTexture.Display();

            target.Draw(_RenderSprite, states);
        }
    }
}
