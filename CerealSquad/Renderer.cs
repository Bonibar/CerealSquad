using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad
{
    public class Renderer
    {
        public enum EResolution
        {
            R1920x1080,
            R2880x1800,
            R800x600,
        };

        struct Resolution
        {
            public uint width { get; set; }
            public uint height { get; set; }

            public Resolution(uint _width = 1920, uint _height = 1080)
            {
                width = _width;
                height = _height;
            }
        };

        #region Ref
        private RenderWindow win = null;
        private WindowsManager events = null;
        InputManager im = null;
        #endregion

        #region Windows Parameters
        private Dictionary<EResolution, Resolution> resolutionContext = new Dictionary<EResolution, Resolution>();

        EResolution resolutionType = EResolution.R800x600;
        bool windowed = true;
        string name = "[DEV] Cereal Squad";

        View mainView;
        #endregion

        public Renderer()
        {
            // Set all predefine resolutions. Order Bigger to smaller
            resolutionContext[EResolution.R2880x1800] = new Resolution(2880, 1800);
            resolutionContext[EResolution.R1920x1080] = new Resolution(1920, 1080);
            resolutionContext[EResolution.R800x600] = new Resolution(800, 600);
#if !DEBUG
            resolutionType = findAppropriateResolution(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height);
            windowed = false;
            name = "[PROD] Cereal Squad";
#endif
            mainView = new View(new FloatRect(0f, 0f, getWidth(), getHeight()));
        }

        /// <summary>
        /// Look the current resolution of Deskop and find the appropriate resolution
        /// </summary>
        /// <param name="width">Uint</param>
        /// <param name="height">Uint</param>
        /// <returns>EResolution</returns>
        private EResolution findAppropriateResolution(uint width, uint height)
        {
            foreach (KeyValuePair<EResolution, Resolution> entry in resolutionContext) {
                if (width <= entry.Value.width && height <= entry.Value.height) {
                    return entry.Key;
                }
            }

            return EResolution.R800x600;
        }

        /// <summary>
        /// Return current Width used in Renderer
        /// </summary>
        /// <returns>Uint</returns>
        public uint getWidth()
        {
            return resolutionContext[resolutionType].width;
        }

        /// <summary>
        /// Return current Height used in Renderer
        /// </summary>
        /// <returns>Uint</returns>
        public uint getHeight()
        {
            return resolutionContext[resolutionType].height;
        }

        /// <summary>
        /// Initialisation
        /// </summary>
        public void initialization()
        {
            win = new RenderWindow(new VideoMode(getWidth(), getHeight()), name, (windowed ? Styles.Close : Styles.Fullscreen));
            im = new InputManager(win);
            events = new WindowsManager(win);
        }

        /// <summary>
        /// Exemple of simple loop
        /// </summary>
        public void loop()
        {
            while (isOpen())
            {
                DispatchEvents();

                Clear(Color.White);
                Display();
            }
        }

        /// <summary>
        /// Check if the windows is still open
        /// </summary>
        /// <returns>boolean</returns>
        public bool isOpen()
        {
            return win.IsOpen;
        }

        /// <summary>
        /// Dispatch event to Windows Manager
        /// </summary>
        public void DispatchEvents()
        {
            win.DispatchEvents();
        }

        /// <summary>
        /// Clear the screen with the given color
        /// </summary>
        /// <param name="color">Color</param>
        public void Clear(Color color)
        {
            win.Clear(color);
        }

        /// <summary>
        /// Display the buffer to screen
        /// </summary>
        public void Display()
        {
            win.Display();
        }

        /// <summary>
        /// Change the current resolution of renderer or option windowed. Close the previous windows and reload it.
        /// </summary>
        /// <param name="new_resolution">EResolution</param>
        public void ChangeConfigWindows(EResolution new_resolution, bool windowed)
        {
            resolutionType = new_resolution;
            win.Close();
            win = new RenderWindow(new VideoMode(getWidth(), getHeight()), name, (windowed ? Styles.Close : Styles.Fullscreen));
            mainView = new View(new FloatRect(0f, 0f, getWidth(), getHeight()));
            im = new InputManager(win);
            events = new WindowsManager(win);
        }
    }
}
