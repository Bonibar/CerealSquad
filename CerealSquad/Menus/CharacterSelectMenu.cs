using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.InputManager;
using SFML.Graphics;
using SFML.System;

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
            public Type Type { get; protected set; }
            public uint ControllerId { get; protected set; }
            public uint KeyboardId { get; protected set; }
            public uint Selection { get; protected set; }
            public bool LockedChoice { get; protected set; }

            private uint _Id;
            private Renderer _Renderer;
            private Text _PlayerText;
            private Text _JoinText;
            private Graphics.RegularSprite _Fork;
            private Graphics.RegularSprite _Knife;
            private Graphics.RegularSprite _Overlay;

            private Text[] _SelectionText = new Text[CharacterSelectMenu.CHARACTER_COUNT];

            private void init(Type type, uint id)
            {
                Type = type;
                ControllerId = 0;
                KeyboardId = 0;

                _Id = id;

                float x_margin = _Renderer.Win.GetView().Size.X / CharacterSelectMenu.PLAYER_COUNT;
                float x_padding = x_margin / 2;
                float y_margin = 0;
                float y_padding = y_margin / 2 + 13;

                List<String> char_names = new List<string> { "Mike", "Jack", "Orange Hina", "Tchong" };

                uint i = 0;
                while (i < CharacterSelectMenu.CHARACTER_COUNT)
                {
                    _SelectionText[i] = new Text(char_names[(int)i % char_names.Count], Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));

                    float text_x_offset = (_SelectionText[i].GetLocalBounds().Left + _SelectionText[i].GetLocalBounds().Width) / 2;

                    _SelectionText[i].Position = new Vector2f((x_margin * _Id + x_padding) - text_x_offset, 57 + (y_margin * _Id + y_padding));
                    i++;
                }

                _PlayerText = new Text("Player " + (id + 1), Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));
                _PlayerText.Position = new Vector2f((x_margin * _Id + x_padding) - (_PlayerText.GetLocalBounds().Left + _PlayerText.GetLocalBounds().Width) / 2, (y_margin * _Id + y_padding));

                _JoinText = new Text("Join", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular));
                _JoinText.Position = new Vector2f((x_margin * _Id + x_padding) - (_JoinText.GetLocalBounds().Left + _JoinText.GetLocalBounds().Width) / 2, 57 + (y_margin * _Id + y_padding));
                _JoinText.Color = Color.Green;

                Factories.TextureFactory.Instance.load("CS_Fork", "Assets/HUD/Fork.png");
                _Fork = new Graphics.RegularSprite(Factories.TextureFactory.Instance.getTexture("CS_Fork"), new Vector2i(64, 64), new IntRect(0, 0, 64, 64));
                _Fork.Position = new Vector2f(_PlayerText.Position.X - _Fork.Size.X, 0);

                Factories.TextureFactory.Instance.load("CS_Knife", "Assets/HUD/Knife.png");
                _Knife = new Graphics.RegularSprite(Factories.TextureFactory.Instance.getTexture("CS_Knife"), new Vector2i(64, 64), new IntRect(0, 0, 64, 64));
                _Knife.Position = new Vector2f(_PlayerText.Position.X + _PlayerText.GetLocalBounds().Left + _PlayerText.GetLocalBounds().Width, 0);

                Factories.TextureFactory.Instance.load("CS_Overlay", "Assets/HUD/SelectionPlayerOverlay.png");
                _Overlay = new Graphics.RegularSprite(Factories.TextureFactory.Instance.getTexture("CS_Overlay"), new Vector2i(64 * 6, 19 * 6), new IntRect(0, (19 * (int)_Id), 64, 19));
                _Overlay.Position = new Vector2f(_PlayerText.Position.X + (_PlayerText.GetLocalBounds().Left + _PlayerText.GetLocalBounds().Width) / 2 - _Overlay.Size.X / 2, 0);
            }

            public Player(uint id, Renderer renderer)
            {
                _Renderer = renderer;

                init(Type.Undefined, id);
            }
            protected Player(Type type, uint id, Renderer renderer, List<uint> LockedList)
            {
                _Renderer = renderer;

                Selection = 0;
                if (LockedList.Contains(Selection))
                    SelectNext(LockedList);

                init(type, id);
            }

            public void SelectNext(List<uint> lockedList)
            {
                if (!LockedChoice)
                {
                    bool prevent_continue = false;
                    uint checking_counter = 0;
                    while (!prevent_continue)
                    {
                        if (checking_counter > CharacterSelectMenu.CHARACTER_COUNT)
                            throw new NotSupportedException("No character left for selection");
                        Selection++;
                        Selection = Selection % CharacterSelectMenu.CHARACTER_COUNT;
                        if (!lockedList.Contains(Selection))
                            prevent_continue = true;
                        checking_counter++;
                    }
                }
            }

            public void SelectPrevious(List<uint> lockedList)
            {
                if (!LockedChoice)
                {
                    bool prevent_continue = false;
                    uint checking_counter = 0;
                    while (!prevent_continue)
                    {
                        if (checking_counter > CharacterSelectMenu.CHARACTER_COUNT)
                            throw new NotSupportedException("No character left for selection");
                        if (Selection == 0)
                            Selection = CharacterSelectMenu.CHARACTER_COUNT;
                        Selection--;
                        if (!lockedList.Contains(Selection))
                            prevent_continue = true;
                        checking_counter++;
                    }
                }
            }

            public void LockSelection(bool _lock)
            {
                LockedChoice = _lock;
                if (_lock)
                    _SelectionText[Selection].Color = new Color(250, 65, 65, 175);
                else
                    _SelectionText[Selection].Color = Color.White;
            }

            public void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(_Overlay, states);
                target.Draw(_Fork, states);
                target.Draw(_Knife, states);
                target.Draw(_PlayerText, states);
                if (Type == Type.Undefined)
                    target.Draw(_JoinText, states);
                else
                    target.Draw(_SelectionText[Selection], states);
            }
        }

        class ControllerPlayer : Player
        {
            public ControllerPlayer(uint controllerId, uint id, Renderer renderer, List<uint> LockedList) : base(Type.Controller, id, renderer, LockedList)
            {
                ControllerId = controllerId;
            }
        }

        class KeyboardPlayer : Player
        {
            public KeyboardPlayer(uint keyboardId, uint id, Renderer renderer, List<uint> LockedList) : base(Type.Keyboard, id, renderer, LockedList)
            {
                if (keyboardId > 2 || keyboardId < 1)
                    throw new ArgumentOutOfRangeException("Keyboard Id can only be 1 or 2");
                KeyboardId = keyboardId;
            }
        }
    }

    namespace Characters
    {
        abstract class Character : Drawable
        {
            public int id_on_it { get; private set; }
            public bool id_locked { get; private set; }

            public Character(Renderer renderer)
            {
                id_on_it = -1;
                id_locked = false;
            }

            public virtual void Select(int id) { id_on_it = id; }
            public virtual void Lock(bool isLocked) { id_locked = true; }

            public abstract void Draw(RenderTarget target, RenderStates states);
            public abstract void Update(Time DeltaTime);
        }

        class Mike : Character
        {
            Graphics.AnimatedSprite _Sprite;

            public Mike(Renderer renderer) : base(renderer)
            {
                Factories.TextureFactory.Instance.load("CS_Mike", "Assets/Character/Selection/MikeSelection.png");

                int display_size = (int)renderer.Win.GetView().Size.X / ((int)CharacterSelectMenu.CHARACTER_COUNT + 1);

                _Sprite = new Graphics.AnimatedSprite((uint)display_size, (uint)display_size);
                _Sprite.addAnimation(Graphics.EStateEntity.IDLE, "CS_Mike", new List<uint> { 0 }, new Vector2u(128, 128));
                _Sprite.addAnimation(Graphics.EStateEntity.WALKING_DOWN, "CS_Mike", Enumerable.Range(0, 5).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 100);
                _Sprite.Loop = false;
                _Sprite.Position = new Vector2f(display_size, renderer.Win.GetView().Size.Y - display_size / 2);
            }

            public override void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(_Sprite, states);
            }

            public override void Lock(bool isLocked)
            {
                base.Lock(isLocked);
                if (isLocked)
                    _Sprite.PlayAnimation(Graphics.EStateEntity.WALKING_DOWN);
                else
                    _Sprite.PlayAnimation(Graphics.EStateEntity.IDLE);
            }

            public override void Update(Time DeltaTime)
            {
                _Sprite.Update(DeltaTime);
            }
        }

        class Jack : Character
        {
            Graphics.AnimatedSprite _Sprite;

            public Jack(Renderer renderer) : base(renderer)
            {
                Factories.TextureFactory.Instance.load("CS_Jack", "Assets/Character/Selection/JackSelection.png");

                int display_size = (int)renderer.Win.GetView().Size.X / ((int)CharacterSelectMenu.CHARACTER_COUNT + 1);

                _Sprite = new Graphics.AnimatedSprite((uint)display_size, (uint)display_size);
                _Sprite.addAnimation(Graphics.EStateEntity.IDLE, "CS_Jack", new List<uint> { 0 }, new Vector2u(128, 128));
                _Sprite.addAnimation(Graphics.EStateEntity.WALKING_DOWN, "CS_Jack", Enumerable.Range(0, 12).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 100);
                _Sprite.Loop = false;
                _Sprite.Position = new Vector2f(display_size * 2, renderer.Win.GetView().Size.Y - display_size / 2);
            }

            public override void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(_Sprite, states);
            }

            public override void Lock(bool isLocked)
            {
                base.Lock(isLocked);
                if (isLocked)
                    _Sprite.PlayAnimation(Graphics.EStateEntity.WALKING_DOWN);
                else
                    _Sprite.PlayAnimation(Graphics.EStateEntity.IDLE);
            }

            public override void Update(Time DeltaTime)
            {
                _Sprite.Update(DeltaTime);
            }
        }

        class OrangeHina : Character
        {
            Graphics.AnimatedSprite _Sprite;

            public OrangeHina(Renderer renderer) : base(renderer)
            {
                Factories.TextureFactory.Instance.load("CS_Hina", "Assets/Character/Selection/HinaSelection.png");

                int display_size = (int)renderer.Win.GetView().Size.X / ((int)CharacterSelectMenu.CHARACTER_COUNT + 1);

                _Sprite = new Graphics.AnimatedSprite((uint)display_size, (uint)display_size);
                _Sprite.addAnimation(Graphics.EStateEntity.IDLE, "CS_Hina", new List<uint> { 0 }, new Vector2u(128, 128));
                _Sprite.addAnimation(Graphics.EStateEntity.WALKING_DOWN, "CS_Hina", Enumerable.Range(0, 9).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 100);
                _Sprite.Loop = false;
                _Sprite.Position = new Vector2f(display_size * 3, renderer.Win.GetView().Size.Y - display_size / 2);
            }

            public override void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(_Sprite, states);
            }

            public override void Lock(bool isLocked)
            {
                base.Lock(isLocked);
                if (isLocked)
                    _Sprite.PlayAnimation(Graphics.EStateEntity.WALKING_DOWN);
                else
                    _Sprite.PlayAnimation(Graphics.EStateEntity.IDLE);
            }

            public override void Update(Time DeltaTime)
            {
                _Sprite.Update(DeltaTime);
            }
        }

        class Tchong : Character
        {
            Graphics.AnimatedSprite _Sprite;

            public Tchong(Renderer renderer) : base(renderer)
            {
                Factories.TextureFactory.Instance.load("CS_Tchong", "Assets/Character/Selection/TchongSelection.png");

                int display_size = (int)renderer.Win.GetView().Size.X / ((int)CharacterSelectMenu.CHARACTER_COUNT + 1);

                _Sprite = new Graphics.AnimatedSprite((uint)display_size, (uint)display_size);
                _Sprite.addAnimation(Graphics.EStateEntity.IDLE, "CS_Tchong", new List<uint> { 0 }, new Vector2u(128, 128));
                _Sprite.addAnimation(Graphics.EStateEntity.WALKING_DOWN, "CS_Tchong", Enumerable.Range(0, 12).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 100);
                _Sprite.Loop = false;
                _Sprite.Position = new Vector2f(display_size * 4, renderer.Win.GetView().Size.Y - display_size / 2);
            }

            public override void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(_Sprite, states);
            }

            public override void Lock(bool isLocked)
            {
                base.Lock(isLocked);
                if (isLocked)
                    _Sprite.PlayAnimation(Graphics.EStateEntity.WALKING_DOWN);
                else
                    _Sprite.PlayAnimation(Graphics.EStateEntity.IDLE);
            }

            public override void Update(Time DeltaTime)
            {
                _Sprite.Update(DeltaTime);
            }
        }
    }

    class CharacterSelectMenu : Menu, Drawable
    {
        public static uint PLAYER_COUNT = 4;
        public static uint CHARACTER_COUNT = 4;

        public Players.Player[] Players { get; private set; }

        private Renderer _Renderer;
        private GameWorld.GameManager _GameManager;
        private Text _StartGameText;
        private RectangleShape _StartGameShape;
        private Sounds.JukeBox Jukebox = new Sounds.JukeBox();

        private Graphics.AnimatedSprite _BackgroundImage;

        private Characters.Character[] _Characters;

        public CharacterSelectMenu(Renderer renderer, InputManager.InputManager inputManager, GameWorld.GameManager gameManager) : base(inputManager)
        {
            if (renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");
            if (gameManager == null)
                throw new ArgumentNullException("Game Manager cannot be null");

            _Renderer = renderer;
            _GameManager = gameManager;

            _Characters = new Characters.Character[CHARACTER_COUNT];
            _Characters[0] = new Characters.Mike(_Renderer);
            _Characters[1] = new Characters.Jack(_Renderer);
            _Characters[2] = new Characters.OrangeHina(_Renderer);
            _Characters[3] = new Characters.Tchong(_Renderer);

            Players = new Players.Player[PLAYER_COUNT];
            uint i = 0;
            while (i < PLAYER_COUNT)
            {
                Players[i] = new Players.Player(i, _Renderer);
                i++;
            }

            Factories.TextureFactory.Instance.load("S_CS_BackgroundImage", "Assets/Background/CharacterSelection.png");

            _BackgroundImage = new Graphics.AnimatedSprite(new Vector2u((uint)_Renderer.Win.GetView().Size.X, (uint)_Renderer.Win.GetView().Size.Y));
            _BackgroundImage.addAnimation(Graphics.EStateEntity.IDLE, "S_CS_BackgroundImage", Enumerable.Range(0, 8).Select(s => (uint)s).ToList(), new Vector2u(800, 450), 200);
            _BackgroundImage.Loop = true;
            _BackgroundImage.Position = new Vector2f(_Renderer.Win.GetView().Size.X / 2, _Renderer.Win.GetView().Size.Y / 2);

            Jukebox.loadMusic(0, "Assets/Music/CharacterSelection.ogg");

            Buttons.IButton returnButton = new Buttons.BackButton("", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie), 0, this);
            MenuItem back_Button = new MenuItem(returnButton, MenuItem.ItemType.KeyBinded, InputManager.Keyboard.Key.Escape);
            _menuList.Add(back_Button);

            _StartGameText = new Text("Validate to start !", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.XirodRegular), 80);
            _StartGameText.Position = new Vector2f(renderer.Win.GetView().Size.X / 2 - (_StartGameText.GetLocalBounds().Left + _StartGameText.GetLocalBounds().Width) / 2, renderer.Win.GetView().Size.Y / 3 - (_StartGameText.GetLocalBounds().Top + _StartGameText.GetLocalBounds().Height) / 2);
            _StartGameText.Color = Color.Red;

            _StartGameShape = new RectangleShape(new Vector2f(renderer.Win.GetView().Size.X, _StartGameText.GetLocalBounds().Top + _StartGameText.GetLocalBounds().Height + 20));
            _StartGameShape.Position = new Vector2f(renderer.Win.GetView().Size.X / 2 - (_StartGameShape.GetLocalBounds().Left + _StartGameShape.GetLocalBounds().Width) / 2, renderer.Win.GetView().Size.Y / 3 - (_StartGameShape.GetLocalBounds().Top + _StartGameShape.GetLocalBounds().Height) / 2);
            _StartGameShape.FillColor = Color.White;

            _InputManager.JoystickButtonPressed += _InputManager_JoystickButtonPressed;
            _InputManager.JoystickMoved += _InputManager_JoystickMoved;

            _InputManager.KeyboardKeyPressed += _InputManager_KeyboardKeyPressed;
        }

        public override void Update(Time DeltaTime)
        {
            uint i = 0;
            while (i < CHARACTER_COUNT)
            {
                _Characters[i].Update(DeltaTime);
                i++;
            }
            _BackgroundImage.Update(DeltaTime);
        }

        public override void Show()
        {
            uint i = 0;
            while (i < PLAYER_COUNT)
            {
                if (Players[i].Type != Menus.Players.Type.Undefined)
                    Players[i] = new Players.Player(i, _Renderer);
                i++;
            }

            Jukebox.PlayMusic(0, true);

            base.Show();
        }

        public override void Hide()
        {
            Jukebox.StopMusic(0);

            base.Hide();
        }

        private List<uint> GetLockedList()
        {
            List<uint> LockedList = new List<uint>();
            uint locked_i = 0;
            while (locked_i < PLAYER_COUNT)
            {
                if (Players[locked_i].Type != Menus.Players.Type.Undefined && Players[locked_i].LockedChoice)
                    LockedList.Add(Players[locked_i].Selection);
                locked_i++;
            }

            return LockedList;
        }

        private bool AllPlayersReady()
        {
            
            return Players.FirstOrDefault(x => x.Type != Menus.Players.Type.Undefined && x.LockedChoice != true) == null && Players.FirstOrDefault(x => x.Type != Menus.Players.Type.Undefined) != null;
        }

        private void UpdateEffects()
        {
            uint i = 0;
            _Characters.ToList().ForEach(x => x.Select(-1));
            while (i < PLAYER_COUNT)
            {
                if (Players[i].Type != Menus.Players.Type.Undefined)
                {
                    _Characters[Players[i].Selection].Select((int)i);
                    _Characters[Players[i].Selection].Lock(Players[i].LockedChoice);
                }
                i++;
            }
        }

        private void _InputManager_KeyboardKeyPressed(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (Displayed)
            {
                List<uint> LockedList = GetLockedList();

                if (e.KeyCode == InputManager.Keyboard.Key.Space || e.KeyCode == InputManager.Keyboard.Key.Z)
                {
                    if (Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Keyboard && x.KeyboardId == 1) == null) // Joining
                        JoinKeyboardPlayer(source, e, 1, LockedList);
                    else if (AllPlayersReady())
                        StartGame();
                    else
                    {
                        Players.Player currentPlayer = Players.First(x => x.Type == Menus.Players.Type.Keyboard && x.KeyboardId == 1);
                        if (!currentPlayer.LockedChoice)
                            ValidatePlayer(currentPlayer, LockedList);
                        else
                            ReturnPlayer(currentPlayer);
                    }
                }
                else if (e.KeyCode == InputManager.Keyboard.Key.BackSpace || e.KeyCode == InputManager.Keyboard.Key.S)
                    ReturnPlayer(Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Keyboard && x.KeyboardId == 1));
                else if (e.KeyCode == InputManager.Keyboard.Key.Q)
                    SelectPreviousPlayer(Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Keyboard && x.KeyboardId == 1), LockedList);
                else if (e.KeyCode == InputManager.Keyboard.Key.D)
                    SelectNextPlayer(Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Keyboard && x.KeyboardId == 1), LockedList);
            }
        }

        private void _InputManager_JoystickMoved(object source, InputManager.Joystick.MoveEventArgs e)
        {
            List<uint> LockedList = GetLockedList();

            if (Displayed && (e.Axis == InputManager.Joystick.Axis.X || e.Axis == InputManager.Joystick.Axis.PovX || e.Axis == InputManager.Joystick.Axis.U))
            {
                if (e.Position > 99)
                    SelectNextPlayer(Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId), LockedList);
                else if (e.Position < -99)
                    SelectPreviousPlayer(Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId), LockedList);
            }
        }

        private void _InputManager_JoystickButtonPressed(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            if (Displayed)
            {
                List<uint> LockedList = GetLockedList();

                if (e.Button == 0) // A Button
                {
                    if (Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId) == null) // Joining
                        JoinControllerPlayer(source, e, LockedList);
                    else if (AllPlayersReady())
                        StartGame();
                    else
                        ValidatePlayer(Players.First(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId), LockedList);
                }
                else if (e.Button == 1)
                    ReturnPlayer(Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId));
                else if (e.Button == 4)
                    SelectPreviousPlayer(Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId), LockedList);
                else if (e.Button == 5)
                    SelectNextPlayer(Players.FirstOrDefault(x => x.Type == Menus.Players.Type.Controller && x.ControllerId == e.JoystickId), LockedList);
            }
        }

        private void JoinControllerPlayer(object source, InputManager.Joystick.ButtonEventArgs e, List<uint> LockedList)
        {
            uint i = 0;
            while (i < 4 && Players[i] != null && Players[i].Type != Menus.Players.Type.Undefined)
                i++;
            if (i < 4) // Team is not full
            {
                System.Diagnostics.Debug.WriteLine("NEW PLAYER JOINED");
                Players[i] = new Players.ControllerPlayer(e.JoystickId, i, _Renderer, LockedList);
            }

            UpdateEffects();
        }
        private void JoinKeyboardPlayer(object source, InputManager.Keyboard.KeyEventArgs e, uint keyboardId, List<uint> LockedList)
        {
            uint i = 0;
            while (i < 4 && Players[i] != null && Players[i].Type != Menus.Players.Type.Undefined)
                i++;
            if (i < 4) // Team is not full
            {
                System.Diagnostics.Debug.WriteLine("NEW PLAYER JOINED");
                Players[i] = new Players.KeyboardPlayer(keyboardId, i, _Renderer, LockedList);
            }

            UpdateEffects();
        }

        private void SelectPreviousPlayer(Players.Player currentPlayer, List<uint> LockedList)
        {
            if (currentPlayer != null)
                currentPlayer.SelectPrevious(LockedList);

            UpdateEffects();
        }
        private void SelectNextPlayer(Players.Player currentPlayer, List<uint> LockedList)
        {
            if (currentPlayer != null)
                currentPlayer.SelectNext(LockedList);

            UpdateEffects();
        }

        private void ReturnPlayer(Players.Player currentPlayer)
        {
            int id = Array.IndexOf(Players, currentPlayer);
            if (id != -1 && Players[id].Type != Menus.Players.Type.Undefined)
            {
                if (Players[id].LockedChoice == true)
                    Players[id].LockSelection(false);
                else
                    Players[id] = new Players.Player((uint)id, _Renderer);
            }

            UpdateEffects();
        }
        private void ValidatePlayer(Players.Player currentPlayer, List<uint> LockedList)
        {
            currentPlayer.LockSelection(true);
            LockedList.Add(currentPlayer.Selection);
            uint i = 0;
            while (i < PLAYER_COUNT)
            {
                if (Players[i].Type != Menus.Players.Type.Undefined && Players[i].LockedChoice == false)
                {
                    while (LockedList.Contains(Players[i].Selection))
                        Players[i].SelectNext(LockedList);
                }
                i++;
            }

            UpdateEffects();
        }

        private void StartGame()
        {
            if (AllPlayersReady())
            {
                MenuManager.Instance.Clear();
                _GameManager.newGame();
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_BackgroundImage, states);

            uint i = 0;
            while (i < PLAYER_COUNT)
            {
                target.Draw(Players[i], states);
                i++;
            }
            i = 0;
            while (i < CHARACTER_COUNT)
            {
                target.Draw(_Characters[i], states);
                i++;
            }
            base.Draw(target, states);

            if (AllPlayersReady())
            {
                target.Draw(_StartGameShape, states);
                target.Draw(_StartGameText, states);
            }
        }
    }
}
