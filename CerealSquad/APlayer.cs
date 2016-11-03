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
        protected bool _specialActive;
        protected int _weight;

        protected struct s_input
        {
            public bool _isRightPressed;
            public bool _isLeftPressed;
            public bool _isUpPressed;
            public bool _isDownPressed;
        }

        protected s_input _playerInput;

        public int Weight
        {
            get
            {
                if (_die)
                    return 0;
                return _weight;
            }

            set
            {
                _weight = value;
            }
        }

        public APlayer(IEntity owner, s_position position, InputManager input) : base(owner)
        {
            _pos = position;
            _type = e_EntityType.Player;
            input.KeyboardKeyPressed += thinkMove;
            input.KeyboardKeyReleased += thinkAction;
            _playerInput._isRightPressed = false;
            _playerInput._isLeftPressed = false;
            _playerInput._isUpPressed = false;
            _playerInput._isDownPressed = false;
            _specialActive = false;
            _weight = 1;
        }

        protected void special_end()
        {
            _specialActive = false;
        }

        protected void special_start()
        {
            _specialActive = true;
        }

        protected void move_up_release()
        {
            _playerInput._isUpPressed = false;
        }

        protected void move_down_release()
        {
            _playerInput._isDownPressed = false;
        }

        protected void move_right_release()
        {
            _playerInput._isRightPressed = false;
        }

        protected void move_left_release()
        {
            _playerInput._isLeftPressed = false;
        }

        protected void move_up()
        {
            _playerInput._isUpPressed = true;
        }

        protected void move_down()
        {
            _playerInput._isDownPressed = true;
        }

        protected void move_left()
        {
            _playerInput._isLeftPressed = true;
        }

        protected void move_right()
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
            if (!_die)
            {
                if (_specialActive)
                    AttaqueSpe();
                move();
            }
            else
            {
                if (_ressources.isFinished())
                    destroy();
            }
            _ressources.update(deltaTime);
        }

        public abstract void AttaqueSpe();

        //
        // EName must be consecutive int start at 0 for using inside a list
        //
        public enum EName
        {
            Jack = 0,
            Orangina = 1,
            Mike = 2,
            Tchong = 3
        }

        public abstract EName getName();
    }
}
