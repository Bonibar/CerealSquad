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

        private Graphics.ASprite[] _background;

        private LoadingScreen() { }
        public LoadingScreen(Renderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");

            _Renderer = renderer;

            Factories.TextureFactory.Instance.initTextures();
            Factories.TextureFactory.Instance.load("LS_background", "Assets/Loading/loading.png");

            int x = (int)renderer.Win.GetView().Size.X;
            int y = (int)renderer.Win.GetView().Size.Y;

            _background = new Graphics.ASprite[10];

            uint i = 0;
            while (i < 10)
            {
                _background[i] = new Graphics.RegularSprite(Factories.TextureFactory.Instance.getTexture("LS_background"), new SFML.System.Vector2i(x, y), new IntRect(192 * (int)i, 0, 192, 108));
                i++;
            }

            
            //_background.Origin = new SFML.System.Vector2f(renderer.Win.GetView().Size.X / 2, renderer.Win.GetView().Size.Y / 2);
        }

        public int state = 9;

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_background[state]);
        }
    }
}
