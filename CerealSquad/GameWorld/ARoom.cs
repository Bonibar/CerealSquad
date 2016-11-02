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
        public static readonly uint TILE_SIZE = 64;

        private static readonly SFML.System.Vector2f GROUND_TRANSFORM = new SFML.System.Vector2f(1f, 1f);

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
        public Sprite _RenderSprite { get; }
        private RenderTexture _RenderTexture = null;
        private Dictionary<RoomParser.t_cellpos, RoomParser.t_cellcontent> Cells = null;
        private Graphics.EnvironmentResources er = new Graphics.EnvironmentResources();

        public ARoom(s_MapPos Pos, string MapFile, e_RoomType Type = 0)
        {
            RoomType = Type;
            Position = Pos;
            Cells = RoomParser.ParseRoom(MapFile);
            Size = new s_MapSize(Cells.Keys.OrderBy(x => x.Column).Last().Column + 1, Cells.Keys.OrderBy(x => x.Row).Last().Row + 1);
            _RenderTexture = new RenderTexture(Size.Width * TILE_SIZE, Size.Height * TILE_SIZE);
            _RenderSprite = new Sprite(_RenderTexture.Texture);
            _RenderSprite.Position = new SFML.System.Vector2f(Position.X * TILE_SIZE * GROUND_TRANSFORM.X, Position.Y * TILE_SIZE * GROUND_TRANSFORM.Y);
            parseRoom();

            /*View v = _RenderTexture.GetView();
            v.Size = new SFML.System.Vector2f(v.Size.X, v.Size.Y * 2);
            v.Center = v.Size * 0.5f;
            v.Rotate(45f);
            _RenderTexture.SetView(v);*/

            _RenderSprite.Scale = new SFML.System.Vector2f(_RenderSprite.Scale.X * GROUND_TRANSFORM.X, _RenderSprite.Scale.Y * GROUND_TRANSFORM.Y);
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
                er.AddSprite(cell.Key.Column, cell.Key.Row, cell.Value.TexturePath, uint.Parse(cell.Value.Texture.ToString()));
            }
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
