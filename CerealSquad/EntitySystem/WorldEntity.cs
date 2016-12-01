using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class WorldEntity : AEntity, Drawable
    {
        public int PlayerNumber { get; set; }

        private List<AEntity> allEntities;

        public WorldEntity() : base(null)
        {
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            _children.ToList().ForEach(i => i.update(deltaTime, world));
            //_children = _children.ToList().OrderBy(x => x.ressourcesEntity.CollisionBox.Height + x.ressourcesEntity.CollisionBox.Top).ToList();

            // order by hitbox.y position
            allEntities = GetAllEntities().OrderBy(entity => entity.ressourcesEntity.HitBox.Height + entity.ressourcesEntity.HitBox.Top).ToList();
        }

        private List<AEntity> GetCollidingEntitiesRecursive(IEntity Owner, CircleShape Circle)
        {
            List<AEntity> Tmp = new List<AEntity>();

            Owner.getChildren().ToList<IEntity>().ForEach(i => {
                Tmp = Tmp.Concat(GetCollidingEntitiesRecursive(i, Circle)).ToList();
            });

            if (Owner.getEntityType() != e_EntityType.World)
                if (((AEntity)Owner).ressourcesEntity.IsTouchingCollisionBox(Circle))
                    Tmp.Add((AEntity)Owner);

            return Tmp;
        }

        public List<AEntity> GetCollidingEntities(CircleShape Circle)
        {
            return GetCollidingEntitiesRecursive(this, Circle);
        }

        private List<AEntity> GetCollidingEntitiesRecursive(IEntity Owner, EntityResources Other)
        {
            List<AEntity> Tmp = new List<AEntity>();

            Owner.getChildren().ToList<IEntity>().ForEach(i => {
                Tmp = Tmp.Concat(GetCollidingEntitiesRecursive(i, Other)).ToList();
            });

            if (Owner.getEntityType() != e_EntityType.World)
                if (((AEntity)Owner).ressourcesEntity.IsTouchingCollisionBox(Other) && Other != Owner.ressourcesEntity)
                    Tmp.Add((AEntity)Owner);

            return Tmp;
        }

        private List<AEntity> GetTouchingEntitiesRecursive(IEntity Owner, EntityResources Other)
        {
            List<AEntity> Tmp = new List<AEntity>();

            Owner.getChildren().ToList<IEntity>().ForEach(i => {
                Tmp = Tmp.Concat(GetTouchingEntitiesRecursive(i, Other)).ToList();
            });

            if (Owner.getEntityType() != e_EntityType.World)
                if (((AEntity)Owner).ressourcesEntity.IsTouchingHitBox(Other) && Other != Owner.ressourcesEntity)
                    Tmp.Add((AEntity)Owner);

            return Tmp;
        }

        public List<AEntity> GetCollidingEntities(EntityResources Other)
        {
            return GetCollidingEntitiesRecursive(this, Other);
        }

        public List<AEntity> GetTouchingEntities(EntityResources Other)
        {
            return GetTouchingEntitiesRecursive(this, Other);
        }

        private List<AEntity> GetAllEntitiesRecursive(IEntity Owner)
        {
            List<AEntity> Tmp = new List<AEntity>();

            Owner.getChildren().ToList<IEntity>().ForEach(i => {
                Tmp = Tmp.Concat(GetAllEntitiesRecursive(i)).ToList();
            });

            if (Owner.getEntityType() != e_EntityType.World)
                Tmp.Add((AEntity)Owner);

            return Tmp;
        }

        public List<AEntity> GetAllEntities()
        {
            return GetAllEntitiesRecursive(this);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            allEntities?.ForEach(entity => {
                if (entity.getEntityType() == e_EntityType.Player)
                    target.Draw(((APlayer)entity).TrapDeliver, states);
                target.Draw(entity.ressourcesEntity, states);
            });

        }
    }
}
