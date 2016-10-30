using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    class EntityResource : Transformable, IResource
    {
        private ASprite sprite;

        public JukeBox JukeBox { get; set; }

        /// <summary>
        /// Initialize a animated sprite
        /// </summary>
        /// <param name="Texture">String</param>
        /// <param name="size">Vector2i</param>
        public void InitializationAnimatedSprite(String Texture, Vector2i size)
        {
            sprite = new AnimatedSprite(Texture, size);  
        }

        /// <summary>
        /// Initialize a regular sprite
        /// </summary>
        /// <param name="Texture"></param>
        /// <param name="size"></param>
        /// <param name="textureRect"></param>
        public void InitializationRegularSprite(String Texture, Vector2i size, IntRect textureRect)
        {
            sprite = new RegularSprite(Texture, size, textureRect);
        }

        /// <summary>
        /// Play animation
        /// </summary>
        /// <param name="animation">EStateEntity</param>
        public void PlayAnimation(EStateEntity animation)
        {
            if (sprite.type == ETypeSprite.ANIMATED)
            {
                ((AnimatedSprite)sprite).PlayAnimation(animation);
            }
        }

        /// <summary>
        /// Update the current frame of animation in function of time
        /// Do nothing if it's not an animation
        /// </summary>
        /// <param name="DeltaTime">Time</param>
        public void Update(Time DeltaTime)
        {
            if (sprite.type == ETypeSprite.ANIMATED)
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
    }
}
