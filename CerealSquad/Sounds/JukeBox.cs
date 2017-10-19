using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;

namespace CerealSquad.Sounds
{
    class JukeBox
    {
        #region Singleton
        public static JukeBox Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested() { }

            internal static readonly JukeBox instance = new JukeBox();
        }
        #endregion

        private Dictionary<string, Sound> sounds;
        private Dictionary<string, Music> musics;
        private Factories.SoundBufferFactory buffers;

        public int LimitSound { get; set; }
        public int LimitMusic { get; set; }

        private JukeBox()
        {
            LimitMusic = 1;
            LimitSound = 200;
            sounds = new Dictionary<string, Sound>();
            musics = new Dictionary<string, Music>();
            buffers = Factories.SoundBufferFactory.Instance;
        }

        /// <summary>
        /// Return true if the sound is already charged
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        private bool soundInitializedExist(string id)
        {
            if (!sounds.ContainsKey(id))
                return false;
            if (sounds[id].SoundBuffer == null)
                return false;
            return true;
        }

        /// <summary>
        /// Volume between 0 to 100
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        public void SetVolumeSound(string id, float volume)
        {
            if (!soundInitializedExist(id))
                return;
            if (volume > 100.0f)
                volume = 100.0f;
            else if (volume < 0.0f)
                volume = 0.0f;
            sounds[id].Volume = volume;
        }

        /// <summary>
        /// Volume between 0 to 100
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        public void SetVolumeMusic(string id, float volume)
        {
            if (!musicExist(id))
                return;
            if (volume > 100.0f)
                volume = 100.0f;
            else if (volume < 0.0f)
                volume = 0.0f;
            musics[id].Volume = volume;
        }

        /// <summary>
        /// Check if the music instance exists
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>bool</returns>
        private bool musicExist(string id)
        {
           return musics.ContainsKey(id);
        }

        /// <summary>
        /// Return the number of sounds playing
        /// </summary>
        /// <returns></returns>
        public int getNumberSoundPlaying()
        {
            int nbSoundPlaying = 0;

            foreach (KeyValuePair<string, Sound> entry in sounds)
            {
                if (entry.Value.Status == SoundStatus.Playing)
                    nbSoundPlaying++;
            }

            return nbSoundPlaying;
        }

        /// <summary>
        /// Return the number of music playing
        /// </summary>
        /// <returns></returns>
        public int getNumberMusicPlaying()
        {
            int nbMusicPlaying = 0;

            foreach (KeyValuePair<string, Music> entry in musics)
            {
                if (entry.Value.Status == SoundStatus.Playing)
                    nbMusicPlaying++;
            }

            return nbMusicPlaying;
        }

        /// <summary>
        /// Load sound into JukeBox. Name is the name given to SoundBuffer.
        /// Id represents the id of the sound
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">String</param>
        public void loadSound(string id, string name)
        {
            if (soundInitializedExist(id))
                return;
            Sound s = new Sound(buffers.getBuffer(name));
            sounds[id] = s;
        }

        /// <summary>
        /// Load music instance
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        public void loadMusic(string id, string name)
        {
            if (musicExist(id))
                return;
            musics[id] = new Music(name);
        }

        /// <summary>
        /// Play sound 
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="loop">bool</param>
        public void PlaySound(string id, bool loop = false)
        {
            if (!soundInitializedExist(id))
                throw new Exception("Sound " + id + " not know");

            if (getNumberSoundPlaying() < LimitSound)
            {
                if (sounds[id].Status != SoundStatus.Playing)
                {
                    sounds[id].Loop = loop;
                    sounds[id].Play();
                } else
                {
                    if (sounds[id].PlayingOffset > SFML.System.Time.FromMilliseconds(400))
                    sounds[id].PlayingOffset = SFML.System.Time.Zero;
                }
            }
        }

        /// <summary>
        /// Play sound 
        /// </summary>
        /// <param name="id">int</param>
        public void StopSound(string id)
        {
            if (!soundInitializedExist(id))
                throw new Exception("Sound " + id + " not know");

            sounds[id].Stop();
        }

        /// <summary>
        /// Play music
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="loop">bool</param>
        public void PlayMusic(string id, bool loop = true)
        {
            if (!musicExist(id))
                throw new Exception("Music " + id + " not know");
            if (getNumberMusicPlaying() < LimitMusic)
            {
                musics[id].Loop = loop;
                musics[id].Play();
            }
        }

        /// <summary>
        /// Pause the music
        /// </summary>
        /// <param name="id">int</param>
        public void PauseMusic(string id)
        {
            if (!musicExist(id))
                throw new Exception("Music " + id + " not know");
            musics[id].Pause();
        }

        /// <summary>
        /// Stop the music
        /// </summary>
        /// <param name="id">int</param>
        public void StopMusic(string id)
        {
            if (!musicExist(id))
                throw new Exception("Music " + id + " not know");
            musics[id].Stop();
        }


    }
}
