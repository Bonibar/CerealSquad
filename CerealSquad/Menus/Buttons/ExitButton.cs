﻿using System;
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
                Text.Color = SFML.Graphics.Color.Green;
            else
                Text.Color = SFML.Graphics.Color.White;
        }

        protected SFML.Graphics.Text Text;

        private ExitButton() { }
        public ExitButton(string text, SFML.Graphics.Font font, int offsety, SFML.Window.Window win)
        {
            _Win = win;
            Text = new SFML.Graphics.Text(text, font);
            Text.Position = new SFML.System.Vector2f(_pos.X, _pos.Y + offsety);
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
