using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.Global;
using SFML.Graphics;

namespace CerealSquad.Graphics
{
    class HUD : Drawable
    {
        private APlayer _Player;
        private ASprite _Character;
        private RenderTexture _PortraitTarget;
        private ASprite _Portrait;
        private e_TrapType _OldTrap;
        private ASprite _Trap;

        private HUD() { }
        public HUD(uint playerNumber, uint maxPlayer, ref APlayer player, Renderer renderer)
        {
            if (player == null)
                throw new ArgumentNullException("Player cannot be null");
            if (playerNumber == 0)
                throw new FormatException("Player number must start at 1");

            _Player = player;
            Factories.TextureFactory.Instance.load("HUD_PlayerCharacter", "Assets/HUD/PlayerCharacterHUD.png");

            _Character = new RegularSprite(Factories.TextureFactory.Instance.getTexture("HUD_PlayerCharacter"), new SFML.System.Vector2i(256, 128), new IntRect(0, (64 * (int)(playerNumber - 1)), 128, 64));

            float x = renderer.Win.GetView().Size.X / maxPlayer;
            float center_x = x / 2 + _Character.Size.X / 2;
            float y = renderer.Win.GetView().Size.Y - _Character.Size.Y - 10;

            _Character.Position = new SFML.System.Vector2f(x * (int)playerNumber - center_x, y);

            _PortraitTarget = new RenderTexture(110, 110);
            _Portrait = new RegularSprite(_PortraitTarget.Texture, new SFML.System.Vector2i(110, 110), new IntRect(0, 0, 110, 110));
            _Portrait.Position = new SFML.System.Vector2f(_Character.Position.X + 9, _Character.Position.Y + 9);

            _OldTrap = e_TrapType.NONE;

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
            if (_OldTrap != _Player.TrapInventory)
            {
                switch (_Player.TrapInventory)
                {
                    case e_TrapType.BOMB:
                        _Trap = new RegularSprite(Factories.TextureFactory.Instance.getTexture("HUD_ICON_Bomb"), new SFML.System.Vector2i(50, 50), new IntRect(0, 0, 128, 128));
                        _Trap.Position = new SFML.System.Vector2f(_Character.Position.X + 163, _Character.Position.Y + 38);
                        break;
                    case e_TrapType.BEAR_TRAP:
                        _Trap = new RegularSprite(Factories.TextureFactory.Instance.getTexture("HUD_ICON_BearTrap"), new SFML.System.Vector2i(50, 50), new IntRect(0, 0, 128, 128));
                        _Trap.Position = new SFML.System.Vector2f(_Character.Position.X + 163, _Character.Position.Y + 38);
                        break;
                    case e_TrapType.WALL:
                        _Trap = new RegularSprite(Factories.TextureFactory.Instance.getTexture("HUD_ICON_Wall"), new SFML.System.Vector2i(50, 50), new IntRect(0, 0, 64, 64));
                        _Trap.Position = new SFML.System.Vector2f(_Character.Position.X + 163, _Character.Position.Y + 38);
                        break;
                    default:
                        _Trap = null;
                        break;
                }
                _OldTrap = _Player.TrapInventory;
            }
        }

        public void Update(SFML.System.Time DeltaTime)
        {
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
