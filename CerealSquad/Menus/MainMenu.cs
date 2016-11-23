using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.InputManager.Keyboard;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Menus
{
    class MainMenu : Menu
    {
        #region Items
        public abstract class MainMenuItem : MenuItem
        {
            public MainMenuItem(ItemAction action, ItemType type = ItemType.Normal, Key keyboardKey = Key.Unknown, uint joystickKey = 0) : base(type, keyboardKey, joystickKey)
            {
                Action = action;
            }

            public ItemAction Action { get; protected set; }
        }

        public class NewGameItem : MainMenuItem
        {
            Text _Text;

            public NewGameItem(Renderer renderer, ItemAction action = ItemAction.NewGame, ItemType type = ItemType.Normal, Key keyboardKey = Key.Unknown, uint joystickKey = 0) : base(action, type, keyboardKey, joystickKey)
            {
                _Text = new Text("New Game", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie));
                _Text.CharacterSize = 80 * (uint)renderer.Win.GetView().Size.X / 1980;
                _Text.Position = new Vector2f(renderer.Win.GetView().Size.X / 2 - (_Text.GetLocalBounds().Left + _Text.GetLocalBounds().Width) / 2,
                renderer.Win.GetView().Size.Y / 2 - (_Text.GetLocalBounds().Height + _Text.GetLocalBounds().Top) / 2 - _Text.CharacterSize);
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
        public class ExitItem : MainMenuItem
        {
            Text _Text;

            public ExitItem(Renderer renderer, ItemAction action = ItemAction.Exit, ItemType type = ItemType.Normal, Key keyboardKey = Key.Unknown, uint joystickKey = 0) : base(action, type, keyboardKey, joystickKey)
            {
                _Text = new Text("Exit", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie));
                _Text.CharacterSize = 80 * (uint)renderer.Win.GetView().Size.X / 1980;
                _Text.Position = new Vector2f(renderer.Win.GetView().Size.X / 2 - (_Text.GetLocalBounds().Left + _Text.GetLocalBounds().Width) / 2,
                    renderer.Win.GetView().Size.Y / 2 - (_Text.GetLocalBounds().Height + _Text.GetLocalBounds().Top) / 2 + _Text.CharacterSize);
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
        #endregion

        public enum ItemAction
        {
            [Description("Empty")]
            Empty = -1,
            [Description("New Game")]
            NewGame = 0,
            [Description("Exit")]
            Exit = 1
        }

        private Renderer _Renderer;
        private GameWorld.GameManager _GameManager;

        private Graphics.AnimatedSprite _BackgroundImage;
        private Graphics.RegularSprite _Rafiki;

        public MainMenu(Renderer renderer, InputManager.InputManager inputManager, GameWorld.GameManager gameManager) : base(inputManager)
        {
            if (renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");
            if (inputManager == null)
                throw new ArgumentNullException("Input Manager cannot be null");
            if (gameManager == null)
                throw new ArgumentNullException("Game Manager cannot be null");

            _Renderer = renderer;
            _GameManager = gameManager;

            _InputManager.KeyboardKeyPressed += _InputManager_KeyboardKeyPressed;
            _InputManager.KeyboardKeyReleased += _InputManager_KeyboardKeyReleased;
            _InputManager.JoystickButtonPressed += _InputManager_JoystickButtonPressed;
            _InputManager.JoystickButtonReleased += _InputManager_JoystickButtonReleased;
            _InputManager.JoystickMoved += _InputManager_JoystickMoved;

            _menuList.Add(new NewGameItem(_Renderer));
            _menuList.Add(new ExitItem(_Renderer));
            nextMenu();

            Factories.TextureFactory.Instance.load("MainMenuBackgroundImage", "Assets/Background/MainMenuBackground.png");

            _BackgroundImage = new Graphics.AnimatedSprite(new Vector2u((uint)_Renderer.Win.GetView().Size.X, (uint)_Renderer.Win.GetView().Size.Y));
            _BackgroundImage.addAnimation((uint)Graphics.EStateEntity.IDLE, "MainMenuBackgroundImage", Enumerable.Range(0, 5).Select(s => (uint)s).ToList(), new Vector2u(800, 450), 200);
            _BackgroundImage.Loop = true;
            _BackgroundImage.Position = new Vector2f(_BackgroundImage.Size.X / 2, _BackgroundImage.Size.Y / 2);

            Factories.TextureFactory.Instance.load("MainMenuRafiki", "Assets/Background/RafikiBlack.png");
            _Rafiki = new Graphics.RegularSprite(Factories.TextureFactory.Instance.getTexture("MainMenuRafiki"), new Vector2i(128 * (int)_Renderer.Win.GetView().Size.X / 1980, 128 * (int)_Renderer.Win.GetView().Size.X / 1980), new IntRect(0, 0, 512, 512));
            _Rafiki.Position = new Vector2f(2 * _BackgroundImage.Size.X / 3 - 80 * (uint)renderer.Win.GetView().Size.X / 1980, 2 * _BackgroundImage.Size.Y / 5);
        }

        private void _ExecuteAction()
        {
            MainMenuItem _current = (MainMenuItem)_menuList.FirstOrDefault(i => i.Selected);

            if (_current != null)
            {
                switch (_current.Action)
                {
                    case ItemAction.NewGame:
                        MenuManager.Instance.Clear();
                        _GameManager.newGame();
                        break;
                    case ItemAction.Exit:
                        MenuManager.Instance.Clear();
                        _Renderer.Win.Close();
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

        public override void Update(Time DeltaTime)
        {
            _BackgroundImage.Update(DeltaTime);
            base.Update(DeltaTime);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_BackgroundImage, states);
            target.Draw(_Rafiki, states);
            base.Draw(target, states);
        }
    }
}
