using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using CerealSquad.GameWorld;
using CerealSquad.Graphics;

namespace CerealSquad.TrapEntities
{
    class SugarWall : ATrap
    {
        public static readonly FloatRect COLLISION_BOX = new FloatRect(25, 0, 25, 32);
        public static readonly FloatRect HIT_BOX = new FloatRect(25, 30, 25, 32);

        private Timer Timer = new Timer(Time.FromSeconds(10));

        public SugarWall(IEntity owner) : base(owner, e_DamageType.NONE, 0)
        {
            Factories.TextureFactory.Instance.load("SugarWall", "Assets/Trap/SugarWall.png");

            TrapType = e_TrapType.WALL;
            Cooldown = Time.FromSeconds(0.5f);

            ressourcesEntity = new EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));
            ressourcesEntity.AddAnimation(0, "SugarWall", new List<uint> { 0, 1, 2, 3, 4, 5, 6 }, new Vector2u(64, 64));
            ressourcesEntity.AddAnimation(1, "SugarWall", new List<uint> { 6, 5, 4, 3, 2, 1, 0 }, new Vector2u(64, 64));
            ressourcesEntity.Loop = false;

            ressourcesEntity.CollisionBox = COLLISION_BOX;
            ressourcesEntity.HitBox = HIT_BOX;
            Timer.Start();
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Timer.IsTimerOver() && !Die) {
                Die = true;
                ressourcesEntity.PlayAnimation(1);
                ressourcesEntity.Loop = false;
            }

            if (Die)
            {
                if (ressourcesEntity.Pause == true)
                    destroy();
            }

            ressourcesEntity.Update(deltaTime);
        }
    }
}
