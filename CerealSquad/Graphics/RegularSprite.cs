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

        public RegularSprite(string Texture) : base(Texture)
        {
            Type = ETypeSprite.REGULAR;
            sprite.Texture = texture;
        }

        public RegularSprite(string Texture, IntRect rect) : base(Texture)
        {
            Type = ETypeSprite.REGULAR;
            sprite.Texture = texture;
            sprite.TextureRect = rect;
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
