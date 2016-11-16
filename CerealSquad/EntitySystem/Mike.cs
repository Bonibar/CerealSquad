using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class Mike : APlayer
    {
        public Mike(IEntity owner, s_position position, InputManager.InputManager input, int type = 0, int id = 1) : base(owner, position, input, type, id)
        {
        }

        public override void AttaqueSpe()
        {
            throw new NotImplementedException();
        }

        public override EName getName()
        {
            return EName.Jack;
        }
    }
}
