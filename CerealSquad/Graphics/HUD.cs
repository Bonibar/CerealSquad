using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.Global;
using CerealSquad.EntitySystem;
using SFML.Graphics;

namespace CerealSquad.Graphics
{
    class HUD : Drawable
    {
        private APlayer _Player;
        private ASprite _Character;
        private RenderTexture _PortraitTarget;
        private ASprite _Portrait;
        private EntitySystem.e_TrapType _OldTrap;
        private ASprite _Trap;

        private Renderer _Renderer;
        private uint _maxPlayer;
        private uint _playerNumber;

        private HUD() { }
        public HUD(uint playerNumber, uint maxPlayer, ref APlayer player, Renderer renderer)
        {
            if (player == null)
                throw new ArgumentNullException("Player cannot be null");
            if (playerNumber == 0)
                throw new FormatException("Player number must start at 1");

            _Player = player;
            _Renderer = renderer;

            _maxPlayer = maxPlayer;
            _playerNumber = playerNumber;

            Factories.TextureFactory.Instance.load("HUD_PlayerCharacter", "Assets/HUD/PlayerCharacterHUD.png");

            _Character = new RegularSprite(Factories.TextureFactory.Instance.getTexture("HUD_PlayerCharacter"), new SFML.System.Vector2i(256 * (int)renderer.Win.GetView().Size.X / 1980, 128 * (int)renderer.Win.GetView().Size.X / 1980), new IntRect(0, (64 * (int)(playerNumber - 1)), 128, 64));

            _PortraitTarget = new RenderTexture(110, 110);
            _Portrait = new RegularSprite(_PortraitTarget.Texture, new SFML.System.Vector2i(110 * (int)renderer.Win.GetView().Size.X / 1980, 110 * (int)renderer.Win.GetView().Size.X / 1980), new IntRect(0, 0, 110, 110));

            _OldTrap = EntitySystem.e_TrapType.NONE;

            UpdatePosition();
            PreloadTexture();
        }

        public void PreloadTexture()
        {
            Factories.TextureFactory.Instance.load("HUD_ICON_Bomb", "Assets/Trap/Bomb.png");
            Factories.TextureFactory.Instance.load("HUD_ICON_BearTrap", "Assets/Trap/Beartrap.png");
            Factories.TextureFactory.Instance.load("HUD_ICON_Wall", "Assets/Trap/SugarWall.png");
        }

        private void UpdateTrapIcon()
        {
            switch (_Player.TrapInventory)
            {
                case EntitySystem.e_TrapType.BOMB:
                    if (_OldTrap != _Player.TrapInventory)
                        _Trap = new RegularSprite(Factories.TextureFactory.Instance.getTexture("HUD_ICON_Bomb"), new SFML.System.Vector2i(50 * (int)_Renderer.Win.GetView().Size.X / 1980, 50 * (int)_Renderer.Win.GetView().Size.X / 1980), new IntRect(0, 0, 128, 128));
                    _Trap.Position = new SFML.System.Vector2f(_Character.Position.X + 163 * (int)_Renderer.Win.GetView().Size.X / 1980, _Character.Position.Y + 38 * (int)_Renderer.Win.GetView().Size.X / 1980);
                    break;
                case EntitySystem.e_TrapType.BEAR_TRAP:
                    if (_OldTrap != _Player.TrapInventory)
                        _Trap = new RegularSprite(Factories.TextureFactory.Instance.getTexture("HUD_ICON_BearTrap"), new SFML.System.Vector2i(50 * (int)_Renderer.Win.GetView().Size.X / 1980, 50 * (int)_Renderer.Win.GetView().Size.X / 1980), new IntRect(0, 0, 128, 128));
                    _Trap.Position = new SFML.System.Vector2f(_Character.Position.X + 163 * (int)_Renderer.Win.GetView().Size.X / 1980, _Character.Position.Y + 38 * (int)_Renderer.Win.GetView().Size.X / 1980);
                    break;
                case EntitySystem.e_TrapType.WALL:
                    if (_OldTrap != _Player.TrapInventory)
                        _Trap = new RegularSprite(Factories.TextureFactory.Instance.getTexture("HUD_ICON_Wall"), new SFML.System.Vector2i(50 * (int)_Renderer.Win.GetView().Size.X / 1980, 50 * (int)_Renderer.Win.GetView().Size.X / 1980), new IntRect(64 * 6, 0, 64, 64));
                    _Trap.Position = new SFML.System.Vector2f(_Character.Position.X + 163 * (int)_Renderer.Win.GetView().Size.X / 1980, _Character.Position.Y + 38 * (int)_Renderer.Win.GetView().Size.X / 1980);
                    break;
                default:
                    _Trap = null;
                    break;
            }
            _OldTrap = _Player.TrapInventory;
        }

        private void UpdatePosition()
        {
            SFML.System.Vector2f cameraOrigin = _Renderer.Win.MapPixelToCoords(new SFML.System.Vector2i(0, 0));

            float x = _Renderer.Win.GetView().Size.X / _maxPlayer;
            float center_x = x / 2 + _Character.Size.X / 2;
            float y = _Renderer.Win.GetView().Size.Y - _Character.Size.Y - 10;

            _Character.Position = new SFML.System.Vector2f(x * (int)_playerNumber - center_x + cameraOrigin.X, y + cameraOrigin.Y);
            _Portrait.Position = new SFML.System.Vector2f(_Character.Position.X + 9 * (int)_Renderer.Win.GetView().Size.X / 1980, _Character.Position.Y + 9 * (int)_Renderer.Win.GetView().Size.X / 1980);
        }

        public void Update(SFML.System.Time DeltaTime)
        {
            UpdatePosition();
            UpdateTrapIcon();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (_Character != null)
                target.Draw(_Character);
            if (_PortraitTarget != null && _Player != null && _Portrait != null)
            {
                _PortraitTarget.Clear(Color.Transparent);
                _Player.ressourcesEntity.Draw(_PortraitTarget);
                _PortraitTarget.Display();

                target.Draw(_Portrait, states);
            }
            if (_Player != null && _Trap != null)
            {
                target.Draw(_Trap, states);
            }
        }
    }
}
