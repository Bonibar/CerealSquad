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
        private WindowsManager events = null;

        public SFMLImplementation.EntityResources entityResource;

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
            events = new WindowsManager(win);

            SFMLImplementation.TextureFactory.Instance.load("CharacterTest", "Assets/test.png");

            return true;
        }

        public void loop()
        {
            
            entityResource = new SFMLImplementation.EntityResources("CharacterTest");
            entityResource.Position = new SFML.System.Vector2f(20, 20);
            entityResource.Rotation = 45;
            entityResource.playAnimation(SFMLImplementation.EntityResources.EState.WALKING_RIGHT);

            SFML.System.Clock frameClock = new SFML.System.Clock();

            InputManager im = new InputManager(win);
            im.KeyboardKeyPressed += Im_KeyboardKeyPressed;

            while (win.IsOpen)
            {
                win.DispatchEvents();

                SFML.System.Time frameTime = frameClock.Restart();
                entityResource.update(frameTime);



                win.Clear(Color.Blue);
                win.Draw(entityResource);
                win.Display();
            }
        }

        private void Im_KeyboardKeyPressed(object source, Keyboard.KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keyboard.Key.B))
            {
                entityResource.playAnimation(SFMLImplementation.EntityResources.EState.WALKING_LEFT);
            }
        }
    }
}
