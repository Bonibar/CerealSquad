using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CerealSquad.GameWorld;

namespace CerealSquad.TrapEntities
{
    class BearTrap : ATrap
    {
        public static readonly SFML.Graphics.FloatRect COLLISION_BOX = new SFML.Graphics.FloatRect(12, 12, 12, 12);

        public BearTrap(IEntity owner) : base(owner, e_DamageType.TRUE_DAMAGE, 0)
        {
            TrapType = e_TrapType.BEAR_TRAP;
            Factories.TextureFactory.Instance.load("BearTrap", "Assets/Trap/Beartrap.png");

            ressourcesEntity = new Graphics.EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((Graphics.AnimatedSprite)_ressources.sprite).addAnimation(Graphics.EStateEntity.IDLE, "BearTrap", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ressourcesEntity.CollisionBox = COLLISION_BOX;
        }

        public override void update(Time deltaTime, AWorld world)
        {
            ressourcesEntity.Update(deltaTime);
        }
    }
}
