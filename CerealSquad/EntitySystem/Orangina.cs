using CerealSquad.Graphics;
using CerealSquad.InputManager.Keyboard;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.Factories;
using SFML.Graphics;

namespace CerealSquad
{
    class Orangina : APlayer
    {
        public Orangina(IEntity owner, s_position position, InputManager.InputManager input, int type = 0, int id = 1) : base(owner, position, input, type, id)
        {
            _speed = 5;
            _ressources = new EntityResources();

            Factories.TextureFactory.Instance.load("HinaWalking", "Assets/Character/HinaWalking.png");
            Factories.TextureFactory.Instance.load("HinaDying", "Assets/Character/Death/HinaDying.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "HinaWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "HinaWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "HinaWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "HinaWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "HinaWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.DYING, "HinaDying", Enumerable.Range(0, 25).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 50);

            _ressources.CollisionBox = new FloatRect(new Vector2f(11.0f, -20.0f), new Vector2f(11.0f, 29.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(11.0f, 29.0f), new Vector2f(11.0f, 29.0f));
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
            return EName.Orangina;
        }
    }
}
