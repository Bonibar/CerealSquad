using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using System.Xml.Serialization;

namespace CerealSquad.InputManager
{
    namespace Keyboard
    {
        public delegate void KeyEventHandler(object source, KeyEventArgs e);

        public enum Key
        {
            Unknown = SFML.Window.Keyboard.Key.Unknown,
            A = SFML.Window.Keyboard.Key.A,
            B = SFML.Window.Keyboard.Key.B,
            C = SFML.Window.Keyboard.Key.C,
            D = SFML.Window.Keyboard.Key.D,
            E = SFML.Window.Keyboard.Key.E,
            F = SFML.Window.Keyboard.Key.F,
            G = SFML.Window.Keyboard.Key.G,
            H = SFML.Window.Keyboard.Key.H,
            I = SFML.Window.Keyboard.Key.I,
            J = SFML.Window.Keyboard.Key.J,
            K = SFML.Window.Keyboard.Key.K,
            L = SFML.Window.Keyboard.Key.L,
            M = SFML.Window.Keyboard.Key.M,
            N = SFML.Window.Keyboard.Key.N,
            O = SFML.Window.Keyboard.Key.O,
            P = SFML.Window.Keyboard.Key.P,
            Q = SFML.Window.Keyboard.Key.Q,
            R = SFML.Window.Keyboard.Key.R,
            S = SFML.Window.Keyboard.Key.S,
            T = SFML.Window.Keyboard.Key.T,
            U = SFML.Window.Keyboard.Key.U,
            V = SFML.Window.Keyboard.Key.V,
            W = SFML.Window.Keyboard.Key.W,
            X = SFML.Window.Keyboard.Key.X,
            Y = SFML.Window.Keyboard.Key.Y,
            Z = SFML.Window.Keyboard.Key.Z,
            Num0 = SFML.Window.Keyboard.Key.Num0,
            Num1 = SFML.Window.Keyboard.Key.Num1,
            Num2 = SFML.Window.Keyboard.Key.Num2,
            Num3 = SFML.Window.Keyboard.Key.Num3,
            Num4 = SFML.Window.Keyboard.Key.Num4,
            Num5 = SFML.Window.Keyboard.Key.Num5,
            Num6 = SFML.Window.Keyboard.Key.Num6,
            Num7 = SFML.Window.Keyboard.Key.Num7,
            Num8 = SFML.Window.Keyboard.Key.Num8,
            Num9 = SFML.Window.Keyboard.Key.Num9,
            Escape = SFML.Window.Keyboard.Key.Escape,
            LControl = SFML.Window.Keyboard.Key.LControl,
            LShift = SFML.Window.Keyboard.Key.LShift,
            LAlt = SFML.Window.Keyboard.Key.LAlt,
            LSystem = SFML.Window.Keyboard.Key.LSystem,
            RControl = SFML.Window.Keyboard.Key.RControl,
            RShift = SFML.Window.Keyboard.Key.RShift,
            RAlt = SFML.Window.Keyboard.Key.RAlt,
            RSystem = SFML.Window.Keyboard.Key.RSystem,
            Menu = SFML.Window.Keyboard.Key.Menu,
            LBracket = SFML.Window.Keyboard.Key.LBracket,
            RBracket = SFML.Window.Keyboard.Key.RBracket,
            SemiColon = SFML.Window.Keyboard.Key.SemiColon,
            Comma = SFML.Window.Keyboard.Key.Comma,
            Period = SFML.Window.Keyboard.Key.Period,
            Quote = SFML.Window.Keyboard.Key.Quote,
            Slash = SFML.Window.Keyboard.Key.Slash,
            BackSlash = SFML.Window.Keyboard.Key.BackSlash,
            Tilde = SFML.Window.Keyboard.Key.Tilde,
            Equal = SFML.Window.Keyboard.Key.Equal,
            Dash = SFML.Window.Keyboard.Key.Dash,
            Space = SFML.Window.Keyboard.Key.Space,
            Return = SFML.Window.Keyboard.Key.Return,
            BackSpace = SFML.Window.Keyboard.Key.BackSpace,
            Tab = SFML.Window.Keyboard.Key.Tab,
            PageUp = SFML.Window.Keyboard.Key.PageUp,
            PageDown = SFML.Window.Keyboard.Key.PageDown,
            End = SFML.Window.Keyboard.Key.End,
            Home = SFML.Window.Keyboard.Key.Home,
            Insert = SFML.Window.Keyboard.Key.Insert,
            Delete = SFML.Window.Keyboard.Key.Delete,
            Add = SFML.Window.Keyboard.Key.Add,
            Subtract = SFML.Window.Keyboard.Key.Subtract,
            Multiply = SFML.Window.Keyboard.Key.Multiply,
            Divide = SFML.Window.Keyboard.Key.Divide,
            Left = SFML.Window.Keyboard.Key.Left,
            Right = SFML.Window.Keyboard.Key.Right,
            Up = SFML.Window.Keyboard.Key.Up,
            Down = SFML.Window.Keyboard.Key.Down,
            Numpad0 = SFML.Window.Keyboard.Key.Numpad0,
            Numpad1 = SFML.Window.Keyboard.Key.Numpad0,
            Numpad2 = SFML.Window.Keyboard.Key.Numpad2,
            Numpad3 = SFML.Window.Keyboard.Key.Numpad3,
            Numpad4 = SFML.Window.Keyboard.Key.Numpad4,
            Numpad5 = SFML.Window.Keyboard.Key.Numpad5,
            Numpad6 = SFML.Window.Keyboard.Key.Numpad6,
            Numpad7 = SFML.Window.Keyboard.Key.Numpad7,
            Numpad8 = SFML.Window.Keyboard.Key.Numpad8,
            Numpad9 = SFML.Window.Keyboard.Key.Numpad9,
            F1 = SFML.Window.Keyboard.Key.F1,
            F2 = SFML.Window.Keyboard.Key.F2,
            F3 = SFML.Window.Keyboard.Key.F3,
            F4 = SFML.Window.Keyboard.Key.F4,
            F5 = SFML.Window.Keyboard.Key.F5,
            F6 = SFML.Window.Keyboard.Key.F6,
            F7 = SFML.Window.Keyboard.Key.F7,
            F8 = SFML.Window.Keyboard.Key.F8,
            F9 = SFML.Window.Keyboard.Key.F9,
            F10 = SFML.Window.Keyboard.Key.F10,
            F11 = SFML.Window.Keyboard.Key.F11,
            F12 = SFML.Window.Keyboard.Key.F12,
            F13 = SFML.Window.Keyboard.Key.F13,
            F14 = SFML.Window.Keyboard.Key.F14,
            F15 = SFML.Window.Keyboard.Key.F15,
            Pause = SFML.Window.Keyboard.Key.Pause,
            KeyCount = SFML.Window.Keyboard.Key.KeyCount,
        }

