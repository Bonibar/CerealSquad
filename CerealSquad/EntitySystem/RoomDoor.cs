using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.GameWorld;
using SFML.System;

namespace CerealSquad.EntitySystem
{
    class RoomDoor : AEntity
    {
        public RoomDoor(IEntity owner, s_position position, ARoom room) : base(owner)
        {
            _ressources = new Graphics.EntityResources();

            Factories.TextureFactory.Instance.load("RoomDoor", "Assets/GameplayElement/Fire.png");

            _ressources.InitializationAnimatedSprite(new Vector2u(64, 64));
            _ressources.AddAnimation(0, "RoomDoor", new List<uint> { 0, 1 }, new Vector2u(128, 128), 200);

            _type = e_EntityType.Room;

            _ressources.CollisionBox = new SFML.Graphics.FloatRect(32, 32, 32, 32);
            _ressources.HitBox = new SFML.Graphics.FloatRect(32, 32, 32, 32);

            _ressources.Loop = true;

            _pos = position + new s_position(room.Position.X, room.Position.Y);
            Pos = Pos;
            _CollidingType.Add(e_EntityType.Player);
            _CollidingType.Add(e_EntityType.Ennemy);
            _CollidingType.Add(e_EntityType.ProjectileEnemy);
            _CollidingType.Add(e_EntityType.ProjectilePlayer);
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Die)
                destroy();
            else
                _ressources.Update(deltaTime);
        }
    }
}
