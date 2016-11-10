using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.Global;

namespace CerealSquad.Entities
{
    class Crates
    {
        public enum e_TrapType { Bomb, BearTrap }

        s_Pos<int> Pos = new s_Pos<int>(0, 0);
        e_TrapType Item = 0;

        public Crates(s_Pos<int> _Pos, e_TrapType _Item)
        {
            Pos = _Pos;
            Item = _Item;
        }


    }
}
