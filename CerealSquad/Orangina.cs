using CerealSquad.Keyboard;
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
            _inputPress = new Dictionary<Key, functionMove>();
            _inputPress[Keyboard.Key.Up] = move_up;
            _inputPress[Keyboard.Key.Left] = move_left;
            _inputPress[Keyboard.Key.Down] = move_down;
            _inputPress[Keyboard.Key.Right] = move_right;
            _inputRelease = new Dictionary<Key, functionMove>();
            _inputRelease[Keyboard.Key.Up] = move_up_release;
            _inputRelease[Keyboard.Key.Left] = move_left_release;
            _inputRelease[Keyboard.Key.Down] = move_down_release;
            _inputRelease[Keyboard.Key.Right] = move_right_release;
            _ressources = new SFMLImplementation.EntityResources("orangina", 32, 32);
        }

        public override void AttaqueSpe()
        {
            throw new NotImplementedException();
        }

        public override EName getName()
        {
            return EName.Orangina;
        }
    }
}
