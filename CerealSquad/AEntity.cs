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

        public struct s_size
        {
            public int _width;
            public int _length;

            s_size(int width = 1, int length = 1)
            {
                _width = width;
                _length = length;
            }
        }

        public struct s_position
        {
            public int _x;
            public int _y;
            public double _trueX;
            public double _trueY;
            public int _layer;

            public s_position(double x = -1, double y = -1, int layer = -1)
            {
                _x = (int)x;
                _trueX = x;
                _y = (int)y;
                _trueY = y;
                _layer = layer;
            }

            public static s_position operator +(s_position pos, s_position other)
            {
                pos._trueX += other._trueX;
                pos._trueY += other._trueY;
                pos._x = (int)pos._trueX;
                pos._y = (int)pos._trueY;
                pos._x += other._x;
                pos._y += other._y;
                pos._layer += other._layer;

                return (pos);
            }
          
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
        public AEntity(IEntity owner, s_size size = new s_size())
        {
            _owner = owner;
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
        public void move(/* , Map map*/)
        {
            switch (_move)
            {
                case EMovement.Up:
                    _pos += new s_position(0, _speed, 0);
                    break;
                case EMovement.Down:
                    _pos += new s_position(0, -_speed, 0);
                    break;
                case EMovement.Right:
                    _pos += new s_position(_speed, 0, 0);
                    break;
                case EMovement.Left:
                    _pos += new s_position(-_speed, 0, 0);
                    break;
            }
        }

        public abstract void update();

        public void die()
        {
            _die = true;
            //Launch dying animation
        }

        // destroy the object
        // Should be call after the end of death animation
        public void destroy()
        {
            _owner.removeChild(this);
        }
    }
}
