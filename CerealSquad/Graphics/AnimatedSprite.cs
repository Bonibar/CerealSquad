﻿using System;
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

           /* Animation walkingAnimationDown = new Animation();
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

            animator.setAnimation(walkingAnimationDown);*/
        }

        /// <summary>
        /// Add animation
        /// </summary>
        /// <param name="type">EStateEntity</param>
        /// <param name="texturePalette">List<int></int></param>
        public void addAnimation(EStateEntity type, String textureAnimation, List<uint> texturePalette, Vector2u _size)
        {
            Animation anim = new Animation();
            PaletteManager.Instance.AddPaletteInformations(textureAnimation, _size.X, _size.Y);
            anim.setSpriteSheet(Factories.TextureFactory.Instance.getTexture(textureAnimation));
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

        public bool isFinished()
        {
            return (animator.m_isPaused);
        }
    }
}
