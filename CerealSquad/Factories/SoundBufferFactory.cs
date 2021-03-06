﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;

namespace CerealSquad.Factories
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
        private Object Locker = new Object();

        /// <summary>
        /// Init all sound buffer needed
        /// </summary>
        public void initSoundBuffer()
        {
            _SoundBuffers = new Dictionary<String, SoundBuffer>();
            if (_SoundBuffers == null)
                throw new OutOfMemoryException("Failed to load sound buffers");
            // TMP
            Instance.load("BearTrap", "Assets/Sound/Beartrap.ogg");
            Instance.load("Construction", "Assets/Sound/Construction.ogg");
            Instance.load("Explosion", "Assets/Sound/Explosion.ogg");
            Instance.load("CrackingEggs", "Assets/Sound/CrackingEggs.ogg");
            Instance.load("Ghost", "Assets/Sound/ghost.ogg");
            Instance.load("SugarWall", "Assets/Sound/SugarWallLowSound.ogg");
            Instance.load("StoryBegins", "Assets/Sound/And_so_the_story_begins.ogg");
            Instance.load("CerealsHelp", "Assets/Sound/Cereals_might_help_you.ogg");
        }

        /// <summary>
        /// Check if a Sound buffer already exists.
        /// </summary>
        /// <param name="name">String</param>
        /// <returns>bool</returns>
        public bool exists(String name)
        {
            bool result = false;

            lock (Locker)
                result = _SoundBuffers.ContainsKey(name);

            return result;
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
                return true;

            lock (Locker)
            {
                SoundBuffer buffer = new SoundBuffer(filename);
                _SoundBuffers[name] = buffer;
            }

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
                throw new Exception("Unable to find sound buffer " + name);
            SoundBuffer result = null;

            lock (Locker)
                result = _SoundBuffers[name];

            return result;
        }
    }
}
