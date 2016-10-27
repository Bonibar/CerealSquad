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
        bool Selected { get; set; }

        SFML.Graphics.Drawable getDrawable();
        void Trigger();
        void Trigger(object source, InputManager.Keyboard.KeyEventArgs e);
        void Trigger(object source, InputManager.Joystick.ButtonEventArgs e);
        void Trigger(object source, InputManager.Joystick.MoveEventArgs e);
    }
}
