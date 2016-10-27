using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class Jack : APlayer
    {
        public Jack(IEntity owner, s_position position, InputManager input) : base(owner, position, input)
        {
        }

        public override void AttaqueSpe()
        {
            throw new NotImplementedException();
        }
    }
}
