using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.InputManager.Keyboard;

namespace CerealSquad
{
    abstract class APlayer : AEntity
    {
        protected delegate void functionMove();

        protected Dictionary<Key, functionMove> _inputPress;
        protected Dictionary<Key, functionMove> _inputRelease;
        protected bool _specialActive;
        protected int _weight;
        protected bool _isChoosingTarget;
        protected EMovement _targeting;

        protected struct s_input
        {
            public bool _isRightPressed;
            public bool _isLeftPressed;
            public bool _isUpPressed;
            public bool _isDownPressed;
            public bool _isTrapDownPressed;
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

        public APlayer(IEntity owner, s_position position, InputManager.InputManager input) : base(owner)
        {
            _pos = position;
            _type = e_EntityType.Player;
            input.KeyboardKeyPressed += thinkMove;
            input.KeyboardKeyReleased += thinkAction;
            _playerInput._isRightPressed = false;
            _playerInput._isLeftPressed = false;
            _playerInput._isUpPressed = false;
            _playerInput._isDownPressed = false;
            _playerInput._isTrapDownPressed = false;
            _specialActive = false;
            _weight = 1;
            _isChoosingTarget = false;
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

        protected void put_trap_release()
        {
            _playerInput._isTrapDownPressed = false;
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

        protected void put_trap()
        {
            _playerInput._isTrapDownPressed = true;
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
            if (_playerInput._isTrapDownPressed)
                _move = EMovement.None;
            else if (_playerInput._isRightPressed && !_playerInput._isLeftPressed && !_playerInput._isDownPressed && !_playerInput._isUpPressed)
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

        public void putTrap()
        {
            if (_playerInput._isTrapDownPressed && !_isChoosingTarget)
                _isChoosingTarget = true;
            else if (!_playerInput._isTrapDownPressed && _isChoosingTarget)
            {
                if (_playerInput._isRightPressed && !_playerInput._isLeftPressed && !_playerInput._isDownPressed && !_playerInput._isUpPressed)
                    _targeting = EMovement.Right;
                else if (!_playerInput._isRightPressed && _playerInput._isLeftPressed && !_playerInput._isDownPressed && !_playerInput._isUpPressed)
                    _targeting = EMovement.Left;
                else if (!_playerInput._isRightPressed && !_playerInput._isLeftPressed && _playerInput._isDownPressed && !_playerInput._isUpPressed)
                    _targeting = EMovement.Down;
                else if (!_playerInput._isRightPressed && !_playerInput._isLeftPressed && !_playerInput._isDownPressed && _playerInput._isUpPressed)
                    _targeting = EMovement.Up;
                else
                    _targeting = EMovement.None;

                if (_targeting != EMovement.None)
                {
                    ATrap trap = new TrapEntities.BearTrap(this);
                    trap.setPosition(_targeting);
                    addChild(trap);
                }
                _isChoosingTarget = false;
            }
        }

        public override void update(SFML.System.Time deltaTime)
        {
            if (!_die)
            {
                if (_specialActive)
                    AttaqueSpe();
                move();
                putTrap();
            }
            else
            {
                if (_ressources.isFinished())
                    destroy();
            }
            _ressources.Update(deltaTime);
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
