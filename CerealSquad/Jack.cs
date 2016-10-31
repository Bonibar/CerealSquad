using CerealSquad.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

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
            _inputPress[Keyboard.Key.A] = special_start;
            _inputRelease = new Dictionary<Key, functionMove>();
            _inputRelease[Keyboard.Key.Z] = move_up_release;
            _inputRelease[Keyboard.Key.Q] = move_left_release;
            _inputRelease[Keyboard.Key.S] = move_down_release;
            _inputRelease[Keyboard.Key.D] = move_right_release;
            _inputRelease[Keyboard.Key.A] = special_end;
            _ressources = new SFMLImplementation.EntityResources("jack", 32, 32);
        }

        public override void AttaqueSpe()
        {
            _weight = 10;
        }

        public override void update(Time deltaTime)
        {
            _weight = 1;
            base.update(deltaTime);
        }

        public override EName getName()
        {
            return EName.Jack;
        }
    }
}
