using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{

    abstract class AEntity : IEntity
    {
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
            public int _layer;

            s_position(int x = -1, int y = -1, int layer = -1)
            {
                _x = x;
                _y = y;
                _layer = layer;
            }

            public static s_position operator +(s_position pos, s_position other)
            {
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

        private s_position Pos
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

        public AEntity(IEntity owner, s_size size = new s_size())
        {
            _owner = owner;
            _children = new List<IEntity>();
            _type = e_EntityType.World;
            _damageType = e_DamageType.NONE;
            _size = size;
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
        public void move(s_position pos/* , Map map*/)
        {
            // TODO add the check of the map
            _pos += pos;
        }

        public abstract void update();
    }
}
