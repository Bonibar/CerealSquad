using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CerealSquad.GameWorld;
using SFML.Graphics;
using CerealSquad.Graphics;
using CerealSquad.Sounds;

namespace CerealSquad.TrapEntities
{
    class Bomb : ATrap
    {
        public static readonly SFML.Graphics.FloatRect COLLISION_BOX = new SFML.Graphics.FloatRect(21, 19, 21, 19);

        public Time TimeRemaining { get { return Timer.Time; } set { Timer.Time = value; } }

        private Timer Timer = new Timer(Time.FromSeconds(10));
        private Timer TimerDelete = new Timer(Time.FromSeconds(0.2f));
        private Timer TimerTrigger = new Timer(Time.FromSeconds(0.2f));

        //public EntityResources SecondaryResourcesEntity { get; set; }

        private uint state = 0;

        public Bomb(IEntity owner) : base(owner, e_DamageType.BOMB_DAMAGE, 2)
        {
            TrapType = e_TrapType.BOMB;
            Cooldown = Time.FromSeconds(0.5f);
            Factories.TextureFactory.Instance.load("Bomb", "Assets/Trap/Bomb.png");
            Factories.TextureFactory.Instance.load("BombExpl", "Assets/Trap/BombExploading.png");

            ressourcesEntity = new EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));
            ressourcesEntity.JukeBox.loadSound("Explosion", "Explosion");

            ressourcesEntity.AddAnimation((uint)Graphics.EStateEntity.IDLE, "Bomb", new List<uint> { 0, 1 }, new Vector2u(128, 128));
            ressourcesEntity.AddAnimation((uint)Graphics.EStateEntity.DYING, "BombExpl", new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, new Vector2u(128, 128), 112);

            ressourcesEntity.secondarySprite.Add(new EllipseShapeSprite(new Vector2f(Range * 64.0f, Range / 2.0f * 64.0f), new Color(219, 176, 10, 100), new Color(219, 130, 10, 255)));
            ressourcesEntity.secondarySprite.ForEach(i => i.Displayed = false);

            ressourcesEntity.CollisionBox = COLLISION_BOX;
            Timer.Start();
            _CollidingType.Add(e_EntityType.ProjectileEnemy);
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Timer.IsTimerOver() && !TimerDelete.Started)
                Trigger();
            else if (TimerDelete.Started && TimerDelete.IsTimerOver())
            {
                Die = true;
                destroy();
            }

            if (Triggered)
            {
                if (TimerTrigger.IsTimerOver() || !TimerTrigger.Started)
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
            ressourcesEntity.PlayAnimation((uint)Graphics.EStateEntity.DYING);
            ((AnimatedSprite)ressourcesEntity.sprite).SetColor(new Color(255, 255, 255, 200));


            ressourcesEntity.secondarySprite.ForEach(i => i.Displayed = true);

            List<AEntity> allEntities = ((WorldEntity)getRootEntity()).GetAllEntities();

            allEntities.ForEach(i =>
            {
                if (!i.Equals(this))
                    i.attemptDamage(this, getDamageType(), Range, Range / 2.0f);
            });
            state++;
            ressourcesEntity.JukeBox.PlaySound("Explosion");
        }

        public override bool attemptDamage(IEntity Sender, e_DamageType damage)
        {

            switch(Sender.getEntityType())
            {
                case e_EntityType.PlayerTrap:
                    Trigger(true);
                    break;
                case e_EntityType.Player:
                case e_EntityType.ProjectileEnemy:
                    Trigger(false);
                    break;
            }

            return true;
        }

        public override void Trigger(bool delay = false)
        {
            Triggered = true;
            if (delay && !TimerTrigger.Started)
                TimerTrigger.Start();
            else if (!delay && TimerTrigger.Started)
                TimerTrigger.Stop();
        }
    }
}
