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
    /**
     *   USAGE : 
       
        SFML.System.Clock frameClock = new SFML.System.Clock();
        entityResource = new EntityResources("CharacterTest", 32);
        entityResource.Position = new Vector2f(20, 20);
        entityResource.Rotation = 45
        entityResource.playAnimation(EntityResources.EState.WALKING_RIGHT);

        while (win.IsOpen)
            {
                win.DispatchEvents();

                SFML.System.Time frameTime = frameClock.Restart();
                entityResource.update(frameTime);

                win.Clear(Color.Blue);
                win.Draw(entityResource);
                win.Display();
            }
    */

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

        
        //private Sprite hitbox;

        private Dictionary<EState, Animation> _animations = new Dictionary<EState, Animation>();
        private Dictionary<EState, Sound> _sounds = new Dictionary<EState, Sound>();

        private String texture;

        private AnimatedSprite animatedSprite = new AnimatedSprite();

        public EntityResources(String textureName, int width, int height)
        {
            Texture texture = TextureFactory.Instance.getTexture(textureName);
            Position = new Vector2f(20, 20);

            Animation walkingAnimationIdle = new Animation();
            walkingAnimationIdle.setSpriteSheet(texture);
            walkingAnimationIdle.addFrame(new IntRect(width, 0, width, height));

            Animation walkingAnimationDown = new Animation();
            walkingAnimationDown.setSpriteSheet(texture);
            walkingAnimationDown.addFrame(new IntRect(width, 0, width, height));
            walkingAnimationDown.addFrame(new IntRect(width * 2, 0, width, height));
            walkingAnimationDown.addFrame(new IntRect(width, 0, width, height));
            walkingAnimationDown.addFrame(new IntRect(0, 0, width, height));

            Animation walkingAnimationLeft = new Animation();
            walkingAnimationLeft.setSpriteSheet(texture);
            walkingAnimationLeft.addFrame(new IntRect(width, height, width, height));
            walkingAnimationLeft.addFrame(new IntRect(width * 2, height, width, height));
            walkingAnimationLeft.addFrame(new IntRect(width, height, width, height));
            walkingAnimationLeft.addFrame(new IntRect(0, height, width, height));

            Animation walkingAnimationRight = new Animation();
            walkingAnimationRight.setSpriteSheet(texture);
            walkingAnimationRight.addFrame(new IntRect(width, height * 2, width, height));
            walkingAnimationRight.addFrame(new IntRect(width * 2, height * 2, width, height));
            walkingAnimationRight.addFrame(new IntRect(width, height * 2, width, height));
            walkingAnimationRight.addFrame(new IntRect(0, height * 2, width, height));

            Animation walkingAnimationUp = new Animation();
            walkingAnimationUp.setSpriteSheet(texture);
            walkingAnimationUp.addFrame(new IntRect(width, height * 3, width, height));
            walkingAnimationUp.addFrame(new IntRect(width * 2, height * 3, width, height));
            walkingAnimationUp.addFrame(new IntRect(width, height * 3, width, height));
            walkingAnimationUp.addFrame(new IntRect(0, height * 3, width, height));

            _animations.Add(EState.IDLE, walkingAnimationIdle);
            _animations.Add(EState.WALKING_UP, walkingAnimationUp);
            _animations.Add(EState.WALKING_DOWN, walkingAnimationDown);
            _animations.Add(EState.WALKING_LEFT, walkingAnimationLeft);
            _animations.Add(EState.WALKING_RIGHT, walkingAnimationRight);
        }

        /// <summary>
        /// Get the global bounds (the bounds of default sprite)
        /// </summary>
        /// <returns>FloatRect</returns>
        public FloatRect getGlobalBounds()
        {
            return Transform.TransformRect(animatedSprite.getLocalBounds());
        }

        /// <summary>
        /// Play the animation wanted from the begining (will stop current animation)
        /// </summary>
        /// <param name="state">EState</param>
        public void playAnimation(EState state)
        {
            animatedSprite.Play(_animations[state]);
        }

        /// <summary>
        /// Update the entityResources
        /// </summary>
        /// <param name="deltaTime"></param>
        public void update(Time deltaTime)
        {
            animatedSprite.update(deltaTime);
        }

        /// <summary>
        /// Draw the entityResources
        /// </summary>
        /// <param name="target">RenderTarget</param>
        /// <param name="states">RenderStates</param>
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            animatedSprite.Draw(target, states);
        }
    }
}
