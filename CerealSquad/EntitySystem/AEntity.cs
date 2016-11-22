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
        protected List<EMovement> _move;
        protected float _inputForce;
        protected EntityResources _ressources;

        protected static bool m_debug = false; // Capitain obvious: use for the debug with breakpoint

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

        public s_position HitboxPos
        {
            get
            {
                return RessourceEntityPositionToEntityPosition(new SFML.System.Vector2f(ressourcesEntity.CollisionBox.Left + ressourcesEntity.CollisionBox.Width / 2, ressourcesEntity.CollisionBox.Top + ressourcesEntity.CollisionBox.Height / 2));
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
            _inputForce = 1;
            _die = false;
            _move = new List<EMovement> { EMovement.None };
        }

        public void addChild(IEntity child)
        {
            _children.Add(child);
        }

        private bool NotInCircleRange(IEntity Sender, float Range)
        {
            double Distance = Math.Sqrt(Math.Pow(Sender.Pos._trueX - Pos._trueX, 2.0f) + Math.Pow(Sender.Pos._trueY - Pos._trueY, 2.0f));
            if (ressourcesEntity != null)
                Distance -= ressourcesEntity.HitBox.Width / 64.0f / 2.0f;

            return !(Distance > Range);
        }

        public virtual bool attemptDamage(IEntity Sender, e_DamageType damage, float Range)
        {
            if (NotInCircleRange(Sender, Range))
                return false;

            if ((getEntityType() == e_EntityType.EnnemyTrap || getEntityType() == e_EntityType.PlayerTrap)
                && !((ATrap)this).Triggered)
                ((ATrap)this).Trigger();

            return true;
        }

        protected static bool IsInEllipse(double x_el, double y_el, double x, double y, double rX, double rY)
        {
            double a = Math.Pow(x - x_el, 2.0f) / Math.Pow(rX, 2.0f);
            double b = Math.Pow(y - y_el, 2.0f) / Math.Pow(rY, 2.0f);

            return (a + b) <= 1;
        }

        private bool NotInEllipseRange(IEntity Sender, float RadiusX, float RadiusY)
        {
            return !IsInEllipse(Sender.Pos._trueX, Sender.Pos._trueY, Pos._trueX, Pos._trueY, RadiusX, RadiusY);
        }

        public virtual bool attemptDamage(IEntity Sender, e_DamageType damage, float RadiusRangeX, float RadiusRangeY)
        {
            if (NotInEllipseRange(Sender, RadiusRangeX, RadiusRangeY))
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

        public virtual bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            return false;
        }

        public virtual bool IsCollidingWithWall(AWorld World, EntityResources Res)
        {
            return World.IsCollidingWithWall(ressourcesEntity);
        }

        public bool IsColliding(AWorld World)
        {
            if (ressourcesEntity == null)
                return false;

            if (IsCollidingWithWall(World, ressourcesEntity))
                return true;

            List<AEntity> collidingEntities = ((WorldEntity)getRootEntity()).GetCollidingEntities(ressourcesEntity);

            if (collidingEntities.Count == 0)
                return false;

            return IsCollidingEntity(World, collidingEntities);
        }

        public virtual bool IsCollidingAndDead(AWorld World)
        {
            return false;
        }
        
        public virtual bool inRoom(s_position pos)
        {
            return true;
        }
        
        #region Move
        protected bool executeRightMove(AWorld world, double speedMove, bool PerformMovement = false)
        {
            if (_move.Contains(EMovement.Right))
            {
                var OldResourcePosition = _ressources.Position;
                s_position NewPosition = Pos + new s_position(speedMove, 0, 0);
                _ressources.Position = EntityPositionToResourcesEntityPosition(NewPosition);
                if (IsColliding(world) || !inRoom(NewPosition))
                {
                    _ressources.Position = OldResourcePosition;
                    return false;
                }
                if (PerformMovement)
                {
                    _ressources.PlayAnimation((uint)EStateEntity.WALKING_RIGHT);
                    Pos = NewPosition;
                }
            }
            return true;
        }

        protected bool executeLeftMove(AWorld world, double speedMove, bool PerformMovement = false)
        {
            if (_move.Contains(EMovement.Left))
            {
                var OldResourcePosition = _ressources.Position;
                s_position NewPosition = Pos + new s_position(-speedMove, 0, 0);
                _ressources.Position = EntityPositionToResourcesEntityPosition(NewPosition);
                if (IsColliding(world))
                {
                    _ressources.Position = OldResourcePosition;
                    return false;
                }
                if (PerformMovement)
                {
                    Pos = NewPosition;
                    _ressources.PlayAnimation((uint)EStateEntity.WALKING_LEFT);
                }
            }
            return true;
        }

        protected bool executeDownMove(AWorld world, double speedMove, bool PerformMovement = false)
        {
            if (_move.Contains(EMovement.Down))
            {
                var OldResourcePosition = _ressources.Position;
                s_position NewPosition = Pos + new s_position(0, speedMove, 0);
                _ressources.Position = EntityPositionToResourcesEntityPosition(NewPosition);
                if (IsColliding(world))
                {
                    _ressources.Position = OldResourcePosition;
                    return false;
                }
                if (PerformMovement)
                {
                    Pos = NewPosition;
                    _ressources.PlayAnimation((uint)EStateEntity.WALKING_DOWN);
                }
            }
            return true;
        }

        protected bool executeUpMove(AWorld world, double speedMove, bool PerformMovement = false)
        {
            if (_move.Contains(EMovement.Up))
            {
                var OldResourcePosition = _ressources.Position;
                s_position NewPosition = Pos + new s_position(0, -speedMove, 0);
                _ressources.Position = EntityPositionToResourcesEntityPosition(NewPosition);
                if (IsColliding(world))
                {
                    _ressources.Position = OldResourcePosition;
                    return false;
                }
                if (PerformMovement)
                {
                    Pos = NewPosition;
                    _ressources.PlayAnimation((uint)EStateEntity.WALKING_UP);
                }
            }
            return true;
        }

        private void executeMove(AWorld world, SFML.System.Time deltaTime, bool DirectDiagonal = true)
        {
            double speedMove = (_speed * deltaTime.AsSeconds()) * _inputForce;
            int failed = 0;

            failed += !executeLeftMove(world, speedMove) ? 1 : 0;
            failed += !executeRightMove(world, speedMove) ? 1 : 0;
            failed += !executeUpMove(world, speedMove) ? 1 : 0;
            failed += !executeDownMove(world, speedMove) ? 1 : 0;

            if (_move.Count == 2 && failed == 0)
                speedMove *= 1.0f / Math.Sqrt(2.0f);

            executeLeftMove(world, speedMove, true);
            executeRightMove(world, speedMove, true);
            executeUpMove(world, speedMove, true);
            executeDownMove(world, speedMove, true);

            if (_move.Contains(EMovement.None))
                ((AnimatedSprite)_ressources.sprite).Pause = true;

            if (IsCollidingAndDead(world))
                die();
        }

        // Use this function for moving the entity whitout his action(ex: knockback)
        // Move the entity relative to his actual position
        public virtual void move(AWorld world, SFML.System.Time deltaTime)
        {
            executeMove(world, deltaTime);
        }
#endregion

        public abstract void update(SFML.System.Time deltaTime, AWorld world);

        public virtual void die()
        {
            _die = true;
            //
            // TODO add Dying animation
            //
            //_ressources.PlayAnimation((uint)EStateEntity.DYING);
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

        private SFML.System.Vector2f EntityPositionToResourcesEntityPosition(s_position Pos)
        {
            return new SFML.System.Vector2f(((float)Pos._trueX * 64.0f) + (ressourcesEntity.Size.X / 2.0f), ((float)Pos._trueY * 64.0f) + (ressourcesEntity.Size.Y / 2.0f));
        }

        private s_position RessourceEntityPositionToEntityPosition(SFML.System.Vector2f pos)
        {
            return (new s_position(pos.X  / 64.0f, pos.Y / 64.0f));
        }

        private void setResourceEntityPosition()
        {
            ressourcesEntity.Position = EntityPositionToResourcesEntityPosition(Pos);
            ressourcesEntity.UpdateDebug();
        }
    }
}
