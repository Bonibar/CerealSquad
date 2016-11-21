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
        protected scentMap _scentMap;

        //
        // Need because otherwise the children will have the same seed for the random
        // Other way use a random master who will decide of each seed of the random
        //
        private int _child;

        public EggEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _child = 1;
            _speed = 1.7f;
            _scentMap = new scentMap(_room.Size.Height, _room.Size.Width);
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
            Pos = Pos; //very important
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

        public override void die()
        {
            if (!_die)
            {
                _die = true;
            }
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Die)
            {
                if (ressourcesEntity.isFinished())
                {
                    _owner.addChild(new HalfEggEnemy(_owner, new s_position(Pos._trueX - _room.Position.X, Pos._trueY - _room.Position.Y), _room));
                    if (_child == 0)
                        destroy();
                    _child -= 1;
                }
                _ressources.PlayAnimation((uint)EStateEntity.DYING);
                _ressources.Loop = false;
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