        public class KeyEventArgs : EventArgs
        {
            public Key KeyCode { get; }
            /// <summary>
            /// Is the Shift key pressed?
            /// </summary>
            public bool Shift { get; }
            /// <summary>
            /// Is the Ctrl key pressed?
            /// </summary>
            public bool Ctrl { get; }
            /// <summary>
            /// Is the Alt key pressed?
            /// </summary>
            public bool Alt { get; }
            /// <summary>
            /// Is the System key pressed?
            /// </summary>
            public bool System { get; }

            public KeyEventArgs(SFML.Window.KeyEventArgs keyboardEvent)
            {
                KeyCode = (Key)keyboardEvent.Code;
                Shift = keyboardEvent.Shift;
                Ctrl = keyboardEvent.Control;
                Alt = keyboardEvent.Alt;
                System = keyboardEvent.System;
            }
            public KeyEventArgs(Key keyCode, bool shiftPressed = false, bool ctrlPressed = false, bool altPressed = false, bool systemPressed = false)
            {
                KeyCode = keyCode;
                Shift = shiftPressed;
                Ctrl = ctrlPressed;
                Alt = altPressed;
                System = systemPressed;
            }
        }
    }

    namespace Joystick
    {
        public delegate void ButtonEventHandler(object source, ButtonEventArgs e);
        public delegate void MoveEventHandler(object source, MoveEventArgs e);

