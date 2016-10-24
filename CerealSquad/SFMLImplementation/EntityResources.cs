using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Audio;

namespace CerealSquad.SFMLImplementation
{
    public class EntityResources
    {
        public enum EState
        {
            IDLE,
            WALKING,
            DYING,
            PUTTING_TRAP
        };

        // Change hitbox with animation;
        private Sprite hitbox;

        private Dictionary<EState, Animation> _animations;
        private Dictionary<EState, Sound> _sounds;

        private AnimatedSprite animatedSprite;

        public EntityResources()
        {
            // TODO move another place
            TextureFactory.Instance.load("TextureI", "someimage");

            Texture texture = TextureFactory.Instance.getTexture("TextureI");

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
        }
    }
}
