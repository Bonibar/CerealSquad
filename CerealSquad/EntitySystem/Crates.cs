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
        //s_Pos<int> Position = new s_Pos<int>(0, 0);
        public e_TrapType Item { get; set; }
        private bool PickState = false;
        private bool PickAnimation = false;

        public Crates(IEntity owner, s_Pos<int> _Pos, e_TrapType _Item = 0, s_size size = default(s_size)) : base(owner, size)
        {
            Item = _Item;
            _type = e_EntityType.Crate;
            Factories.TextureFactory.Instance.load("CrateFloating", "Assets/GameplayElement/Crates.png");
            Factories.TextureFactory.Instance.load("CrateOpening", "Assets/GameplayElement/CratesOpening.png");

            _ressources = new EntityResources();
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));
            Pos = new s_position(_Pos.X, _Pos.Y);

            List<uint> PosFrames = new List<uint>();
            for (uint i = 0; i < 30; i++)
            {
                PosFrames.Add(i);
            }

            ((AnimatedSprite)_ressources.sprite).addAnimation(0, "CrateFloating", PosFrames, new Vector2u(128, 128), 50);
            ((AnimatedSprite)_ressources.sprite).addAnimation(1, "CrateOpening", PosFrames, new Vector2u(128, 128), 50);
            _ressources.CollisionBox = new FloatRect(new Vector2f(32.0f, 32.0f), new Vector2f(32.0f, 32.0f));
        }

        public void pickCrate()
        {
            ((AnimatedSprite)_ressources.sprite).Loop = false;
            PickState = true;
        }

        public override void update(Time deltaTime, AWorld world)
        {
            _ressources.Update(deltaTime);
            if (PickState && !PickAnimation && ((AnimatedSprite)_ressources.sprite).isFinished())
            {
                ((AnimatedSprite)_ressources.sprite).PlayAnimation(1);
                PickAnimation = true;
            }
            else if (PickState && PickAnimation && ((AnimatedSprite)_ressources.sprite).isFinished())
                this.destroy();
        }
    }
}
