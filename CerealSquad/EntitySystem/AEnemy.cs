using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CerealSquad.APlayer;

namespace CerealSquad
{
    abstract class AEnemy : AEntity
    {

        public AEnemy(IEntity owner, s_position position) : base(owner)
        {
            _pos = position;
            _type = e_EntityType.Ennemy;
        }

        public virtual void attack()
        {
            throw new NotImplementedException();
        }

        public abstract void think();

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

            protected void reset()
            {
                _map = new int[_x][][];
                for (int i = 0; i < _x; i++)
                {
                    _map[i] = new int[_y][];
                    for (int j = 0; j < _y; j++)
                    {
                        _map[i][j] = new int[4];
                        _map[i][j][0] = 0;
                        _map[i][j][1] = 0;
                        _map[i][j][2] = 0;
                        _map[i][j][3] = 0;
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

            public void update(WorldEntity world)
            {
                reset();
                foreach (IEntity entity in world.getChildren())
                {
                    if (entity.getEntityType() == e_EntityType.Player)
                    {
                        APlayer p = (APlayer)entity;
                        propagateHeat(p.Pos._x, p.Pos._y, 100, p.getName(), p.Weight);
                    }
                }
            }

            public virtual int getScent(int x, int y)
            {
                if (x > 0 && x < _x && y > 0 && y < _y)
                {
                    int scent = 0;
                    if (_map[x][y][0] != -1)
                        scent += _map[x][y][0];
                    if (_map[x][y][1] != -1)
                        scent += _map[x][y][1];
                    if (_map[x][y][2] != -1)
                        scent += _map[x][y][2];
                    if (_map[x][y][3] != -1)
                        scent += _map[x][y][3];
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
