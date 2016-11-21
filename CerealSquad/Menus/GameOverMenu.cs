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
        private InputManager.InputManager _InputManager;
        private Text _GameOverText;

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
                _Text.Position = new Vector2f(renderer.Win.GetView().Size.X / 2 - (_Text.GetLocalBounds().Left + _Text.GetLocalBounds().Width) / 2, 500);
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

        public GameOverMenu(Renderer renderer, InputManager.InputManager inputManager)
        {
            if (renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");
            if (inputManager == null)
                throw new ArgumentNullException("InputManager cannot be null");

            _Renderer = renderer;
            _InputManager = inputManager;

            _menuList.Add(new ReturnMenuItem(_Renderer));
            nextMenu();

            _GameOverText = new Text("Validate to start !", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular), 80);
            _GameOverText.Position = new Vector2f(renderer.Win.GetView().Size.X / 2 - (_GameOverText.GetLocalBounds().Left + _GameOverText.GetLocalBounds().Width) / 2, renderer.Win.GetView().Size.Y / 3 - (_GameOverText.GetLocalBounds().Top + _GameOverText.GetLocalBounds().Height) / 2);
            _GameOverText.Color = Color.Red;
        }
    }
}
