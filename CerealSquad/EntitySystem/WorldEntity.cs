using CerealSquad.GameWorld;
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
            _children.ToList<IEntity>().ForEach(i => check_death(i));
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

        public void draw(Renderer win)
        {
            _children.ToList<IEntity>().ForEach(i => win.Draw(i.ressourcesEntity));
        }
    }
}
