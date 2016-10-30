using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace CerealSquad.Graphics
{
    class EnvironmentResource
    {
        private Dictionary<String, ASprite> sprites;
    
        public JukeBox JukeBox { get; set; }


        public EnvironmentResource()
        {
            sprites = new Dictionary<string, ASprite>();
        }

        /// <summary>
        /// Add sprite to Environement (With a given name, if not a default will be given)
        /// </summary>
        /// <param name="name">String</param>
        /// <param name="sprite">ASprite</param>
        public void AddSprite(ASprite sprite, String name = "default")
        {
            if (!spriteExists(name) && name.Equals("default"))
            {
                name = name + "-01";
                int i = 1;
                while (sprites.ContainsKey(name)) {
                    i++;
                    name = name.Insert(7, i.ToString());
                }
            }
            else if (!spriteExists(name))
            {
                throw new Exception("Sprite already exists");
            }

            sprites.Add(name, sprite);
        }

        /// <summary>
        /// Get the sprite by name from environment
        /// </summary>
        /// <param name="name">String</param>
        /// <param name="sprite">ASprite</param>
        /// <returns>ASprite</returns>
        public ASprite getSprite(String name, ASprite sprite)
        {
            if (!spriteExists(name))
                throw new Exception("Sprite not found");
            return sprites[name];
        }

        /// <summary>
        /// Remove the sprite from the environment
        /// </summary>
        /// <param name="name">String</param>
        public void removeSprite(String name)
        {
            if (!spriteExists(name))
                sprites.Remove(name);
        }

        /// <summary>
        /// check if the sprite exists
        /// </summary>
        /// <param name="name">String</param>
        /// <returns>bool</returns>
        private bool spriteExists(String name)
        {
            return sprites.ContainsKey(name);
        }

        /// <summary>
        /// Update the current frame of animation in function of time
        /// Do nothing if it's not an animation
        /// </summary>
        /// <param name="DeltaTime">Time</param>
        public void Update(Time DeltaTime)
        {
            foreach (KeyValuePair<String, ASprite> entry in sprites)
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
            foreach (KeyValuePair<String, ASprite> entry in sprites)
            {
                entry.Value.Draw(target, states);
            }
        }
    }
}
