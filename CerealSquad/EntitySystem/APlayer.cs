using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.InputManager.Keyboard;
using CerealSquad.GameWorld;
using SFML.Graphics;

namespace CerealSquad
{
    abstract class APlayer : AEntity
    {
        enum SKeyPlayer
        {
            UNKNOW = -1,
            MOVE_UP = 0,
            MOVE_DOWN,
            MOVE_RIGHT,
            MOVE_LEFT,
            PUT_TRAP,
            SPATTACK,
            MENU
        };

        protected InputManager.InputManager InputManager;

        protected bool _specialActive;
        protected int _weight;

        public EntitySystem.TrapDeliver TrapDeliver { get; protected set; }
        public e_TrapType TrapInventory { get; set; }

        public List<EMovement> MoveStack = new List<EMovement>();
        public bool TrapPressed = false;

        public int TypeInput { get; set; }
        public int Id { get; protected set; }

        protected enum ETrapPuting
        {
            NO_PUTTING = 0,
            START_SELECTING = 1,
            END_SELECTING = 3,
            PUTTING = 4
        }

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

        public APlayer(IEntity owner, s_position position, InputManager.InputManager input, int type = 0, int id = 1) : base(owner)
        {
            _pos = position;
            _type = e_EntityType.Player;

            _specialActive = false;
            _weight = 1;
            TrapDeliver = new EntitySystem.TrapDeliver(this);

            TypeInput = type;
            Id = id;
            if (TypeInput == 0)
            {
                input.KeyboardKeyPressed += Input_KeyboardKeyPressed;
                input.KeyboardKeyReleased += Input_KeyboardKeyReleased;
            }
            else
            {
                input.JoystickMoved += Input_JoystickMoved;
                input.JoystickButtonPressed += Input_JoystickButtonPressed;
                input.JoystickButtonReleased += Input_JoystickButtonReleased;
                input.JoystickConnected += Input_JoystickConnected;
                input.JoystickDisconnected += Input_JoystickDisconnected;
            }

            // for test, add Trap to inventory
            TrapInventory = e_TrapType.BOMB;
            InputManager = input;
        }

        private void Input_KeyboardKeyReleased(object source, KeyEventArgs e)
        {
            SKeyPlayer action = (SKeyPlayer)InputManager.GetAssociateFunction(Id, CerealSquad.InputManager.Player.Type.Keyboard, ((int)e.KeyCode));

            switch (action)
            {
                case SKeyPlayer.MOVE_UP:
                    MoveStack.Remove(EMovement.Up);
                    break;
                case SKeyPlayer.MOVE_DOWN:
                    MoveStack.Remove(EMovement.Down);
                    break;
                case SKeyPlayer.MOVE_LEFT:
                    MoveStack.Remove(EMovement.Left);
                    break;
                case SKeyPlayer.MOVE_RIGHT:
                    MoveStack.Remove(EMovement.Right);
                    break;
                case SKeyPlayer.PUT_TRAP:
                    TrapPressed = false;
                    break;
                default:
                    break;
            }
        }

        private void Input_KeyboardKeyPressed(object source, KeyEventArgs e)
        {
            SKeyPlayer action = (SKeyPlayer)InputManager.GetAssociateFunction(Id, CerealSquad.InputManager.Player.Type.Keyboard, ((int)e.KeyCode));

            switch (action)
            {
                case SKeyPlayer.MOVE_UP:
                    MoveStack.Add(EMovement.Up);
                    break;
                case SKeyPlayer.MOVE_DOWN:
                    MoveStack.Add(EMovement.Down);
                    break;
                case SKeyPlayer.MOVE_LEFT:
                    MoveStack.Add(EMovement.Left);
                    break;
                case SKeyPlayer.MOVE_RIGHT:
                    MoveStack.Add(EMovement.Right);
                    break;
                case SKeyPlayer.PUT_TRAP:
                    TrapPressed = true;
                    break;
                default:
                    break;
            }
        }

        private void Input_JoystickDisconnected(object source, InputManager.Joystick.ConnectionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Input_JoystickConnected(object source, InputManager.Joystick.ConnectionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Input_JoystickButtonReleased(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            SKeyPlayer action = (SKeyPlayer)InputManager.GetAssociateFunction(Id, CerealSquad.InputManager.Player.Type.Controller, ((int)e.Button), false);

            switch (action)
            {
                case SKeyPlayer.PUT_TRAP:
                    TrapPressed = false;
                    break;
                default:
                    break;
            }
        }

        private void Input_JoystickButtonPressed(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            SKeyPlayer action = (SKeyPlayer)InputManager.GetAssociateFunction(Id, CerealSquad.InputManager.Player.Type.Controller, ((int)e.Button), false);

            switch (action)
            {
                case SKeyPlayer.PUT_TRAP:
                    TrapPressed = true;
                    break;
                default:
                    break;
            }
        }

        private void Input_JoystickMoved(object source, InputManager.Joystick.MoveEventArgs e)
        {
            SKeyPlayer action = (SKeyPlayer)InputManager.GetAssociateFunction(Id, CerealSquad.InputManager.Player.Type.Controller, ((int)e.Axis), true);

            switch (action)
            {
                case SKeyPlayer.MOVE_UP:
                    MoveStack.Remove(EMovement.Up);
                    MoveStack.Remove(EMovement.Down);
                    if (e.Position > 30)
                        MoveStack.Add(EMovement.Down);
                    else if (e.Position < -30)
                        MoveStack.Add(EMovement.Up);
                    break;
                case SKeyPlayer.MOVE_LEFT:
                    MoveStack.Remove(EMovement.Left);
                    MoveStack.Remove(EMovement.Right);
                    if (e.Position > 30)
                        MoveStack.Add(EMovement.Right);
                    else if (e.Position < -30)
                        MoveStack.Add(EMovement.Left);
                    break;
                default:
                    break;
            }
        }

        public override void move(AWorld world, SFML.System.Time deltaTime)
        {
            if (TrapPressed || TrapDeliver.IsDelivering())
                _move = new List<EMovement> { EMovement.None };
            else
                _move = MoveStack;
        
            base.move(world, deltaTime);
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            if (!_die)
            {
                if (_specialActive)
                    AttaqueSpe();
                move(world, deltaTime);
                TrapDeliver.Update(deltaTime, world, MoveStack, TrapPressed);
            }
            else
            {
                if (_ressources.isFinished())
                    destroy();
            }
            _ressources.Update(deltaTime);
            _children.ToList().ForEach(i => i.update(deltaTime, world));
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

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool baseResult = base.IsCollidingEntity(World, CollidingEntities);
            bool result = false;
            CollidingEntities.ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.PlayerTrap)
                    result = true;
            });

            return result || baseResult;
        }

        public override bool IsCollidingAndDead(AWorld World)
        {
            bool result = false;
            List<AEntity> allEntities = ((WorldEntity)getRootEntity()).GetAllEntities();

            allEntities.ForEach(i =>
            {
                if (!i.Equals(this))
                    if (i.getEntityType() == e_EntityType.Ennemy)
                        result = true;
            });

            return result;
        }
    }
}
