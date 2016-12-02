using CerealSquad.InputManager.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using CerealSquad.Graphics;
using CerealSquad.GameWorld;
using CerealSquad.Factories;

namespace CerealSquad.EntitySystem
{
    class Jack : APlayer
    {
        public Jack(IEntity owner, s_position position, InputManager.InputManager input, int type = 0, int id = 1) : base(owner, position, input, type, id)
        {
            _speed = 5;
            _ressources = new EntityResources();

            Factories.TextureFactory.Instance.load("JackWalking", "Assets/Character/JackWalking.png");
            Factories.TextureFactory.Instance.load("JackDying", "Assets/Character/Death/JackDying.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "JackWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "JackWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "JackWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "JackWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "JackWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.DYING, "JackDying", Enumerable.Range(0, 20).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 50);

            _ressources.CollisionBox = new FloatRect(new Vector2f(28.0f, -15.0f), new Vector2f(28.0f, 24.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(28.0f, 26.0f), new Vector2f(28.0f, 24.0f));
            Pos = position;
        }

        public override void AttaqueSpe()
        {
            _weight = 10;
        }

        public override void update(Time deltaTime, AWorld world)
        {
            _weight = 1;
            base.update(deltaTime, world);
        }

        public override EName getName()
        {
            return EName.Jack;
        }
    }
}
