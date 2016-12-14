using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.GameWorld;

namespace CerealSquad.EntitySystem
{
    class TutorialGhostEnnemy : GhostEnemy
    {
        public override bool Active
        {
            get
            {
                return base.Active;
            }

            set
            {
                base.Active = false;
            }
        }

        public TutorialGhostEnnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            
        }
    }
}
