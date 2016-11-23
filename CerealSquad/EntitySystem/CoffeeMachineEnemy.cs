﻿using CerealSquad.Debug;
using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using System;
using CerealSquad.TrapEntities;
using CerealSquad.EntitySystem.Projectiles;

namespace CerealSquad.EntitySystem
{
    class CoffeeMachineEnemy : AEnemy
    {
        private double _attackCoolDown;
        private int _hp;
        private double _invuln;

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
            _speed = 1;
            _invuln = 0;
            _hp = 3;
            _scentMap = new scentMap(room.Size.Height, room.Size.Width);
            _attackCoolDown = 1; // 1 sec
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("CoffeeMachineEmptyWalking", "Assets/Enemies/Boss/CoffeeMachineEmptyWalking.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineMidWalking", "Assets/Enemies/Boss/CoffeeMachineMidWalking.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineWalking", "Assets/Enemies/Boss/CoffeeMachineWalking.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineThrowing", "Assets/Enemies/Boss/CoffeeMachineThrowingCoffee.png");

            Factories.TextureFactory.Instance.load("CoffeeMachineDying", "Assets/Enemies/Boss/CoffeeMachineDying.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineToEmpty", "Assets/Enemies/Boss/CoffeeMachineToEmpty.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineToMid", "Assets/Enemies/Boss/CoffeeMachineToMid.png");

            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));


            ressourcesEntity.AddAnimation((uint)SCoffeeState.IDLE, "CoffeeMachineWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));

            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_DOWN, "CoffeeMachineWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_LEFT, "CoffeeMachineWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_RIGHT, "CoffeeMachineWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_UP, "CoffeeMachineWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SCoffeeState.THROWING_COFFEE, "CoffeeMachineThrowing", Enumerable.Range(0, 10).Select(i => (uint)i).ToList(), new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SCoffeeState.DYING, "CoffeeMachineDying", Enumerable.Range(0, 14).Select(i => (uint)i).ToList(), new Vector2u(128, 128));

            _ressources.CollisionBox = new FloatRect(new Vector2f(19.0f, -10.0f), new Vector2f(19.0f, 24.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(19.0f, 25.0f), new Vector2f(19.0f, 24.0f));
            Pos = Pos;
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

        public override void die()
        {
            if (_invuln <= 0)
                base.die();
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            if (_invuln > 0)
                _invuln -= deltaTime.AsSeconds();
            if (_attackCoolDown > 0)
                _attackCoolDown -= deltaTime.AsSeconds();
            if (Die)
            {
                if (ressourcesEntity.isFinished())
                    destroy();
            }
            else
            {
                if (Active)
                {
                    _scentMap.update((WorldEntity)_owner, _room);
                    think(world, deltaTime);
                }
                move(world, deltaTime);
            }
            _ressources.Update(deltaTime);
        }

        private bool canAttack(WorldEntity world)
        {
            double deltaX = 0;
            double deltaY = 0;
            bool result = false;
            bool end = false;
            s_position pos = getCoord(Pos);

            if (_attackCoolDown > 0)
                return (false);

            switch (_move[0])
            {
                case EMovement.Down:
                    deltaY = 0.5;
                    break;
                case EMovement.Up:
                    deltaY = -0.5;
                    break;
                case EMovement.Right:
                    deltaX = 0.5;
                    break;
                case EMovement.Left:
                    deltaX = -0.5;
                    break;
            }
            world.GetAllEntities().ForEach(ent =>
            {
                if (ent.getEntityType() == e_EntityType.Player)
                {
                    for (int i = 0; i < 90; i += 1)
                    {
                        if (IsInEllipse(Pos._trueX + deltaX * i, Pos._trueY + deltaY * i, ent.HitboxPos._trueX, ent.Pos._trueY, 0.2, 0.2))
                            result = true;
                        if (_room.getPosition((uint)(pos._trueX + deltaX * i), (uint)(pos._trueY + deltaY * i)) == RoomParser.e_CellType.Wall
                        || _room.getPosition((uint)(pos._trueX + deltaX * i), (uint)(pos._trueY + deltaY * i)) == RoomParser.e_CellType.Void)
                            end = true;
                        if (result || end)
                            break;
                    }
                }
            });
            return (result);
        }

        protected void attack()
        {
            _attackCoolDown = 1;
            new CoffeeProjectile(_owner, _move[0], HitboxPos);
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage)
        {
            bool result = false;
            if (_invuln <= 0)
                switch (damage)
                {
                    case e_DamageType.BOMB_DAMAGE:
                    case e_DamageType.TRUE_DAMAGE:
                        _hp -= 1;
                        if (_hp == 0)
                            die();
                        else
                            receiveDamage();
                        if (_hp == 1)
                        {
                            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_DOWN, "CoffeeMachineEmptyWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_LEFT, "CoffeeMachineEmptyWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
                            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_RIGHT, "CoffeeMachineEmptyWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
                            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_UP, "CoffeeMachineEmptyWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
                        }
                        else
                        {
                            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_DOWN, "CoffeeMachineMidWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
                            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_LEFT, "CoffeeMachineMidWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
                            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_RIGHT, "CoffeeMachineMidWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
                            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_UP, "CoffeeMachineMidWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
                        }
                        result = true;
                        break;
                }

            return result;
        }

        private void receiveDamage()
        {
            _invuln = 3;
            new CoffeePool(_owner, new s_position(Pos._trueX + 1, Pos._trueY));
            new CoffeePool(_owner, new s_position(Pos._trueX - 1, Pos._trueY));
            new CoffeePool(_owner, new s_position(Pos._trueX, Pos._trueY + 1));
            new CoffeePool(_owner, new s_position(Pos._trueX, Pos._trueY - 1));
        }

        public override void think(AWorld world, SFML.System.Time deltaTime)
        {
            bool result = true;
            result &= executeUpMove(world, Speed * deltaTime.AsSeconds());
            result &= executeDownMove(world, Speed * deltaTime.AsSeconds());
            result &= executeLeftMove(world, Speed * deltaTime.AsSeconds());
            result &= executeRightMove(world, Speed * deltaTime.AsSeconds());
            if (canAttack((WorldEntity)_owner))
                attack();
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
        }
    }
}
