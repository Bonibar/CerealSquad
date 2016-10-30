using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
            R800x450,
        };

        struct Resolution
        {
            public uint width { get; set; }
            public uint height { get; set; }

            public Resolution(uint _width = 800, uint _height = 600)
            {
                width = _width;
                height = _height;
            }
        };

        #region Ref
        public RenderWindow Win { get; private set; }
        #endregion

        #region Windows Parameters
        private Dictionary<EResolution, Resolution> resolutionContext = new Dictionary<EResolution, Resolution>();
        EResolution resolutionType = EResolution.R800x450;
        bool windowed = true;
        string title = "Cereal Squad";
        View currentView;
        #endregion

        #region Event
        public delegate void WindowsEventHandler(object source, WindowsEventArgs e);

        public class WindowsEventArgs : EventArgs
        {
            public RenderWindow Windows { get; }

            public WindowsEventArgs(RenderWindow windows)
            {
                Windows = windows;
            }
        }

        public event WindowsEventHandler WindowsClosed;
        public event WindowsEventHandler WindowsCreated;
        #endregion

        public Renderer()
        {
            Win = null;
            // Set all predefine resolutions. Order Bigger to smaller
            resolutionContext[EResolution.R2880x1800] = new Resolution(2880, 1800);
            resolutionContext[EResolution.R1920x1080] = new Resolution(1920, 1080);
            resolutionContext[EResolution.R800x450] = new Resolution(800, 450);
#if !DEBUG
            resolutionType = findAppropriateResolution(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height);
            windowed = false;
#endif
            currentView = new View(new FloatRect(0, 0, 800, 450));
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

            return EResolution.R800x450;
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
        public void Initialization()
        {
            Win = new RenderWindow(new VideoMode(getWidth(), getHeight()), title, (windowed ? Styles.Close : Styles.Fullscreen));
            Win.SetView(currentView);
        }

        public void ResetWindow()
        {
            Win = new RenderWindow(new VideoMode(getWidth(), getHeight()), title, (windowed ? Styles.Close : Styles.Fullscreen));
            Win.SetView(currentView);
            Win.DefaultView.Viewport = new FloatRect(new Vector2f(0, 0), scaleToFit(new Vector2f(currentView.Viewport.Width, currentView.Viewport.Height), new Vector2f(getWidth(), getHeight())));
        }

        public static Vector2f scaleToFit(Vector2f inh, Vector2f clip )
        {
            Vector2f ret = new Vector2f(inh.X, inh.Y);

            if ( ( clip.Y* inh.X ) / inh.Y >= clip.X )
            {
                ret.Y = ( clip.X * inh.Y ) / inh.X;
                ret.X = clip.X;
            }
            else if ( ( clip.X * inh.Y ) / inh.X >= clip.Y )
            {
                ret.X = ( clip.Y * inh.X ) / inh.Y;
                ret.Y = clip.Y;
            }
            else
                ret = clip;
            return ret;
        }

        /// <summary>
        /// Exemple of simple loop
        /// </summary>
        public void Loop()
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
            return Win.IsOpen;
        }

        /// <summary>
        /// Dispatch event to Windows Manager
        /// </summary>
        public void DispatchEvents()
        {
            Win.DispatchEvents();
        }

        /// <summary>
        /// Clear the screen with the given color
        /// </summary>
        /// <param name="color">Color</param>
        public void Clear(Color color)
        {
            Win.Clear(color);
        }

        /// <summary>
        /// Display the buffer to screen
        /// </summary>
        public void Display()
        {
            Win.Display();
        }

        /// <summary>
        /// Change the current resolution of renderer.
        /// </summary>
        /// <param name="new_resolution">EResolution</param>
        public void ChangeResolution(EResolution new_resolution)
        {
            resolutionType = new_resolution;
            Win.Size = new Vector2u(getWidth(), getHeight());
            Win.DefaultView.Viewport = new FloatRect(new Vector2f(0, 0), scaleToFit(new Vector2f(currentView.Viewport.Width, currentView.Viewport.Height), new Vector2f(getWidth(), getHeight())));
            //Win.SetView(new View(new FloatRect(0, 0, getWidth(), getHeight())));
        }

        /// <summary>
        /// Change the option windowed of the windows. (Delete and recrete the window)
        /// </summary>
        /// <param name="enabled"></param>
        public void SetFullScreenEnabled(bool enabled)
        {
            windowed = !enabled;
            WindowsClosed?.Invoke(this, new WindowsEventArgs(Win));
            Win.Close();
            ResetWindow();
            WindowsCreated?.Invoke(this, new WindowsEventArgs(Win));
        }

        /// <summary>
        /// Set the framerate (ms)
        /// </summary>
        /// <param name="limit"></param>
        public void SetFrameRate(uint limit)
        {
            Win.SetFramerateLimit(limit);
        }

        /// <summary>
        /// Set the synchronisation Vertical
        /// </summary>
        /// <param name="SyncVertical"></param>
        public void SetSyncVertical(bool SyncVertical)
        {
            Win.SetVerticalSyncEnabled(SyncVertical);
        }

        /// <summary>
        /// Set the mouse cursor visible or hidden
        /// </summary>
        /// <param name="visible">bool</param>
        public void SetMouseCursorVisible(bool visible)
        {
            Win.SetMouseCursorVisible(visible);
        }

        /// <summary>
        /// Set the key repeated enabled
        /// </summary>
        /// <param name="enabled"></param>
        public void SetKeyRepeatedEnabled(bool enabled)
        {
            Win.SetKeyRepeatEnabled(enabled);
        }

        /// <summary>
        /// Draw drawable
        /// </summary>
        /// <param name="obj"></param>
        public void Draw (Drawable obj)
        {
            Win.Draw(obj);
        }
    }
}
