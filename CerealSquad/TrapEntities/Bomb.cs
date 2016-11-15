using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CerealSquad.GameWorld;
using SFML.Graphics;
using CerealSquad.Graphics;

namespace CerealSquad.TrapEntities
{
    class Bomb : ATrap
    {
        public static readonly SFML.Graphics.FloatRect COLLISION_BOX = new SFML.Graphics.FloatRect(12, 12, 12, 12);

        public Time Cooldown { get { return Timer.Time; } set { Timer.Time = value; } }
        private Timer Timer = new Timer(Time.FromSeconds(10));
        private Timer TimerDelete = new Timer(Time.FromSeconds(0.2f));
        private Timer TimerTrigger = new Timer(Time.FromSeconds(0.2f));

        private uint state = 0;

        public Bomb(IEntity owner) : base(owner, e_DamageType.BOMB_DAMAGE, 2)
        {
            TrapType = e_TrapType.BOMB;
            Factories.TextureFactory.Instance.load("Bomb", "Assets/Trap/Bomb.png");
            Factories.TextureFactory.Instance.load("BombExpl", "Assets/Trap/BombExploading.png");
            Factories.TextureFactory.Instance.load("MegaExpl", "Assets/GameplayElement/BombSphereExploading.png");

            ressourcesEntity = new EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));
            
            ressourcesEntity.AddAnimation((uint)Graphics.EStateEntity.IDLE, "Bomb", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)Graphics.EStateEntity.DYING, "BombExpl", new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, new Vector2u(128, 128), 112);
           
            EntityResources secondary = new EntityResources();
            secondary.sprite = new EllipseShapeSprite(new Vector2f(Range * 64.0f, Range / 2.0f * 64.0f), new Color(219, 176, 10, 100), new Color(219, 130, 10, 255));
            secondary.sprite.Displayed = false;

            SecondaryResourcesEntities.Add(secondary);

            ressourcesEntity.CollisionBox = COLLISION_BOX;
            Timer.Start();
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Timer.IsTimerOver() && !TimerDelete.Started)
                Trigger();
            else if (TimerDelete.Started && TimerDelete.IsTimerOver())
            {
                // SHOULD BE GONE
                getOwner().removeChild(this);
                Die = true;
            }

            if (Triggered)
            {
                if (TimerTrigger.IsTimerOver())
                {
                    if (state == 0)
                        StartExplosion();
                    else
                    {
                        float speed = 500f * deltaTime.AsSeconds();
                        if (ressourcesEntity.Size.X + speed < 64.0f * Range)
                            ressourcesEntity.Size = new Vector2f(ressourcesEntity.Size.X + speed, ressourcesEntity.Size.Y + speed);
                    }
                }
            }

            ressourcesEntity.Update(deltaTime);
        }

        private void StartExplosion()
        {
            TimerDelete.Start();
            ressourcesEntity.PlayAnimation(Graphics.EStateEntity.DYING);
            ((AnimatedSprite)ressourcesEntity.sprite).SetColor(new Color(255, 255, 255, 200));

            SecondaryResourcesEntities.ForEach(i =>
            {
                i.sprite.Displayed = true;
                i.Position = ressourcesEntity.Position;
            });

            List<AEntity> allEntities = ((WorldEntity)getRootEntity()).GetAllEntities();

            allEntities.ForEach(i =>
            {
                if (!i.Equals(this))
                    i.attemptDamage(this, getDamageType(), Range - (ressourcesEntity.HitBox.Width / 2.0f / 64.0f));
            });
            state++;
        }

        public override void Trigger()
        {
            Triggered = true;
            if (!TimerTrigger.Started)
                TimerTrigger.Start();
        }
    }
}
