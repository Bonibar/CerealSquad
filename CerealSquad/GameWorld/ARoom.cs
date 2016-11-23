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

        #region Events
        public delegate void RoomCinematicEventHandler(object sender, RoomCinematicEventArg e);

        public class RoomCinematicEventArg {}

        public event RoomCinematicEventHandler RoomCinematicStart;
        public event RoomCinematicEventHandler RoomCinematicEnd;
        #endregion

        public enum e_RoomType { FightRoom, TransitionRoom };
        public enum e_RoomState { Idle, Starting, Started, Finished }

        public WorldEntity WorldEntity { get; protected set; }

        public e_RoomType RoomType { get; private set; }
        public s_Pos<int> Position { get; private set; }
        public s_MapSize Size { get; private set; }
        public RegularSprite _RenderSprite { get; }
        public e_RoomState State { get; private set; }

        private RenderTexture _RenderTexture = null;
        private RoomParser.s_room ParsedRoom = null;
        private EnvironmentResources er = new EnvironmentResources();
        private List<Crates> _Crates = new List<Crates>();
        private List<AEnemy> _Ennemies = new List<AEnemy>();
        private List<RoomDoor> _Doors = new List<RoomDoor>();

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
            _RenderSprite.Position = new SFML.System.Vector2f(Position.X * TILE_SIZE, Position.Y * TILE_SIZE);
            parseRoom();
            State = e_RoomState.Idle;

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
            if (RoomType == e_RoomType.FightRoom)
                spawnEnnemies();
            if (RoomType == e_RoomType.FightRoom)
                spawnDoors();
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

        private s_Pos<int> getLocalPos(IEntity entity)
        {
            s_Pos<int> result = new s_Pos<int>(-1, -1);

            int xEntity = entity.Pos._x / 64;
            int yEntity = entity.Pos._y / 64;


            if (xEntity < Position.X)
                result.X = 0;
            else if (xEntity >= Position.X + Size.Width)
                result.X = Position.X + (int)Size.Width - 1;
            else
                result.X = xEntity - Position.X;

            if (yEntity < Position.Y)
                result.Y = 0;
            else if (yEntity >= Position.Y + Size.Height)
                result.Y = Position.Y + (int)Size.Height - 1;
            else
                result.Y = yEntity - Position.Y;

            return result;
        }

        public void Start(List<APlayer> _players)
        {
            if (State == e_RoomState.Idle)
            {
                if (ParsedRoom.Cells.Count(i => i.Value.Type == RoomParser.e_CellType.Spawn) > 0)
                {
                    s_Pos<int> playerLocalPos = getLocalPos(_players.First());
                    if (playerLocalPos.X != -1 && playerLocalPos.Y != -1)
                    {
                        s_Pos<uint> cellPos = ParsedRoom.Cells
                            .Where(i => i.Value.Type == RoomParser.e_CellType.Spawn).OrderBy(i => i.Key.X - playerLocalPos.X + i.Key.Y - playerLocalPos.Y).First().Key;

                        RoomCinematicStart?.Invoke(this, new RoomCinematicEventArg());
                        s_position pos = new s_position((cellPos.X + Position.X) - 1, (cellPos.Y + Position.Y) - 1);
                        _players.ForEach(i => i.moveTo(pos));
                    }
                }
                State = e_RoomState.Starting;
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
            if (_Crates.Count > 0)
            {
                _Crates.FindAll(x => x.Picked == true && x.Respawn == false).ForEach(i => i.update(DeltaTime, null));
                var _respawnCrates = _Crates.FindAll(x => x.Respawn == true);
                _respawnCrates.ForEach((Crates crate) =>
                {
                    int id = _Crates.IndexOf(crate);
                    _Crates.Remove(crate);
                    RoomParser.s_crate toRespawn = ParsedRoom.Crates.ElementAt(id);
                    s_Pos<int> spawnPoint = toRespawn.Pos[_Rand.Next(0, toRespawn.Pos.Count)];
                    bool isColliding = true;
                    while (isColliding)
                    {
                        spawnPoint = toRespawn.Pos[_Rand.Next(0, toRespawn.Pos.Count)];
                        if (_Crates.FindAll(x => (int)x.Pos._trueX == spawnPoint.X && (int)x.Pos._trueY == spawnPoint.Y).Count == 0)
                            isColliding = false;
                    }
                    _Crates.Insert(id, new Crates(WorldEntity, spawnPoint, toRespawn.Types[_Rand.Next(0, toRespawn.Types.Count)]));
                });
            }
        }

        private void spawnDoors()
        {
            ParsedRoom.Cells.Where(i => i.Value.Type == RoomParser.e_CellType.Door).ToList().ForEach(i => _Doors.Add(new RoomDoor(WorldEntity, new s_position(i.Key.X, i.Key.Y), this)));
            _Doors.ForEach(i => i.ressourcesEntity.EnableCollision = false);
            _Doors.ForEach(i => i.ressourcesEntity.sprite.Displayed = false);
        }

        private void spawnEnnemies()
        {
            foreach (var ennemy in ParsedRoom.Ennemies)
            {
                s_Pos<int> spawnPoint = ennemy.Pos[_Rand.Next(0, ennemy.Pos.Count)];
                bool isColliding = true;
                while (isColliding)
                {
                    spawnPoint = ennemy.Pos[_Rand.Next(0, ennemy.Pos.Count)];
                    if (_Ennemies.FindAll(x => (int)x.Pos._trueX == spawnPoint.X && (int)x.Pos._trueY == spawnPoint.Y).Count == 0)
                        isColliding = false;
                }
                _Ennemies.Add(Factories.EnnemyFactory.CreateEnnemy(WorldEntity, spawnPoint, this, ennemy.Types[_Rand.Next(0, ennemy.Types.Count)]));
            }
        }

        public void Update(SFML.System.Time DeltaTime, List<APlayer> players)
        {
            spawnCrates(DeltaTime);
            if (State == e_RoomState.Starting)
            {
                if (players.Count(i => i.FinishedMovement == true) == players.Count)
                {
                    _Ennemies.ForEach(i => i.Active = true);
                    _Doors.ForEach(i => i.ressourcesEntity.EnableCollision = true);
                    _Doors.ForEach(i => i.ressourcesEntity.sprite.Displayed = true);
                    State = e_RoomState.Started;
                    RoomCinematicEnd?.Invoke(this, new RoomCinematicEventArg());
                }
            }
            else if (State == e_RoomState.Started && _Ennemies.Count(i => i.Die == false) == 0)
            {
                _Ennemies.Clear();
                _Doors.ForEach(i => i.Die = true);
                _Doors.Clear();
                State = e_RoomState.Finished;
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