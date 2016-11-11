using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace CerealSquad.Graphics
{
    class EnvironmentResources : SFML.Graphics.Drawable
    {
        private Dictionary<Vector2u, ASprite> sprites;
    
        public Sounds.JukeBox JukeBox { get; set; }


        public EnvironmentResources()
        {
            sprites = new Dictionary<Vector2u, ASprite>();
        }

        /// <summary>
        /// Add sprite to Environement
        /// </summary>
        /// <param name="name">String</param>
        /// <param name="sprite">ASprite</param>
        public void AddSprite(uint x, uint y, String palette, uint texture)
        {
            KeyValuePair<IntRect, Texture> infos = PaletteManager.Instance.GetInfoFromPalette(palette, texture);

            // TODO should not be 64x64 hardcoded
            ASprite sprite = new RegularSprite(infos.Value, new Vector2i(64, 64), infos.Key);
            sprite.Position = new Vector2f(64 * x, 64 * y);

            sprites[new Vector2u(x, y)] = sprite;
        }

        /// <summary>
        /// Get sprite from environment position
        /// </summary>
        /// <param name="x">uint</param>
        /// <param name="y">uint</param>
        /// <returns></returns>
        public ASprite GetSpriteOnPosition(uint x, uint y)
        {
            foreach (KeyValuePair<Vector2u, ASprite> entry in sprites)
            {
                if (entry.Key.X.Equals(x) && entry.Key.Y.Equals(y))
                {
                    return entry.Value;
                }
            }

            throw new Exception("Can't find sprite on this position");
        }

        /// <summary>
        /// Update the current frame of animation in function of time
        /// Do nothing if it's not an animation
        /// </summary>
        /// <param name="DeltaTime">Time</param>
        public void Update(Time DeltaTime)
        {
            foreach (KeyValuePair<Vector2u, ASprite> entry in sprites)
            {
                if (entry.Value.Type == ETypeSprite.ANIMATED)
                {
                    ((AnimatedSprite)entry.Value).Update(DeltaTime);
                }
            }
        }

        /// <summary>
        /// Draw the Sprite
        /// </summary>
        /// <param name="target">RenderTarget</param>
        /// <param name="states">RenderStates</param>
        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (KeyValuePair<Vector2u, ASprite> entry in sprites)
            {
                // TODO CHANGE POSITION
                entry.Value.Draw(target, states);
            }
        }
    }
}
