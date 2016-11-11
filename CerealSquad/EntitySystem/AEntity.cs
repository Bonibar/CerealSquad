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
        public enum EMovement
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
                setResourceEntityPosition();
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

        public bool attemptDamage(IEntity Sender, e_DamageType damage, float Range)
        {
            double Distance = Math.Sqrt(Math.Pow(Sender.Pos._trueX - Pos._trueX, 2.0f) + Math.Pow(Sender.Pos._trueY - Pos._trueY, 2.0f));

            if (Distance > Range)
                return false;

            if ((getEntityType() == e_EntityType.EnnemyTrap || getEntityType() == e_EntityType.PlayerTrap)
                && !((ATrap)this).Triggered)
                ((ATrap)this).Trigger();

            return true;
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

        public bool IsColliding(AWorld world, EntityResources Res)
        {
            if (world.IsCollidingWithWall(Res))
                return true;
            return false;
        }

        // Use this function for moving the entity whitout his action(ex: knockback)
        // Move the entity relative to his actual position
        public virtual void move(AWorld world, SFML.System.Time deltaTime)
        {
            EStateEntity anim = EStateEntity.IDLE;
            var OldResourcePosition = _ressources.Position;
            s_position NewPosition = _pos;

            double speedMove = _speed * deltaTime.AsSeconds();

            switch (_move)
            {
                case EMovement.Up:
                    NewPosition += new s_position(0, -speedMove, 0);
                    anim = EStateEntity.WALKING_UP;
                    break;
                case EMovement.Down:
                    NewPosition += new s_position(0, +speedMove, 0);
                    anim = EStateEntity.WALKING_DOWN;
                    break;
                case EMovement.Right:
                    NewPosition += new s_position(speedMove, 0, 0);
                    anim = EStateEntity.WALKING_RIGHT;
                    break;
                case EMovement.Left:
                    NewPosition += new s_position(-speedMove, 0, 0);
                    anim = EStateEntity.WALKING_LEFT;
                    break;
                case EMovement.None:
                    _ressources.PlayAnimation(EStateEntity.IDLE);
                    return;
            }

            _ressources.PlayAnimation(anim);
            _ressources.Position = new SFML.System.Vector2f((float)NewPosition._trueX * 64, (float)NewPosition._trueY * 64);

           if (!IsColliding(world, ressourcesEntity))
                _pos = NewPosition;
            else
                _ressources.Position = OldResourcePosition;
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

        public IEntity getRootEntity()
        {
            IEntity parent = this;

            while (parent.getOwner() != null)
                parent = parent.getOwner();

            return parent;
        }

        private void setResourceEntityPosition()
        {
            ressourcesEntity.Position = new SFML.System.Vector2f((float)Pos._trueX + ressourcesEntity.Size.X / 2.0f, (float)Pos._trueY + ressourcesEntity.Size.Y / 2.0f);
        }
    }
}
