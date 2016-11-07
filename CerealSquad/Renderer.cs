using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.ComponentModel;

namespace CerealSquad
{
    public class Renderer
    {
        public enum EResolution
        {
            [Description("3840x2160")]
            R3840x2160,
            [Description("2560x1440")]
            R2560x1440,
            [Description("1920x1080")]
            R1920x1080,
            [Description("1600x900")]
            R1600x900,
            [Description("1366x768")]
            R1366x768,
            [Description("1280x720")]
            R1280x720,
            [Description("1024x576")]
            R1024x576,
            [Description("854x480")]
            R854x480,
            [Description("800x450")]
            R800x450,
        };

        struct SResolution
        {
            public uint width { get; set; }
            public uint height { get; set; }

            public SResolution(uint _width = 800, uint _height = 600)
            {
                width = _width;
                height = _height;
            }
        };

        #region Ref
        public RenderWindow Win { get; private set; }
        #endregion

        #region Windows Parameters

        private bool _fullScreen = false;
        public bool FullScreen { get { return _fullScreen; } set { _fullScreen = value; SetFullScreen(value); } }

        private String _title = "Cereal Squad";
        public String Title { get { return _title; } set { _title = value;  if (Win != null) Win.SetTitle(value); } }

        private uint _frameRate = 60;
        public uint FrameRate {  get { return _frameRate;  } set { _frameRate = value; if (Win != null) Win.SetFramerateLimit(value); } }

        private bool _verticalSync = true;
        public bool VerticalSync {  get { return _verticalSync; } set { _verticalSync = value; if (Win != null) Win.SetVerticalSyncEnabled(value); } }

        private bool _keyRepeated = false;
        public bool KeyRepeated { get { return _keyRepeated; } set { _keyRepeated = value; if (Win != null) Win.SetKeyRepeatEnabled(value); } }

        private bool _mouseCursorVisible = false;
        public bool MouseCursorVisible {  get { return _mouseCursorVisible; } set { _mouseCursorVisible = value; if (Win != null) Win.SetMouseCursorVisible(value); } }

        private EResolution _resolution = EResolution.R800x450;
        public EResolution Resolution { get { return _resolution; } set { _resolution = value; SetResolution(value); } }


        private Dictionary<EResolution, SResolution> resolutionContext = new Dictionary<EResolution, SResolution>();
        private View currentView;
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
            resolutionContext[EResolution.R3840x2160] = new SResolution(3840, 2160);
            resolutionContext[EResolution.R2560x1440] = new SResolution(2560, 1440);
            resolutionContext[EResolution.R1920x1080] = new SResolution(1920, 1080);
            resolutionContext[EResolution.R1600x900] = new SResolution(1600, 900);
            resolutionContext[EResolution.R1366x768] = new SResolution(1366, 768);
            resolutionContext[EResolution.R1280x720] = new SResolution(1280, 720);
            resolutionContext[EResolution.R1024x576] = new SResolution(1024, 576);
            resolutionContext[EResolution.R854x480] = new SResolution(854, 480);
            resolutionContext[EResolution.R800x450] = new SResolution(800, 450);
#if !DEBUG
            resolutionType = findAppropriateResolution(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height);
            windowed = false;
#endif
            currentView = new View(new FloatRect(0, 0, 1920, 1080));
        }

