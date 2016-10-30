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
        protected Texture texture = null;
    
        public ETypeSprite Type { get; protected set; }

        //protected JukeBox jukebox = new JukeBox();

        public ASprite(String Texture)
        {
            texture = TextureFactory.Instance.getTexture(Texture);
        }

        public ASprite()
        {
        }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
