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

        public RegularSprite(Texture Texture, Vector2i size, IntRect textureRect)
        {
            base.Size = new Vector2u((uint)size.X, (uint)size.Y);
            Type = ETypeSprite.REGULAR;
            texture = Texture;
            vertices[0] = new Vertex(new Vector2f(0, 0), new Vector2f(textureRect.Left, textureRect.Top));
            vertices[1] = new Vertex(new Vector2f(size.X, 0), new Vector2f(textureRect.Left + textureRect.Width, textureRect.Top));
            vertices[2] = new Vertex(new Vector2f(size.X, size.Y), new Vector2f(textureRect.Left + textureRect.Width, textureRect.Top + textureRect.Height));
            vertices[3] = new Vertex(new Vector2f(0, size.Y), new Vector2f(textureRect.Left, textureRect.Top + textureRect.Height));
        }

        /// <summary>
        /// Transform the 4 points of the sprite vertices
        /// </summary>
        /// <param name="TopLeft">Vector2i</param>
        /// <param name="TopRight">Vector2i</param>
        /// <param name="BottomRight">Vector2i</param>
        /// <param name="BottomLeft">Vector2i</param>
        public void TransformVertex(Vector2i TopLeft, Vector2i TopRight, Vector2i BottomRight, Vector2i BottomLeft)
        {
            // Top Left
            vertices[0] = new Vertex(new Vector2f(vertices[0].Position.X + TopLeft.X, vertices[0].Position.Y + TopLeft.Y), new Vector2f(vertices[0].TexCoords.X, vertices[0].TexCoords.Y));
            // Top Right
            vertices[1] = new Vertex(new Vector2f(vertices[1].Position.X + TopRight.X, vertices[1].Position.Y + TopRight.Y), new Vector2f(vertices[1].TexCoords.X, vertices[1].TexCoords.Y));
            // Bottom Right
            vertices[2] = new Vertex(new Vector2f(vertices[2].Position.X + BottomRight.X, vertices[2].Position.Y + BottomRight.Y), new Vector2f(vertices[2].TexCoords.X, vertices[2].TexCoords.Y));
            // Bottom Left
            vertices[3] = new Vertex(new Vector2f(vertices[3].Position.X + BottomLeft.X, vertices[3].Position.Y + BottomLeft.Y), new Vector2f(vertices[3].TexCoords.X, vertices[3].TexCoords.Y));
        }

        /// <summary>
        /// Draw the Sprite
        /// </summary>
        /// <param name="target">RenderTarget</param>
        /// <param name="states">RenderStates</param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            states.Texture = texture;
            target.Draw(vertices, states);
        }
    }
}
