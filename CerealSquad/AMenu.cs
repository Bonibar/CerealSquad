using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    public class AMenu
    {
        protected SFML.Graphics.RenderWindow _Win;
        protected InputManager _InputManager;

        public AMenu(SFML.Graphics.RenderWindow win, InputManager inputManager)
        {
            _InputManager = inputManager;
            _Win = win;
        }

        private bool _displayed = false;
        public bool Displayed { get { return _displayed; } }

        SFML.Graphics.Text back = new SFML.Graphics.Text("Back", new SFML.Graphics.Font("Fonts/xirod.regular.ttf"));

        public void Show()
        {
            if (!Displayed)
            {
                _displayed = true;
            }
        }

        public void Hide()
        {
            if (Displayed)
            {
                _displayed = false;
            }
        }

        public void Toggle()
        {
            if (Displayed)
                Hide();
            else
                Show();
        }

        public void update()
        {
            if (Displayed)
            {
                
                _Win.Draw(back);
            }
        }
    }
}
