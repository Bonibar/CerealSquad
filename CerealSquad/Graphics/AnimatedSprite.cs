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
        private Texture texture;
        protected Vector2i size = new Vector2i(0, 0);
        public bool Loop
        {
            get
            {
                return (animator.m_isLooped);
            }
            set
            {
                animator.m_isLooped = value;
            }
        }

        public AnimatedSprite(Texture Texture, Vector2i Size)
        {
            size = Size;
            texture = Texture;
            initialization();
        }

        public AnimatedSprite(Texture Texture, int Width, int Height)
        {
            size = new Vector2i(Width, Height);
            texture = Texture;
            initialization();
        }

        private void initialization()
        {
            Type = ETypeSprite.ANIMATED;
            Position = new Vector2f(0, 0);

            Animation walkingAnimationDown = new Animation();
            walkingAnimationDown.setSpriteSheet(texture);
            walkingAnimationDown.addFrame(new IntRect(size.X, 0, size.X, size.Y));
            walkingAnimationDown.addFrame(new IntRect(size.X * 2, 0, size.X, size.Y));
            walkingAnimationDown.addFrame(new IntRect(size.X, 0, size.X, size.Y));
            walkingAnimationDown.addFrame(new IntRect(0, 0, size.X, size.Y));

            Animation walkingAnimationLeft = new Animation();
            walkingAnimationLeft.setSpriteSheet(texture);
            walkingAnimationLeft.addFrame(new IntRect(size.X, size.Y, size.X, size.Y));
            walkingAnimationLeft.addFrame(new IntRect(size.X * 2, size.Y, size.X, size.Y));
            walkingAnimationLeft.addFrame(new IntRect(size.X, size.Y, size.X, size.Y));
            walkingAnimationLeft.addFrame(new IntRect(0, size.Y, size.X, size.Y));

            Animation walkingAnimationRight = new Animation();
            walkingAnimationRight.setSpriteSheet(texture);
            walkingAnimationRight.addFrame(new IntRect(size.X, size.Y * 2, size.X, size.Y));
            walkingAnimationRight.addFrame(new IntRect(size.X * 2, size.Y * 2, size.X, size.Y));
            walkingAnimationRight.addFrame(new IntRect(size.X, size.Y * 2, size.X, size.Y));
            walkingAnimationRight.addFrame(new IntRect(0, size.Y * 2, size.X, size.Y));

            Animation walkingAnimationUp = new Animation();
            walkingAnimationUp.setSpriteSheet(texture);
            walkingAnimationUp.addFrame(new IntRect(size.X, size.Y * 3, size.X, size.Y));
            walkingAnimationUp.addFrame(new IntRect(size.X * 2, size.Y * 3, size.X, size.Y));
            walkingAnimationUp.addFrame(new IntRect(size.X, size.Y * 3, size.X, size.Y));
            walkingAnimationUp.addFrame(new IntRect(0, size.Y * 3, size.X, size.Y));

            Animation dyingAnimation = new Animation();
            dyingAnimation.setSpriteSheet(texture);
            dyingAnimation.addFrame(new IntRect(size.X, size.Y * 4, size.X, size.Y));
            dyingAnimation.addFrame(new IntRect(size.X * 2, size.Y * 4, size.X, size.Y));
            dyingAnimation.addFrame(new IntRect(size.X, size.Y * 4, size.X, size.Y));
            dyingAnimation.addFrame(new IntRect(0, size.Y * 4, size.X, size.Y));

            animations.Add((uint)EStateEntity.IDLE, walkingAnimationDown);
            animations.Add((uint)EStateEntity.WALKING_UP, walkingAnimationUp);
            animations.Add((uint)EStateEntity.WALKING_DOWN, walkingAnimationDown);
            animations.Add((uint)EStateEntity.WALKING_LEFT, walkingAnimationLeft);
            animations.Add((uint)EStateEntity.WALKING_RIGHT, walkingAnimationRight);
            animations.Add((uint)EStateEntity.DYING, dyingAnimation);

            animator.setAnimation(walkingAnimationDown);
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

        public bool isFinished()
        {
            return (animator.m_isPaused);
        }
    }
}
