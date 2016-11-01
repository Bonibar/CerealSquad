using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.Menus.Buttons
{
    public class BackButton : IButton
    {
        private Menu _Menu;
        t_pos _pos = new t_pos(300, 300);
        private bool selected;
        public bool Selected { get { return selected; } set { selected = value; selectionChanged(); } }

        protected void selectionChanged()
        {
            if (Selected)
                Text.Color = Color.Green;
            else
                Text.Color = Color.White;
        }

        protected Text Text;

        private BackButton() { }
        public BackButton(string text, Font font, int offsety, Menu menu)
        {
            _Menu = menu;
            Text = new Text(text, font);
            Text.CharacterSize = 64;
            Text.Position = new SFML.System.Vector2f(_pos.X, _pos.Y + offsety);
        }

        public void Trigger(object source, InputManager.Keyboard.KeyEventArgs e, bool up = true)
        {
            if (e.KeyCode == InputManager.Keyboard.Key.Return && up == true)
            {
                selectionChanged();
                MenuManager.Instance.RemoveMenu(_Menu);
            }
            else if (e.KeyCode == InputManager.Keyboard.Key.Return && up == false)
            {
                Text.Color = Color.Red;
            }
        }
        public void Trigger(object source, InputManager.Joystick.ButtonEventArgs e, bool up = true) { }
        public void Trigger(object source, InputManager.Joystick.MoveEventArgs e) { }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Text, states);
        }
    }
}
