using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    class EllipseShape : Shape
    {
        public Vector2f Radius { get; set; }

        public uint PointCount { get; set; }

        public EllipseShape(Vector2f radius)
        {
            Radius = radius;
            PointCount = 30;
        }

        public override Vector2f GetPoint(uint index)
        {
            double angle = index * 2 * Math.PI / PointCount - Math.PI / 2;
            double x = Math.Cos(angle) * Radius.X;
            double y = Math.Sin(angle) * Radius.Y;

            return new Vector2f((float)Radius.X + (float)x, (float)Radius.Y + (float)y);
        }

        public override uint GetPointCount()
        {
            return PointCount;
        }
    }

}

