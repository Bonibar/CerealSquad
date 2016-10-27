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
        protected Vector2i size = new Vector2i(0, 0);
        protected Texture texture = null;
    
        public ETypeSprite type { get; protected set; }

        //protected JukeBox jukebox = new JukeBox();

        public ASprite(String Texture, Vector2i Size)
        {
            size = Size;
            texture = TextureFactory.Instance.getTexture(Texture);
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
