using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    abstract class ATrap : AEntity
    {
        protected int _range;

        public int Range
        {
            get
            {
                return _range;
            }

            set
            {
                _range = value;
            }
        }

        public ATrap(IEntity owner, e_DamageType damage, int range = 1) : base(owner)
        {
            _damageType = damage;
            if (owner.getEntityType() == e_EntityType.Player || owner.getEntityType() == e_EntityType.PlayerTrap)
                _type = e_EntityType.PlayerTrap;
            else
                _type = e_EntityType.EnnemyTrap;
            _range = range;
        }
    }
}
