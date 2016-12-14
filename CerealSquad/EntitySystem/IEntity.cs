using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.EntitySystem
{
    public enum e_DamageType
    {
        BOMB_DAMAGE,
        TRUE_DAMAGE,
        ENEMY_DAMAGE,
        PROJECTILE_ENEMY_DAMAGE,
        COFFE_DAMAGE,
        NONE
    }

    public enum e_TrapType
    {
        BOMB,
        BEAR_TRAP,
        WALL,
        COFFE,
        NONE
    }

    public enum e_EntityType
    {
        Player,
        PlayerTrap,
        Ennemy,
        EnnemyTrap,
        World,
        Crate,
        ProjectilePlayer,
        ProjectileEnemy,
        Room,
        DeliverCloud
    }

    public enum e_EnnemyType
    {
        RiceBowl,
        Egg,
        HalfEgg,
        Ghost,
        CoffeeMachine,
        TutorialGhost,
        Baggy
    }

    public enum EMovement
    {
        Up,
        Down,
        Right,
        Left,
        None
    }

    public struct s_position
    {
        public double X;
        public double Y;
        public int _layer;

        public s_position(double x = -1, double y = -1, int layer = -1)
        {
            X = x;
            Y = y;
            _layer = layer;
        }

        public static s_position operator +(s_position pos, s_position other)
        {
            pos.X += other.X;
            pos.Y += other.Y;
            pos._layer += other._layer;

            return (pos);
        }
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

    interface IEntity
    {
        s_position Pos
        {
            get;
            set;
        }
        s_position HitboxPos
        {
            get;
        }
        s_size Size
        {
            get;
            set;
        }
        double Speed
        {
            get;
            set;
        }
        EntityResources ressourcesEntity
        {
            get;
            set;
        }

        e_EntityType Type
        {
            get;
        }

        bool Die
        {
            get;
            set;
        }

        void update(SFML.System.Time deltaTime, AWorld world);
        bool collideWithType(e_EntityType type);

        bool attemptDamage(IEntity Sender, e_DamageType damage, bool isHitBox = false);
        bool attemptDamage(IEntity Sender, e_DamageType damage, float Range);
        bool attemptDamage(IEntity Sender, e_DamageType damage, float RadiusRangeX, float RadiusRangeY);

        IEntity getOwner();
        void setOwner(IEntity owner);
        ICollection<IEntity> getChildren();
        void addChild(IEntity child);
        bool removeChild(IEntity child);
        e_DamageType getDamageType();
        e_EntityType getEntityType();
        IEntity getRootEntity();
        EMovement getOrientation();
    }
}
