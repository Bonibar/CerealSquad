using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using static CerealSquad.APlayer;
using CerealSquad.Graphics;
using CerealSquad.GameWorld;
using CerealSquad.Factories;
using SFML.Graphics;


namespace CerealSquad
{
    class JackEnnemy : AEnemy
    {
        protected static JackEnnemyScentMap _scentMap;

        protected class JackEnnemyScentMap : scentMap
        {
            public JackEnnemyScentMap(uint x, uint y) : base(x, y)
            {
            }

            public override int getScent(int x, int y)
            {
                if (x >= 0 && x < _x && y >= 0 && y < _y)
                {
                    int scent = 0;
                    if (_map[x][y][(int)EName.Tchong] != -1)
                        scent += _map[x][y][(int)EName.Tchong];
                    return (scent);
                }
                return (-1);
            }
        }

        public JackEnnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 3;
            _scentMap = new JackEnnemyScentMap(_room.Size.Height, _room.Size.Width);
            _ressources = new EntityResources();
            Factories.TextureFactory.Instance.load("JackHunter", "Assets/Character/JackHunter.png");
            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));          
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.IDLE, "JackHunter", new List<uint> { 0, 1, 2 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_DOWN, "JackHunter", new List<uint> { 0, 1, 2 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_LEFT, "JackHunter", new List<uint> { 3, 4, 5 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_RIGHT, "JackHunter", new List<uint> { 6, 7, 8 }, new Vector2u(64, 64));
            ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.WALKING_UP, "JackHunter", new List<uint> { 9, 10, 11 }, new Vector2u(64, 64));
            // ((AnimatedSprite)_ressources.sprite).addAnimation((uint)EStateEntity.DYING, "JackHunter", new List<uint> { 12, 13, 14 }, new Vector2u(64, 64));

            _ressources.CollisionBox = new FloatRect(new Vector2f(28.0f, 0.0f), new Vector2f(26.0f, 24.0f));
            Pos = position;
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage, float Range)
        {
            double Distance = Math.Sqrt(Math.Pow(Sender.Pos._trueX - Pos._trueX, 2.0f) + Math.Pow(Sender.Pos._trueY - Pos._trueY, 2.0f));
            if (ressourcesEntity != null)
                Distance -= ressourcesEntity.HitBox.Width / 64.0f / 2.0f;

            if (Distance > Range)
                return false;

            die();

            return true;
        }

        //
        // TODO check egality in scent
        //
        public override void think(AWorld world, Time deltaTime)
        {
            s_position pos = getCoord(HitboxPos);
            var position = ressourcesEntity.Position;

            _move = new List<EMovement> { EMovement.Up, EMovement.Down, EMovement.Right, EMovement.Left };
            int left = executeLeftMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x - 1, pos._y) : 0;
            ressourcesEntity.Position = position;
            int right = executeRightMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x + 1, pos._y): 0;
            ressourcesEntity.Position = position;
            int top = executeUpMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x, pos._y - 1) : 0;
            ressourcesEntity.Position = position;
            int bottom = executeDownMove(world, Speed * deltaTime.AsSeconds()) ? _scentMap.getScent(pos._x, pos._y + 1) : 0;
            ressourcesEntity.Position = position;
            int here = _scentMap.getScent(pos._x, pos._y);
            int maxscent = Math.Max(top, Math.Max(bottom, Math.Max(right, left)));

            if (maxscent == 0 && here == 0)
                _move = new List<EMovement> { EMovement.None };
            else if (maxscent <= here && moveSameTile((WorldEntity)_owner))
            {
                move(world, deltaTime)
                #region EmptyStatement
#pragma warning disable CS0642 // Possible mistaken empty statement
                ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                #endregion
            }
            else if (maxscent == top)
                _move = new List<EMovement> { EMovement.Up };
            else if (maxscent == bottom)
                _move = new List<EMovement> { EMovement.Down };
            else if (maxscent == right)
                _move = new List<EMovement> { EMovement.Right };
            else if (maxscent == left)
                _move = new List<EMovement> { EMovement.Left };
                
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
                _scentMap.update((WorldEntity)_owner, _room);
                think(world, deltaTime);
                move(world, deltaTime);
            }
            _ressources.Update(deltaTime);
        }

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool baseResult = base.IsCollidingEntity(World, CollidingEntities);
            bool result = false;

            CollidingEntities.ForEach(i =>
            {
                if (i.getEntityType() == e_EntityType.PlayerTrap)
                    Die = true;
            });


            return result || baseResult;
        }
    }
}