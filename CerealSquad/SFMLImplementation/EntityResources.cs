using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;

namespace CerealSquad.SFMLImplementation
{
    public class EntityResources : Transformable, Drawable
    {
        public enum EState
        {
            IDLE,
            WALKING_UP,
            WALKING_DOWN,
            WALKING_LEFT,
            WALKING_RIGHT,
            DYING,
            PUTTING_TRAP
        };

        // Change hitbox with animation;
        private Sprite hitbox;

        private Dictionary<EState, Animation> _animations = new Dictionary<EState, Animation>();
        private Dictionary<EState, Sound> _sounds = new Dictionary<EState, Sound>();

        private String texture;

        private AnimatedSprite animatedSprite = new AnimatedSprite();

        public EntityResources(String textureName)
        {
            Texture texture = TextureFactory.Instance.getTexture(textureName);

            Animation walkingAnimationDown = new Animation();
            walkingAnimationDown.setSpriteSheet(texture);
            walkingAnimationDown.addFrame(new IntRect(32, 0, 32, 32));
            walkingAnimationDown.addFrame(new IntRect(64, 0, 32, 32));
            walkingAnimationDown.addFrame(new IntRect(32, 0, 32, 32));
            walkingAnimationDown.addFrame(new IntRect(0, 0, 32, 32));

            Animation walkingAnimationLeft = new Animation();
            walkingAnimationLeft.setSpriteSheet(texture);
            walkingAnimationLeft.addFrame(new IntRect(32, 32, 32, 32));
            walkingAnimationLeft.addFrame(new IntRect(64, 32, 32, 32));
            walkingAnimationLeft.addFrame(new IntRect(32, 32, 32, 32));
            walkingAnimationLeft.addFrame(new IntRect(0, 32, 32, 32));

            Animation walkingAnimationRight = new Animation();
            walkingAnimationRight.setSpriteSheet(texture);
            walkingAnimationRight.addFrame(new IntRect(32, 64, 32, 32));
            walkingAnimationRight.addFrame(new IntRect(64, 64, 32, 32));
            walkingAnimationRight.addFrame(new IntRect(32, 64, 32, 32));
            walkingAnimationRight.addFrame(new IntRect(0, 64, 32, 32));

            Animation walkingAnimationUp = new Animation();
            walkingAnimationUp.setSpriteSheet(texture);
            walkingAnimationUp.addFrame(new IntRect(32, 96, 32, 32));
            walkingAnimationUp.addFrame(new IntRect(64, 96, 32, 32));
            walkingAnimationUp.addFrame(new IntRect(32, 96, 32, 32));
            walkingAnimationUp.addFrame(new IntRect(0, 96, 32, 32));

            _animations.Add(EState.WALKING_UP, walkingAnimationUp);
            _animations.Add(EState.WALKING_DOWN, walkingAnimationDown);
            _animations.Add(EState.WALKING_LEFT, walkingAnimationLeft);
            _animations.Add(EState.WALKING_RIGHT, walkingAnimationRight);

            animatedSprite.Play(walkingAnimationUp);

        }

        /// <summary>
        /// Get the global bounds (the bounds of default sprite)
        /// </summary>
        /// <returns>FloatRect</returns>
        public FloatRect getGlobalBounds()
        {
            return Transform.TransformRect(animatedSprite. getLocalBounds());
        }

        public void update(Time deltaTime)
        {
            animatedSprite.update(deltaTime);
        }

        public void playAnimation(EntityResources.EState state)
        {
            animatedSprite.Play(_animations[state]);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            animatedSprite.Draw(target, states);
        }
    }
}
