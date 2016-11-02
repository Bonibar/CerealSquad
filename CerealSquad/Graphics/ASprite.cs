using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace CerealSquad.Graphics
{
    enum ETypeSprite
    {
        REGULAR,
        ANIMATED
    }

    abstract class ASprite : Transformable, Drawable
    {
        public ETypeSprite Type { get; protected set; }

        public ASprite()
        {
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
