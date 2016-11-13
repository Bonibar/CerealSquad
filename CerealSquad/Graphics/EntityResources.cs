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

        public Vector2f Size { get { return sprite.Size; } set { sprite.Size = value; } }

        #region Collision
        private bool _CollisionBoxDefault = true;
        private FloatRect _CollisionBox;
        public FloatRect CollisionBox { get { return geCollisionBox(); } set { _CollisionBox = value; _CollisionBoxDefault = false; } }
        #endregion

        #region Hitbox
        private bool _HitBoxDefault = true;
        private FloatRect _HitBox;
        public FloatRect HitBox { get { return getHitBox(); } set { _HitBox = value; _HitBoxDefault = false; } }
        #endregion

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

        private FloatRect geCollisionBox()
        {
            if (!_CollisionBoxDefault)
                return _CollisionBox;
            return new FloatRect(new Vector2f(Position.X - ((float)sprite.Size.X / 2.0f), Position.Y - ((float)sprite.Size.Y / 2.0f)),
                new Vector2f(Position.X + ((float)sprite.Size.X / 2.0f), Position.Y + ((float)sprite.Size.Y / 2.0f)));
        }

        private FloatRect getHitBox()
        {
            if (_HitBoxDefault)
                return new FloatRect(new Vector2f(Position.X - CollisionBox.Left, Position.Y - CollisionBox.Top),
                    new Vector2f(CollisionBox.Left + CollisionBox.Width, CollisionBox.Top + CollisionBox.Height));
            return _HitBox;
        }

        /// <summary>
        /// Check collision with CircleShape
        /// </summary>
        /// <param name="Circle"></param>
        /// <returns></returns>
        public bool IsTouchingHitBox(CircleShape Circle)
        {
            FloatRect CenteredRect = new FloatRect(
                new Vector2f(HitBox.Left + HitBox.Width / 2.0f, HitBox.Top + HitBox.Height / 2.0f),
                new Vector2f(HitBox.Width / 2.0f, HitBox.Top / 2.0f)
                );
            double circleDistanceX = Math.Abs(Circle.Position.X - CenteredRect.Left);
            double circleDistanceY = Math.Abs(Circle.Position.Y - CenteredRect.Top);

            if (circleDistanceX > (CenteredRect.Width / 2.0f + Circle.Radius))
                return false;
            if (circleDistanceY > (CenteredRect.Height / 2.0f + Circle.Radius))
                return false;

            if (circleDistanceX <= (CenteredRect.Width / 2.0f))
                return true;
            if (circleDistanceY <= (CenteredRect.Height / 2.0f))
                return true;

            double cornerDistance_sq = Math.Pow((circleDistanceX - CenteredRect.Width / 2.0f), 2.0f) + Math.Pow((circleDistanceY - CenteredRect.Height / 2.0f), 2.0f);

            return (cornerDistance_sq <= Math.Pow((Circle.Radius), 2.0f));
        }

        /// <summary>
        /// Check collision with other EntityResources hitBox
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool IsTouchingHitBox(EntityResources Other)
        {
            return HitBox.Intersects(Other.HitBox);
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
