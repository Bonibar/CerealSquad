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
        public bool Displayed { get; set; }
        public int Level { get; set; }

        private Vector2f size;
        public Vector2f Size { get { return size; } set { size = value; UpdateSize(); } }

        protected abstract void UpdateSize();

        public ASprite()
        {
            Displayed = true;
            Level = 0;
        }
        
        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
