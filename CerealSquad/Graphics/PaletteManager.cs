﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.Graphics
{
    class PaletteManager
    {

        #region Singleton
        private PaletteManager()
        {
        }

        public static PaletteManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested() { }

            internal static readonly PaletteManager instance = new PaletteManager();
        }
        #endregion

        struct Palette
        {
            public uint height;
            public uint width;
            public uint perLine;
            public uint total;
            public String name;
        };

        private Dictionary<Palette, Texture> content = new Dictionary<Palette, Texture>();
        private Factories.TextureFactory textureFactory = Factories.TextureFactory.Instance;
        private Object locker = new object();

        public bool Exists(String name)
        {
            bool result = false;

            lock (locker)
                foreach (KeyValuePair<Palette, Texture> entry in content)
                {
                    if (entry.Key.name.Equals(name))
                    {
                        result = true;
                        break;
                    }

                }

            return result;
        }

        public void AddPaletteInformations(String name, uint width = 64, uint height = 64)
        {
            if (Exists(name))
                return;

            Texture texture = textureFactory.getTexture(name);
            Palette palette = new Palette();

            palette.name = name;
            palette.width = width;
            palette.height = height;
            palette.perLine = (uint)texture.Size.X / width;
            palette.total = ((uint)texture.Size.Y / height) * palette.perLine;

            lock (locker)
                content[palette] = texture;
        }

        public KeyValuePair<IntRect, Texture> GetInfoFromPalette(String name, uint number)
        {
            KeyValuePair<IntRect, Texture> t = new KeyValuePair<IntRect, Texture>(new IntRect(), null);

            lock (locker)
                foreach (KeyValuePair<Palette, Texture> entry in content)
                {
                    if (entry.Key.name.Equals(name))
                    {
                        Palette palette = entry.Key;
                        t = new KeyValuePair<IntRect, Texture>(new IntRect(((int)number % (int)palette.perLine) * (int)palette.width, (int)number / (int)palette.perLine * (int)palette.height, (int)palette.width, (int)palette.height), entry.Value);
                        break;
                    }
                }

            if (t.Value == null)
                throw new Exception("Palette not found");
            return t;
        }
    }
}
