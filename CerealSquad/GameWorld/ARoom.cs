using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using CerealSquad.Graphics;

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
        public RegularSprite _RenderSprite { get; }
        private RenderTexture _RenderTexture = null;
        private Dictionary<RoomParser.t_cellpos, RoomParser.t_cellcontent> Cells = null;
        private EnvironmentResources er = new Graphics.EnvironmentResources();

        private RegularSprite _WallSprite;
        private RegularSprite _WallLeftSprite;
        private RegularSprite _WallRightSprite;
        private RenderTexture _WallTexture;
        private RenderTexture _WallFrontTexture;
        private EnvironmentResources wallEr;

        public ARoom(s_MapPos Pos, string MapFile, e_RoomType Type = 0)
        {
            RoomType = Type;
            Position = Pos;
            Cells = RoomParser.ParseRoom(MapFile);
            Size = new s_MapSize(Cells.Keys.OrderBy(x => x.Column).Last().Column + 1, Cells.Keys.OrderBy(x => x.Row).Last().Row + 1);
            _RenderTexture = new RenderTexture(Size.Width * TILE_SIZE, Size.Height * TILE_SIZE);
            IntRect rect = new IntRect(0, 0, (int)_RenderTexture.Size.X, (int)_RenderTexture.Size.Y);
            _RenderSprite = new RegularSprite(_RenderTexture.Texture, new SFML.System.Vector2i((int)_RenderTexture.Size.X, (int)_RenderTexture.Size.Y), rect);
            _RenderSprite.Position = new SFML.System.Vector2f(Position.X * TILE_SIZE * GROUND_TRANSFORM.X, Position.Y * TILE_SIZE * GROUND_TRANSFORM.Y);
            parseRoom();

            /*View v = _RenderTexture.GetView();
            v.Size = new SFML.System.Vector2f(v.Size.X, v.Size.Y * 2);
            v.Center = v.Size * 0.5f;
            v.Rotate(45f);
            _RenderTexture.SetView(v);*/

            //_RenderSprite.Scale = new SFML.System.Vector2f(_RenderSprite.Scale.X * GROUND_TRANSFORM.X, _RenderSprite.Scale.Y * GROUND_TRANSFORM.Y);
            //_RenderSprite.TransformVertex(new SFML.System.Vector2i(100, 0), new SFML.System.Vector2i(-100, 0), new SFML.System.Vector2i(0, 0), new SFML.System.Vector2i(0, 0));


            /*
             * building wall
            */
            Factories.TextureFactory.Instance.load("Assets/Tiles/DefaultWall.png", "Assets/Tiles/DefaultWall.png");
            Graphics.PaletteManager.Instance.AddPaletteInformations("Assets/Tiles/DefaultWall.png");
            _WallTexture = new RenderTexture((Size.Width + 1) * TILE_SIZE, 1 * TILE_SIZE);
            _WallFrontTexture = new RenderTexture((Size.Width + 1) * TILE_SIZE, 2 * TILE_SIZE);
            IntRect wallRect = new IntRect(0, 0, (int)_WallTexture.Size.X, (int)_WallTexture.Size.Y);
            _WallSprite = new RegularSprite(_WallFrontTexture.Texture, new SFML.System.Vector2i((int)_WallTexture.Size.X, (int)_WallTexture.Size.Y), wallRect);
            _WallSprite.Position = new SFML.System.Vector2f(((int)Position.X) * TILE_SIZE, ((int)Position.Y - 1) * TILE_SIZE);
            _WallLeftSprite = new RegularSprite(_WallTexture.Texture, new SFML.System.Vector2i((int)_WallTexture.Size.X, (int)_WallTexture.Size.Y), wallRect);
            _WallLeftSprite.Position = new SFML.System.Vector2f(((int)Position.X) * TILE_SIZE, ((int)Position.Y - 1) * TILE_SIZE);
            _WallLeftSprite.Rotation = 90f;
            _WallRightSprite = new RegularSprite(_WallTexture.Texture, new SFML.System.Vector2i((int)_WallTexture.Size.X, (int)_WallTexture.Size.Y), wallRect);
            _WallRightSprite.Position = new SFML.System.Vector2f(((int)Position.X + Size.Width + 1) * TILE_SIZE, ((int)Position.Y - 1) * TILE_SIZE);
            _WallRightSprite.Rotation = 90f;

            wallEr = new EnvironmentResources();
            uint i = 0;
            while (i < Size.Width + 1)
            {
                uint j = 0;
                while (j < 2)
                {
                    wallEr.AddSprite(i, j, "Assets/Tiles/DefaultWall.png", (i == Size.Width || j == 1 ? (uint)1 : 0));
                    j++;
                }
                i++;
            }
            //_RenderSprite.TransformVertex(new SFML.System.Vector2i((int)TILE_SIZE * 2, 0), new SFML.System.Vector2i((int)TILE_SIZE * -2, 0), new SFML.System.Vector2i(0, 0), new SFML.System.Vector2i(0, 0));
            //_WallSprite.TransformVertex(new SFML.System.Vector2i(0, 0), new SFML.System.Vector2i(0, 0), new SFML.System.Vector2i((int)TILE_SIZE * -2, 0), new SFML.System.Vector2i((int)TILE_SIZE * 2, 0));
            //_WallLeftSprite.TransformVertex(new SFML.System.Vector2i((int)TILE_SIZE * 2, (int)TILE_SIZE * 2), new SFML.System.Vector2i((int)TILE_SIZE * 4, (int)TILE_SIZE * 4), new SFML.System.Vector2i(0, (int)TILE_SIZE * 2), new SFML.System.Vector2i(0, 0));
            //_WallRightSprite.TransformVertex(new SFML.System.Vector2i((int)TILE_SIZE * -4, (int)TILE_SIZE * 4), new SFML.System.Vector2i((int)TILE_SIZE * -2, (int)TILE_SIZE * 2), new SFML.System.Vector2i(0, 0), new SFML.System.Vector2i(0, (int)TILE_SIZE * 2));
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
                er.AddSprite(cell.Key.Column, cell.Key.Row, cell.Value.TexturePath, uint.Parse(cell.Value.Texture.ToString()));
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _RenderTexture.Clear();
            _RenderTexture.Draw(er, states);
            _RenderTexture.Display();
            //states.Transform.Combine(new Transform(.5f, .5f, 0f, 0f, .5f, 0f, 0f, 0f, 0f));
            //_WallTexture.Clear(Color.Red);
            //_WallTexture.Draw(wallEr);
            //_WallTexture.Display();

            //_WallFrontTexture.Clear(Color.Green);
            //_WallFrontTexture.Draw(wallEr);
            //_WallFrontTexture.Display();

            /*states.Transform.Combine(new Transform(
                1f, 0f, 0f,
                0f, .5f, 0f,
                0f, 0f, 1f));
            */

            target.Draw(_RenderSprite, states);

            //target.Draw(_WallSprite, states);
            //target.Draw(_WallLeftSprite, states);
            //target.Draw(_WallRightSprite, states);
            //target.Draw(_RenderSprite2, states);
        }
    }
}
