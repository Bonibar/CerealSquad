using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus.Buttons
{
    public class ExitButton : TextButton
    {
        private SFML.Window.Window _Win;

        public ExitButton(string text, SFML.Graphics.Font font, int offsety, SFML.Window.Window win) : base(text, font, offsety)
        {
            _Win = win;
        }

        // TO REMOVE WHEN GOING BACK TO ABSTRACT CLASS
        public override void Trigger()
        {
            _Win.Close();
        }
        public override void Trigger(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (e.KeyCode == InputManager.Keyboard.Key.Return)
            {
                Text.Color = SFML.Graphics.Color.Red;
                _Win.Close();
            }
        }
        public override void Trigger(object source, InputManager.Joystick.ButtonEventArgs e) { }
        public override void Trigger(object source, InputManager.Joystick.MoveEventArgs e) { }
    }
}
