﻿using System;
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

        // TODO function move
        // Animation must be implemented by the child
        public void move(/* Map map*/)
        {
            // Check the validity of the movement with the map
            throw new NotImplementedException();
        }

        public virtual void attack()
        {
            throw new NotImplementedException();
        }

        public abstract void think();
    }
}