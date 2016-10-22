using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    enum e_DamageType
    {
        BOMB_DAMAGE,
        NONE
    }

    enum e_EntityType
    {
        Player,
        PlayerTrap,
        Ennemy,
        EnnemyTrap,
        World
    }

    interface IEntity
    {
        void update();
        bool attemptDamage(IEntity Sender, e_DamageType damage);
        IEntity getOwner();
        ICollection<IEntity> getChildren();
        void addChild(IEntity child);
        bool removeChild(IEntity child);
        e_DamageType getDamageType();
        e_EntityType getEntityType();
    }
}
