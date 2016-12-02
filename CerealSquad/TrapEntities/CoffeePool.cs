using CerealSquad.GameWorld;
using CerealSquad.EntitySystem;
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
        public static readonly SFML.Graphics.FloatRect COLLISION_BOX = new SFML.Graphics.FloatRect(50, 25, 50, 25);

        enum SStateCoffeeTrap
        {
            SPREADING,
            STAYING,
            DYING
        }

        public CoffeePool(IEntity owner, s_position pos) : base(owner, e_DamageType.COFFE_DAMAGE, 0)
        {
            TrapType = e_TrapType.COFFE;
            _damageType = e_DamageType.COFFE_DAMAGE;
            Factories.TextureFactory.Instance.load("CoffeeStaying", "Assets/Enemies/Boss/CoffeeStaying.png");
            Factories.TextureFactory.Instance.load("CoffeeSpreading", "Assets/Enemies/Boss/CoffeeSpreading.png");
            ressourcesEntity = new Graphics.EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(128, 128));

            ressourcesEntity.AddAnimation((uint)SStateCoffeeTrap.SPREADING, "CoffeeSpreading", Enumerable.Range(0, 11).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 100);
            ressourcesEntity.AddAnimation((uint)SStateCoffeeTrap.STAYING, "CoffeeStaying", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SStateCoffeeTrap.DYING, "CoffeeSpreading", new List<uint> { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }, new Vector2u(128, 128), 100);
            
            ressourcesEntity.CollisionBox = COLLISION_BOX;
            Pos = pos;
            _CollidingType.Add(e_EntityType.Player);
            ressourcesEntity.PlayAnimation((uint)SStateCoffeeTrap.SPREADING);
            ressourcesEntity.Loop = false;
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage)
        {
            Sender.attemptDamage(this, _damageType);

            return true;
        }

        public bool IsReadyToDie()
        {
            return Die && ressourcesEntity.Pause;
        }

        public override void die()
        {
            if (!Die)
            {
                base.die();
                ressourcesEntity.PlayAnimation((uint)SStateCoffeeTrap.DYING);
                ressourcesEntity.Loop = false;
            }
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (ressourcesEntity.Animation == (uint)SStateCoffeeTrap.SPREADING && ressourcesEntity.Pause)
            {
                ressourcesEntity.PlayAnimation((uint)SStateCoffeeTrap.STAYING);
                ressourcesEntity.Loop = true;
            }
            else if (IsReadyToDie())
                destroy();
            ressourcesEntity.Update(deltaTime);
        }
    }
}
