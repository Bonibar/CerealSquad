using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    public class SpriteAnimator : Drawable
    {
        private Animation m_animation;

        public Time m_frameTime { get; set; }

        public bool m_isPaused { get; set; }
        public bool m_isLooped { get; set; }

        private Time m_currentTime;
        private int m_currentFrame;
        private Texture m_texture;
        private List<Vertex> m_vertices = new List<Vertex>();
        public Vector2f Size { get; set; }


        public SpriteAnimator()
        {
            m_frameTime = Time.FromSeconds(0.2f);
            m_isLooped = true;
            m_isPaused = false;
            m_animation = null;

            m_vertices.Add(new Vertex());
            m_vertices.Add(new Vertex());
            m_vertices.Add(new Vertex());
            m_vertices.Add(new Vertex());

            setColor(Color.White);
        }

        /// <summary>
        /// Check if the animator have an animation
        /// </summary>
        /// <returns>bool</returns>
        public bool HaveAnimation()
        {
            return m_animation != null;
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
                    if (m_currentFrame + 1 < m_animation.getSize()) {
                        m_currentFrame++;
                    } else {
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
        public void setAnimation(Animation animation)
        {
            m_animation = animation;
            m_texture = m_animation.Texture;
            m_currentFrame = 0;
            m_frameTime = m_animation.Time;
            setFrame(m_currentFrame);
        }

        /// <summary>
        /// Set the frame of the animation
        /// </summary>
        /// <param name="time">Time</param>
        public void setFrameTime(Time time)
        {
            m_frameTime = time;
        }

        /// <summary>
        /// Play the animation
        /// </summary>
        public void Play()
        {
            m_isPaused = false;
        }

        /// <summary>
        /// Play a new animation, same behabior as setAnimation(). If animation is the same, same as Play()
        /// </summary>
        /// <param name="animation">Animation</param>
        public void Play(Animation animation)
        {
            if (getAnimation() != animation)
                setAnimation(animation);
            Play();
        }

        /// <summary>
        /// Pause the animation
        /// </summary>
        public void Pause()
        {
            m_isPaused = true;
        }

        /// <summary>
        /// Stop the animation and reset to the frame 0
        /// </summary>
        public void Stop()
        {
            m_isPaused = true;
            m_currentFrame = 0;
            setFrame(m_currentFrame);
        }

        /// <summary>
        ///  Set if the animation is looped or played only one
        /// </summary>
        /// <param name="looped"></param>
        public void setLooped(bool looped)
        {
            m_isLooped = looped;
        }

        /// <summary>
        /// Set the color
        /// </summary>
        /// <param name="color"></param>
        public void setColor(Color color)
        {
            m_vertices[0] = new Vertex(m_vertices[0].Position, color, m_vertices[0].TexCoords);
            m_vertices[1] = new Vertex(m_vertices[1].Position, color, m_vertices[1].TexCoords);
            m_vertices[2] = new Vertex(m_vertices[2].Position, color, m_vertices[2].TexCoords);
            m_vertices[3] = new Vertex(m_vertices[3].Position, color, m_vertices[3].TexCoords);
        }

        /// <summary>
        /// Get the current animation
        /// </summary>
        /// <returns>Animation</returns>
        public Animation getAnimation()
        {
            return m_animation;
        }

        /// <summary>
        /// Set the current frame of the animation
        /// </summary>
        /// <param name="newFrame">int</param>
        /// <param name="resetTime">bool</param>
        public void setFrame(int newFrame, bool resetTime = true)
        {
            if (m_animation != null)
            {
                Animation.SAnimation animation = m_animation.getFrame(m_currentFrame);
                IntRect rect = animation.Rect;

                float left = (float)(rect.Left) + 0.0001f;
                float right = left + (float)(rect.Width);
                float top = (float)(rect.Top);
                float bottom = top + (float)(rect.Height);

                //Center is top left
                /* m_vertices[0] = new Vertex(new Vector2f(0f, 0f), m_vertices[0].Color,  new Vector2f(left, top));
                 m_vertices[1] = new Vertex(new Vector2f(0f, animation.Size.Y), m_vertices[1].Color, new Vector2f(left, bottom));
                 m_vertices[2] = new Vertex(new Vector2f(animation.Size.X, animation.Size.Y), m_vertices[2].Color, new Vector2f(right, bottom));
                 m_vertices[3] = new Vertex(new Vector2f(animation.Size.X, 0f), m_vertices[3].Color, new Vector2f(right, top));*/

                // Center is bottom middle
                /*m_vertices[0] = new Vertex(new Vector2f(-(animation.Size.X / 2), -animation.Size.Y), m_vertices[0].Color, new Vector2f(left, top));
                m_vertices[1] = new Vertex(new Vector2f(-(animation.Size.X / 2), 0f), m_vertices[1].Color, new Vector2f(left, bottom));
                m_vertices[2] = new Vertex(new Vector2f(animation.Size.X / 2, 0f), m_vertices[2].Color, new Vector2f(right, bottom));
                m_vertices[3] = new Vertex(new Vector2f(animation.Size.X / 2, -animation.Size.Y), m_vertices[3].Color, new Vector2f(right, top));
                */

                // Center is middle middle
                /* m_vertices[0] = new Vertex(new Vector2f(-(animation.Size.X / 2), -(animation.Size.Y / 2)), m_vertices[0].Color, new Vector2f(left, top));
                 m_vertices[1] = new Vertex(new Vector2f(-(animation.Size.X / 2), animation.Size.Y / 2), m_vertices[1].Color, new Vector2f(left, bottom));
                 m_vertices[2] = new Vertex(new Vector2f(animation.Size.X / 2, animation.Size.Y / 2), m_vertices[2].Color, new Vector2f(right, bottom));
                 m_vertices[3] = new Vertex(new Vector2f(animation.Size.X / 2, -(animation.Size.Y / 2)), m_vertices[3].Color, new Vector2f(right, top));*/

                m_vertices[0] = new Vertex(new Vector2f(-(Size.X / 2), -(Size.Y / 2)), m_vertices[0].Color, new Vector2f(left, top));
                m_vertices[1] = new Vertex(new Vector2f(-(Size.X / 2), Size.Y / 2), m_vertices[1].Color, new Vector2f(left, bottom));
                m_vertices[2] = new Vertex(new Vector2f(Size.X / 2, Size.Y / 2), m_vertices[2].Color, new Vector2f(right, bottom));
                m_vertices[3] = new Vertex(new Vector2f(Size.X / 2, -(Size.Y / 2)), m_vertices[3].Color, new Vector2f(right, top));
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
                states.Texture = m_texture;
                target.Draw(m_vertices.ToArray(), 0, 4, PrimitiveType.Quads, states);
            }
        }
    }
}
