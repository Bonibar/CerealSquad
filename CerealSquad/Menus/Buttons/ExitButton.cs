using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus.Buttons
{
    public class ExitButton : IButton
    {
        private SFML.Window.Window _Win;
        t_pos _pos = new t_pos(300, 300);
        private bool selected;
        public bool Selected { get { return selected; } set { selected = value; selectionChanged(); } }

        protected void selectionChanged()
        {
            if (Selected)
            {
                Text = new SFML.Graphics.Text(">" + _Text, CerealSquad.Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 64);
                Text.Color = SFML.Graphics.Color.Green;
                Text.Style = SFML.Graphics.Text.Styles.Bold;
                Text.Position = new SFML.System.Vector2f(_pos.X, _pos.Y + _Offsety);
            }
            else
            {
                Text = new SFML.Graphics.Text(" " + _Text, CerealSquad.Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 64);
                Text.Color = SFML.Graphics.Color.White;
                Text.Style = SFML.Graphics.Text.Styles.Bold;
                Text.Position = new SFML.System.Vector2f(_pos.X, _pos.Y + _Offsety);
            }
        }

        protected SFML.Graphics.Text Text;
        protected string _Text;
        protected int _Offsety;

        private ExitButton() { }
        public ExitButton(string text, SFML.Graphics.Font font, int offsety, SFML.Window.Window win)
        {
            _Win = win;
            _Text = text;
            Text = new SFML.Graphics.Text(" " + text, font, 64);
            Text.Style = SFML.Graphics.Text.Styles.Bold;
            Text.Position = new SFML.System.Vector2f(_pos.X, _pos.Y + offsety);
            _Offsety = offsety;
        }

        public SFML.Graphics.Drawable getDrawable()
        {
            return Text;
        }

        public void Trigger(object source, InputManager.Keyboard.KeyEventArgs e, bool up = true)
        {
            if (e.KeyCode == InputManager.Keyboard.Key.Return && up == true)
            {
                selectionChanged();
                _Win.Close();
            }
            else if (e.KeyCode == InputManager.Keyboard.Key.Return && up == false)
            {
                Text.Color = SFML.Graphics.Color.Red;
            }
        }
        public void Trigger(object source, InputManager.Joystick.ButtonEventArgs e, bool up = true) { }
        public void Trigger(object source, InputManager.Joystick.MoveEventArgs e) { }
    }
}
