using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.GameWorld;
using SFML.System;

namespace CerealSquad.EntitySystem
{
    class RoomEntity : AEntity
    {
        public RoomEntity(IEntity owner, s_size size = default(s_size)) : base(owner, size)
        {
            _type = e_EntityType.World;
            _ressources = new Graphics.EntityResources();
            _ressources.CollisionBox = new SFML.Graphics.FloatRect(0, 0, 0, 0);
        }

        public override void update(Time deltaTime, AWorld world)
        {
            _children.ToList().ForEach(i => i.update(deltaTime, world));
        }
    }
}
