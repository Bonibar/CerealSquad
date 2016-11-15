using CerealSquad.Graphics;
using CerealSquad.InputManager.Keyboard;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.Factories;

namespace CerealSquad
{
    class Orangina : APlayer
    {
        public Orangina(IEntity owner, s_position position, InputManager.InputManager input) : base(owner, position, input)
        {
            _speed = 5;
            Factories.TextureFactory.Instance.load("orangina", "Assets/Character/orangina.png");
            ressourcesEntity = new EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));
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
