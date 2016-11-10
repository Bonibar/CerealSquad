using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.Global;
using CerealSquad.Graphics;
using SFML.System;
using SFML.Graphics;
using CerealSquad.GameWorld;

namespace CerealSquad.EntitySystem
{
    class Crates : AEntity
    {
        public enum e_TrapType { Bomb, BearTrap }

        s_Pos<int> Position = new s_Pos<int>(0, 0);
        e_TrapType Item = 0;

        public Crates(IEntity owner, s_Pos<int> _Pos, e_TrapType _Item, s_size size = default(s_size)) : base(owner, size)
        {
            Position = _Pos;
            Item = _Item;

            _ressources = new EntityResources();

            Factories.TextureFactory.Instance.load("CrateFloating", "Assets/GameplayElement/Crates.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            List<uint> PosFrames = new List<uint>();
            for (uint i = 0; i < 30; i++)
            {
                PosFrames.Add(i);
            }

            ((AnimatedSprite)_ressources.sprite).addAnimation(EStateEntity.IDLE, "CrateFloating", PosFrames, new Vector2u(128, 128), 50);
            //Ressources.CollisionBox = new FloatRect(new Vector2f(28.0f, 0.0f), new Vector2f(26.0f, 24.0f));

            _ressources.Position = new Vector2f(Position.X * 64, Position.Y * 64);
        }

        public override void update(Time deltaTime, AWorld world)
        {
            _ressources.Update(deltaTime);
        }
    }
}
