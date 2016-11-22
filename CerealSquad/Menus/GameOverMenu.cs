using CerealSquad.InputManager.Keyboard;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Menus
{
    class GameOverMenu : Menu
    {
        private Renderer _Renderer;
        private Text _GameOverText;
        //private GameWorld.GameManager _GameManager;

        public abstract class GameOverMenuItem : MenuItem
        {
            public GameOverMenuItem(GameOverAction action, ItemType type = ItemType.Normal, Key keyboardKey = Key.Unknown, uint joystickKey = 0) : base(type, keyboardKey, joystickKey)
            {
                Action = action;
            }

            public GameOverAction Action { get; protected set; }
        }

        public class ReturnMenuItem : GameOverMenuItem
        {
            Text _Text;

            public ReturnMenuItem(Renderer renderer, GameOverAction action = GameOverAction.ReturnToMainMenu, ItemType type = ItemType.Normal, Key keyboardKey = Key.Unknown, uint joystickKey = 0) : base(action, type, keyboardKey, joystickKey)
            {
                _Text = new Text("Return to Main Menu", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie));
                _Text.CharacterSize = 80;
                _Text.Position = new Vector2f(renderer.Win.GetView().Size.X / 2 - (_Text.GetLocalBounds().Left + _Text.GetLocalBounds().Width) / 2, renderer.Win.GetView().Size.Y / 2);
            }

            public override void Select(bool select)
            {
                base.Select(select);
                if (Selected)
                    _Text.Color = Color.Green;
                else
                    _Text.Color = Color.White;
            }

            public override void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(_Text, states);
            }
        }

        public enum GameOverAction
        {
            [Description("Empty")]
            Empty = -1,
            [Description("Return to Main Menu")]
            ReturnToMainMenu = 0
        }

        public GameOverMenu(Renderer renderer, InputManager.InputManager inputmanager) : base (inputmanager)
        {
            if (renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");
            if (_InputManager == null)
                throw new ArgumentNullException("InputManager cannot be null");

            _Renderer = renderer;

            _InputManager.KeyboardKeyPressed += _InputManager_KeyboardKeyPressed;
            _InputManager.KeyboardKeyReleased += _InputManager_KeyboardKeyReleased;
            _InputManager.JoystickButtonPressed += _InputManager_JoystickButtonPressed;
            _InputManager.JoystickButtonReleased += _InputManager_JoystickButtonReleased;
            _InputManager.JoystickMoved += _InputManager_JoystickMoved;

            _menuList.Add(new ReturnMenuItem(_Renderer));
            nextMenu();

            _GameOverText = new Text("Game Over", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular), 80);
            _GameOverText.Position = new Vector2f(renderer.Win.GetView().Size.X / 2 - (_GameOverText.GetLocalBounds().Left + _GameOverText.GetLocalBounds().Width) / 2, renderer.Win.GetView().Size.Y / 2.75f);
            _GameOverText.Color = Color.Red;
        }

        private void _ExecuteAction()
        {
            GameOverMenuItem _current = (GameOverMenuItem)_menuList.FirstOrDefault(i => i.Selected);

            if (_current != null)
            {
                switch (_current.Action)
                {
                    case GameOverAction.ReturnToMainMenu:
                        MenuManager.Instance.Clear();
                        break;
                }
            }
        }

        #region 
        private Key _KeyPressed = Key.Unknown;
        private void _InputManager_KeyboardKeyReleased(object source, KeyEventArgs e)
        {
            if (Displayed)
            {
                if (e.KeyCode == _KeyPressed)
                {
                    switch (e.KeyCode)
                    {
                        case Key.Return:
                            _ExecuteAction();
                            break;
                    }
                }
                _KeyPressed = Key.Unknown;
            }
        }
        private void _InputManager_KeyboardKeyPressed(object source, KeyEventArgs e)
        {
            if (Displayed)
            {
                _KeyPressed = e.KeyCode;
                switch (_KeyPressed)
                {
                    case Key.Return:

                        break;
                    case Key.Down:
                        nextMenu();
                        break;
                    case Key.Up:
                        previousMenu();
                        break;
                }
            }
        }

        private float lastYAxis = 0;
        private int lastJoyButton = -1;
        private void _InputManager_JoystickMoved(object source, InputManager.Joystick.MoveEventArgs e)
        {
            if (Displayed)
            {
                if (e.Axis == InputManager.Joystick.Axis.Y)
                {
                    if (e.Position > 90 && e.Position > lastYAxis)
                    {
                        nextMenu();
                        lastYAxis = 150;
                    }
                    else if (e.Position < -90 && e.Position < lastYAxis)
                    {
                        previousMenu();
                        lastYAxis = -150;
                    }
                    else
                        lastYAxis = e.Position;
                }
            }
        }
        private void _InputManager_JoystickButtonReleased(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            if (Displayed)
            {
                if ((int)e.Button == lastJoyButton)
                {
                    if (e.Button == 0)
                        _ExecuteAction();
                }
            }
        }
        private void _InputManager_JoystickButtonPressed(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            if (Displayed)
            {
                lastJoyButton = (int)e.Button;
            }
        }
        #endregion

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_GameOverText, states);
            base.Draw(target, states);
        }
    }
}
