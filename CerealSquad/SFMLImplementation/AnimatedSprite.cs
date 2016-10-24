using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.SFMLImplementation
{
    public class AnimatedSprite : Transformable, Drawable
    {
        private Animation m_animation;

        public Time m_frameTime { get; private set; }

        public bool m_isPaused { get; set; }
        public bool m_isLooped { get; set; }

        private Time m_currentTime;
        private int m_currentFrame;
        private Texture m_texture;
        private Vertex[] m_vertices;


        public AnimatedSprite()
        {
            m_frameTime = Time.FromSeconds(0.2f);
            m_isLooped = true;
            m_isPaused = false;
            m_animation = null;
        }

        /// <summary>
        /// update the animated sprite given the amount of time since the last update.
        /// </summary>
        /// <param name="deltaTime">Time</param>
        public void update(Time deltaTime)
        {
            // if not paused and we have a valid animation
            if (!m_isPaused && m_animation != null)
            {
                // add delta time
                m_currentTime += deltaTime;

                // if current time is bigger then the frame time advance one frame
                if (m_currentTime >= m_frameTime)
                {
                    // reset time, but keep the remainder
                    m_currentTime = Time.FromMicroseconds(m_currentTime.AsMicroseconds() % m_frameTime.AsMicroseconds());

                    // get next Frame index
                    if (m_currentFrame + 1 < m_animation.getSize())
                        m_currentFrame++;
                    else {
                        // animation has ended
                        m_currentFrame = 0; // reset to start
                        if (!m_isLooped) {
                            m_isPaused = true;
                        }
                    }

                    // set the current frame, not reseting the time
                    setFrame(m_currentFrame, false);
                }
            }
        }

        /// <summary>
        /// Change the current animation of the sprite (Reset to 0 the frame)
        /// </summary>
        /// <param name="animation">Animation</param>
        void setAnimation(Animation animation)
        {
            m_animation = animation;
            m_texture = m_animation.getSpriteSheet();
            m_currentFrame = 0;
            setFrame(m_currentFrame);
        }

        /// <summary>
        /// Set the frame of the animation
        /// </summary>
        /// <param name="time">Time</param>
        void setFrameTime(Time time)
        {
            m_frameTime = time;
        }

        /// <summary>
        /// Play the animation
        /// </summary>
        void Play()
        {
            m_isPaused = false;
        }

        /// <summary>
        /// Play a new animation, same behabior as setAnimation(). If animation is the same, same as Play()
        /// </summary>
        /// <param name="animation">Animation</param>
        void Play(Animation animation)
        {
            if (getAnimation() != animation)
                setAnimation(animation);
            Play();
        }

        /// <summary>
        /// Pause the animation
        /// </summary>
        void Pause()
        {
            m_isPaused = true;
        }

        /// <summary>
        /// Stop the animation and reset to the frame 0
        /// </summary>
        void Stop()
        {
            m_isPaused = true;
            m_currentFrame = 0;
            setFrame(m_currentFrame);
        }

        /// <summary>
        ///  Set if the animation is looped or played only one
        /// </summary>
        /// <param name="looped"></param>
        void setLooped(bool looped)
        {
            m_isLooped = looped;
        }

        /// <summary>
        /// Set the color
        /// </summary>
        /// <param name="color"></param>
        void setColor(Color color)
        {
            m_vertices[0].Color = color;
            m_vertices[1].Color = color;
            m_vertices[2].Color = color;
            m_vertices[3].Color = color;
        }

        /// <summary>
        /// Get the current animation
        /// </summary>
        /// <returns>Animation</returns>
        Animation getAnimation()
        {
            return m_animation;
        }

        /// <summary>
        /// Get the local bounds (The bounds of the current frame)
        /// </summary>
        /// <returns>FloatRect</returns>
        FloatRect getLocalBounds()
        {
            IntRect rect = m_animation.getFrame(m_currentFrame);

            float width = (float)(Math.Abs(rect.Width));
            float height = (float)(Math.Abs(rect.Height));

            return new FloatRect(0f, 0f, width, height);
        }

        /// <summary>
        /// Get the global bounds (the bounds of default sprite)
        /// </summary>
        /// <returns>FloatRect</returns>
        FloatRect getGlobalBounds()
        {
            return Transform.TransformRect(getLocalBounds());
        }

        void setFrame(int newFrame, bool resetTime = true)
        {
            if (m_animation != null)
            {
                //calculate new vertex positions and texture coordiantes
                IntRect rect = m_animation.getFrame(newFrame);

                m_vertices[0].Position = new Vector2f(0f, 0f);
                m_vertices[1].Position = new Vector2f(0f, (float)(rect.Height));
                m_vertices[2].Position = new Vector2f((float)(rect.Width), (float)(rect.Height));
                m_vertices[3].Position = new Vector2f((float)(rect.Width), 0f);

                float left = (float)(rect.Left) + 0.0001f;
                float right = left + (float)(rect.Width);
                float top = (float)(rect.Top);
                float bottom = top + (float)(rect.Height);

                m_vertices[0].TexCoords = new Vector2f(left, top);
                m_vertices[1].TexCoords = new Vector2f(left, bottom);
                m_vertices[2].TexCoords = new Vector2f(right, bottom);
                m_vertices[3].TexCoords = new Vector2f(right, top);
            }

            if (resetTime)
                m_currentTime = Time.Zero;
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="target">RenderTarget</param>
        /// <param name="states">RenderStates</param>
        public void Draw(RenderTarget target, RenderStates states)
        {
            if (m_animation != null && m_texture != null)
            {
                states.Transform *= Transform;
                states.Texture = m_texture;
                target.Draw(m_vertices, 0, 4, PrimitiveType.Quads, states);
            }
        }
    }
}
