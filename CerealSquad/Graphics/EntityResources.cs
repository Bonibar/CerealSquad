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

        public List<ASprite> secondarySprite = new List<ASprite>();
        public Sounds.JukeBox JukeBox { get; set; }

        private RectangleShape CollisionBoxRectangle = new RectangleShape();
        private RectangleShape HitBoxRectangle = new RectangleShape();
        public bool Debug { get; set; }

        public Vector2f Size { get { return sprite.Size; } set { sprite.Size = value; } }

        #region Collision
        private bool _CollisionBoxDefault = true;
        private FloatRect _CollisionBox;
        public FloatRect CollisionBox { get { return geCollisionBox(); } set { _CollisionBox = value; _CollisionBoxDefault = false; UpdateDebug(); } }
        #endregion

        #region Hitbox
        private bool _HitBoxDefault = true;
        private FloatRect _HitBox;
        public FloatRect HitBox { get { return getHitBox(); } set { _HitBox = value; _HitBoxDefault = false; UpdateDebug(); } }
        #endregion

        public EntityResources()
        {
            Debug = true;
        }

        public void UpdateDebug()
        {
            CollisionBoxRectangle.FillColor = new Color(0, 0, 0, 20);
            CollisionBoxRectangle.OutlineColor = Color.Red;
            CollisionBoxRectangle.OutlineThickness = 1;
            CollisionBoxRectangle.Size = new Vector2f(CollisionBox.Width, CollisionBox.Height);
            CollisionBoxRectangle.Position = new Vector2f(CollisionBox.Left, CollisionBox.Top);

            HitBoxRectangle.FillColor = new Color(0, 0, 0, 0);
            HitBoxRectangle.OutlineColor = Color.Green;
            HitBoxRectangle.OutlineThickness = 1;
            HitBoxRectangle.Size = new Vector2f(HitBox.Width, HitBox.Height);
            HitBoxRectangle.Position = new Vector2f(HitBox.Left, HitBox.Top);
        }

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
            if (_CollisionBoxDefault)
                return new FloatRect(new Vector2f(Position.X - ((float)sprite.Size.X / 2.0f), Position.Y - ((float)sprite.Size.Y / 2.0f)),
                    new Vector2f((float)sprite.Size.X, (float)sprite.Size.Y));
            return new FloatRect(new Vector2f(Position.X - _CollisionBox.Left, Position.Y - _CollisionBox.Top),
                    new Vector2f(_CollisionBox.Left + _CollisionBox.Width, _CollisionBox.Top + _CollisionBox.Height));
        }

        private FloatRect getHitBox()
        {
            if (_HitBoxDefault)
                return CollisionBox;
            return new FloatRect(new Vector2f(Position.X - _HitBox.Left, Position.Y - _HitBox.Top),
                    new Vector2f(_HitBox.Left + _HitBox.Width, _HitBox.Top + _HitBox.Height));
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

        public bool IsTouchingCollisionBox(CircleShape Circle)
        {
            FloatRect CenteredRect = new FloatRect(
                new Vector2f(CollisionBox.Left + CollisionBox.Width / 2.0f, CollisionBox.Top + CollisionBox.Height / 2.0f),
                new Vector2f(CollisionBox.Width / 2.0f, CollisionBox.Top / 2.0f)
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

        public bool IsTouchingCollisionBox(EntityResources Other)
        {
            return CollisionBox.Intersects(Other.CollisionBox);
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
            UpdateDebug();
        }

        /// <summary>
        /// Initialize a regular sprite
        /// </summary>
        /// <param name="Texture"></param>
        /// <param name="textureRect"></param>
        public void InitializationRegularSprite(String Texture, Vector2i Size, IntRect textureRect)
        {
            sprite = new RegularSprite(Factories.TextureFactory.Instance.getTexture(Texture), Size, textureRect);
            UpdateDebug();
        }

        /// <summary>
        /// Play animation
        /// </summary>
        /// <param name="animation">uint</param>
        public void PlayAnimation(uint animation)
        {
            if (sprite.Type == ETypeSprite.ANIMATED)
                ((AnimatedSprite)sprite).PlayAnimation((uint)animation);
        }

        /// <summary>
        /// Time is in millisecond
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Texture"></param>
        /// <param name="frames"></param>
        /// <param name="size"></param>
        /// <param name="time"></param>
        public void AddAnimation(uint id, string Texture, List<uint> frames, Vector2u size, int time = -1)
        {
            if (sprite.Type != ETypeSprite.ANIMATED)
                return;
            ((AnimatedSprite)sprite).addAnimation(id, Texture, frames, size, time);
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
        public void Draw(RenderTarget target)
        {
            RenderStates localStates = new RenderStates(RenderStates.Default);
            localStates.Transform.Translate(target.Size.X / 2, target.Size.Y / 2);
            secondarySprite.ForEach(i => i.Draw(target, localStates));
            sprite.Draw(target, localStates);
            if (Debug)
            {
                target.Draw(CollisionBoxRectangle);
                target.Draw(HitBoxRectangle);
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
            secondarySprite.ForEach(i => i.Draw(target, states));
            sprite.Draw(target, states);
            if (Debug)
            {
                target.Draw(CollisionBoxRectangle);
                target.Draw(HitBoxRectangle);
            }
           
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
