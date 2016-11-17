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
        private Dictionary<int, Sound> sounds;
        private Dictionary<int, Music> musics;
        private Factories.SoundBufferFactory buffers;

        public int LimitSound { get; set; }
        public int LimitMusic { get; set; }

        public JukeBox()
        {
            LimitMusic = 1;
            LimitSound = 200;
            sounds = new Dictionary<int, Sound>();
            musics = new Dictionary<int, Music>();
            buffers = Factories.SoundBufferFactory.Instance;
        }

        /// <summary>
        /// Return true if the sound is already charged
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        private bool soundInitializedExist(int id)
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
        public void SetVolumeSound(int id, float volume)
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
        public void SetVolumeMusic(int id, float volume)
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
        private bool musicExist(int id)
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

            foreach (KeyValuePair<int, Sound> entry in sounds)
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

            foreach (KeyValuePair<int, Music> entry in musics)
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
        public void loadSound(int id, string name)
        {
            Sound s = new Sound(buffers.getBuffer(name));
            sounds[id] = s;
        }

        /// <summary>
        /// Load music instance
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="name">string</param>
        public void loadMusic(int id, string name)
        {
            musics[id] = new Music(name);
        }

        /// <summary>
        /// Play sound 
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="loop">bool</param>
        public void PlaySound(int id, bool loop = false)
        {
            if (!soundInitializedExist(id))
                throw new Exception("Sound " + id + " not know");

            if (getNumberSoundPlaying() < LimitSound)
            {
                sounds[id].Loop = loop;
                sounds[id].Play();
            }
        }

        /// <summary>
        /// Play music
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="loop">bool</param>
        public void PlayMusic(int id, bool loop = true)
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
        public void PauseMusic(int id)
        {
            if (!musicExist(id))
                throw new Exception("Music " + id + " not know");
            musics[id].Pause();
        }

        /// <summary>
        /// Stop the music
        /// </summary>
        /// <param name="id">int</param>
        public void StopMusic(int id)
        {
            if (!musicExist(id))
                throw new Exception("Music " + id + " not know");
            musics[id].Stop();
        }


    }
}
