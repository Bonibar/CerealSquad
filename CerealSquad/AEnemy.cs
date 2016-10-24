using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    abstract class AEnemy : AEntity
    {
        public AEnemy(IEntity owner, s_position position) : base(owner)
        {
            _pos = position;
            _type = e_EntityType.Ennemy;
        }

        public virtual void attack()
        {
            throw new NotImplementedException();
        }

        public abstract void think();
    }
}
