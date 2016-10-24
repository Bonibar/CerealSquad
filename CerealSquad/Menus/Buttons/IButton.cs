using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus.Buttons
{
    public struct t_pos
    {
        public t_pos(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public uint X { get; }
        public uint Y { get; }
    }

    public interface IButton
    {
        SFML.Graphics.Drawable getDrawable();
    }
}
