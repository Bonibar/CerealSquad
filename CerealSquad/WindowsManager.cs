using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace CerealSquad
{
    class WindowsManager
    {
        private RenderWindow renderWindow;

        public WindowsManager(RenderWindow _renderWindow)
        {
            renderWindow = _renderWindow;

            renderWindow.Closed += RenderWindow_Closed;
            renderWindow.Resized += RenderWindow_Resized;
            renderWindow.LostFocus += RenderWindow_LostFocus;
            renderWindow.GainedFocus += RenderWindow_GainedFocus;
            renderWindow.KeyPressed += RenderWindow_KeyPressed;
            renderWindow.KeyReleased += RenderWindow_KeyReleased;
            
        }

        private void RenderWindow_Resized(object sender, SizeEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
        }

        private void RenderWindow_GainedFocus(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
        }

        private void RenderWindow_KeyReleased(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
        }

        private void RenderWindow_KeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            
            if (e.Code == SFML.Window.Keyboard.Key.Escape) {
                window.Close();
            }
        }

        private void RenderWindow_Closed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        private void RenderWindow_LostFocus(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
        }
    }
}
