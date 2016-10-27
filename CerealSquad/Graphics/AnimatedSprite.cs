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

        public AnimatedSprite(String Texture, Vector2i Size) : base(Texture, Size)
        {
            initialization();
        }

        public AnimatedSprite(String Texture, int Width, int Height) : base(Texture, new Vector2i(Width, Height))
        {
            initialization();
        }

        private void initialization()
        {
            type = ETypeSprite.ANIMATED;
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

            /*animator.Add(EState.WALKING_UP, walkingAnimationUp);
            animator.Add(EState.WALKING_DOWN, walkingAnimationDown);
            animator.Add(EState.WALKING_LEFT, walkingAnimationLeft);
            animator.Add(EState.WALKING_RIGHT, walkingAnimationRight);*/
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
