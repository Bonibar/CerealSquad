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
        public Tchong(IEntity owner, s_position position, InputManager.InputManager input, int type = 0, int id = 1) : base(owner, position, input, type, id)
        {
            _speed = 5;
            this._ressources = new EntityResources();

            Factories.TextureFactory.Instance.load("ChongWalking", "Assets/Character/ChongWalking.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "ChongWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "ChongWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "ChongWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "ChongWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "ChongWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)EStateEntity.DYING, "ChongWalking", new List<uint> { 12, 13, 14 }, new Vector2u(64, 64));
            Vector2f pos = _ressources.Position;
            Pos = Pos;


            _ressources.CollisionBox = new FloatRect(new Vector2f(12.0f, -20.0f), new Vector2f(12.0f, 27.0f));
            Pos = position;
        }

        public override void AttaqueSpe()
        {
            Console.Out.Write(Pos._trueX);
            Console.Out.Write(" ");
            Console.Out.Write(Pos._trueY);
            Console.Out.Write(" ");
            Console.Out.Write(_ressources.Position.X);
            Console.Out.Write(" ");
            Console.Out.Write(_ressources.Position.Y);
            Console.Out.Write("\n");
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
