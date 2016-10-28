using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace CerealSquad.Graphics
{
    class EntityResource : IResource
    {
        private ASprite sprite;

        public JukeBox JukeBox { get; set; }
        
        public void InitializationSprite(String Texture, Vector2i size, bool animated = false)
        {
            if (animated)
                sprite = new AnimatedSprite(Texture, size);
            else
                sprite = new RegularSprite(Texture, size);
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
            sprite.Draw(target, states);
        }
    }
}
