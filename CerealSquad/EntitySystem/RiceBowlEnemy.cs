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
    class RiceBowlEnemy : AEnemy
    {
        protected scentMap _scentMap;

        private double _attackCoolDown;

        public RiceBowlEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 2;
            _attackCoolDown = 5; // 5 sec
            _scentMap = new scentMap(room.Size.Height, room.Size.Width);
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("RiceBowlWalking", "Assets/Enemies/Normal/RiceBowlWalking.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "RiceBowlWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "RiceBowlWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "RiceBowlWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "RiceBowlWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "RiceBowlWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
            //  ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.DYING, "HalfEggyWalking", Enumerable.Range(0, 14).Select(i => (uint)i).ToList(), new Vector2u(128, 128));

            _ressources.CollisionBox = new FloatRect(new Vector2f(21.0f, -10.0f), new Vector2f(21.0f, 20.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(21.0f, 20.0f), new Vector2f(21.0f, 20.0f));
            Pos = position;
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
                    if (_move.Count > 1)
                        _move.Remove(EMovement.None);
                    if (_move.Count > 1)
                        _r = 10;
                    _move = new List<EMovement> { _move[_rand.Next() % _move.Count] };
                }
            }
        }

        private bool canAttack(WorldEntity world)
        {
            double deltaX = 0;
            double deltaY = 0;
            bool result = false;
            bool end = false;

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
            for (int i = 0; i < 30; i += 1)
            {
                world.GetAllEntities().ForEach(ent =>
                {
                    if (IsInEllipse(Pos._trueX + deltaX * i, Pos._trueY + deltaY * i, ent.Pos._trueX, ent.Pos._trueY, 0.5, 0.5))
                        result = true;
                    if (_room.getPosition((uint)(Pos._trueX + deltaX * i), (uint)(Pos._trueY + deltaY * i)) == RoomParser.e_CellType.Wall)
                        end = true;
                });
                if (result || end)
                    break;
            }
            return (false);
        }

        public override void attack()
        {
            _attackCoolDown = 5;
            //
            // TODO Alpha implement
            //
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
    }
}
