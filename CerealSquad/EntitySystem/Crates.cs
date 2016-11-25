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
        public bool Picked { get; private set; }
        private bool PickState = false;
        public bool Respawn { get { return Picked && TimeToRespawn <= 0; } }
        private int TimeToRespawn = -1;
        private int RespawnTime;

        public Crates(IEntity owner, s_Pos<int> _Pos, e_TrapType _Item = 0, int respawnTime = 3600, s_size size = default(s_size)) : base(owner, size)
        {
            Picked = false;
            Item = _Item;
            RespawnTime = respawnTime;
            _type = e_EntityType.Crate;
            Factories.TextureFactory.Instance.load("CrateFloating", "Assets/GameplayElement/Crates.png");

            _ressources = new EntityResources();
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));
            Pos = new s_position(_Pos.X, _Pos.Y);

            List<uint> PosFrames = new List<uint>();
            for (uint i = 0; i < 12; i++)
            {
                PosFrames.Add(i);
            }

            ((AnimatedSprite)_ressources.sprite).addAnimation(0, "CrateFloating", PosFrames, new Vector2u(128, 128), 80);
            _ressources.CollisionBox = new FloatRect(21f, -6f, 17f, 24f);

            _CollidingType.Add(e_EntityType.Player);
        }

        public void pickCrate()
        {
            ((AnimatedSprite)_ressources.sprite).Loop = false;
            PickState = true;
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (_ressources != null)
                _ressources.Update(deltaTime);
            if (TimeToRespawn > -1 && Picked)
                TimeToRespawn -= deltaTime.AsMilliseconds();
            else if (PickState && _ressources != null)
            {
                Picked = true;
                PickState = false;
                destroy();
                _ressources = null;
                TimeToRespawn = RespawnTime;
            }
        }
    }
}
