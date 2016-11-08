using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CerealSquad.GameWorld;

namespace CerealSquad.TrapEntities
{
    class Bomb : ATrap
    {
        public Bomb(IEntity owner) : base(owner, e_DamageType.BOMB_DAMAGE, 1)
        {
            Factories.TextureFactory.Instance.load("Bomb", "Assets/Bomb.png");
            Factories.TextureFactory.Instance.load("BombExpl", "Assets/BombExploded.png");

            ressourcesEntity = new Graphics.EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            //((Graphics.AnimatedSprite)_ressources.sprite).addAnimation(Graphics.EStateEntity.DYING, "Bomb", new List<uint> { 0, 1 }, new Vector2u(256, 256));
            ((Graphics.AnimatedSprite)_ressources.sprite).addAnimation(Graphics.EStateEntity.IDLE, "BombExpl", new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, new Vector2u(128, 128));

        }

        public override void update(Time deltaTime, AWorld world)
        {
            ressourcesEntity.Update(deltaTime);
        }

    }
}
