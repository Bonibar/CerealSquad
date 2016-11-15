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
        private VertexArray vertices = new VertexArray(PrimitiveType.Quads, 4);
        private Texture texture;
        private IntRect textureRect;

        public RegularSprite(Texture Texture, Vector2i size, IntRect TextureRect)
        {
            base.Size = new Vector2f(size.X, size.Y);
            Type = ETypeSprite.REGULAR;
            texture = Texture;
            textureRect = TextureRect;
            vertices[0] = new Vertex(new Vector2f(0, 0), new Vector2f(textureRect.Left, textureRect.Top));
            vertices[1] = new Vertex(new Vector2f(Size.X, 0), new Vector2f(textureRect.Left + textureRect.Width, textureRect.Top));
            vertices[2] = new Vertex(new Vector2f(Size.X, Size.Y), new Vector2f(textureRect.Left + textureRect.Width, textureRect.Top + textureRect.Height));
            vertices[3] = new Vertex(new Vector2f(0, Size.Y), new Vector2f(textureRect.Left, textureRect.Top + textureRect.Height));
        }

        /// <summary>
        /// Draw the Sprite
        /// </summary>
        /// <param name="target">RenderTarget</param>
        /// <param name="states">RenderStates</param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Displayed)
            {
                states.Transform *= Transform;
                states.Texture = texture;
                target.Draw(vertices, states);
            }
        }

        protected override void UpdateSize()
        {
            vertices[0] = new Vertex(new Vector2f(0, 0), new Vector2f(textureRect.Left, textureRect.Top));
            vertices[1] = new Vertex(new Vector2f(Size.X, 0), new Vector2f(textureRect.Left + textureRect.Width, textureRect.Top));
            vertices[2] = new Vertex(new Vector2f(Size.X, Size.Y), new Vector2f(textureRect.Left + textureRect.Width, textureRect.Top + textureRect.Height));
            vertices[3] = new Vertex(new Vector2f(0, Size.Y), new Vector2f(textureRect.Left, textureRect.Top + textureRect.Height));
        }
    }
}
