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


        // TODO Add save of the input

        // TODO add the sprite dictionnary

        // TODO Add InputManager
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
        }

        public void move_up_release()
        {
            if (_move == EMovement.Up)
                _move = EMovement.None;
        }

        public void move_down_release()
        {
            if (_move == EMovement.Down)
                _move = EMovement.None;
        }

        public void move_right_release()
        {
            if (_move == EMovement.Right)
                _move = EMovement.None;
        }

        public void move_left_release()
        {
            if (_move == EMovement.Left)
                _move = EMovement.None;
        }

        public void move_up()
        {
            _move = EMovement.Up;
        }

        public void move_down()
        {
            _move = EMovement.Down;
        }

        public void move_left()
        {
            _move = EMovement.Left;
        }

        public void move_right()
        {
            _move = EMovement.Right;
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

        public override void update()
        {
            move();
        }

        public abstract void AttaqueSpe();
    }
}
