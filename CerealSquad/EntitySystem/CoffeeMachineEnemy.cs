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
        private double _attackCoolDown;
        private double _attackCoolDownUltime;

        protected scentMap _scentMap;

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
            _scentMap = new scentMap(room.Size.Height, room.Size.Width);
            _attackCoolDown = 5; // 5 sec
            _attackCoolDown = 30; // 30 sec
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
            _attackCoolDown -= deltaTime.AsSeconds();
            if (Die)
            {
                if (ressourcesEntity.isFinished())
                    destroy();
            }
            else
            {
                _scentMap.update((WorldEntity)_owner, _room);
                think(world, deltaTime);
                move(world, deltaTime);
            }
            _ressources.Update(deltaTime);
        }
        
        protected void attack()
        {
            _attackCoolDown = 5;
            _attackCoolDownUltime = 30;
            //
            // TODO Alpha implement
            //
        }

        public override void think(AWorld world, SFML.System.Time deltaTime)
        {
            bool result = true;
            result &= executeUpMove(world, Speed * deltaTime.AsSeconds());
            result &= executeDownMove(world, Speed * deltaTime.AsSeconds());
            result &= executeLeftMove(world, Speed * deltaTime.AsSeconds());
            result &= executeRightMove(world, Speed * deltaTime.AsSeconds());
            if (_r > 0 && result)
                _r -= 1;
            else
            {
                _r = 0;
                s_position pos = getCoord(HitboxPos);
                var position = ressourcesEntity.Position;
                EMovement lastMove = _move[0];
                _move = new List<EMovement> { EMovement.Up, EMovement.Down, EMovement.Right, EMovement.Left };
                int left = executeLeftMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x - 1, pos._y) : 0;
                ressourcesEntity.Position = position;
                int right = executeRightMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x + 1, pos._y) : 0;
                ressourcesEntity.Position = position;
                int top = executeUpMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x, pos._y - 1) : 0;
                ressourcesEntity.Position = position;
                int bottom = executeDownMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x, pos._y + 1) : 0;
                ressourcesEntity.Position = position;
                int here = _scentMap.getScent(pos._x, pos._y);
                int maxscent = Math.Max(top, Math.Max(bottom, Math.Max(right, left)));
                _move = new List<EMovement> { EMovement.None };

                if (maxscent == 0 && here == 0)
                {
                    _move = new List<EMovement> { EMovement.Down, EMovement.Left, EMovement.Right, EMovement.Up };
                    _move = new List<EMovement> { _move[_rand.Next() % _move.Count] };
                    _r = 30;
                }
                else if (maxscent <= here && moveSameTile(world, (WorldEntity)_owner, deltaTime))
                {
                    if (_attackCoolDown <= 0)
                        attack();
                }
                else
                {
                    if (maxscent == top)
                        _move.Add(EMovement.Up);
                    if (maxscent == bottom)
                        _move.Add(EMovement.Down);
                    if (maxscent == right)
                        _move.Add(EMovement.Right);
                    if (maxscent == left)
                        _move.Add(EMovement.Left);
                    _move.Remove(EMovement.None);
                    if (_move.Contains(lastMove))
                    {
                        _move = new List<EMovement> { lastMove };
                    }
                    else
                    {
                        _move = new List<EMovement> { _move[_rand.Next() % _move.Count] };
                        _r = 10;
                    }
                }
            }
            if (_attackCoolDownUltime <= 0)
                attack();
        }
    }
}
