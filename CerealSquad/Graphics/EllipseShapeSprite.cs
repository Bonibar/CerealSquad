using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    class EllipseShapeSprite : ASprite
    {
        EllipseShape EllipseShape;

        public EllipseShapeSprite(Vector2f Size, Color color, Color outlineColor)
        {
            EllipseShape = new EllipseShape(Size);
            base.Size = Size;
            EllipseShape.FillColor = color;
            EllipseShape.OutlineColor = outlineColor;
            EllipseShape.OutlineThickness = 2;
            EllipseShape.Position = new Vector2f(0, 0);
            EllipseShape.Origin = new Vector2f(Size.X, Size.Y);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Displayed)
                EllipseShape.Draw(target, states);
        }

        protected override void UpdateSize()
        {
            EllipseShape.Radius = base.Size;
        }
    }
}
