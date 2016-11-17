using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.Downloaders
{
    class LoadingScreen : Drawable
    {
        private Renderer _Renderer;

        private Graphics.AnimatedSprite _background;

        private LoadingScreen() { }
        public LoadingScreen(Renderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");

            _Renderer = renderer;

            Factories.TextureFactory.Instance.load("LS_background", "Assets/Loading/loading.png");

            int x = (int)renderer.Win.GetView().Size.X;
            int y = (int)renderer.Win.GetView().Size.Y;

            _background = new Graphics.AnimatedSprite((uint)x, (uint)y);

            _background.addAnimation((uint)Graphics.EStateEntity.IDLE, "LS_background", new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0 }, new SFML.System.Vector2u(192, 136));
            _background.Loop = true;


            _background.Position = new SFML.System.Vector2f(x / 2, y / 2);
        }

        public void update(SFML.System.Time deltaTime)
        {
            _background.Update(deltaTime);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_background);
        }
    }
}
