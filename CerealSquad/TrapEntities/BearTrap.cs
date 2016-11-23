using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CerealSquad.GameWorld;
using CerealSquad.Sounds;

namespace CerealSquad.TrapEntities
{
    class BearTrap : ATrap
    {
        public static readonly SFML.Graphics.FloatRect COLLISION_BOX = new SFML.Graphics.FloatRect(21, 12, 21, 12);
        public bool Triggered { get; private set; }

        enum SStateBearTrap
        {
            READY = 0,
            TRIGGERED
        }

        public BearTrap(IEntity owner) : base(owner, e_DamageType.TRUE_DAMAGE, 0)
        {
            TrapType = e_TrapType.BEAR_TRAP;
            Cooldown = Time.FromSeconds(0.2f);
            Triggered = false;
            Factories.TextureFactory.Instance.load("BearTrap", "Assets/Trap/Beartrap.png");

            ressourcesEntity = new Graphics.EntityResources();
            ressourcesEntity.JukeBox.loadSound("BearTrap", "BearTrap");

            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            ressourcesEntity.AddAnimation((uint)SStateBearTrap.READY, "BearTrap", new List<uint> { 0 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SStateBearTrap.TRIGGERED, "BearTrap", new List<uint> { 1, 1 }, new Vector2u(128, 128));
            ressourcesEntity.CollisionBox = COLLISION_BOX;

            _CollidingType.Add(e_EntityType.Ennemy);
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage)
        {
            if ((getEntityType() == e_EntityType.EnnemyTrap
                || getEntityType() == e_EntityType.PlayerTrap)
                && !Triggered)
                Trigger(true);

            Sender.attemptDamage(this, e_DamageType.TRUE_DAMAGE);

            return true;
        }

        public override void Trigger(bool delay = false)
        {
            if (!Triggered)
            {
                Triggered = true;
                ressourcesEntity.JukeBox.PlaySound("BearTrap");
                ressourcesEntity.PlayAnimation((uint)SStateBearTrap.TRIGGERED);
                ressourcesEntity.Loop = false;
                Die = true;
            }
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Die && ressourcesEntity.Pause)
                destroy();
            ressourcesEntity.Update(deltaTime);
        }
    }
}
