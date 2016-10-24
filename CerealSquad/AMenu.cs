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
        protected InputManager.InputManager _InputManager;

        public AMenu(SFML.Graphics.RenderWindow win, InputManager.InputManager inputManager)
        {
            _InputManager = inputManager;
            _Win = win;
            back = new SFML.Graphics.Text("Back", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));
        }

        private bool _displayed = false;
        public bool Displayed { get { return _displayed; } }
        private Factories.FontFactory.FontFactory fontFactory = Factories.FontFactory.FontFactory.Instance;

        SFML.Graphics.Text back;

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