        public delegate void ConnectEventHandler(object source, ConnectionEventArgs e);
        public delegate void DisconnectEventHandler(object source, ConnectionEventArgs e);

        public enum Axis
        {
            X = SFML.Window.Joystick.Axis.X,
            Y = SFML.Window.Joystick.Axis.Y,
            Z = SFML.Window.Joystick.Axis.Z,
            R = SFML.Window.Joystick.Axis.R,
            U = SFML.Window.Joystick.Axis.U,
            V = SFML.Window.Joystick.Axis.V,
            PovX = SFML.Window.Joystick.Axis.PovX,
            PovY = SFML.Window.Joystick.Axis.PovY
        }

        public class ButtonEventArgs
        {
            /// <summary>
            /// Index of the joystick
            /// </summary>
            public uint JoystickId { get; }
            /// <summary>
            /// Index of the button
            /// </summary>
            public uint Button { get; }

            public ButtonEventArgs(SFML.Window.JoystickButtonEventArgs e)
            {
                Button = e.Button;
                JoystickId = e.JoystickId;
            }
            public ButtonEventArgs(uint button, uint joystickId)
            {
                Button = button;
                JoystickId = joystickId;
            }
        }
        public class MoveEventArgs
        {
            /// <summary>
            /// Index of the joystick
            /// </summary>
            public uint JoystickId { get; }
            /// <summary>
            /// Current position of the axis, in range [-100 .. 100]
            /// </summary>
            public float Position { get; }
            public Axis Axis { get; }

            public MoveEventArgs(SFML.Window.JoystickMoveEventArgs e)
            {
                JoystickId = e.JoystickId;
                Position = e.Position;
                Axis = (Axis)e.Axis;
            }
            public MoveEventArgs(uint joystickId, float position, Axis axis)
            {
                JoystickId = joystickId;
                Position = position;
                Axis = axis;
            }
        }
        public class ConnectionEventArgs
        {
            /// <summary>
            /// Index of the joystick
            /// </summary>
            public uint JoystickId { get; }

            public ConnectionEventArgs(SFML.Window.JoystickConnectEventArgs e)
            {
                JoystickId = e.JoystickId;
            }
            public ConnectionEventArgs(uint joystickId)
            {
                JoystickId = joystickId;
            }
        }
    }

    namespace Player
    {
        public enum Type
        {
            Keyboard,
            Controller
        }

        public class Player
        {
            private static readonly string FOLDER_PATH = "KeyMaps/";

            public uint Id { get; }
            public Type Type { get; }

            private Dictionary<int, int> _KeyMap = new Dictionary<int, int>();
            XmlSerializer serializer = new XmlSerializer(typeof(item[]), new XmlRootAttribute() { ElementName = "items" });

            private Player() { }
            public Player(uint id, Type type)
            {
                Id = id;
                Type = type;

                LoadKeyMap();
            }

