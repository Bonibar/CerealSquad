using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    public class Animation
    {
        public struct SAnimation
        {
            public IntRect Rect;
            public Vector2f Size;

            public SAnimation(IntRect _rect, SFML.System.Vector2f _size)
            {
                Rect = _rect;
                Size = _size;
            }
        }

        public void addFrame(float width, float height, IntRect rect)
        {
            m_frames.Add(new SAnimation(rect, new SFML.System.Vector2f(width, height)));
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
            return m_frames.Count<SAnimation>();
        }

        public SAnimation getFrame(int n)
        {
            return m_frames[n];
        }

        private List<SAnimation> m_frames = new List<SAnimation>();
        private Texture m_texture = null;
    }
}
