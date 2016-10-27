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
        private Sprite sprite;

        public RegularSprite(string Texture, Vector2i Size) : base(Texture, Size)
        {
            sprite.Scale = new Vector2f(Size.X, Size.Y);
            type = ETypeSprite.REGULAR;
        }

        public RegularSprite(string Texture, int Width, int Height) : base(Texture, new Vector2i(Width, Height))
        {
            sprite.Scale = new Vector2f(Width, Height);
            type = ETypeSprite.REGULAR;
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
