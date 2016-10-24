using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.SFMLImplementation
{
    public sealed class TextureFactory
    {
        private TextureFactory()
        {
        }

        public static TextureFactory Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly TextureFactory instance = new TextureFactory();
        }

        private Dictionary<String, Texture> content;

        public bool exists(String name)
        {
            return content.ContainsKey(name);
        }

        public bool load(String name, String filename)
        {
            if (exists(name)) {
                return true;
            }

            try
            {
                Texture texture = new Texture(filename);
                content[name] = texture;
            } catch (SFML.LoadingFailedException e)
            {
                //TODO inform user
                return false;
            }

            return true;
        }

        public Texture getTexture(String name)
        {
            if (!exists(name)) {
                return null;
            }

            return content[name];
        }
    }

}
