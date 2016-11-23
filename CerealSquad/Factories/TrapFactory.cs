using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Factories
{
    static class TrapFactory
    {
        public static ATrap CreateTrap(IEntity owner, e_TrapType type)
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
                case e_TrapType.WALL:
                    result = new TrapEntities.SugarWall(owner);
                    break;
                default:
                    throw new Exception("Invalid Trap entity requested");
            }

            return result;
        }

        public static SFML.Graphics.FloatRect GetCollisionBox(e_TrapType type)
        {
            SFML.Graphics.FloatRect result = new SFML.Graphics.FloatRect();
            switch (type)
            {
                case e_TrapType.BEAR_TRAP:
                    result = TrapEntities.BearTrap.COLLISION_BOX;
                    break;
                case e_TrapType.BOMB:
                    result = TrapEntities.Bomb.COLLISION_BOX;
                    break;
                case e_TrapType.WALL:
                    result = TrapEntities.SugarWall.COLLISION_BOX;
                    break;
                case e_TrapType.COFFE:
                    result = TrapEntities.CoffeePool.COLLISION_BOX;
                    break;
                default:
                    throw new Exception("Invalid Trap entity requested");
            }

            return result;
        }
       
    }
}
