using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using CerealSquad.GameWorld;

namespace CerealSquad.TrapEntities
{
    class SugarWall : ATrap
    {
        public static readonly SFML.Graphics.FloatRect COLLISION_BOX = new SFML.Graphics.FloatRect(25, 12, 25, 32);
        public static readonly SFML.Graphics.FloatRect HIT_BOX = new SFML.Graphics.FloatRect(25, 30, 25, 32);

        public SugarWall(IEntity owner) : base(owner, e_DamageType.NONE, 0)
        {
            TrapType = e_TrapType.WALL;
            Factories.TextureFactory.Instance.load("SugarWall", "Assets/Trap/SugarWall.png");

            ressourcesEntity = new Graphics.EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            ressourcesEntity.AddAnimation(0, "SugarWall", new List<uint> { 0 }, new Vector2u(128, 128));
            ressourcesEntity.CollisionBox = COLLISION_BOX;
            ressourcesEntity.HitBox = HIT_BOX;
        }

        public override void Trigger()
        {
            return;
        }

        public override void update(Time deltaTime, AWorld world)
        {
            ressourcesEntity.Update(deltaTime);
        }
    }
}
