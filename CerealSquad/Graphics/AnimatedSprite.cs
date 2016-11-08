using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    class AnimatedSprite : ASprite
    {
        protected Dictionary<uint, Animation> animations = new Dictionary<uint, Animation>();
        protected SpriteAnimator animator = new SpriteAnimator();
        protected Vector2u size = new Vector2u(0, 0);


        public Time Speed { set { animator.m_frameTime = value; } get { return animator.m_frameTime; } }
        public bool Loop { get { return (animator.m_isLooped); } set { animator.m_isLooped = value; } }

        public AnimatedSprite(Vector2u Size)
        {
            size = Size;
            initialization();
        }

        public AnimatedSprite(uint Width, uint Height)
        {
            size = new Vector2u(Width, Height);
            initialization();
        }

        private void initialization()
        {
            Type = ETypeSprite.ANIMATED;
            Position = new Vector2f(0, 0);
        }

        public bool isFinished()
        {
            return (animator.m_isPaused);
        }

        public void SetColor(Color color)
        {
            animator.setColor(color);
        }

        /// <summary>
        /// Get the bounds
        /// </summary>
        /// <returns>FloatRect</returns>
        public FloatRect getBounds()
        {
            return new FloatRect(Position.X, Position.Y, size.X, size.Y);
        }

        /// <summary>
        /// Add animation. 
        /// Time is in millisecond
        /// Size is the real size of individual sprite in texture
        /// </summary>
        /// <param name="type"></param>
        /// <param name="textureAnimation"></param>
        /// <param name="texturePalette"></param>
        /// <param name="_size"></param>
        /// <param name="time"></param>
        public void addAnimation(EStateEntity type, String textureAnimation, List<uint> texturePalette, Vector2u _size, int time = -1)
        {
            Animation anim = new Animation();
            PaletteManager.Instance.AddPaletteInformations(textureAnimation, _size.X, _size.Y);
            anim.Texture = Factories.TextureFactory.Instance.getTexture(textureAnimation);
            if (time != -1)
                anim.Time = Time.FromMilliseconds(time);
            texturePalette.ForEach((uint i) => {
                KeyValuePair<IntRect, Texture> palette = PaletteManager.Instance.GetInfoFromPalette(textureAnimation, i);
                anim.addFrame(size.X, size.Y, palette.Key);
            });
            animations.Add((uint)type, anim);

            if (!animator.HaveAnimation())
                animator.setAnimation(anim);
        }

        /// <summary>
        /// Play animation
        /// </summary>
        /// <param name="animation">EStateEntity</param>
        public void PlayAnimation(EStateEntity animation)
        {
            animator.Play(animations[(uint)animation]);
        }

        /// <summary>
        /// Reset animation
        /// </summary>
        public void ResetAnimation()
        {
            animator.setFrame(0);
        }


        /// <summary>
        /// Update the current frame of animation in function of time
        /// </summary>
        /// <param name="DeltaTime">Time</param>
        public void Update(Time DeltaTime)
        {
            animator.update(DeltaTime);
        }

        /// <summary>
        /// Draw the Sprite
        /// </summary>
        /// <param name="target">RenderTarget</param>
        /// <param name="states">RenderStates</param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            animator.Draw(target, states);
        }
    }
}
