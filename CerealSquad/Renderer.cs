using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace CerealSquad
{
    public class Renderer
    {
        private RenderWindow win = null;
        private WindowsEventHandler events = null;

        uint width = 800;
        uint height = 600;
        string name = "[DEV] Cereal Squad";


        public Renderer()
        {
            width = 800;
            height = 600;
#if ! DEBUG
            if (VideoMode.FullscreenModes.Length > 0) {
                width = VideoMode.FullscreenModes[0].Width;
                height = VideoMode.FullscreenModes[0].Height;
                name = "[PROD] Cereal Squad";
            }
#endif
        }

        public bool initialization()
        {
            win = new RenderWindow(new VideoMode(width, height), name);
            events = new WindowsEventHandler(win);

            InputManager manager = new InputManager(win);
            manager.KeyPressed += Manager_KeyPressed;

            return true;
        }

        private void Manager_KeyPressed(object source, Keyboard.KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("KEYPRESSED - " + e.KeyCode.ToString());
        }

        public void loop()
        {
            while (win.IsOpen)
            {
                win.DispatchEvents();
                win.Clear(Color.Magenta);
                win.Display();
            }
        }



        public static void CallToChildThread()
        {
            Console.WriteLine("Child thread starts");
        }
    }
}
