using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    enum EStateWorld { DEFAULT }
    enum EStateEntity : byte
    {
        IDLE = 0,
        WALKING_UP,
        WALKING_DOWN,
        WALKING_RIGHT,
        WALKING_LEFT,
        DYING,
    }

    interface IResource : Drawable
    {
        void Update(Time DeltaTime);
    }
}
