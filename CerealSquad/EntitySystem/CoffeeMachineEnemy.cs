using CerealSquad.Debug;
using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CerealSquad.EntitySystem
{
    class CoffeeMachineEnemy : AEnemy
    {
        protected enum SCoffeeState
        {
            IDLE = 0,
            WALKING_UP,
            WALKING_DOWN,
            WALKING_RIGHT,
            WALKING_LEFT,
            DYING,
            THROWING_COFFEE,
        }
        public CoffeeMachineEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 5;
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("CoffeeMachineWalking", "Assets/Enemies/Boss/CoffeeMachineWalking.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineThrowing", "Assets/Enemies/Boss/CoffeeMachineThrowingCoffee.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)SCoffeeState.IDLE, "CoffeeMachineWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)SCoffeeState.WALKING_DOWN, "CoffeeMachineWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)SCoffeeState.WALKING_LEFT, "CoffeeMachineWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)SCoffeeState.WALKING_RIGHT, "CoffeeMachineWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)SCoffeeState.WALKING_UP, "CoffeeMachineWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)SCoffeeState.THROWING_COFFEE, "CoffeeMachineThrowing", Enumerable.Range(0, 10).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
            //  ((AnimatedSprite)_ressources.sprite).addAnimation((uint)SCoffeeState.DYING, "CoffeeMachineDying", Enumerable.Range(0, 14).Select(i => (uint)i).ToList(), new Vector2u(128, 128));

            _ressources.CollisionBox = new FloatRect(new Vector2f(19.0f, -10.0f), new Vector2f(19.0f, 24.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(19.0f, 25.0f), new Vector2f(19.0f, 24.0f));
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

        public override void update(SFML.System.Time deltaTime, AWorld world)
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

        public override void think(AWorld world, SFML.System.Time deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
