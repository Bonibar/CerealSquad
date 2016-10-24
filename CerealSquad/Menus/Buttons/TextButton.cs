using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus.Buttons
{
    public class TextButton : IButton
    {
        t_pos _pos = new t_pos(500, 500);

        SFML.Graphics.Text Text;

        private TextButton() { }
        public TextButton(string text, SFML.Graphics.Font font, int offsety)
        {
            Text = new SFML.Graphics.Text(text, font);
            Text.Position = new SFML.System.Vector2f(_pos.X, _pos.Y + offsety);
        }

        public SFML.Graphics.Drawable getDrawable()
        {
            return Text;
        }
    }
}
