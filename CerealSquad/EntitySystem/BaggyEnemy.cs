using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.EntitySystem
{
    class BaggyEnemy : AEnemy
    {
        protected enum SBaggyState
        {
            IDLE = 0,
            WALKING_UP,
            WALKING_DOWN,
            WALKING_RIGHT,
            WALKING_LEFT,
            PHASE_ONE_TO_TWO,
            DYING,
            SUMMONING,
        }

        public BaggyEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 2;
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("BaggyHiding", "Assets/Enemies/Boss/BaggyHiding.png");
            Factories.TextureFactory.Instance.load("BaggyPhase1toPhase2", "Assets/Enemies/Boss/BaggyPhase1toPhase2.png");
            Factories.TextureFactory.Instance.load("BaggyPhase1Walking", "Assets/Enemies/Boss/BaggyPhase1Walking.png");
            Factories.TextureFactory.Instance.load("BaggyPhase2Walking", "Assets/Enemies/Boss/BaggyPhase2Walking.png");
            Factories.TextureFactory.Instance.load("BaggySummoning", "Assets/Enemies/Boss/BaggySummoning.png");
            Factories.TextureFactory.Instance.load("BaggyDying", "Assets/Enemies/Boss/BaggyDying.png");


            _ressources.InitializationAnimatedSprite(new Vector2u(128, 128));
            ChangingAnimationPhase(1);

            _ressources.CollisionBox = new FloatRect(new Vector2f(17.0f, 0.0f), new Vector2f(17.0f, 24.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(17.0f, 24.0f), new Vector2f(17.0f, 24.0f));
            Pos = Pos; // very important
        }

        private void ChangingAnimationPhase(int phase)
        {
            if (phase == 1)
            {
                ressourcesEntity.AddAnimation((uint)SBaggyState.IDLE, "BaggyPhase1Walking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_DOWN, "BaggyPhase1Walking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_UP, "BaggyPhase1Walking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_RIGHT, "BaggyPhase1Walking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_LEFT, "BaggyPhase1Walking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.SUMMONING, "BaggySummoning", Enumerable.Range(0, 10).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
            }
            else
            {
                ressourcesEntity.AddAnimation((uint)SBaggyState.IDLE, "BaggyPhase2Walking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_DOWN, "BaggyPhase2Walking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_UP, "BaggyPhase2Walking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_RIGHT, "BaggyPhase2Walking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.WALKING_LEFT, "BaggyPhase2Walking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
                ressourcesEntity.AddAnimation((uint)SBaggyState.SUMMONING, "BaggySummoning", Enumerable.Range(11, 21).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
            }
            ressourcesEntity.AddAnimation((uint)SBaggyState.PHASE_ONE_TO_TWO, "BaggyPhase1toPhase2", Enumerable.Range(0, 5).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SBaggyState.DYING, "BaggyDying", Enumerable.Range(0, 32).Select(i => (uint)i).ToList(), new Vector2u(128, 128));

        }

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool baseResult = base.IsCollidingEntity(World, CollidingEntities);
            bool result = false;

            CollidingEntities.ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.PlayerTrap && ((ATrap)i).TrapType == e_TrapType.WALL)
                    result = true;
            });


            return result || baseResult;
        }

        public override void die()
        {
            if (!Die)
            {
                PlayAnimation((uint)SBaggyState.DYING);
                ressourcesEntity.Loop = false;
                base.die();
            }
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Die)
            {
                if (ressourcesEntity.Pause)
                    destroy();
            }
            else
            {
                if (Active)
                {
                    // _scentMap.update((WorldEntity)_owner.getOwner(), _room);
                    // think(world, deltaTime);
                }
                move(world, deltaTime);
            }
            _ressources.Update(deltaTime);
        }

        public override void think(AWorld world, Time deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}