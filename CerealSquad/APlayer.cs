using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.Keyboard;

namespace CerealSquad
{
    abstract class APlayer : AEntity
    {
        protected delegate void functionMove();

        protected Dictionary<Key, functionMove> _inputPress;
        protected Dictionary<Key, functionMove> _inputRelease;
        
        protected struct s_input
        {
            public bool _isRightPressed;
            public bool _isLeftPressed;
            public bool _isUpPressed;
            public bool _isDownPressed;
        }

        protected s_input _playerInput;

        public APlayer(IEntity owner, s_position position, InputManager input) : base(owner)
        {
            _pos = position;
            _type = e_EntityType.Player;
            input.KeyboardKeyPressed += thinkMove;
            input.KeyboardKeyReleased += thinkAction;
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
            _playerInput._isRightPressed = false;
            _playerInput._isLeftPressed = false;
            _playerInput._isUpPressed = false;
            _playerInput._isDownPressed = false;
        }

        public void move_up_release()
        {
            _playerInput._isUpPressed = false;
        }

        public void move_down_release()
        {
            _playerInput._isDownPressed = false;
        }

        public void move_right_release()
        {
            _playerInput._isRightPressed = false;
        }

        public void move_left_release()
        {
            _playerInput._isLeftPressed = false;
        }

        public void move_up()
        {
            _playerInput._isUpPressed = true;
        }

        public void move_down()
        {
            _playerInput._isDownPressed = true;
        }

        public void move_left()
        {
            _playerInput._isLeftPressed = true;
        }

        public void move_right()
        {
            _playerInput._isRightPressed = true;
        }

        private void thinkMove(object source, KeyEventArgs e)
        {
            if (_inputPress.ContainsKey(e.KeyCode))
                _inputPress[e.KeyCode]();
        }

        private void thinkAction(object source, KeyEventArgs e)
        {
            if (_inputRelease.ContainsKey(e.KeyCode))
                _inputRelease[e.KeyCode]();
        }

        public override void move()
        {
            if (_playerInput._isRightPressed && !_playerInput._isLeftPressed && !_playerInput._isDownPressed && !_playerInput._isUpPressed)
                _move = EMovement.Right;
            else if (!_playerInput._isRightPressed && _playerInput._isLeftPressed && !_playerInput._isDownPressed && !_playerInput._isUpPressed)
                _move = EMovement.Left;
            else if (!_playerInput._isRightPressed && !_playerInput._isLeftPressed && _playerInput._isDownPressed && !_playerInput._isUpPressed)
                _move = EMovement.Down;
            else if (!_playerInput._isRightPressed && !_playerInput._isLeftPressed && !_playerInput._isDownPressed && _playerInput._isUpPressed)
                _move = EMovement.Up;
            else
                _move = EMovement.None;
            base.move();
        }

        public override void update(SFML.System.Time deltaTime)
        {
            move();
            _ressources.update(deltaTime);
        }

        public abstract void AttaqueSpe();
    }
}
