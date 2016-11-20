using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.GameWorld;
using SFML.System;
using CerealSquad.Graphics;
using SFML.Graphics;

namespace CerealSquad.EntitySystem
{
    class EggEnemy : AEnemy
    {
        public EggEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 5;
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("EggWalking", "Assets/Enemies/Normal/EggyWalking.png");
            Factories.TextureFactory.Instance.load("EggBreaking", "Assets/Enemies/Normal/EggyBreaking.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "EggWalking", new List<uint> { 0 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "EggWalking", new List<uint> { 0, 1, 2, 3 }, new Vector2u(128, 128), 150);
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "EggWalking", new List<uint> { 12, 13, 14, 15 }, new Vector2u(128, 128), 150);
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "EggWalking", new List<uint> { 8, 9, 10, 11 }, new Vector2u(128, 128), 150);
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "EggWalking", new List<uint> { 4, 5, 6, 7 }, new Vector2u(128, 128), 150);
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.DYING, "EggBreaking", Enumerable.Range(0, 14).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 45);

            _ressources.CollisionBox = new FloatRect(new Vector2f(26.0f, 0.0f), new Vector2f(26.0f, 26.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(26.0f, 26.0f), new Vector2f(26.0f, 26.0f));
            Pos = position;
        }

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool baseResult = base.IsCollidingEntity(World, CollidingEntities);
            bool result = false;

            CollidingEntities.ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.PlayerTrap && ((ATrap)i).TrapType != e_TrapType.WALL)
                    Die = true;
                else if (i.getEntityType() == e_EntityType.PlayerTrap && ((ATrap)i).TrapType != e_TrapType.WALL)
                    result = true;
            });


            return result || baseResult;
        }

        public override void think(AWorld world, Time deltaTime)
        {
            throw new NotImplementedException();
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Die)
            {
                if (ressourcesEntity.isFinished())
                    destroy();
            }
            else
            {
                // _scentMap.update((WorldEntity)_owner);
                //think();
                move(world, deltaTime);
            }
            _ressources.Update(deltaTime);
        }
    }
}
