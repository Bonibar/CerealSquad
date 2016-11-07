using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class Tchong : APlayer
    {
        public Tchong(IEntity owner, s_position position, InputManager.InputManager input) : base(owner, position, input)
        {
        }

        public override void AttaqueSpe()
        {
            throw new NotImplementedException();
        }

        public override EName getName()
        {
            return EName.Tchong;
        }
    }
}
