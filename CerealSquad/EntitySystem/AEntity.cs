using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{

    abstract class AEntity : IEntity
    {
        protected enum EMovement
        {
            Up,
            Down,
            Right,
            Left,
            None
        }

        protected IEntity _owner;
        protected ICollection<IEntity> _children;
        protected e_EntityType _type;
        protected e_DamageType _damageType;
        protected s_position _pos;
        protected s_size _size;
        protected double _speed;
        protected bool _die;
        protected EMovement _move;
        protected EntityResources _ressources;

        public s_position Pos
        {
            get
            {
                return _pos;
            }

            set
            {
                _pos = value;
            }
        }
        
        public double Speed
        {
            get
            {
                return _speed;
            }

            set
            {
                _speed = value;
            }
        }

        public EntityResources ressourcesEntity
        {
            get
            {
                return _ressources;
            }

            set
            {
                _ressources = value;
            }
        }

        public s_size Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public bool Die
        {
            get
            {
                return _die;
            }

            set
            {
                _die = value;
            }
        }

        public AEntity(IEntity owner, s_size size = new s_size())
        {
            _owner = owner;
            if (owner != null)
                _owner.addChild(this);
            _children = new List<IEntity>();
            _type = e_EntityType.World;
            _damageType = e_DamageType.NONE;
            _size = size;
            _speed = 0;
            _die = false;
            _move = EMovement.None;
        }

        public void addChild(IEntity child)
        {
            _children.Add(child);
        }

        public bool attemptDamage(IEntity Sender, e_DamageType damage)
        {
            return false;
        }

        public ICollection<IEntity> getChildren()
        {
            return (_children);
        }

        public e_DamageType getDamageType()
        {
            return (_damageType);
        }

        public e_EntityType getEntityType()
        {
            return _type;
        }

        public IEntity getOwner()
        {
            return (_owner);
        }

        public bool removeChild(IEntity child)
        {
            return (_children.Remove(child));
        }

        // Use this function for moving the entity whitout his action(ex: knockback)
        // Move the entity relative to his actual position
        public virtual void move(AWorld world)
        {
            var ressourcePos = _ressources.Position;
            s_position newPos = _pos;
            s_position colPos = _pos;
            EStateEntity anim = EStateEntity.IDLE;
            switch (_move)
            {
                case EMovement.Up:
                    newPos += new s_position(0, -_speed, 0);
                    colPos = newPos;
                    colPos += new s_position(0.99, 0, 0);
                    anim = EStateEntity.WALKING_UP;
                    ressourcePos.Y -= 64 * (float)_speed;
                    break;
                case EMovement.Down:
                    newPos += new s_position(0, +_speed, 0);
                    colPos = newPos;
                    colPos += new s_position(0.99, 0.99, 0);
                    anim = EStateEntity.WALKING_DOWN;
                    ressourcePos.Y += 64 * (float)_speed;
                    break;
                case EMovement.Right:
                    newPos += new s_position(_speed, 0, 0);
                    colPos = newPos;
                    colPos += new s_position(0.99, 0, 0);
                    anim = EStateEntity.WALKING_RIGHT;
                    ressourcePos.X += 64 * (float)_speed;
                    break;
                case EMovement.Left:
                    newPos += new s_position(-_speed, 0, 0);
                    colPos = newPos;
                    anim = EStateEntity.WALKING_LEFT;
                    ressourcePos.X -= 64 * (float)_speed;
                    break;
                case EMovement.None:
                    _ressources.PlayAnimation(EStateEntity.IDLE);
                    return;
            }
            if (world.getPosition(colPos._x, colPos._y) == RoomParser.e_CellType.Normal)
            {
                _pos = newPos;
                _ressources.Position = ressourcePos;
                _ressources.PlayAnimation(anim);
            }
        }

        public abstract void update(SFML.System.Time deltaTime, AWorld world);

        public void die()
        {
            _die = true;
            _ressources.PlayAnimation(EStateEntity.DYING);
            _ressources.Loop = false;
        }

        // destroy the object
        // Should be call after the end of death animation
        public void destroy()
        {
            _owner.removeChild(this);
        }
    }
}
