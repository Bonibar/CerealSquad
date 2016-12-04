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

namespace CerealSquad.EntitySystem
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
        private bool _moveTo;
        private s_position _moveToPos;
        public bool FinishedMovement;
        private bool _center;
        private scentMap _scentMap;

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
            _moveTo = false;
            FinishedMovement = true;
            _center = false;

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
            _CollidingType.Add(e_EntityType.ProjectileEnemy);
            _CollidingType.Add(e_EntityType.EnnemyTrap);
        }

        private void Input_KeyboardKeyReleased(object source, KeyEventArgs e)
        {
            if (!BlockInputs)
            {
                SKeyPlayer action = (SKeyPlayer)InputManager.GetAssociateFunction(Id, CerealSquad.InputManager.Player.Type.Keyboard, ((int)e.KeyCode));

                switch (action)
                {
                    case SKeyPlayer.MOVE_UP:
                        while (MoveStack.Contains(EMovement.Up))
                            MoveStack.Remove(EMovement.Up);
                        break;
                    case SKeyPlayer.MOVE_DOWN:
                        while (MoveStack.Contains(EMovement.Down))
                            MoveStack.Remove(EMovement.Down);
                        break;
                    case SKeyPlayer.MOVE_LEFT:
                        while (MoveStack.Contains(EMovement.Left))
                            MoveStack.Remove(EMovement.Left);
                        break;
                    case SKeyPlayer.MOVE_RIGHT:
                        while (MoveStack.Contains(EMovement.Right))
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
            if (!BlockInputs && e.JoystickId == Id)
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
            if (!BlockInputs && e.JoystickId == Id)
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
            if (!BlockInputs && e.JoystickId == Id)
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

        public void CancelTrapDelivery()
        {
            TrapPressed = false;
            TrapDeliver.Cancel();
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
            if (!_moveTo)
            {
                BlockInputs = true;
                _speed = Math.Abs(pos.X - _pos.X) + Math.Abs(pos.Y - _pos.Y) / 4;
                _moveTo = true;
                _moveToPos = pos;
                _scentMap = new scentMap(40, 40, this);
                FinishedMovement = false;
            }
        }

        public void moveToPos(AWorld world, SFML.System.Time deltaTime)
        {
            if (_center == true)
            {
                if (Math.Abs(HitboxPos.X - _moveToPos.X) < 0.1 && Math.Abs(HitboxPos.Y - _moveToPos.Y) < 0.1)
                {
                    MoveStack.Clear();
                    _moveTo = false;
                    _center = false;
                    BlockInputs = false;
                    FinishedMovement = true;
                    _speed = 5;
                    _ressources.PlayAnimation(0);
                }
                else
                {
                    MoveStack.Clear();
                    if (Math.Abs(HitboxPos.X - _moveToPos.X) >= 0.1)
                    {
                        if (HitboxPos.X - _moveToPos.X > 0)
                            MoveStack.Add(EMovement.Left);
                        else
                            MoveStack.Add(EMovement.Right);
                    }
                    else
                    {
                        if (HitboxPos.Y - _moveToPos.Y > 0)
                            MoveStack.Add(EMovement.Up);
                        else
                            MoveStack.Add(EMovement.Down);
                    }

                }
            }
            else
            {
                _scentMap.update((WorldEntity)_owner, world, _moveToPos);
                double speedMove = _speed * deltaTime.AsSeconds();
                _move = new List<EMovement> { EMovement.Right, EMovement.Left, EMovement.Down, EMovement.Up };
                int top = executeUpMove(world, speedMove) ? _scentMap.getScent(20, 19) : -1;
                int bottom = executeDownMove(world, speedMove) ? _scentMap.getScent(20, 21) : -1;
                int right = executeRightMove(world, speedMove) ? _scentMap.getScent(21, 20) : -1;
                int left = executeLeftMove(world, speedMove) ? _scentMap.getScent(19, 20) : -1;
                int max = Math.Max(top, Math.Max(bottom, Math.Max(right, left)));
                MoveStack.Clear();

                if (top == bottom && left == right && top == left)
                {
                    _center = true;
                }
                else if (max == top)
                    MoveStack.Add(EMovement.Up);
                else if (max == bottom)
                    MoveStack.Add(EMovement.Down);
                else if (max == right)
                    MoveStack.Add(EMovement.Right);
                else if (max == left)
                    MoveStack.Add(EMovement.Left);
            }
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            if (!_die)
            {
                if (_specialActive)
                    AttaqueSpe();
                if (TrapTrigger)
                    triggerTrap();
                if (_moveTo)
                    moveToPos(world, deltaTime);
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

        protected override void IsTouchingHitBoxEntities(AWorld world, List<AEntity> touchingEntities)
        {
            touchingEntities.ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.ProjectileEnemy)
                    attemptDamage(i, i.getDamageType());
                i.attemptDamage(this, _damageType);
            });
        }

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
                } else if (i.getEntityType() == e_EntityType.ProjectileEnemy)
                    attemptDamage(i, i.getDamageType());
                i.attemptDamage(this, _damageType);
            });

            return result || baseResult;
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage)
        {
            bool result = false;

            switch(damage)
            {
                case e_DamageType.ENEMY_DAMAGE:
                case e_DamageType.PROJECTILE_ENEMY_DAMAGE:
                case e_DamageType.COFFE_DAMAGE:
                    die();
                    result = true;
                    break;
                   
            }

            return result;
        }

        private class scentMap
        {
            protected int[][] _map;
            protected uint _x;
            protected uint _y;
            protected IEntity _player;

            public int[][] Map
            {
                get
                {
                    return _map;
                }

                set
                {
                    _map = value;
                }
            }

            public scentMap(uint x, uint y, IEntity player)
            {
                _x = x;
                _y = y;
                _player = player;
            }

            protected void reset(AWorld world)
            {
                int baseX = (int)_player.HitboxPos.X - 20;
                int baseY = (int)_player.HitboxPos.Y - 20;
                _map = new int[_x][];
                for (int i = 0; i < _x; i++)
                {
                    _map[i] = new int[_y];
                    for (int j = 0; j < _y; j++)
                    {
                        if (world.getPosition(baseX + i, baseY + j) == RoomParser.e_CellType.Normal || world.getPosition(baseX + i, baseY + j) == RoomParser.e_CellType.Door || world.getPosition(baseX + i, baseY + j) == RoomParser.e_CellType.Spawn)
                        {
                            _map[i][j] = 0;
                        }
                        else
                        {
                            _map[i][j] = -1;
                        }
                    }
                }
            }

            protected virtual void check_obstacle(WorldEntity world)
            {
                foreach (IEntity entity in world.getChildren())
                {
                    if (entity.getEntityType() == e_EntityType.PlayerTrap && ((ATrap)entity).TrapType == e_TrapType.WALL)
                    {
                        _map[(int)entity.Pos.X][(int)entity.Pos.Y] = -1;
                    }
                }
            }

            public void propagateHeat(int x, int y, int intensity)
            {
                if (x >= 0 && x < _x && y >= 0 && y < _y && _map[x][y] != -1 && _map[x][y] < intensity)
                {
                    _map[x][y] = intensity;
                    if (intensity > 1)
                    {
                        propagateHeat(x - 1, y, intensity - 1);
                        propagateHeat(x + 1, y, intensity - 1);
                        propagateHeat(x, y - 1, intensity - 1);
                        propagateHeat(x, y + 1, intensity - 1);
                    }
                }
            }

            public void update(WorldEntity worldEntity, AWorld world, s_position moveToPos)
            {
                reset(world);
                check_obstacle(worldEntity);
                propagateHeat(20 - (int)_player.HitboxPos.X + (int)moveToPos.X, 20 - (int)_player.HitboxPos.Y + (int)moveToPos.Y, 200);
            }

            public virtual int getScent(int x, int y)
            {
                if (x >= 0 && x < _x && y >= 0 && y < _y)
                {
                    int scent = 0;
                    scent += _map[x][y];
                    if (scent < 0)
                        scent = -1;
                    return (scent);
                }
                return (-1);
            }

            public void dump()
            {
                for (int y = 0; y < _y; y++)
                {
                    for (int x = 0; x < _x; x++)
                    {
                        System.Console.Out.Write(getScent(x, y));
                        System.Console.Out.Write(" ");
                    }
                    Console.Out.Write('\n');
                }
                Console.Out.Write('\n');
            }
        }
    }
}
