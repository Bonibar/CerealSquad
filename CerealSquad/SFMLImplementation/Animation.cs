using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace CerealSquad.SFMLImplementation
{
    public class Animation
    {

        public void addFrame(IntRect rect)
        {
            m_frames.Add(rect);
        }

        public void setSpriteSheet(Texture texture)
        {
            m_texture = texture;
        }

        public Texture getSpriteSheet()
        {
            return m_texture;
        }

        public int getSize()
        {
            return m_frames.Count<IntRect>();
        }

        public IntRect getFrame(int n)
        {
            return m_frames[n];
        }

        private List<IntRect> m_frames = new List<IntRect>();
        private Texture m_texture = null;
    }
}
