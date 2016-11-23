using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.InputManager.Keyboard;
using CerealSquad.GameWorld;
using SFML.Graphics;
using CerealSquad.EntitySystem;
using CerealSquad.EntitySystem.Projectiles;
using CerealSquad.Graphics;

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
            MENU,
            TRIGGER_TRAP,
        };

        protected InputManager.InputManager InputManager;

        protected bool _specialActive;
        protected int _weight;

        public TrapDeliver TrapDeliver { get; protected set; }
        public e_TrapType TrapInventory { get; set; }

        public List<EMovement> MoveStack = new List<EMovement>();
        public bool TrapPressed = false;
        public bool TrapTrigger = false;
        private IEntity owner;
        private s_position position;

        public int TypeInput { get; set; }
        public int Id { get; protected set; }

        public bool BlockInputs = false;
        private bool _center;
        private bool _moveTo;
        private s_position _moveToPos;
        public bool FinishedMovement;

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
            _center = false;
            _moveTo = false;
            FinishedMovement = true;

            _specialActive = false;
            _weight = 1;
            TrapDeliver = new TrapDeliver(this);

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
            
            TrapInventory = e_TrapType.NONE;
            InputManager = input;
            _CollidingType.Add(e_EntityType.Ennemy);
        }

        private void Input_KeyboardKeyReleased(object source, KeyEventArgs e)
        {
            if (!BlockInputs)
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
                    case SKeyPlayer.SPATTACK:
                        _specialActive = false;
                        break;
                    case SKeyPlayer.TRIGGER_TRAP:
                        TrapTrigger = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void Input_KeyboardKeyPressed(object source, KeyEventArgs e)
        {
            if (!BlockInputs)
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
                    case SKeyPlayer.SPATTACK:
                        _specialActive = true;
                        break;
                    case SKeyPlayer.TRIGGER_TRAP:
                        TrapTrigger = true;
                        break;
                    default:
                        break;
                }
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
            if (!BlockInputs)
            {
                SKeyPlayer action = (SKeyPlayer)InputManager.GetAssociateFunction(Id, CerealSquad.InputManager.Player.Type.Controller, ((int)e.Button), false);

                switch (action)
                {
                    case SKeyPlayer.PUT_TRAP:
                        TrapPressed = false;
                        break;
                    case SKeyPlayer.TRIGGER_TRAP:
                        TrapTrigger = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void Input_JoystickButtonPressed(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            if (!BlockInputs)
            {
                SKeyPlayer action = (SKeyPlayer)InputManager.GetAssociateFunction(Id, CerealSquad.InputManager.Player.Type.Controller, ((int)e.Button), false);

                switch (action)
                {
                    case SKeyPlayer.PUT_TRAP:
                        TrapPressed = true;
                        break;
                    case SKeyPlayer.TRIGGER_TRAP:
                        TrapTrigger = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void Input_JoystickMoved(object source, InputManager.Joystick.MoveEventArgs e)
        {
            if (!BlockInputs)
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
        }

        public override void move(AWorld world, SFML.System.Time deltaTime)
        {
            if ((TrapPressed || TrapDeliver.IsDelivering()) && TrapInventory != e_TrapType.NONE)
                _move = new List<EMovement> { EMovement.None };
            else
                _move = MoveStack;
        
            base.move(world, deltaTime);
        }

        public void moveTo(s_position pos)
        {
            BlockInputs = true;
            _speed = 1;
            _moveTo = true;
            _moveToPos = pos;
            FinishedMovement = false;
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            if (!_die)
            {
                if (_specialActive)
                    AttaqueSpe();
                if (_center)
                    center();
                if (_moveTo)
                    moveToPos();
                if (TrapTrigger)
                    triggerTrap();
                move(world, deltaTime);
                TrapDeliver.Update(deltaTime, world, MoveStack, TrapPressed);
            }
            else
            {
                die();
                if (_ressources.isFinished())
                    destroy();
            }
            _ressources.Update(deltaTime);
            _children.ToList().ForEach(i => i.update(deltaTime, world));
        }

        private void center()
        {
            if (Math.Abs(HitboxPos._trueX - _moveToPos._x - 0.5) < 0.1 && Math.Abs(HitboxPos._trueY - _moveToPos._y - 0.5) < 0.1)
            {
                _center = false;
                BlockInputs = false;
                MoveStack.Clear();
                _ressources.PlayAnimation((uint)EStateEntity.IDLE);
                FinishedMovement = true;
                _speed = 5;
            }
            else if (Math.Abs(HitboxPos._trueX - _moveToPos._x - 0.5) > 0.1)
            {
                if (HitboxPos._trueX - _moveToPos._x - 0.5 < 0)
                {
                    MoveStack.Clear();
                    MoveStack.Add(EMovement.Right);
                }
                else
                {
                    MoveStack.Clear();
                    MoveStack.Add(EMovement.Left);
                }
            }
            else
            {
                if (HitboxPos._trueY - _moveToPos._y - 0.5 < 0)
                {
                    MoveStack.Clear();
                    MoveStack.Add(EMovement.Down);
                }
                else
                {
                    MoveStack.Clear();
                    MoveStack.Add(EMovement.Up);
                }
            }
        }

        private void moveToPos()
        {
            if (Math.Abs(HitboxPos._x - _moveToPos._x) == 0 && Math.Abs(HitboxPos._y - _moveToPos._y) == 0)
            {
                    _moveTo = false;
                    _center = true;
            }
            else if (Math.Abs(HitboxPos._x - _moveToPos._x) != 0)
            {
                if (HitboxPos._trueX - _moveToPos._x < 0)
                {
                    MoveStack.Clear();
                    MoveStack.Add(EMovement.Right);
                }
                else
                {
                    MoveStack.Clear();
                    MoveStack.Add(EMovement.Left);
                }
            }
            else
            {
                if (HitboxPos._trueY - _moveToPos._y < 0)
                {
                    MoveStack.Clear();
                    MoveStack.Add(EMovement.Down);
                }
                else
                {
                    MoveStack.Clear();
                    MoveStack.Add(EMovement.Up);
                }
            }
        }

        public abstract void AttaqueSpe();

        private void triggerTrap()
        {
            _children.ToList().ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.PlayerTrap)
                    ((ATrap)i).Trigger();
            });
        }

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
                if (i.getEntityType() == e_EntityType.PlayerTrap && ((ATrap)i).TrapType == e_TrapType.WALL)
                    result = true;
                else if (i.getEntityType() == e_EntityType.Crate)
                {
                    TrapInventory = ((Crates)i).Item;
                    ((Crates)i).pickCrate();
                }
                i.attemptDamage(this, _damageType);
            });

            return result || baseResult;
        }

        public override void die()
        {
            base.die();
            ressourcesEntity.PlayAnimation((uint)EStateEntity.DYING);
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage)
        {
            bool result = false;

            switch(damage)
            {
                case e_DamageType.ENEMY_DAMAGE:
                case e_DamageType.PROJECTILE_ENEMY_DAMAGE:
                    die();
                    result = true;
                    break;
                   
            }

            return result;
        }
    }
}
