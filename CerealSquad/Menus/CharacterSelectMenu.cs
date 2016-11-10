using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.InputManager;
using SFML.Graphics;

namespace CerealSquad.Menus
{
    namespace Players
    {
        public enum Type
        {
            Undefined = -1,
            Keyboard = 0,
            Controller
        }

        class Player : Drawable
        {
            private static uint SELECTION_COUNT = 4;

            public Type Type { get; }
            public uint ControllerId { get; protected set; }
            public uint KeyboardId { get; protected set; }
            public uint Selection { get; protected set; }
            public bool LockedChoice { get; protected set; }

            private uint _Id;

            private Player() { }
            public Player(Type type, uint id)
            {
                Type = type;
                ControllerId = 0;
                KeyboardId = 0;

                _Id = id;

                _JoinText.Position = new SFML.System.Vector2f(50 + 400 * _Id, 500);

                _SelectionText[0] = new Text("Mike", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));
                _SelectionText[0].Position = new SFML.System.Vector2f(50 + 400 * _Id, 20);
                _SelectionText[1] = new Text("Jack", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));
                _SelectionText[1].Position = new SFML.System.Vector2f(50 + 400 * _Id, 20);
                _SelectionText[2] = new Text("Orange Hina", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));
                _SelectionText[2].Position = new SFML.System.Vector2f(50 + 400 * _Id, 20);
                _SelectionText[3] = new Text("Tchong", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));
                _SelectionText[3].Position = new SFML.System.Vector2f(50 + 400 * _Id, 20);
            }

            public void SelectNext()
            {
                if (!LockedChoice)
                {
                    Selection++;
                    if (Selection >= SELECTION_COUNT)
                        Selection = 0;
                }
            }

            public void SelectPrevious()
            {
                if (!LockedChoice)
                {
                    if (Selection == 0)
                        Selection = SELECTION_COUNT;
                    Selection--;
                }
            }

            public void LockSelection(bool _lock)
            {
                LockedChoice = _lock;
                if (_lock)
                    _SelectionText[Selection].Color = new Color(65, 65, 65);
                else
                    _SelectionText[Selection].Color = Color.White;
            }

            private Text _JoinText = new Text("Join", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));
            private Text[] _SelectionText = new Text[SELECTION_COUNT];
            public void Draw(RenderTarget target, RenderStates states)
            {
                if (Type == Type.Undefined)
                    target.Draw(_JoinText, states);
                else
                    target.Draw(_SelectionText[Selection], states);
            }
        }

        class ControllerPlayer : Player
        {
            public ControllerPlayer(uint controllerId, uint id) : base(Type.Controller, id)
            {
                ControllerId = controllerId;
            }
        }

        class KeyboardPlayer : Player
        {
            public KeyboardPlayer(uint keyboardId, uint id) : base(Type.Keyboard, id)
            {
                if (keyboardId > 2 || keyboardId < 1)
                    throw new ArgumentOutOfRangeException("Keyboard Id can only be 1 or 2");
                KeyboardId = keyboardId;
            }
        }
    }

    class CharacterSelectMenu : Menu, Drawable
    {
        public Players.Player[] Players { get; private set; }

        public CharacterSelectMenu(InputManager.InputManager inputManager) : base(inputManager)
        {
            Players = new Players.Player[4];
            Players[0] = new Players.Player(Menus.Players.Type.Undefined, 0);
            Players[1] = new Players.Player(Menus.Players.Type.Undefined, 1);
            Players[2] = new Players.Player(Menus.Players.Type.Undefined, 2);
            Players[3] = new Players.Player(Menus.Players.Type.Undefined, 3);

            Buttons.IButton returnButton = new Buttons.BackButton("", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 0, this);
            MenuItem back_Button = new MenuItem(returnButton, MenuItem.ItemType.KeyBinded, InputManager.Keyboard.Key.Escape);
            _menuList.Add(back_Button);

            _InputManager.JoystickButtonPressed += _InputManager_JoystickButtonPressed;
            _InputManager.JoystickMoved += _InputManager_JoystickMoved;
        }

        private void _InputManager_JoystickMoved(object source, InputManager.Joystick.MoveEventArgs e)
        {
            if (Displayed && e.Axis.Equals(InputManager.Joystick.Axis.Z))
                System.Diagnostics.Debug.WriteLine(e.Axis.ToString() + " JOYBUTTON " + e.Position.ToString());
        }

        private void _InputManager_JoystickButtonPressed(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            if (Displayed)
            {
                System.Diagnostics.Debug.WriteLine("JOYBUTTON " + e.Button.ToString());
                if (Players.FirstOrDefault(x => x.Type != Menus.Players.Type.Undefined && x.ControllerId == e.JoystickId) == null) // Joining
                    JoinControllerPlayer(source, e);
                else // Player already in
                {
                    if (e.Button == 0) // A Button
                    {
                        List<uint> LockedList = new List<uint>();
                        uint i = 0;
                        Players.First(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId).LockSelection(true);
                        while (i < 4)
                        {
                            if (Players[i].Type != Menus.Players.Type.Undefined && Players[i].LockedChoice)
                                LockedList.Add(Players[i].Selection);
                            i++;
                        }
                        i = 0;
                        while (i < 4)
                        {
                            if (Players[i].Type != Menus.Players.Type.Undefined && Players[i].LockedChoice == false)
                            {
                                while (LockedList.Contains(Players[i].Selection))
                                    Players[i].SelectNext();
                            }
                            i++;
                        }
                    }
                    else if (e.Button == 1)
                    {
                        uint i = 0;
                        while (i < 4)
                        {
                            if (Players[i].Type == Menus.Players.Type.Controller && Players[i].ControllerId == e.JoystickId)
                            {
                                if (Players[i].LockedChoice == true)
                                    Players[i].LockSelection(false);
                                else
                                    Players[i] = new Players.Player(Menus.Players.Type.Undefined, i);
                            }
                            i++;
                        }
                    }
                    else if (e.Button == 4)
                        Players.First(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId).SelectPrevious();
                    else if (e.Button == 5)
                        Players.First(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId).SelectNext();
                }
                
            }
        }

        private void JoinControllerPlayer(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            uint i = 0;
            while (i < 4 && Players[i] != null && Players[i].Type != Menus.Players.Type.Undefined)
                i++;
            if (i < 4) // Team is not full
            {
                System.Diagnostics.Debug.WriteLine("NEW PLAYER JOINED");
                Players[i] = new Players.ControllerPlayer(e.JoystickId, i);
            }
        }

        public new void Draw(RenderTarget target, RenderStates states)
        {
            uint i = 0;
            while (i < 4)
            {
                target.Draw(Players[i]);
                i++;
            }
            base.Draw(target, states);
        }
    }
}
