using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace CerealSquad.TrapEntities
{
    class BearTrap : ATrap
    {
        public BearTrap(IEntity owner) : base(owner, e_DamageType.BEAR_TRAP, 0)
        {
            Factories.TextureFactory.Instance.load("BearTrap", "Assets/trapbear.png");
            ressourcesEntity = new Graphics.EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((Graphics.AnimatedSprite)_ressources.sprite).addAnimation(Graphics.EStateEntity.IDLE, "BeartTrap", new List<uint> { 0, 1 }, new Vector2u(128, 128));

        }

        public override void update(Time deltaTime)
        {
            ressourcesEntity.Update(deltaTime);
        }
    }
}
