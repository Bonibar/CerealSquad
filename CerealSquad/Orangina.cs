using CerealSquad.Graphics;
using CerealSquad.InputManager.Keyboard;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class Orangina : APlayer
    {
        public Orangina(IEntity owner, s_position position, InputManager.InputManager input) : base(owner, position, input)
        {
            _speed = 1;
            _inputPress = new Dictionary<Key, functionMove>();
            _inputPress[InputManager.Keyboard.Key.Up] = move_up;
            _inputPress[InputManager.Keyboard.Key.Left] = move_left;
            _inputPress[InputManager.Keyboard.Key.Down] = move_down;
            _inputPress[InputManager.Keyboard.Key.Right] = move_right;
            _inputRelease = new Dictionary<Key, functionMove>();
            _inputRelease[InputManager.Keyboard.Key.Up] = move_up_release;
            _inputRelease[InputManager.Keyboard.Key.Left] = move_left_release;
            _inputRelease[InputManager.Keyboard.Key.Down] = move_down_release;
            _inputRelease[InputManager.Keyboard.Key.Right] = move_right_release;
            Factories.TextureFactory.Instance.load("orangina", "Assets/Character/orangina.png");
            _ressources = new EntityResources();
            _ressources.InitializationAnimatedSprite(new Vector2u(32, 32));
            Vector2f pos = _ressources.Position;
            pos.X = position._x * 32;
            pos.Y = position._y * 32;
            _ressources.Position = pos;
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
