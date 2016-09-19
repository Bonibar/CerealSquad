using System;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace CerealSquad
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            RenderWindow win = new RenderWindow(new VideoMode(800, 600), "[DEV] Cereal Squad");
#else
            RenderWindow win = new RenderWindow(new VideoMode(800, 600), "[PROD] Cereal Squad");
#endif
            win.Closed += Win_Closed;

            while (win.IsOpen)
            {
                win.DispatchEvents();
                win.Clear();
                win.Display();
            }
        }

        private static void Win_Closed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
    }
}
