using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;

namespace CerealSquad.Graphics
{
    public sealed class SoundBufferFactory
    {
        #region Singleton
        private SoundBufferFactory()
        {
        }

        public static SoundBufferFactory Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested() { }

            internal static readonly SoundBufferFactory instance = new SoundBufferFactory();
        }
        #endregion

        private Dictionary<String, SoundBuffer> _SoundBuffers;

        /// <summary>
        /// Init all sound buffer needed
        /// </summary>
        public void initSoundBuffer()
        {
            _SoundBuffers = new Dictionary<String, SoundBuffer>();
            if (_SoundBuffers == null)
                throw new OutOfMemoryException("Failed to load sound buffers");

            // use load(name, filename) ...
        }

        /// <summary>
        /// Check if a Sound buffer already exists.
        /// </summary>
        /// <param name="name">String</param>
        /// <returns>bool</returns>
        public bool exists(String name)
        {
            return _SoundBuffers.ContainsKey(name);
        }

        /// <summary>
        /// Load a sound buffer with the given filname and store it with name.
        /// </summary>
        /// <param name="name">String</param>
        /// <param name="filename">String</param>
        /// <returns>bool</returns>
        public bool load(String name, String filename)
        {
            if (exists(name))
            {
                return true;
            }

            SoundBuffer buffer = new SoundBuffer(filename);
            _SoundBuffers[name] = buffer;

            return true;
        }

        /// <summary>
        /// Get the buffer with associed name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Texture</returns>
        public SoundBuffer getBuffer(String name)
        {
            if (!exists(name))
            {
                throw new Exception("Unable to find sound buffer " + name);
            }

            return _SoundBuffers[name];
        }
    }
}
