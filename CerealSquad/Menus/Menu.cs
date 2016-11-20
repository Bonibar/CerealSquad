using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.Menus
{
    public abstract class MenuItem : Drawable
    {
        public enum ItemType
        {
            Normal,
            Disabled,
            KeyBinded
        }

        private MenuItem() { }
        public MenuItem(ItemType type = ItemType.Normal, InputManager.Keyboard.Key keyboardKey = InputManager.Keyboard.Key.Unknown, uint joystickKey = 0)
        {
            Type = type;
            KeyboardKey = keyboardKey;
            JoystickKey = joystickKey;
        }

        public virtual void Select(bool select)
        {
            Selected = select;
        }

        public InputManager.Keyboard.Key KeyboardKey { get; protected set; }
        public uint JoystickKey { get; protected set; }
        public ItemType Type { get; protected set; }
        public bool Selected { get; protected set; }

        public abstract void Draw(RenderTarget target, RenderStates states);
    }

    public abstract class Menu : Drawable
    {
        private bool _displayed = false;
        public bool Displayed { get { return _displayed; } }

        protected List<MenuItem> _menuList = new List<MenuItem>();

        protected void nextMenu()
        {
            List<MenuItem> validItems = _menuList.FindAll(x => x.Type == MenuItem.ItemType.Normal);

            MenuItem _current = validItems.FirstOrDefault<MenuItem>(x => x.Selected == true);
            if (_current == null && validItems.Count > 0)
            {
                validItems[0].Select(true);
            }
            else if (validItems.Count > 0)
            {
                int index = validItems.IndexOf(_current) + 1;
                MenuItem _next = validItems.ElementAt(index > validItems.Count - 1 ? 0 : index);

                _current.Select(false);
                _next.Select(true);
            }
        }
        protected void previousMenu()
        {
            List<MenuItem> validItems = _menuList.FindAll(x => x.Type == MenuItem.ItemType.Normal);

            MenuItem _current = validItems.FirstOrDefault<MenuItem>(x => x.Selected == true);
            if (_current == null && validItems.Count > 0)
            {
                validItems[0].Select(true);
            }
            else if (validItems.Count > 0)
            {
                int index = validItems.IndexOf(_current) - 1;
                MenuItem _prev = validItems.ElementAt(index < 0 ? validItems.Count - 1 : index);

                _current.Select(false);
                _prev.Select(true);
            }
        }

        public virtual void Update(SFML.System.Time DeltaTime) { }

        public virtual void Show()
        {
            System.Diagnostics.Debug.WriteLine("SHOWED MENU");
            if (!Displayed)
            {
                _displayed = true;
            }
        }

        public virtual void Hide()
        {
            if (Displayed)
            {
                _displayed = false;
            }
        }

        public virtual void Toggle()
        {
            if (Displayed)
                Hide();
            else
                Show();
        }

        public virtual void Exit()
        {
            MenuManager.Instance.RemoveMenu(this);
        }

        public void AddItem(MenuItem item)
        {
            _menuList.Add(item);
        }

        public void RemoveItem(MenuItem item)
        {
            _menuList.Remove(item);
        }

        public void ClearItems()
        {
            _menuList.Clear();
        }
            
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            _menuList.ForEach(i => target.Draw(i, states));
        }
    }
}
