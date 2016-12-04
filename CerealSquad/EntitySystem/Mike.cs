using CerealSquad.Graphics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.EntitySystem
{
    class Mike : APlayer
    {
        public Mike(IEntity owner, s_position position, InputManager.InputManager input, int type = 0, int id = 1) : base(owner, position, input, type, id)
        {
            _speed = 5;
            _ressources = new EntityResources();

            Factories.TextureFactory.Instance.load("MikeWalking", "Assets/Character/MikeWalking.png");
            Factories.TextureFactory.Instance.load("MikeDying", "Assets/Character/Death/MikeDying.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "MikeWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "MikeWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "MikeWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "MikeWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "MikeWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.DYING, "MikeDying", Enumerable.Range(0, 7).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 120);

            _ressources.CollisionBox = new FloatRect(new Vector2f(20.0f, -20.0f), new Vector2f(20.0f, 28.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(20.0f, 26.0f), new Vector2f(20.0f, 28.0f));
            Pos = position;
        }

        public override void AttaqueSpe()
        {
            throw new NotImplementedException();
        }

        public override EName getName()
        {
            return EName.Mike;
        }
    }
}
