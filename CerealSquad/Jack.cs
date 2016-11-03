using CerealSquad.InputManager.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CerealSquad.Graphics;

namespace CerealSquad
{
    class Jack : APlayer
    {
        public Jack(IEntity owner, s_position position, InputManager.InputManager input) : base(owner, position, input)
        {
            _speed = 0.5;
            _inputPress = new Dictionary<Key, functionMove>();
            _inputPress[InputManager.Keyboard.Key.Z] = move_up;
            _inputPress[InputManager.Keyboard.Key.Q] = move_left;
            _inputPress[InputManager.Keyboard.Key.S] = move_down;
            _inputPress[InputManager.Keyboard.Key.D] = move_right;
            _inputPress[InputManager.Keyboard.Key.A] = special_start;
            _inputRelease = new Dictionary<Key, functionMove>();
            _inputRelease[InputManager.Keyboard.Key.Z] = move_up_release;
            _inputRelease[InputManager.Keyboard.Key.Q] = move_left_release;
            _inputRelease[InputManager.Keyboard.Key.S] = move_down_release;
            _inputRelease[InputManager.Keyboard.Key.D] = move_right_release;
            _inputRelease[InputManager.Keyboard.Key.A] = special_end;
            _ressources = new EntityResources();
            TextureFactory.Instance.load("jack", "jack.png");
            _ressources.InitializationAnimatedSprite("jack", new Vector2i(32, 32));
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
