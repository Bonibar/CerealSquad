using CerealSquad.GameWorld;
using CerealSquad.Global;
using CerealSquad.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.EntitySystem
{

    abstract class AEntity : IEntity
    {
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
        protected List<e_EntityType> _CollidingType = new List<e_EntityType>();
        static protected bool shootDebug = false;

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

        public e_EntityType Type
        {
            get
            {
                return _type;
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
            child.setOwner(this);
        }

        public void setOwner(IEntity owner)
        {
            _owner = owner;
        }

        #region CheckingRange
        protected bool NotInCircleRange(IEntity Sender, float Range)
        {
            double Distance = Math.Sqrt(Math.Pow(Sender.Pos.X - Pos.X, 2.0f) + Math.Pow(Sender.Pos.Y - Pos.Y, 2.0f));
            if (ressourcesEntity != null)
                Distance -= ressourcesEntity.HitBox.Width / 64.0f / 2.0f;

            return !(Distance > Range);
        }

        protected bool InCircleRange(double x_center, double y_center, IEntity ent, float Range)
        {
            double Distance = Math.Sqrt(Math.Pow(x_center - ent.HitboxPos.X, 2.0f) + Math.Pow(y_center - ent.HitboxPos.Y, 2.0f));
         //   if (ressourcesEntity != null)
         //       Distance -= ressourcesEntity.HitBox.Width / 64.0f / 2.0f;

            return (Distance <= Range);
        }

        protected static bool IsInEllipse(double x_el, double y_el, double x, double y, double rX, double rY)
        {
            double a = Math.Pow(x - x_el, 2.0f) / Math.Pow(rX, 2.0f);
            double b = Math.Pow(y - y_el, 2.0f) / Math.Pow(rY, 2.0f);

            return (a + b) <= 1;
        }

        protected bool NotInEllipseRange(IEntity Sender, float RadiusX, float RadiusY)
        {
            if (ressourcesEntity == null)
                return !IsInEllipse(Sender.Pos.X, Sender.Pos.Y, Pos.X, Pos.Y, RadiusX, RadiusY);

            s_Pos<double> posOne = new s_Pos<double>(ressourcesEntity.HitBox.Left, ressourcesEntity.HitBox.Top);
            s_Pos<double> posTwo = new s_Pos<double>(ressourcesEntity.HitBox.Left, ressourcesEntity.HitBox.Top + ressourcesEntity.HitBox.Height);
            s_Pos<double> posThree = new s_Pos<double>(ressourcesEntity.HitBox.Left + ressourcesEntity.HitBox.Width, ressourcesEntity.HitBox.Top + ressourcesEntity.HitBox.Height);
            s_Pos<double> posFour = new s_Pos<double>(ressourcesEntity.HitBox.Left + ressourcesEntity.HitBox.Width, ressourcesEntity.HitBox.Top);

            int tested = 0;
            double x = Sender.ressourcesEntity.Position.X;
            double y = Sender.ressourcesEntity.Position.Y;

            if (IsInEllipse(x, y, posOne.X, posOne.Y, RadiusX, RadiusY))
                tested++;
            if (IsInEllipse(x, y, posTwo.X, posTwo.Y, RadiusX, RadiusY))
                tested++;
            if (IsInEllipse(x, y, posThree.X, posThree.Y, RadiusX, RadiusY))
                tested++;
            if (IsInEllipse(x, y, posFour.X, posFour.Y, RadiusX, RadiusY))
                tested++;

            return tested == 0;
        }
        #endregion

        public virtual bool attemptDamage(IEntity Sender, e_DamageType damage, bool isHitBox = false)
        {
            return false;
        }

        public virtual bool attemptDamage(IEntity Sender, e_DamageType damage, float RadiusRangeX, float RadiusRangeY)
        {
            if (NotInEllipseRange(Sender, RadiusRangeX * 64.0f, RadiusRangeY * 64.0f))
                return false;

            return attemptDamage(Sender, damage);
        }

        public virtual bool attemptDamage(IEntity Sender, e_DamageType damage, float Range)
        {
            if (NotInCircleRange(Sender, Range))
                return false;

            return attemptDamage(Sender, damage);
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
            bool result = false;

            CollidingEntities.ForEach(ent =>
            {
                if (ent.getEntityType() == e_EntityType.Room)
                    result = true;
            });
            return result;
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

            List<AEntity> collidingEntities = ((WorldEntity)getRootEntity())
                .GetCollidingEntities(ressourcesEntity)
                .Where(i => i.collideWithType(_type))
                .ToList();

            if (collidingEntities.Count == 0)
                return false;

            return IsCollidingEntity(World, collidingEntities);
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
                    PlayAnimation((uint)EStateEntity.WALKING_RIGHT);
                    Pos = NewPosition;
                    world.InvalidatePlayersPosition();
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
                    PlayAnimation((uint)EStateEntity.WALKING_LEFT);
                    world.InvalidatePlayersPosition();
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
                    PlayAnimation((uint)EStateEntity.WALKING_DOWN);
                    world.InvalidatePlayersPosition();
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
                    PlayAnimation((uint)EStateEntity.WALKING_UP);
                    world.InvalidatePlayersPosition();
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

            CheckHitBoxCollision(world);
        }

        // Use this function for moving the entity whitout his action(ex: knockback)
        // Move the entity relative to his actual position
        public virtual void move(AWorld world, SFML.System.Time deltaTime)
        {
            executeMove(world, deltaTime);
        }
#endregion

        protected virtual void IsTouchingHitBoxEntities(AWorld world, List<AEntity> touchingEntities)
        {
        }

        private void CheckHitBoxCollision(AWorld world)
        {
            if (ressourcesEntity == null)
                return;
            List<AEntity> HitboxCollidingEntities = ((WorldEntity)getRootEntity())
                .GetTouchingEntities(ressourcesEntity)
                .Where(i => i.collideWithType(_type))
                .ToList();

            if (HitboxCollidingEntities.Count == 0)
                return;

            IsTouchingHitBoxEntities(world, HitboxCollidingEntities);
        }

        public abstract void update(SFML.System.Time deltaTime, AWorld world);

        public virtual void die()
        {
            _die = true;
            _ressources.Loop = false;
        }

        // destroy the object
        // Should be call after the end of death animation
        public void destroy()
        {
            _owner.removeChild(this);
            _children.ToList().ForEach(i => _owner.addChild(i));            
        }

        public IEntity getRootEntity()
        {
            IEntity parent = this;

            while (parent.getOwner() != null)
                parent = parent.getOwner();

            return parent;
        }

        protected SFML.System.Vector2f EntityPositionToResourcesEntityPosition(s_position Pos)
        {
            return new SFML.System.Vector2f(((float)Pos.X * 64.0f) + (ressourcesEntity.Size.X / 2.0f), ((float)Pos.Y * 64.0f) + (ressourcesEntity.Size.Y / 2.0f));
        }

        protected s_position RessourceEntityPositionToEntityPosition(SFML.System.Vector2f pos)
        {
            return (new s_position(pos.X  / 64.0f, pos.Y / 64.0f));
        }

        protected void setResourceEntityPosition()
        {
            ressourcesEntity.Position = EntityPositionToResourcesEntityPosition(Pos);
            ressourcesEntity.UpdateDebug();
        }

        public bool collideWithType(e_EntityType type)
        {
            return _CollidingType.Contains(type);
        }

        public EMovement getOrientation()
        {
            return (_move.Count > 0) ? _move.ElementAt(_move.Count - 1) : EMovement.None;
        }

        public void PlayAnimation(uint anim)
        {
            if (!Die)
                ressourcesEntity.PlayAnimation(anim);
        }
    }
}
