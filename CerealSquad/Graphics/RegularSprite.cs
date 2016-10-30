using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    class RegularSprite : ASprite
    {
        private Sprite sprite = new Sprite();

        public RegularSprite(string Texture, Vector2i Size) : base(Texture, Size)
        {
            type = ETypeSprite.REGULAR;
            sprite.Texture = texture;
        }

        public RegularSprite(string Texture, Vector2i Size, IntRect textureRect) : base(Texture, Size)
        {
            type = ETypeSprite.REGULAR;
            sprite.Texture = texture;
            sprite.TextureRect = textureRect;
        }

        public RegularSprite(string Texture, int Width, int Height) : base(Texture, new Vector2i(Width, Height))
        {
            type = ETypeSprite.REGULAR;
            sprite.Texture = texture;
        }

        public RegularSprite(string Texture, int Width, int Height, IntRect textureRect) : base(Texture, new Vector2i(Width, Height))
        {
            type = ETypeSprite.REGULAR;
            sprite.Texture = texture;
            sprite.TextureRect = textureRect;
        }

        /// <summary>
        /// Draw the Sprite
        /// </summary>
        /// <param name="target">RenderTarget</param>
        /// <param name="states">RenderStates</param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            sprite.Draw(target, states);
        }
    }
}
