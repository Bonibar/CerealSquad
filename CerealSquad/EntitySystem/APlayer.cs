using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.InputManager.Keyboard;
using CerealSquad.GameWorld;
using SFML.Graphics;
using CerealSquad.EntitySystem;

namespace CerealSquad
{
    abstract class APlayer : AEntity
    {
        protected delegate void functionMove();

        protected Dictionary<Key, functionMove> _inputPress;
        protected Dictionary<Key, functionMove> _inputRelease;
        protected bool _specialActive;
        protected int _weight;

        public EntitySystem.TrapDeliver TrapDeliver { get; protected set; }
        public e_TrapType TrapInventory { get; set; }

        public List<EMovement> MoveStack = new List<EMovement>();
        public bool TrapPressed = false;

        protected enum ETrapPuting
        {
            NO_PUTTING = 0,
            START_SELECTING = 1,
            SELECTING = 2,
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

        public APlayer(IEntity owner, s_position position, InputManager.InputManager input) : base(owner)
        {
            _pos = position;
            _type = e_EntityType.Player;
            input.KeyboardKeyPressed += thinkMove;
            input.KeyboardKeyReleased += thinkAction;
            _specialActive = false;
            _weight = 1;
            TrapDeliver = new EntitySystem.TrapDeliver(this);

            // for test, add Trap to inventory
            TrapInventory = e_TrapType.BOMB;
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
            MoveStack.Remove(EMovement.Up);
        }

        protected void move_down_release()
        {
            MoveStack.Remove(EMovement.Down);
        }

        protected void move_right_release()
        {
            MoveStack.Remove(EMovement.Right);
        }

        protected void move_left_release()
        {
            MoveStack.Remove(EMovement.Left);
        }

        protected void put_trap_release()
        {
            TrapPressed = false;
        }

        protected void move_up()
        {
            MoveStack.Add(EMovement.Up);
        }

        protected void move_down()
        {
            MoveStack.Add(EMovement.Down);
        }

        protected void move_left()
        {
            MoveStack.Add(EMovement.Left);
        }

        protected void move_right()
        {
            MoveStack.Add(EMovement.Right);
        }

        protected void put_trap()
        {
            TrapPressed = true;
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

        public override void move(AWorld world, SFML.System.Time deltaTime)
        {
            if (TrapPressed || !TrapDeliver.IsNotDelivering())
                _move = EMovement.None;
            else if (MoveStack.Count > 0)
                _move = MoveStack.ElementAt(MoveStack.Count - 1);
            else
                _move = EMovement.None;
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
            _children.ToList<IEntity>().ForEach(i => i.update(deltaTime, world));
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
                else if (i.getEntityType() == e_EntityType.Crate)
                {
                    TrapInventory = ((Crates)i).Item;
                    ((Crates)i).pickCrate();
                }
            });

            return result || baseResult;
        }
    }
}
