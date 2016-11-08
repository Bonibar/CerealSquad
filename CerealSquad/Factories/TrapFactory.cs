using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Factories
{
    static class TrapFactory
    {
        public static ATrap createTrap(IEntity owner, e_TrapType type)
        {
            ATrap result = null;
            switch (type)
            {
                case e_TrapType.BEAR_TRAP:
                    result = new TrapEntities.BearTrap(owner);
                    break;
                case e_TrapType.BOMB:
                    result = new TrapEntities.Bomb(owner);
                    break;
                default:
                    throw new Exception("Invalid Trap entity requested");
            }

            return result;
        }
       
    }
}
