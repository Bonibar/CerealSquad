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

        public override void update(SFML.System.Time deltaTime)
        {
            _children.ToList<IEntity>().ForEach(i => check_death(i));
            _children.ToList<IEntity>().ForEach(i => i.update(deltaTime));
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

        private void deepDraw(IEntity owner, Renderer win)
        {
            if (owner.getChildren().Count == 0)
            {
                win.Draw(owner.ressourcesEntity);
                return;
            }
            owner.getChildren().ToList<IEntity>().ForEach(i => deepDraw(i, win)); //i => win.Draw(i.ressourcesEntity)
            win.Draw(owner.ressourcesEntity);
        }

        public void draw(Renderer win)
        {
            deepDraw(this, win);
        }
    }
}
