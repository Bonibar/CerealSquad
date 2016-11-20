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
        public GhostEnemy(IEntity owner, s_position position, ARoom room) : base(owner, position, room)
        {
            _speed = 5;
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
            });


            return result || baseResult;
        }

        public override void think(AWorld world, Time deltatime)
        {
            /*            int left = _scentMap.getScent(_pos._x - 1, _pos._y);
            int right = _scentMap.getScent(_pos._x + 1, _pos._y);
            int top = _scentMap.getScent(_pos._x, _pos._y - 1);
            int bottom = _scentMap.getScent(_pos._x, _pos._y + 1);
            int maxscent = Math.Max(top, Math.Max(bottom, Math.Max(right, left)));

            if (maxscent == 0)
                _move = new List<EMovement> { EMovement.None };
            else if (maxscent == top)
                _move = new List<EMovement> { EMovement.Up };
            else if (maxscent == bottom)
                _move = new List<EMovement> { EMovement.Down };
            else if (maxscent == right)
                _move = new List<EMovement> { EMovement.Right };
            else
                _move = new List<EMovement> { EMovement.Left };*/
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
               // _scentMap.update((WorldEntity)_owner);
                //think();
                move(world, deltaTime);
            }
            _ressources.Update(deltaTime);
        }
    }
}
