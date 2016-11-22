using CerealSquad.Graphics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.GameWorld;

namespace CerealSquad.EntitySystem
{
    class GhostEnemy : AEnemy
    {
        //
        // TODO static scentMap
        //
        protected milkyScentMap _scentMap;

        protected class milkyScentMap : scentMap
        {
            public milkyScentMap(uint x, uint y) : base(x, y)
            {
            }

            protected override void check_obstacle(WorldEntity world)
            {
            }
        }

        public GhostEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 2;
            _scentMap = new milkyScentMap(_room.Size.Height, _room.Size.Width);
            ressourcesEntity = new EntityResources();
            Factories.TextureFactory.Instance.load("MilkyGhost", "Assets/Enemies/Normal/MilkyGhost.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "MilkyGhost", new List<uint> { 0 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "MilkyGhost", new List<uint> { 0, 1, 2, 3 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "MilkyGhost", new List<uint> { 12, 13, 14, 15 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "MilkyGhost", new List<uint> { 8, 9, 10, 11 }, new Vector2u(128, 128));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "MilkyGhost", new List<uint> { 4, 5, 6, 7 }, new Vector2u(128, 128), 150);
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.DYING, "MilkyGhost", Enumerable.Range(0, 14).Select(i => (uint)i).ToList(), new Vector2u(128, 128));

            _ressources.CollisionBox = new FloatRect(new Vector2f(17.0f, 0.0f), new Vector2f(17.0f, 24.0f));
            _ressources.HitBox = new FloatRect(new Vector2f(17.0f, 24.0f), new Vector2f(17.0f, 24.0f));
            Pos = Pos; // very important
        }

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool result = false;

            CollidingEntities.ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.PlayerTrap && ((ATrap)i).TrapType != e_TrapType.WALL)
                    die();
                if (i.getEntityType() == e_EntityType.Player)
                    i.die();
            });

            return result;
        }

        public override void think(AWorld world, Time deltaTime)
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

        public override void update(Time deltaTime, AWorld world)
        {
            if (Die)
            {
                if (ressourcesEntity.isFinished())
                    destroy();
            }
            else
            {
                if (active)
                {
                    _scentMap.update((WorldEntity)_owner, _room);
                    think(world, deltaTime);
                }
                move(world, deltaTime);
            }
            _ressources.Update(deltaTime);
        }
    }
}
