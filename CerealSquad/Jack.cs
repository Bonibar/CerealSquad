using CerealSquad.InputManager.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CerealSquad.Graphics;

namespace CerealSquad
{
    class Jack : APlayer
    {
        public Jack(IEntity owner, s_position position, InputManager.InputManager input) : base(owner, position, input)
        {
            _speed = 0.5;
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
            Factories.TextureFactory.Instance.load("jack", "Assets/Character/jack.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.IDLE, "jack", new List<uint> { 0, 1, 2 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_DOWN, "jack", new List<uint> { 0, 1, 2 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_LEFT, "jack", new List<uint> { 3, 4, 5 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_RIGHT, "jack", new List<uint> { 7, 8, 9 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.WALKING_UP, "jack", new List<uint> { 10, 11, 12 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.DYING, "jack", new List<uint> { 13, 14, 15 }, new Vector2u(64, 64));
            Vector2f pos = _ressources.Position;
            pos.X = position._x * 64;
            pos.Y = position._y * 64;
            _ressources.Position = pos;
        }

        public override void AttaqueSpe()
        {
            _weight = 10;
        }

        public override void update(Time deltaTime)
        {
            _weight = 1;
            base.update(deltaTime);
        }

        public override EName getName()
        {
            return EName.Jack;
        }
    }
}
