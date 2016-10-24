using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus
{
    public class MenuItem
    {
        public enum State
        {
            Normal,
            Disabled,
            Selected
        }

        private MenuItem() { }
        public MenuItem(uint order, Buttons.IButton normalState, Buttons.IButton selectedState = null, Buttons.IButton disabledState = null, bool isDisabled = false)
        {
            Order = order;
            if (normalState != null)
                _States.Add(State.Normal, normalState);
            if (selectedState != null)
                _States.Add(State.Selected, selectedState);
            if (disabledState != null)
                _States.Add(State.Disabled, disabledState);
            if (isDisabled && disabledState != null)
                CurrentState = State.Disabled;
            else
                CurrentState = State.Normal;
        }

        public uint Order { get; private set; }
        private State _CurrentState;
        public State CurrentState {
            get { return _CurrentState; }
            set
            {
                if (_States.ContainsKey(value))
                    _CurrentState = value;
            }
        }

        private Dictionary<State, Buttons.IButton> _States = new Dictionary<State, Buttons.IButton>();

        public Buttons.IButton GetButton() { return _States[CurrentState]; }
        public Buttons.IButton GetButton(State state)
        {
            return null;
        }
    }

    public class AMenu
    {
        protected SFML.Graphics.RenderWindow _Win;
        protected InputManager.InputManager _InputManager;

        protected Factories.FontFactory.FontFactory fontFactory = Factories.FontFactory.FontFactory.Instance;

        private bool _displayed = false;
        public bool Displayed { get { return _displayed; } }

        protected List<MenuItem> _menuList = new List<MenuItem>();

        public AMenu(SFML.Graphics.RenderWindow win, ref InputManager.InputManager inputManager)
        {
            _InputManager = inputManager;
            _Win = win;

            MenuItem item = new MenuItem(0, new Buttons.TextButton("TestLol", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular), 0), new Buttons.TextButton("> TestLol", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular), 0));
            _menuList.Add(item);
            item = new MenuItem(1, new Buttons.TextButton("TestLol2", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular), 50), new Buttons.TextButton("> TestLol2", fontFactory.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular), 50));
            _menuList.Add(item);
            _menuList.First<MenuItem>().CurrentState = MenuItem.State.Selected;

            _InputManager.KeyboardKeyPressed += _InputManager_KeyboardKeyPressed;
        }

        private void nextMenu()
        {
            MenuItem _next = null;
            MenuItem _current = _menuList.Find((MenuItem item) =>
            {
                if (item.CurrentState.Equals(MenuItem.State.Selected))
                    return true;
                return false;
            });
            int index = _menuList.IndexOf(_current);
            if (index >= _menuList.Count - 1)
                _next = _menuList.First<MenuItem>();
            else
                _next = _menuList.ElementAt(index + 1);
            
            if (!_current.Equals(_next))
            {
                _current.CurrentState = MenuItem.State.Normal;
                _next.CurrentState = MenuItem.State.Selected;
            }
        }

        private void previousMenu()
        {
            MenuItem _prev = null;
            MenuItem _current = _menuList.Find((MenuItem item) =>
            {
                if (item.CurrentState.Equals(MenuItem.State.Selected))
                    return true;
                return false;
            });
            int index = _menuList.IndexOf(_current);
            if (index <= 0)
                _prev = _menuList.Last<MenuItem>();
            else
                _prev = _menuList.ElementAt(index - 1);

            if (!_current.Equals(_prev))
            {
                _current.CurrentState = MenuItem.State.Normal;
                _prev.CurrentState = MenuItem.State.Selected;
            }
        }

        private void _InputManager_KeyboardKeyPressed(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (Displayed)
            {
                if (e.KeyCode.Equals(InputManager.Keyboard.Key.Down))
                {
                    nextMenu();
                }
                else if (e.KeyCode.Equals(InputManager.Keyboard.Key.Up))
                {
                    previousMenu();
                }
            }
        }

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
                _menuList.ForEach((MenuItem item) => {
                    _Win.Draw(item.GetButton().getDrawable());
                });
            }
        }
    }
}
