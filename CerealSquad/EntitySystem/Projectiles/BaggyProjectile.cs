﻿using CerealSquad.EntitySystem.Projectile;
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
    class BaggyProjectile : AProjectile
    {
        public BaggyProjectile(IEntity owner, EMovement direction, s_position pos) : base(owner, direction)
        {
            Speed = 10;
            Factories.TextureFactory.Instance.load("BaggyProjectile", "Assets/Enemies/Boss/BaggyProjectile.png");
            ressourcesEntity = new Graphics.EntityResources();
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            _ressources.AddAnimation(0, "BaggyProjectile", new List<uint> { 0 }, new Vector2u(32, 32));
            _ressources.AddAnimation(1, "BaggyProjectile", new List<uint> { 0 }, new Vector2u(32, 32));
            _ressources.AddAnimation(2, "BaggyProjectile", new List<uint> { 0 }, new Vector2u(32, 32));
            _ressources.AddAnimation(3, "BaggyProjectile", new List<uint> { 0 }, new Vector2u(32, 32));
            _ressources.AddAnimation(4, "BaggyProjectile", new List<uint> { 0 }, new Vector2u(32, 32));

            _ressources.CollisionBox = new FloatRect(new Vector2f(16f, 16f), new Vector2f(16f, 16f));
            _type = e_EntityType.ProjectileEnemy;
            _damageType = e_DamageType.PROJECTILE_ENEMY_DAMAGE;

            _CollidingType.Add(e_EntityType.Player);

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
                die();

            return baseResult;
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage, bool isHitBox = false)
        {
            if (damage == e_DamageType.NONE)
            {
                die();
                return true;
            }

            return false;
        }

        protected override void IsTouchingHitBoxEntities(AWorld world, List<AEntity> touchingEntities)
        {
            touchingEntities.ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.Player)
                    attemptDamage(i, i.getDamageType(), true);
                else if (i.getEntityType() == e_EntityType.PlayerTrap)
                    attemptDamage(i, i.getDamageType(), true);

                i.attemptDamage(this, _damageType);
            });
        }

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool baseResult = base.IsCollidingEntity(World, CollidingEntities);
            bool result = false;


            CollidingEntities.ForEach(i =>
            {
                switch (i.getEntityType())
                {
                    case e_EntityType.Ennemy:
                    case e_EntityType.EnnemyTrap:
                    case e_EntityType.Player:
                    case e_EntityType.PlayerTrap:
                    case e_EntityType.Room:
                        result = true;
                        break;
                }

                i.attemptDamage(this, _damageType);
            });

            if (result)
                attemptDamage(this, e_DamageType.NONE);

            return baseResult || result;
        }
    }
}
