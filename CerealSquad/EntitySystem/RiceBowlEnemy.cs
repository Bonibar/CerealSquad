using CerealSquad.Debug;
using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using System;
using CerealSquad.EntitySystem.Projectiles;

namespace CerealSquad.EntitySystem
{
    class RiceBowlEnemy : AEnemy
    {
        protected scentMap _scentMap;

        private double _attackCoolDown;

        public RiceBowlEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 2;
            _attackCoolDown = 1; // 5 sec
            _scentMap = new scentMap(room.Size.Height, room.Size.Width);
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("RiceBowlWalking", "Assets/Enemies/Normal/RiceBowlWalking.png");
            Factories.TextureFactory.Instance.load("RiceBowlDying", "Assets/Enemies/Normal/Death/RiceBowlDying.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "RiceBowlWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "RiceBowlWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "RiceBowlWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "RiceBowlWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "RiceBowlWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.DYING, "RiceBowlDying", Enumerable.Range(0, 9).Select(i => (uint)i).ToList(), new Vector2u(128, 128));

            _ressources.CollisionBox = new FloatRect(new Vector2f(21.0f, -10.0f), new Vector2f(21.0f, 20.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(21.0f, 20.0f), new Vector2f(21.0f, 20.0f));
            Pos = Pos; // Very important
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
                    #region EmptyStatement
#pragma warning disable CS0642 // Possible mistaken empty statement
                    ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                #endregion
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

        public override void die()
        {
            if (!Die)
            {
                base.die();
                ressourcesEntity.PlayAnimation((uint)EStateEntity.DYING);
                ressourcesEntity.Loop = false;
            }
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
                    for (int i = 0; i < 15; i += 1)
                    {
                        if (IsInEllipse(Pos.X + deltaX * i, Pos.Y + deltaY * i, ent.HitboxPos.X, ent.Pos.Y, 0.2, 0.2))
                            result = true;
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
            _attackCoolDown = 1;
            new RiceProjectile(_owner, _move[0], HitboxPos);
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
                if (Active)
                {
                    _scentMap.update((WorldEntity)_owner.getOwner(), _room);
                    think(world, deltaTime);
                }
                move(world, deltaTime);
            }
            _ressources.Update(deltaTime);
        }
    }
}
