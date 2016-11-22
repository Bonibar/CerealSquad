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
    class HalfEggEnemy : AEnemy
    {
        protected scentMap _scentMap;

        private double _invuln;

        public HalfEggEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 4;
            _invuln = 1; // Invulnerability 1sec for fleeing purpose
            _scentMap = new scentMap(room.Size.Height, room.Size.Width);
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("HalfEggyWalking", "Assets/Enemies/Normal/HalfEggyWalking.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "HalfEggyWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "HalfEggyWalking", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "HalfEggyWalking", new List<uint> { 6, 7 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "HalfEggyWalking", new List<uint> { 4, 5 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "HalfEggyWalking", new List<uint> { 2, 3 }, new Vector2u(128, 128));
          //  ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.DYING, "HalfEggyWalking", Enumerable.Range(0, 14).Select(i => (uint)i).ToList(), new Vector2u(128, 128));

            _ressources.CollisionBox = new FloatRect(new Vector2f(17.0f, -5.0f), new Vector2f(17.0f, 13.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(17.0f, 13.0f), new Vector2f(17.0f, 13.0f));
            Pos = position;
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
                int left = executeLeftMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x - 1, pos._y) : -1;
                left = left == -1 ? 1000 : left;
                ressourcesEntity.Position = position;
                int right = executeRightMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x + 1, pos._y) : -1;
                right = right == -1 ? 1000 : right;
                ressourcesEntity.Position = position;
                int top = executeUpMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x, pos._y - 1) : -1;
                top = top == -1 ? 1000 : top;
                ressourcesEntity.Position = position;
                int bottom = executeDownMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x, pos._y + 1) : -1;
                bottom = bottom == -1 ? 1000 : bottom;
                ressourcesEntity.Position = position;
                int here = _scentMap.getScent(pos._x, pos._y);
                here = here == -1 ? 1000 : here;
                int minscent = Math.Min(top, Math.Min(bottom, Math.Min(right, left)));
                _move = new List<EMovement> { EMovement.None };

                if (minscent == 1000 || here <= minscent)
                {
                    _move = new List<EMovement> { EMovement.Down, EMovement.Left, EMovement.Right, EMovement.Up };
                    _move = new List<EMovement> { _move[_rand.Next() % _move.Count] };
                    _r = 10;
                }
                else
                {
                    _move.Remove(EMovement.None);
                    if (minscent == top)
                        _move.Add(EMovement.Up);
                    if (minscent == bottom)
                        _move.Add(EMovement.Down);
                    if (minscent == right)
                        _move.Add(EMovement.Right);
                    if (minscent == left)
                        _move.Add(EMovement.Left);
                    if (_move.Contains(lastMove))
                    {
                        _move = new List<EMovement> { lastMove };
                    }
                    else
                    {
                        _move = new List<EMovement> { _move[_rand.Next() % _move.Count] };
                    }
                }
            }
        }

        public override void die()
        {
            if (_invuln <= 0)
                base.die();
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            _invuln -= deltaTime.AsSeconds();
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
