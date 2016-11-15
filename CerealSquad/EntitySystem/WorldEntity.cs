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
    class WorldEntity : AEntity
    {
        public WorldEntity() : base(null)
        {
        }

        public override void update(SFML.System.Time deltaTime, AWorld world)
        {
            _children.ToList<IEntity>().ForEach(i => i.update(deltaTime, world));
        }

        private void check_death(IEntity i)
        {
            if (i.getEntityType() == e_EntityType.Player && !i.Die)
            {
                _children.ToList<IEntity>().ForEach(j => check_collision(i, j));
            }
        }

        private void check_collision(IEntity i, IEntity j)
        {
            if (j.getEntityType() == e_EntityType.Ennemy && i.Pos._x == j.Pos._x && i.Pos._y == j.Pos._y)
            {
                ((APlayer)i).die();
            }
        }

        private List<AEntity> GetCollidingEntitiesRecursive(IEntity Owner, CircleShape Circle)
        {
            List<AEntity> Tmp = new List<AEntity>();

            Owner.getChildren().ToList<IEntity>().ForEach(i => {
                Tmp = Tmp.Concat(GetCollidingEntitiesRecursive(i, Circle)).ToList();
            });

            if (Owner.getEntityType() != e_EntityType.World)
                if (((AEntity)Owner).ressourcesEntity.IsTouchingHitBox(Circle))
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

            System.Diagnostics.Debug.Write("Recursive : " + Owner.getChildren().ToList<IEntity>().Count + "\n");

            Owner.getChildren().ToList<IEntity>().ForEach(i => {
                System.Diagnostics.Debug.Write("Recursive : " + i.getEntityType() + "\n");
                Tmp = Tmp.Concat(GetCollidingEntitiesRecursive(i, Other)).ToList();
            });

            if (Owner.getEntityType() != e_EntityType.World)
                if (((AEntity)Owner).ressourcesEntity.IsTouchingHitBox(Other) && Other != Owner)
                    Tmp.Add((AEntity)Owner);

            return Tmp;
        }

        public List<AEntity> GetCollidingEntities(EntityResources Other)
        {
            return GetCollidingEntitiesRecursive(this, Other);
        }

        private List<AEntity> GetAllEntitiesRecursive(IEntity Owner)
        {
            List<AEntity> Tmp = new List<AEntity>();

            Owner.getChildren().ToList<IEntity>().ForEach(i => {
                Tmp = Tmp.Concat(GetAllEntitiesRecursive(i)).ToList();
            });

           Tmp.Add((AEntity)Owner);

            return Tmp;
        }

        public List<AEntity> GetAllEntities()
        {
            return GetAllEntitiesRecursive(this);
        }

        private void deepDraw(IEntity owner, Renderer win)
        {
            owner.getChildren().ToList<IEntity>().ForEach(i => deepDraw(i, win));

            // This is ugly.. To change !
            if (owner.getEntityType() == e_EntityType.Player)
                win.Draw(((APlayer)owner).TrapDeliver);

            owner.SecondaryResourcesEntities.OrderBy(v => v.sprite.Level);
            owner.SecondaryResourcesEntities.ForEach(i => win.Draw(i));
            win.Draw(owner.ressourcesEntity);
        }

        public void draw(Renderer win)
        {
            deepDraw(this, win);
        }
    }
}
