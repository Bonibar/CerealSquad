using CerealSquad.GameWorld;
using CerealSquad.Global;
using CerealSquad.EntitySystem;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CerealSquad.EntitySystem.APlayer;

namespace CerealSquad.EntitySystem
{
    abstract class AEnemy : AEntity
    {
        protected ARoom _room;

        protected Random _rand;

        protected int _r;

        public bool Active { get; set; }

        public AEnemy(IEntity owner, s_position position, ARoom room) : base(owner)
        {
            _pos = position + new s_position(room.Position.X, room.Position.Y);
            _type = e_EntityType.Ennemy;
            _room = room;
            _rand = new Random();
            _r = 0;
            Active = false;
            _damageType = e_DamageType.ENEMY_DAMAGE;
            _CollidingType.Add(e_EntityType.Player);
        }

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool baseResult = base.IsCollidingEntity(World, CollidingEntities);
            bool result = false;
            
            CollidingEntities.ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.PlayerTrap && ((ATrap)i).TrapType == e_TrapType.WALL)
                    result = true;
                i.attemptDamage(this, _damageType);
            });

            return result || baseResult;
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage, bool isHitBox = false)
        {
            bool result = false;

            switch (damage)
            {
                case e_DamageType.BOMB_DAMAGE:
                case e_DamageType.TRUE_DAMAGE:
                    die();
                    result = true;
                    break;
            }

            return result;
        }

        public abstract void think(AWorld world, Time deltaTime);

        public s_position getCoord(s_position pos)
        {
            double x = pos.X;
            double y = pos.Y;

            x -= _room.Position.X;
            y -= _room.Position.Y;
            return (new s_position(x, y));
        }

        protected virtual bool moveSameTile(AWorld zone, WorldEntity world, Time deltaTime)
        {
            _move = new List<EMovement> { EMovement.None };
            foreach (IEntity entity in world.getChildren())
            {
                if (entity.getEntityType() == e_EntityType.Player)
                {
                    double diffWidth = (entity.ressourcesEntity.CollisionBox.Left + entity.ressourcesEntity.CollisionBox.Width / 2 - (ressourcesEntity.CollisionBox.Left + ressourcesEntity.CollisionBox.Width / 2)) / 64;
                    double diffHeight = (entity.ressourcesEntity.CollisionBox.Top + entity.ressourcesEntity.CollisionBox.Height / 2 - (ressourcesEntity.CollisionBox.Top + ressourcesEntity.CollisionBox.Height / 2)) / 64;
                    if (Math.Abs(diffWidth) < 2 && Math.Abs(diffHeight) < 2)
                    {
                        if (Math.Abs(diffWidth) > 0.1)
                            _move = new List<EMovement> { diffWidth < 0 ? EMovement.Left : EMovement.Right };
                        else if (Math.Abs(diffHeight) > 0.1)
                        {
                            _move = new List<EMovement> { diffHeight < 0 ? EMovement.Up : EMovement.Down };
                        }
                    }
                }
            }
            bool result = true;
            result &= executeLeftMove(zone, Speed * deltaTime.AsSeconds());
            result &= executeRightMove(zone, Speed * deltaTime.AsSeconds());
            result &= executeUpMove(zone, Speed * deltaTime.AsSeconds());
            result &= executeDownMove(zone, Speed * deltaTime.AsSeconds());
            if (!result)
                _move = new List<EMovement> { EMovement.None };
            return (!_move.Contains(EMovement.None) && result);
        }

        public override bool inRoom(s_position pos)
        {
            bool result = pos.X < _room.Position.X + _room.Size.Width && pos.X >= _room.Position.X
                && pos.Y < _room.Position.Y + _room.Size.Width && pos.Y >= _room.Position.Y;
            return result;
        }

        protected class scentMap
        {
            protected int[][][] _map;
            protected uint _x;
            protected uint _y;

            public int[][][] Map
            {
                get
                {
                    return _map;
                }

                set
                {
                    _map = value;
                }
            }

            public scentMap(uint x, uint y)
            {
                _x = x;
                _y = y;
            }

            protected s_position getCoord(s_position pos, s_Pos<int> room)
            {
                double x = pos.X;
                double y = pos.Y;

                x -= room.X;
                y -= room.Y;
                return (new s_position(x, y));
            }

            protected void reset(ARoom room)
            {
                _map = new int[_x][][];
                for (uint i = 0; i < _x; i++)
                {
                    _map[i] = new int[_y][];
                    for (uint j = 0; j < _y; j++)
                    {
                        _map[i][j] = new int[4];
                        if (room.getPosition(i, j) == RoomParser.e_CellType.Normal)
                        {
                            _map[i][j][0] = 0;
                            _map[i][j][1] = 0;
                            _map[i][j][2] = 0;
                            _map[i][j][3] = 0;
                        }
                        else
                        {
                            _map[i][j][0] = -1;
                            _map[i][j][1] = -1;
                            _map[i][j][2] = -1;
                            _map[i][j][3] = -1;
                        }
                    }
                }
            }

            public void propagateHeat(int x, int y, int intensity, EName character, int characterWeight)
            {
                if (x >= 0 && x < _x && y >= 0 && y < _y && _map[x][y][(int)character] != -1 && _map[x][y][(int)character] < intensity * characterWeight)
                {
                        _map[x][y][(int)character] = intensity * characterWeight;
                        if (intensity > 1)
                        {
                            propagateHeat(x - 1, y, intensity - 1, character, characterWeight);
                            propagateHeat(x + 1, y, intensity - 1, character, characterWeight);
                            propagateHeat(x, y - 1, intensity - 1, character, characterWeight);
                            propagateHeat(x, y + 1, intensity - 1, character, characterWeight);
                        }
                }
            }

            protected virtual void check_player(WorldEntity world, ARoom room)
            {
                foreach (IEntity entity in world.getChildren())
                {
                    if (entity.getEntityType() == e_EntityType.Player)
                    {
                        APlayer p = (APlayer)entity;
                        s_position pos = getCoord(p.HitboxPos, room.Position);
                        propagateHeat((int)pos.X, (int)pos.Y, 100, p.getName(), p.Weight);
                    }
                }
            }

            protected virtual void check_obstacle(WorldEntity world)
            {
                foreach (IEntity entity in world.getChildren())
                {
                    if (entity.getEntityType() == e_EntityType.PlayerTrap && ((ATrap)entity).TrapType == e_TrapType.WALL)
                    {
                        try
                        {
                            _map[(int)entity.Pos.X][(int)entity.Pos.Y][0] = -1;
                            _map[(int)entity.Pos.X][(int)entity.Pos.Y][1] = -1;
                            _map[(int)entity.Pos.X][(int)entity.Pos.Y][2] = -1;
                            _map[(int)entity.Pos.X][(int)entity.Pos.Y][3] = -1;
                        } catch (Exception e) { }
                    }
                }
            }

            public void update(WorldEntity world, ARoom room)
            {
                reset(room);
                check_obstacle(world);
                check_player(world, room);
            }

            public virtual int getScent(int x, int y)
            {
                if (x >= 0 && x < _x && y >= 0 && y < _y)
                {
                    int scent = 0;
                    scent += _map[x][y][0];
                    scent += _map[x][y][1];
                    scent += _map[x][y][2];
                    scent += _map[x][y][3];
                    if (scent < 0)
                        scent = -1;
                    return (scent);
                }
                return (-1);
            }

            public void dump()
            {
                for (int y = 0; y < _y; y++)
                {
                    for (int x = 0; x < _x; x++)
                    {
                        if (_map[x][y][0] == 100)
                            System.Console.Out.Write("J");
                        else if (_map[x][y][1] == 100)
                            System.Console.Out.Write("O");
                        else if (_map[x][y][2] == 100)
                            System.Console.Out.Write("M");
                        else if (_map[x][y][3] == 100)
                            System.Console.Out.Write("T");
                        else
                            System.Console.Out.Write(getScent(x, y));
                        System.Console.Out.Write(" ");
                    }
                    Console.Out.Write('\n');
                }
            }
        }
    }
}
