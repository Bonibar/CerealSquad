using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CerealSquad.Graphics;
using CerealSquad.GameWorld;
using CerealSquad.Factories;
using CerealSquad.InputManager.Keyboard;
using SFML.Graphics;

namespace CerealSquad
{
    class Tchong : APlayer
    {
        public Tchong(IEntity owner, s_position position, InputManager.InputManager input, bool keyboard, uint id) : base(owner, position, input, keyboard, id)
        {
            _speed = 5;
            ressourcesEntity = new EntityResources();

            Factories.TextureFactory.Instance.load("ChongWalking", "Assets/Character/ChongWalking.png");
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            ressourcesEntity.AddAnimation((uint)EStateEntity.IDLE, "ChongWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)EStateEntity.WALKING_DOWN, "ChongWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)EStateEntity.WALKING_LEFT, "ChongWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)EStateEntity.WALKING_RIGHT, "ChongWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)EStateEntity.WALKING_UP, "ChongWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            //ressourcesEntity.AddAnimation((uint)EStateEntity.DYING, "ChongWalking", new List<uint> { 12, 13, 14 }, new Vector2u(64, 64));

            ressourcesEntity.CollisionBox = new FloatRect(new Vector2f(12.0f, -20.0f), new Vector2f(12.0f, 27.0f));
            Pos = position;
        }

        public override void AttaqueSpe()
        {
            _weight = 10;
        }

        public override EName getName()
        {
            return EName.Tchong;
        }

        public override void update(Time deltaTime, AWorld world)
        {
            _weight = 1;
            base.update(deltaTime, world);
        }
    }
}
