using CerealSquad.EntitySystem.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.GameWorld;
using SFML.System;
using SFML.Graphics;
using CerealSquad.Graphics;

namespace CerealSquad.EntitySystem.Projectiles
{
    class RiceProjectile : AProjectile
    {
        public RiceProjectile(IEntity owner, EMovement direction, s_position pos) : base(owner, direction)
        {
            Speed = 10;
            Factories.TextureFactory.Instance.load("RiceProjectile", "Assets/Enemies/Normal/Rice.png");
            ressourcesEntity = new Graphics.EntityResources();
            _ressources.InitializationAnimatedSprite(new Vector2u(10, 10));

            _ressources.AddAnimation(0, "RiceProjectile", new List<uint> { 0 }, new Vector2u(2, 2));
            _ressources.AddAnimation(1, "RiceProjectile", new List<uint> { 0 }, new Vector2u(2, 2));
            _ressources.AddAnimation(2, "RiceProjectile", new List<uint> { 0 }, new Vector2u(2, 2));
            _ressources.AddAnimation(3, "RiceProjectile", new List<uint> { 0 }, new Vector2u(2, 2));
            _ressources.AddAnimation(4, "RiceProjectile", new List<uint> { 0 }, new Vector2u(2, 2));

            _ressources.CollisionBox = new FloatRect(new Vector2f(5f, 5f), new Vector2f(5f, 5f));
            Pos = pos;
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Die)
            {
                destroy();
            }
            else
            {

                base.move(world, deltaTime);
            }
            ressourcesEntity.Update(deltaTime);
        }

        public override bool IsCollidingWithWall(AWorld World, EntityResources Res)
        {
            bool baseResult = base.IsCollidingWithWall(World, Res);

            if (baseResult)
                Die = true;

            return baseResult;
        }

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool baseResult = base.IsCollidingEntity(World, CollidingEntities);
            bool result = false;


            CollidingEntities.ForEach(i =>
            {
                if (i.getEntityType() != e_EntityType.EnnemyTrap && i.getEntityType() != e_EntityType.Ennemy)
                    result = true;
                if (i.getEntityType() == e_EntityType.Player)
                    i.die();
            });

            if (baseResult || result)
                Die = true;

            return baseResult || result;
        }
    }
}
