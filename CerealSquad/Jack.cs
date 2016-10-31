using CerealSquad.Keyboard;
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
            _speed = 0.5;
            _inputPress = new Dictionary<Key, functionMove>();
            _inputPress[Keyboard.Key.Z] = move_up;
            _inputPress[Keyboard.Key.Q] = move_left;
            _inputPress[Keyboard.Key.S] = move_down;
            _inputPress[Keyboard.Key.D] = move_right;
            _inputRelease = new Dictionary<Key, functionMove>();
            _inputRelease[Keyboard.Key.Z] = move_up_release;
            _inputRelease[Keyboard.Key.Q] = move_left_release;
            _inputRelease[Keyboard.Key.S] = move_down_release;
            _inputRelease[Keyboard.Key.D] = move_right_release;
            _ressources = new SFMLImplementation.EntityResources("jack", 32, 32);
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
