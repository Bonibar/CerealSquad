using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus.Buttons
{
    public abstract class TextButton : IButton
    {
        t_pos _pos = new t_pos(300, 300);
        private bool selected;
        public bool Selected { get { return selected; } set { selected = value; selectionChanged(); } }

        protected void selectionChanged()
        {
            if (Selected)
                Text.Color = SFML.Graphics.Color.Green;
            else
                Text.Color = SFML.Graphics.Color.White;
        }

        protected SFML.Graphics.Text Text;

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

        public abstract void Trigger();
        public abstract void Trigger(object source, InputManager.Keyboard.KeyEventArgs e);
        public abstract void Trigger(object source, InputManager.Joystick.ButtonEventArgs e);
        public abstract void Trigger(object source, InputManager.Joystick.MoveEventArgs e);
    }
}
