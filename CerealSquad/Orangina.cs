using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class Orangina : APlayer
    {
        public Orangina(IEntity owner, s_position position, InputManager input) : base(owner, position, input)
        {
            _speed = 1;
            _ressources = new SFMLImplementation.EntityResources("characterTest", 32, 32);
            _ressources.playAnimation(SFMLImplementation.EntityResources.EState.WALKING_RIGHT);
        }

        public override void AttaqueSpe()
        {
            throw new NotImplementedException();
        }
    }
}
