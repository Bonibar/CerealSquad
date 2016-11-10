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

namespace CerealSquad
{
    class Jack : APlayer
    {
        public Jack(IEntity owner, s_position position, InputManager.InputManager input) : base(owner, position, input)
        {
            _speed = 5;
            _inputPress = new Dictionary<Key, functionMove>();
            _inputPress[InputManager.Keyboard.Key.Z] = move_up;
            _inputPress[InputManager.Keyboard.Key.Q] = move_left;
            _inputPress[InputManager.Keyboard.Key.S] = move_down;
            _inputPress[InputManager.Keyboard.Key.D] = move_right;
            _inputPress[InputManager.Keyboard.Key.A] = special_start;
            _inputPress[InputManager.Keyboard.Key.Space] = put_trap;
            _inputRelease = new Dictionary<Key, functionMove>();
            _inputRelease[InputManager.Keyboard.Key.Z] = move_up_release;
            _inputRelease[InputManager.Keyboard.Key.Q] = move_left_release;
            _inputRelease[InputManager.Keyboard.Key.S] = move_down_release;
            _inputRelease[InputManager.Keyboard.Key.D] = move_right_release;
            _inputRelease[InputManager.Keyboard.Key.A] = special_end;
            _inputRelease[InputManager.Keyboard.Key.Space] = put_trap_release;
            _ressources = new EntityResources();

            Factories.TextureFactory.Instance.load("JackWalking", "Assets/Character/JackWalking.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.IDLE, "JackWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_DOWN, "JackWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_LEFT, "JackWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_RIGHT, "JackWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_UP, "JackWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            //((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.DYING, "JackWalking", new List<uint> { 12, 13, 14 }, new Vector2u(64, 64));

            Vector2f pos = _ressources.Position;
            pos.X = position._x * 64;
            pos.Y = position._y * 64;
            _ressources.Position = pos;

            _ressources.CollisionBox = new FloatRect(new Vector2f(28.0f, 0.0f), new Vector2f(26.0f, 24.0f));
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
