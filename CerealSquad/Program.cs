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
            Renderer renderer = new Renderer();
            renderer.Initialization();
            renderer.ChangeResolution(Renderer.EResolution.R1920x1080);
            renderer.SetFullScreenEnabled(true);

            Game game = new Game(renderer);

            game.GameLoop();
            while (renderer.isOpen())
            {
                renderer.DispatchEvents();
                renderer.Clear(SFML.Graphics.Color.Black);
                game.GameLoop();
                renderer.Display();
            }
        }
    }
}
