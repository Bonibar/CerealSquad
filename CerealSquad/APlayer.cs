using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    abstract class APlayer : AEntity
    {
        // TODO Add save of the input

        // TODO add the sprite dictionnary

        // TODO Add InputManager
        public APlayer(IEntity owner, s_position position/*, InputManager input*/) : base(owner)
        {
            _pos = position;
            _type = e_EntityType.Player;
        }

        // TODO function move
        // Animation must be implemented by the child
        public void move(/* Map map*/)
        {
            //
            // Get the input with the inputManager and check with the playerInput for movement
            // Check the validity of the movement with the map
            //
            throw new NotImplementedException();
        }

        public abstract void AttaqueSpe();
    }
}
