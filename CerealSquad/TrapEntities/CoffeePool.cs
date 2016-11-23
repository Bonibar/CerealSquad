using CerealSquad.GameWorld;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.TrapEntities
{
    class CoffeePool : ATrap
    {
        enum SStateCoffeeTrap
        {
            THROWED = 0,
            SPREADING,
            STAYING
        }

        public CoffeePool(IEntity owner, s_position pos) : base(owner, e_DamageType.COFFE_DAMAGE, 0)
        {
            TrapType = e_TrapType.COFFEE;
            Factories.TextureFactory.Instance.load("CoffeeStaying", "Assets/Enemies/Boss/CoffeeStaying.png");
            Factories.TextureFactory.Instance.load("CoffeeSpreading", "Assets/Enemies/Boss/CoffeeSpreading.png");
            Factories.TextureFactory.Instance.load("CoffeeThrowed", "Assets/Enemies/Boss/CoffeeThrowed.png");
            ressourcesEntity = new Graphics.EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            ressourcesEntity.AddAnimation((uint)SStateCoffeeTrap.STAYING, "CoffeeStaying", new List<uint> { 0 }, new Vector2u(128, 128));
            _CollidingType.Add(e_EntityType.Player);
            Pos = pos;
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage)
        {
            Sender.attemptDamage(this, e_DamageType.COFFE_DAMAGE);
            Die = true;
            ressourcesEntity.Loop = false;

            return true;
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Die && ressourcesEntity.Pause)
                destroy();
            ressourcesEntity.Update(deltaTime);
        }
    }
}