        /// <summary>
        /// Look the current resolution of Deskop and find the appropriate resolution
        /// </summary>
        /// <param name="width">Uint</param>
        /// <param name="height">Uint</param>
        /// <returns>EResolution</returns>
        private EResolution findAppropriateResolution(uint width, uint height)
        {
            foreach (KeyValuePair<EResolution, SResolution> entry in resolutionContext) {
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
            return resolutionContext[Resolution].width;
        }

        /// <summary>
        /// Return current Height used in Renderer
        /// </summary>
        /// <returns>Uint</returns>
        public uint getHeight()
        {
            return resolutionContext[Resolution].height;
        }

        /// <summary>
        /// Initialisation
        /// </summary>
        public void Initialization()
        {
            Win = new RenderWindow(new VideoMode(getWidth(), getHeight()), Title, (FullScreen ? Styles.Fullscreen : Styles.Close));
            Win.SetView(currentView);
            Win.SetKeyRepeatEnabled(_keyRepeated);
            Win.SetMouseCursorVisible(_mouseCursorVisible);
            Win.SetFramerateLimit(_frameRate);
            Win.SetVerticalSyncEnabled(_verticalSync);
            Win.Position = new Vector2i((int)VideoMode.DesktopMode.Width / 2 - (int)getWidth() / 2, (int)VideoMode.DesktopMode.Height / 2 - (int)getHeight() / 2);
        }

        public void ResetWindow()
        {
            Win = new RenderWindow(new VideoMode(getWidth(), getHeight()), Title, (FullScreen ? Styles.Fullscreen : Styles.Close));
            Win.SetView(currentView);
            Win.DefaultView.Viewport = new FloatRect(new Vector2f(0, 0), scaleToFit(new Vector2f(currentView.Viewport.Width, currentView.Viewport.Height), new Vector2f(getWidth(), getHeight())));
            Win.SetKeyRepeatEnabled(_keyRepeated);
            Win.SetMouseCursorVisible(_mouseCursorVisible);
            Win.SetFramerateLimit(_frameRate);
            Win.SetVerticalSyncEnabled(_verticalSync);
            Win.Position = new Vector2i((int)VideoMode.DesktopMode.Width / 2 - (int)getWidth() / 2, (int)VideoMode.DesktopMode.Height / 2 - (int)getHeight() / 2);
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
            if (Win == null)
                return;
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
           if (Win == null)
                return false;
            return Win.IsOpen;
        }

        /// <summary>
        /// Dispatch event to Windows Manager
        /// </summary>
        public void DispatchEvents()
        {
            if (Win == null)
                return;
            Win.DispatchEvents();
        }

        /// <summary>
        /// Clear the screen with the given color
        /// </summary>
        /// <param name="color">Color</param>
        public void Clear(Color color)
        {
            if (Win == null)
                return;
            Win.Clear(color);
        }

        /// <summary>
        /// Display the buffer to screen
        /// </summary>
        public void Display()
        {
            if (Win == null)
                return;
            Win.Display();
        }

        /// <summary>
        /// Move the camera
        /// </summary>
        /// <param name="x">int</param>
        /// <param name="y">int</param>
        public void Move(int x, int y)
        {
            if (Win == null)
                return;
            Win.DefaultView.Center = new Vector2f(x, y);
        }

        /// <summary>
        /// Change the current resolution of renderer.
        /// </summary>
        /// <param name="new_resolution">EResolution</param>
        public void SetResolution(EResolution new_resolution)
        {
            if (Win == null)
                return;
            Win.Size = new Vector2u(getWidth(), getHeight());
            Win.DefaultView.Viewport = new FloatRect(new Vector2f(0, 0), scaleToFit(new Vector2f(currentView.Viewport.Width, currentView.Viewport.Height), new Vector2f(getWidth(), getHeight())));
            Win.Position = new Vector2i((int)VideoMode.DesktopMode.Width / 2 - (int)getWidth() / 2, (int)VideoMode.DesktopMode.Height / 2 - (int)getHeight() / 2);
        }

        /// <summary>
        /// Change the option windowed of the windows. (Delete and recrete the window)
        /// </summary>
        /// <param name="enabled"></param>
        public void SetFullScreen(bool enabled)
        {
            if (Win == null)
                return;
            WindowsClosed?.Invoke(this, new WindowsEventArgs(Win));
            Win.Close();
            ResetWindow();
            WindowsCreated?.Invoke(this, new WindowsEventArgs(Win));
        }

        /// <summary>
        /// Draw drawable
        /// </summary>
        /// <param name="obj"></param>
        public void Draw (Drawable obj)
        {
            if (obj != null)
            {
                Win.Draw(obj);
            }
        }
    }
}
