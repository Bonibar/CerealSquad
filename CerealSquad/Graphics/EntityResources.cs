using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    class EntityResources : Transformable, IResource
    {
        public ASprite sprite;
        public Sounds.JukeBox JukeBox { get; set; }

        private Vector2f _SizeHitBox;
        public Vector2f SizeHitBox { get { return getSizeHitBox(); } set { _SizeHitBox = value; } }
        public FloatRect HitBox { get { return getHitBox(); } }

        public bool Loop {
            get {
                if (sprite.Type == ETypeSprite.ANIMATED)
                    return (((AnimatedSprite)sprite).Loop);
                return (false);
            }
            set {
                if (sprite.Type == ETypeSprite.ANIMATED)
                    ((AnimatedSprite)sprite).Loop = value;
            }
        }

        private FloatRect getHitBox()
        {
            return new FloatRect(new Vector2f(sprite.Position.X - ((float)sprite.Size.X / 2.0f), sprite.Position.Y - (float)sprite.Size.Y),
                new Vector2f(sprite.Position.X + ((float)sprite.Size.X / 2.0f), sprite.Position.Y));
        }

        private Vector2f getSizeHitBox()
        {
            if (_SizeHitBox != null)
                return _SizeHitBox;
            return new Vector2f((float)sprite.Size.X, (float)sprite.Size.Y);
        }

        /// <summary>
        /// Initialize a animated sprite
        /// </summary>
        /// <param name="Texture">String</param>
        /// <param name="Size">Vector2i</param>
        public void InitializationAnimatedSprite(Vector2u Size)
        {
            sprite = new AnimatedSprite(Size);
        }

        /// <summary>
        /// Initialize a regular sprite
        /// </summary>
        /// <param name="Texture"></param>
        /// <param name="textureRect"></param>
        public void InitializationRegularSprite(String Texture, Vector2i Size, IntRect textureRect)
        {
            sprite = new RegularSprite(Factories.TextureFactory.Instance.getTexture(Texture), Size, textureRect);
        }

        /// <summary>
        /// Play animation
        /// </summary>
        /// <param name="animation">EStateEntity</param>
        public void PlayAnimation(EStateEntity animation)
        {
            if (sprite.Type == ETypeSprite.ANIMATED)
                ((AnimatedSprite)sprite).PlayAnimation(animation);
        }

        /// <summary>
        /// Reset the animation to 0
        /// </summary>
        public void ResetAnimation()
        {
            if (sprite.Type == ETypeSprite.ANIMATED)
            {
                ((AnimatedSprite)sprite).ResetAnimation();
            }
        }

        /// <summary>
        /// Update the current frame of animation in function of time
        /// Do nothing if it's not an animation
        /// </summary>
        /// <param name="DeltaTime">Time</param>
        public void Update(Time DeltaTime)
        {
            if (sprite.Type == ETypeSprite.ANIMATED)
            {
                ((AnimatedSprite)sprite).Update(DeltaTime);
            }
        }

        /// <summary>
        /// Draw the Sprite
        /// </summary>
        /// <param name="target">RenderTarget</param>
        /// <param name="states">RenderStates</param>
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            sprite.Draw(target, states);
        }

        public bool isFinished()
        {
            if (sprite.Type == ETypeSprite.ANIMATED)
            {
                return (((AnimatedSprite)sprite).isFinished());
            }
            return (true);
        }
    }
}
