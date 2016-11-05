using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace CerealSquad.TrapEntities
{
    class Bomb : ATrap
    {
        public Bomb(IEntity owner) : base(owner, e_DamageType.BOMB_DAMAGE, 1)
        {
           
        }

        public override void update(Time deltaTime)
        {
            ressourcesEntity.Update(deltaTime);
        }
    }
}
