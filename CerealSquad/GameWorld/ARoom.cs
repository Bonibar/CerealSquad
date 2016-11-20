using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using CerealSquad.Graphics;
using CerealSquad.Global;
using CerealSquad.EntitySystem;

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
        private RoomParser.s_room ParsedRoom = null;
        private EnvironmentResources er = new EnvironmentResources();
        public WorldEntity WorldEntity { get; protected set; }
        private List<Crates> _Crates = new List<Crates>();

        private Random _Rand = new Random();
        private Dictionary<int, int> _RespawnCrates = new Dictionary<int, int>();

        public ARoom(s_Pos<int> Pos, string MapFile, WorldEntity worldentity, e_RoomType Type = 0)
        {
            RoomType = Type;
            Position = Pos;
            WorldEntity = worldentity;
            ParsedRoom = RoomParser.ParseRoom(MapFile);
            Size = new s_MapSize(ParsedRoom.Cells.Keys.OrderBy(x => x.X).Last().X + 1, ParsedRoom.Cells.Keys.OrderBy(x => x.Y).Last().Y + 1);
            _RenderTexture = new RenderTexture(Size.Width * TILE_SIZE, Size.Height * TILE_SIZE);
            IntRect rect = new IntRect(0, 0, (int)_RenderTexture.Size.X, (int)_RenderTexture.Size.Y);
            _RenderSprite = new RegularSprite(_RenderTexture.Texture, new SFML.System.Vector2i((int)_RenderTexture.Size.X, (int)_RenderTexture.Size.Y), rect);
            _RenderSprite.Position = new SFML.System.Vector2f(Position.X * TILE_SIZE * GROUND_TRANSFORM.X, Position.Y * TILE_SIZE * GROUND_TRANSFORM.Y);
            parseRoom();

            for (int i = 0; i < ParsedRoom.Crates.Count; i++)
                _RespawnCrates.Add(i, -1);
        }

        private void parseRoom()
        {
            foreach (var cell in ParsedRoom.Cells)
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
                cel = ParsedRoom.Cells.First(z => z.Key.Y.Equals(y) && z.Key.X.Equals(x)).Value.Type;
            }
            return (cel);
        }

        private void spawnCrates(SFML.System.Time DeltaTime)
        {
            if (_Crates.Count == 0)
            {
                foreach (var crate in ParsedRoom.Crates)
                {
                    s_Pos<int> spawnPoint = crate.Pos[_Rand.Next(0, crate.Pos.Count)];
                    bool isColliding = true;
                    while (isColliding)
                    {
                        spawnPoint = crate.Pos[_Rand.Next(0, crate.Pos.Count)];
                        if (_Crates.FindAll(x => (int)x.Pos._trueX == spawnPoint.X && (int)x.Pos._trueY == spawnPoint.Y).Count == 0)
                            isColliding = false;
                    }
                    _Crates.Add(new Crates(WorldEntity, spawnPoint, crate.Types[_Rand.Next(0, crate.Types.Count)]));
                }
            }
            else
            {
                var _cratesToRemove = _Crates.FindAll(x => x.Picked == true);
                _cratesToRemove.ForEach((Crates crate) =>
                {
                    int id = _Crates.IndexOf(crate);
                    _Crates.Remove(crate);
                    _RespawnCrates[id] = 0;
                });
                for (int i = 0; i < _RespawnCrates.Count; i++)
                {
                    if (_RespawnCrates[i] >= 0)
                        _RespawnCrates[i] += DeltaTime.AsMilliseconds();
                    if (_RespawnCrates[i] >= 3600)
                    {
                        System.Diagnostics.Debug.WriteLine("Parsed Rooms Crates : " + ParsedRoom.Crates);
                        RoomParser.s_crate toRespawn = ParsedRoom.Crates.ElementAt(i);
                        s_Pos<int> spawnPoint = toRespawn.Pos[_Rand.Next(0, toRespawn.Pos.Count)];
                        bool isColliding = true;
                        while (isColliding)
                        {
                            spawnPoint = toRespawn.Pos[_Rand.Next(0, toRespawn.Pos.Count)];
                            if (_Crates.FindAll(x => (int)x.Pos._trueX == spawnPoint.X && (int)x.Pos._trueY == spawnPoint.Y).Count == 0)
                                isColliding = false;
                        }
                        _Crates.Insert(i, new Crates(WorldEntity, spawnPoint, toRespawn.Types[_Rand.Next(0, toRespawn.Types.Count)]));
                        _RespawnCrates[i] = -1;
                    }
                }
            }
        }

        public void Update(SFML.System.Time DeltaTime)
        {
            spawnCrates(DeltaTime);
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