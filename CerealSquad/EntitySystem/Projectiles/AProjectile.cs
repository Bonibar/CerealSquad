using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.GameWorld;
using SFML.System;

namespace CerealSquad.EntitySystem.Projectile
{
    abstract class AProjectile : AEntity
    {
        public AProjectile(IEntity owner, EMovement direction) : base(owner)
        {
            _move = new List<EMovement> { direction };
            _type = e_EntityType.ProjectileEnemy;
            _damageType = e_DamageType.PROJECTILE_ENEMY_DAMAGE;

            _CollidingType.Add(e_EntityType.PlayerTrap);
            _CollidingType.Add(e_EntityType.Player);
            _CollidingType.Add(e_EntityType.EnnemyTrap);
            _CollidingType.Add(e_EntityType.Ennemy);
        }
    }
}
