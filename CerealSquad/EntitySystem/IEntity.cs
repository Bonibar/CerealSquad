using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//
// TODO Do a correct namespace
//
namespace CerealSquad
{
    public enum e_DamageType
    {
        BOMB_DAMAGE,
        TRUE_DAMAGE,
        NONE
    }

    public enum e_TrapType
    {
        BOMB,
        BEAR_TRAP,
        WALL,
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
        Projectile
    }

    public enum e_EnnemyType
    {
        RiceBowl,
        Egg,
        HalfEgg,
        Ghost
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
            _x = (int)(x);
            _trueX = x;
            _y = (int)(y);
            _trueY = y;
            _layer = layer;
        }

        public static s_position operator +(s_position pos, s_position other)
        {
            pos._trueX += other._trueX;
            pos._trueY += other._trueY;
            pos._x = (int)(pos._trueX);
            pos._y = (int)(pos._trueY);
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

        bool Die
        {
            get;
            set;
        }

        void update(SFML.System.Time deltaTime, AWorld world);
        bool attemptDamage(IEntity Sender, e_DamageType damage, float Range);
        bool attemptDamage(IEntity Sender, e_DamageType damage, float RadiusRangeX, float RadiusRangeY);

        IEntity getOwner();
        ICollection<IEntity> getChildren();
        void addChild(IEntity child);
        bool removeChild(IEntity child);
        e_DamageType getDamageType();
        e_EntityType getEntityType();
        IEntity getRootEntity();
    }
}
