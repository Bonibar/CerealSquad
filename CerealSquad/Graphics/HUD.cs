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

        private HUD() { }
        public HUD(uint playerNumber, uint maxPlayer, ref APlayer player, Renderer renderer)
        {
            if (player == null)
                throw new ArgumentNullException("Player cannot be null");
            if (playerNumber == 0)
                throw new FormatException("Player number must start at 1");

            _Player = player;
            Factories.TextureFactory.Instance.load("HUD_PlayerCharacter", "Assets/HUD/PlayerCharacterHUD.png");

            _Character = new RegularSprite(Factories.TextureFactory.Instance.getTexture("HUD_PlayerCharacter"), new SFML.System.Vector2i(128, 128), new IntRect((64 * (int)(playerNumber - 1) % 128), 64 * ((int)(playerNumber - 1) / 2), 64, 64));

            float x = renderer.Win.GetView().Size.X / maxPlayer;
            float center_x = x / 2 + _Character.Size.X / 2;
            float y = renderer.Win.GetView().Size.Y - _Character.Size.Y - 10;

            _Character.Position = new SFML.System.Vector2f(x * (int)playerNumber - center_x, y);

            _PortraitTarget = new RenderTexture(110, 110);
            _Portrait = new RegularSprite(_PortraitTarget.Texture, new SFML.System.Vector2i(110, 110), new IntRect(0, 0, 110, 110));
            _Portrait.Position = new SFML.System.Vector2f(_Character.Position.X + 9, _Character.Position.Y + 9);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (_Character != null)
                target.Draw(_Character);
            if (_PortraitTarget != null && _Player != null & _Portrait != null)
            {
                _PortraitTarget.Clear(Color.Transparent);
                _Player.ressourcesEntity.Draw(_PortraitTarget);
                _PortraitTarget.Display();

                target.Draw(_Portrait, states);
            }
        }
    }
}
