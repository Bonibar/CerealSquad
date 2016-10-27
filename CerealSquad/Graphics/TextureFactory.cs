using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.Graphics
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

        private Dictionary<String, Texture> _Textures;

        /// <summary>
        /// Init all textures needed
        /// </summary>
        public void initTextures()
        {
            _Textures = new Dictionary<String, Texture>();
            if (_Textures == null)
                throw new OutOfMemoryException("Failed to load textures");
             
            // use load(name, filename) ...
        }

        /// <summary>
        /// Check if a texture already exists.
        /// </summary>
        /// <param name="name">String</param>
        /// <returns>bool</returns>
        public bool exists(String name)
        {
            return _Textures.ContainsKey(name);
        }

        /// <summary>
        /// Load a texture with the given filname and store it with name.
        /// </summary>
        /// <param name="name">String</param>
        /// <param name="filename">String</param>
        /// <returns>bool</returns>
        public bool load(String name, String filename)
        {
            if (exists(name)) {
                return true;
            }

            Texture texture = new Texture(filename);
            _Textures[name] = texture;

            return true;
        }

        /// <summary>
        /// Get the texture with associed name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Texture</returns>
        public Texture getTexture(String name)
        {
            if (!exists(name)) {
                throw new Exception("Unable to find texture " + name);
            }

            return _Textures[name];
        }
    }

}
