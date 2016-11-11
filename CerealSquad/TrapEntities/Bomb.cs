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
        public Time Cooldown { get { return Timer.Time; } set { Timer.Time = value; } }
        private Timer Timer = new Timer(Time.FromSeconds(7));
        private Timer TimerDelete = new Timer(Time.FromSeconds(0.5f));

        public Bomb(IEntity owner) : base(owner, e_DamageType.BOMB_DAMAGE, 1)
        {
            TrapType = e_TrapType.BOMB;
            Factories.TextureFactory.Instance.load("Bomb", "Assets/Trap/Bomb.png");
            Factories.TextureFactory.Instance.load("BombExpl", "Assets/Trap/BombExploading.png");

            ressourcesEntity = new Graphics.EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            ((Graphics.AnimatedSprite)_ressources.sprite).addAnimation(Graphics.EStateEntity.IDLE, "Bomb", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ((Graphics.AnimatedSprite)_ressources.sprite).addAnimation(Graphics.EStateEntity.DYING, "BombExpl", new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, new Vector2u(128, 128), 112);

            Timer.Start();
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Timer.IsTimerOver() && !TimerDelete.Started)
            {
                TimerDelete.Start();
                ressourcesEntity.PlayAnimation(Graphics.EStateEntity.DYING);
            } 
            else if (TimerDelete.Started && TimerDelete.IsTimerOver())
            {
                // SHOULD BE GONE
                getOwner().removeChild(this);
                Die = true;
            }

            ressourcesEntity.Update(deltaTime);
        }

    }
}
