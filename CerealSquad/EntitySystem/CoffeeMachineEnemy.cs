using CerealSquad.Debug;
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
        private double _autoshoot;
        private bool _takingDamage;

        protected scentMap _scentMap;

        protected enum SCoffeeState
        {
            IDLE = 0,
            WALKING_UP,
            WALKING_DOWN,
            WALKING_RIGHT,
            WALKING_LEFT,
            FULL_TO_MID,
            MID_TO_EMPTY,
            DYING,
            THROWING_COFFEE,
        }

        public CoffeeMachineEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 2;
            _invuln = 0;
            _hp = 3;
            _takingDamage = false;
            _scentMap = new scentMap(room.Size.Height, room.Size.Width);
            _attackCoolDown = 0.5; // 0.5 sec
            _autoshoot = 1.5;
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("CoffeeMachineEmptyWalking", "Assets/Enemies/Boss/CoffeeMachineEmptyWalking.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineMidWalking", "Assets/Enemies/Boss/CoffeeMachineMidWalking.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineWalking", "Assets/Enemies/Boss/CoffeeMachineWalking.png");

            Factories.TextureFactory.Instance.load("CoffeeMachineDying", "Assets/Enemies/Boss/CoffeeMachineDying.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineToEmpty", "Assets/Enemies/Boss/CoffeeMachineToEmpty.png");
            Factories.TextureFactory.Instance.load("CoffeeMachineToMid", "Assets/Enemies/Boss/CoffeeMachineToMid.png");

            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(128, 128));

            ressourcesEntity.sprite.Size = new Vector2f(128, 128);
            ressourcesEntity.AddAnimation((uint)SCoffeeState.IDLE, "CoffeeMachineWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));

            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_DOWN, "CoffeeMachineWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_LEFT, "CoffeeMachineWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_RIGHT, "CoffeeMachineWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SCoffeeState.WALKING_UP, "CoffeeMachineWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)SCoffeeState.DYING, "CoffeeMachineDying", Enumerable.Range(0, 36).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 65);

            ressourcesEntity.AddAnimation((uint)SCoffeeState.MID_TO_EMPTY, "CoffeeMachineToEmpty", Enumerable.Range(0, 12).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 100);
            ressourcesEntity.AddAnimation((uint)SCoffeeState.FULL_TO_MID, "CoffeeMachineToMid", Enumerable.Range(0, 12).Select(i => (uint)i).ToList(), new Vector2u(128, 128), 100);


            _ressources.CollisionBox = new FloatRect(new Vector2f(38.0f, -0f), new Vector2f(38.0f, 48.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(38.0f, 50.0f), new Vector2f(38.0f, 48.0f));
            Pos = Pos;
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
                base.die();
                ressourcesEntity.PlayAnimation((uint)SCoffeeState.DYING);
                ressourcesEntity.Loop = false;
                _children.ToList().ForEach(i => ((AEntity)i).die());
            }
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            if (_invuln > 0)
                _invuln -= deltaTime.AsSeconds();
            if (_attackCoolDown > 0)
                _attackCoolDown -= deltaTime.AsSeconds();
            if (_autoshoot > 0)
                _autoshoot -= deltaTime.AsSeconds();
            if (Die)
            {
                if (ressourcesEntity.Animation != (uint)SCoffeeState.DYING)
                    ressourcesEntity.PlayAnimation((uint)SCoffeeState.DYING);
                else if (ressourcesEntity.Pause)
                    if (_children.Count == 0)
                        destroy();
            }
            else
            {
                if (_takingDamage && ressourcesEntity.Animation != (uint)SCoffeeState.FULL_TO_MID && ressourcesEntity.Animation != (uint)SCoffeeState.MID_TO_EMPTY)
                {
                    if (_hp == 2)
                        ressourcesEntity.PlayAnimation((uint)SCoffeeState.FULL_TO_MID);
                    else if (_hp == 1)
                        ressourcesEntity.PlayAnimation((uint)SCoffeeState.MID_TO_EMPTY);
                }
                else if (_takingDamage && ressourcesEntity.Pause)
                {
                    ressourcesEntity.Loop = true;
                    ressourcesEntity.PlayAnimation((uint)SCoffeeState.IDLE);
                    _takingDamage = false;
                }
                else if (!_takingDamage)
                {
                    if (Active)
                    {
                        _scentMap.update((WorldEntity)_owner.getOwner(), _room);
                        think(world, deltaTime);
                    }
                    move(world, deltaTime);
                }
            }
            _ressources.Update(deltaTime);
            _children.ToList().ForEach(i => i.update(deltaTime, world));
        }

        private bool canAttack(WorldEntity world)
        {
            ressourcesEntity.secondarySprite.Clear();
            double deltaX = 0;
            double deltaY = 0;
            bool result = false;
            bool end = false;
            s_position pos = getCoord(HitboxPos);

            if (_autoshoot <= 0)
                return (true);

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
                    for (int i = 0; i < 30; i += 1)
                    {
                        if (InCircleRange(HitboxPos.X + deltaX * i, HitboxPos.Y + deltaY * i, ent, 0.5f))
                            result = true;
                        if (shootDebug)
                        {
                            EllipseShapeSprite sprite = new EllipseShapeSprite(new Vector2f(0.5f * 64f, 0.5f * 64f), new Color(255, (byte)(10 * i), 0, 255), new Color(255, 0, 0, 0));
                            var posSprite = sprite.EllipseShape.Position;
                            posSprite.X = (float)(deltaX * i * 64);
                            posSprite.Y = (float)(deltaY * i * 64);
                            sprite.EllipseShape.Position = posSprite;
                            ressourcesEntity.secondarySprite.Add(sprite);
                        }
                        if (_room.getPosition((uint)(pos.X + deltaX * i), (uint)(pos.Y + deltaY * i)) == RoomParser.e_CellType.Wall
                        || _room.getPosition((uint)(pos.X + deltaX * i), (uint)(pos.Y + deltaY * i)) == RoomParser.e_CellType.Void)
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
            _attackCoolDown = 0.5f;
            _autoshoot = 1.5f;
            s_position pos = Pos;
            pos.X += ressourcesEntity.Size.X / 128 / 2;
            pos.Y += ressourcesEntity.Size.Y / 128 / 2;
            EMovement move = _move[0];
            if (_move[0] == EMovement.None)
                move = EMovement.Down;
            new CoffeeProjectile(_owner, move, pos);
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage, bool isHitBox = false)
        {
            bool result = false;
            if (_invuln <= 0 && !_takingDamage)
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
            new CoffeePool(this, new s_position(Pos.X, Pos.Y));
            ressourcesEntity.Loop = false;
            System.Diagnostics.Debug.WriteLine("HP : " + _hp);
            if (_hp == 2)
                ressourcesEntity.PlayAnimation((uint)SCoffeeState.FULL_TO_MID);
            else if (_hp == 1)
                ressourcesEntity.PlayAnimation((uint)SCoffeeState.MID_TO_EMPTY);
            _takingDamage = true;
        }

        public override void think(AWorld world, SFML.System.Time deltaTime)
        {
            bool result = true;
            result &= executeUpMove(world, Speed * deltaTime.AsSeconds());
            result &= executeDownMove(world, Speed * deltaTime.AsSeconds());
            result &= executeLeftMove(world, Speed * deltaTime.AsSeconds());
            result &= executeRightMove(world, Speed * deltaTime.AsSeconds());
            if (canAttack((WorldEntity)_owner.getOwner()))
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
                int left = executeLeftMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent((int)pos.X - 1, (int)pos.Y) : 0;
                ressourcesEntity.Position = position;
                int right = executeRightMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent((int)pos.X + 1, (int)pos.Y) : 0;
                ressourcesEntity.Position = position;
                int top = executeUpMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent((int)pos.X, (int)pos.Y - 1) : 0;
                ressourcesEntity.Position = position;
                int bottom = executeDownMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent((int)pos.X, (int)pos.Y + 1) : 0;
                ressourcesEntity.Position = position;
                int here = _scentMap.getScent((int)pos.X, (int)pos.Y);
                int maxscent = Math.Max(top, Math.Max(bottom, Math.Max(right, left)));
                _move = new List<EMovement> { EMovement.None };

                if (maxscent == 0 && here == 0)
                {
                    _move = new List<EMovement> { EMovement.Down, EMovement.Left, EMovement.Right, EMovement.Up };
                    _move = new List<EMovement> { _move[_rand.Next() % _move.Count] };
                    _r = 30;
                }
                else if (maxscent <= here && moveSameTile(world, (WorldEntity)getRootEntity(), deltaTime))
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
