using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.InputManager;

namespace CerealSquad.Menus
{
    class CharacterSelectMenu : Menu
    {

        public CharacterSelectMenu(InputManager.InputManager inputManager) : base(inputManager)
        {
            _InputManager.JoystickButtonPressed += _InputManager_JoystickButtonPressed;
        }

        private void _InputManager_JoystickButtonPressed(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            if (Displayed)
                System.Diagnostics.Debug.WriteLine("JOYBUTTON " + e.Button.ToString());
        }
    }
}