            public void LoadKeyMap()
            {
                System.IO.Directory.CreateDirectory(FOLDER_PATH);
                if (System.IO.File.Exists(FOLDER_PATH + Id + "_" + Enum.GetName(typeof(Type), Type) + ".km"))
                {
                    System.IO.FileStream stream = new System.IO.FileStream(FOLDER_PATH + Id + "_" + Enum.GetName(typeof(Type), Type) + ".km", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
                    var _KeyMap = ((item[])serializer.Deserialize(stream)).ToDictionary(i => i.Key, i => i.Function);
                    stream.Close();
                }
                else
                {
                    ResetKeyMap();
                }
            }

            public void ResetKeyMap()
            {
                _KeyMap.Clear();
                if (Type == Type.Keyboard && Id == 1)
                {
                    _KeyMap.Add((int)Keyboard.Key.Z, 0);
                    _KeyMap.Add((int)Keyboard.Key.S, 1);
                    _KeyMap.Add((int)Keyboard.Key.D, 2);
                    _KeyMap.Add((int)Keyboard.Key.Q, 3);
                    _KeyMap.Add((int)Keyboard.Key.Space, 4);
                    _KeyMap.Add((int)Keyboard.Key.Escape, 6);
                } else if (Type == Type.Keyboard && Id == 2)
                {
                    _KeyMap.Add((int)Keyboard.Key.Numpad5, 0);
                    _KeyMap.Add((int)Keyboard.Key.Numpad2, 1);
                    _KeyMap.Add((int)Keyboard.Key.Numpad3, 2);
                    _KeyMap.Add((int)Keyboard.Key.Numpad1, 3);
                }
            }

            public void SetKey(int key, int function)
            {
                if (_KeyMap.Keys.Contains(key))
                    _KeyMap[key] = function;
                else
                    _KeyMap.Add(key, function);
            }

            public int GetFunction(int key)
            {
                if (_KeyMap.Keys.Contains(key))
                    return _KeyMap[key];
                return -1;
            }

            public void SaveKeyMap()
            {
                System.IO.Directory.CreateDirectory(FOLDER_PATH);
                System.IO.FileStream stream = new System.IO.FileStream(FOLDER_PATH + Id + "_" + Enum.GetName(typeof(Type), Type) + ".km", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                serializer.Serialize(stream, _KeyMap.Select(kv => new item() { Key = kv.Key, Function = kv.Value }).ToArray());
                stream.Flush(true);
                stream.Close();
            }

            public class item
            {
                public int Key;
                public int Function;
            }
        }
    }

    /// <summary>
    /// Class managing all input events (Keyboard, Joystick, etc)
    /// </summary>
    public class InputManager
    {
        private Window _Win;
        private Renderer _Renderer;

        private List<Player.Player> _KeyMaps = new List<Player.Player>();

        /// <summary>
        /// Event fired when a Keyboard key has been pressed
        /// </summary>
        public event Keyboard.KeyEventHandler KeyboardKeyPressed;
        /// <summary>
        /// Event fired when a Keyboard key has been released
        /// </summary>
        public event Keyboard.KeyEventHandler KeyboardKeyReleased;

        /// <summary>
        /// Event fired when a Controller button has been pressed
        /// </summary>
        public event Joystick.ButtonEventHandler JoystickButtonPressed;
        /// <summary>
        /// Event fired when a Controller button has been released
        /// </summary>
        public event Joystick.ButtonEventHandler JoystickButtonReleased;
        /// <summary>
        /// Event fired when a Controller movement has been detected
        /// </summary>
        public event Joystick.MoveEventHandler JoystickMoved;

        /// <summary>
        /// Event fired when a Controller is connected
        /// </summary>
        public event Joystick.ConnectEventHandler JoystickConnected;
        /// <summary>
        /// Event fired when a Controller is disconnected
        /// </summary>
        public event Joystick.ConnectEventHandler JoystickDisconnected;

        private InputManager() { }
        /// <summary>
        /// Manage all input events based on a SFML Window
        /// </summary>
        /// <param name="win">Window that will be listened for input events</param>
        public InputManager(Renderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");
            _Renderer = renderer;

            _Renderer.WindowsClosed += _Renderer_WindowsClosed;
            _Renderer.WindowsCreated += _Renderer_WindowsCreated;

            if (renderer.Win != null)
                registerWindow(renderer.Win);
        }


