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
            ressourcesEntity.InitializationAnimatedSprite("BearTrap", new Vector2i(128, 128));
            ressourcesEntity.Scale = new Vector2f(0.5f, 0.5f);
            System.Diagnostics.Debug.WriteLine("TRAP INIT");
        }

        public override void update(Time deltaTime)
        {
            ressourcesEntity.Update(deltaTime);
        }
    }
}
