using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.Factories
{
    public sealed class TextureFactory
    {
        #region Singleton
        private TextureFactory()
        {
        }

        public static TextureFactory Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested() {}

            internal static readonly TextureFactory instance = new TextureFactory();
        }
        #endregion


        private Object locker = new Object();
        private Dictionary<String, Texture> _Textures = new Dictionary<string, Texture>();

        /// <summary>
        /// Init all textures needed
        /// </summary>
        public void initTextures()
        {
            load("TestTile", "Assets/Tiles/TestTile.png");

            Graphics.PaletteManager.Instance.AddPaletteInformations("TestTile");
        }

        /// <summary>
        /// Check if a texture already exists.
        /// </summary>
        /// <param name="name">String</param>
        /// <returns>bool</returns>
        public bool exists(String name)
        {
            bool result = false;

            lock(locker)
                result = _Textures.ContainsKey(name);

            return result;
        }

        /// <summary>
        /// Load a texture with the given filname and store it with name.
        /// </summary>
        /// <param name="name">String</param>
        /// <param name="filename">String</param>
        /// <returns>bool</returns>
        public bool load(String name, String filename)
        {
            if (exists(name))
                return true;

            lock (locker) {
                Texture texture = new Texture(filename);
                _Textures[name] = texture;
            }

            return true;
        }

        /// <summary>
        /// Get the texture with associed name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Texture</returns>
        public Texture getTexture(String name)
        {
            Texture texture = null;

            if (!exists(name))

                throw new Exception("Unable to find texture " + name);

            lock (locker)
                texture = _Textures[name];

            return texture;
        }
    }

}