        public void SaveKeyMaps()
        {
            _KeyMaps.ForEach(x => x.SaveKeyMap());
        }
        public void LoadKeyMaps()
        {
            _KeyMaps.ForEach(x => x.LoadKeyMap());
        }
        public void SetAssociateFunction(int id, Player.Type type, int key, int function)
        {
            Player.Player _current = _KeyMaps.Find(x => x.Type == type && x.Id == id);
            if (_current == null)
                throw new ArgumentOutOfRangeException("KeyMap id not found");
            _current.SetKey(key, function);
        }
        public int GetAssociateFunction(int id, Player.Type type, int key)
        {
            if (_KeyMaps.Count == 0)
                _KeyMaps.Add(new Player.Player(1, Player.Type.Keyboard));
            Player.Player _current = _KeyMaps.Find(x => x.Type == type && x.Id == id);
            if (_current == null)
                throw new ArgumentOutOfRangeException("KeyMap id not found");
            return _current.GetFunction(key);
        }

        private void _Renderer_WindowsCreated(object source, Renderer.WindowsEventArgs e)
        {
            if (_Win != null)
                unregisterWindow();
            registerWindow(e.Windows);
        }
        private void _Renderer_WindowsClosed(object source, Renderer.WindowsEventArgs e)
        {
            if (e.Windows == _Win)
                unregisterWindow();
        }

        private void registerWindow(Window win)
        {
            if (win != null)
            {
                if (_Win != null)
                    unregisterWindow();
                _Win = win;
                _Win.KeyPressed += SFML_KeyboardKeyPressed;
                _Win.KeyReleased += SFML_KeyboardKeyReleased;

                _Win.JoystickButtonPressed += SFML_JoystickButtonPressed;
                _Win.JoystickButtonReleased += SFML_JoystickButtonReleased;
                _Win.JoystickMoved += SFML_JoystickMoved;

                _Win.JoystickConnected += SFML_JoystickConnected;
                _Win.JoystickDisconnected += SFML_JoystickDisconnected;
            }
        }
        private void unregisterWindow()
        {
            if (_Win != null)
            {
                _Win.KeyPressed -= SFML_KeyboardKeyPressed;
                _Win.KeyReleased -= SFML_KeyboardKeyReleased;

                _Win.JoystickButtonPressed -= SFML_JoystickButtonPressed;
                _Win.JoystickButtonReleased -= SFML_JoystickButtonReleased;
                _Win.JoystickMoved -= SFML_JoystickMoved;

                _Win.JoystickConnected -= SFML_JoystickConnected;
                _Win.JoystickDisconnected -= SFML_JoystickDisconnected;
                _Win = null;
            }
        }

        private void SFML_JoystickConnected(object sender, JoystickConnectEventArgs e)
        {
            JoystickConnected?.Invoke(sender, new Joystick.ConnectionEventArgs(e));
        }
        private void SFML_JoystickDisconnected(object sender, JoystickConnectEventArgs e)
        {
            JoystickDisconnected?.Invoke(sender, new Joystick.ConnectionEventArgs(e));
        }

        private void SFML_JoystickMoved(object sender, JoystickMoveEventArgs e)
        {
            JoystickMoved?.Invoke(sender, new Joystick.MoveEventArgs(e));
        }
        private void SFML_JoystickButtonPressed(object sender, JoystickButtonEventArgs e)
        {
            JoystickButtonPressed?.Invoke(sender, new Joystick.ButtonEventArgs(e));
        }
        private void SFML_JoystickButtonReleased(object sender, JoystickButtonEventArgs e)
        {
            JoystickButtonReleased?.Invoke(sender, new Joystick.ButtonEventArgs(e));
        }

        private void SFML_KeyboardKeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            KeyboardKeyPressed?.Invoke(sender, new Keyboard.KeyEventArgs(e));
        }
        private void SFML_KeyboardKeyReleased(object sender, SFML.Window.KeyEventArgs e)
        {
            KeyboardKeyReleased?.Invoke(sender, new Keyboard.KeyEventArgs(e));
        }
    }
}
