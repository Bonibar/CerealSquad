using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using CerealSquad.GameWorld;
using CerealSquad.Graphics;
using CerealSquad.EntitySystem;

namespace CerealSquad.TrapEntities
{
    class SugarWall : ATrap
    {
        public static readonly FloatRect COLLISION_BOX = new FloatRect(25, 0, 25, 32);
        public static readonly FloatRect HIT_BOX = new FloatRect(25, 30, 25, 32);
        public bool MainWall { get; set; }

        private int NumberSideWall = 5;
        private int WallTriedToPut = 0;
        private EntityTimer Timer = new EntityTimer(Time.FromSeconds(10));
        private EntityTimer TimerPropagation = new EntityTimer(Time.FromSeconds(0.2f));

        public SugarWall(IEntity owner, bool isMainWall = true) : base(owner, e_DamageType.NONE, 0)
        {
            Factories.TextureFactory.Instance.load("SugarWall", "Assets/Trap/SugarWall.png");

            TrapType = e_TrapType.WALL;
            Cooldown = Time.FromSeconds(0.5f);

            ressourcesEntity = new EntityResources();
            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));
            ressourcesEntity.AddAnimation(0, "SugarWall", new List<uint> { 0, 1, 2, 3, 4, 5, 6 }, new Vector2u(64, 64));
            ressourcesEntity.AddAnimation(1, "SugarWall", new List<uint> { 6, 5, 4, 3, 2, 1, 0 }, new Vector2u(64, 64));
            ressourcesEntity.Loop = false;
            ressourcesEntity.JukeBox.loadSound("SugarWall", "SugarWall");

            ressourcesEntity.CollisionBox = COLLISION_BOX;
            ressourcesEntity.HitBox = HIT_BOX;
            ressourcesEntity.PlayAnimation(0);

            Timer.Start();

            MainWall = isMainWall;

            ressourcesEntity.JukeBox.PlaySound("SugarWall");
            _CollidingType.Add(e_EntityType.Player);
            _CollidingType.Add(e_EntityType.Ennemy);
            _CollidingType.Add(e_EntityType.ProjectileEnemy);
            TimerPropagation.Start();
        }

        public override void update(Time deltaTime, AWorld World)
        {
            if (Timer.IsTimerOver() && !Die) {
                Die = true;
                ressourcesEntity.PlayAnimation(1);
                ressourcesEntity.Loop = false;
                ressourcesEntity.JukeBox.PlaySound("SugarWall");
            }

            if (TimerPropagation.IsTimerOver() && !Die && MainWall)
            {
                if (WallTriedToPut < NumberSideWall * 2)
                {
                    Vector2f posOne = new Vector2f();
                    Vector2f posTwo = new Vector2f();
                    float mul = 1f + WallTriedToPut / 2;

                    if (Orientation.Equals(EMovement.Down) || Orientation.Equals(EMovement.Up) || Orientation.Equals(EMovement.None))
                    {
                        posOne = new Vector2f(ressourcesEntity.Position.X - (ressourcesEntity.CollisionBox.Width + 1) * mul, ressourcesEntity.Position.Y);
                        posTwo = new Vector2f(ressourcesEntity.Position.X + (ressourcesEntity.CollisionBox.Width + 1) * mul, ressourcesEntity.Position.Y);
                    }
                    else if (Orientation.Equals(EMovement.Right) || Orientation.Equals(EMovement.Left))
                    {
                        posOne = new Vector2f(ressourcesEntity.Position.X, ressourcesEntity.Position.Y - (ressourcesEntity.CollisionBox.Height + 1) * mul);
                        posTwo = new Vector2f(ressourcesEntity.Position.X, ressourcesEntity.Position.Y + (ressourcesEntity.CollisionBox.Height + 1) * mul);
                    }

                    SugarWall childOne = new SugarWall(this, false);
                    SugarWall childTwo = new SugarWall(this, false);
                    childOne.setPosition(posOne);
                    childTwo.setPosition(posTwo);

                    if (World.IsCollidingWithWall(childOne.ressourcesEntity)
                            || World.WorldEntity.GetCollidingEntities(childOne.ressourcesEntity).Count > 0)
                        removeChild(childOne);
                    if (World.IsCollidingWithWall(childTwo.ressourcesEntity)
                            || World.WorldEntity.GetCollidingEntities(childTwo.ressourcesEntity).Count > 0)
                        removeChild(childTwo);

                    TimerPropagation.Start();
                    WallTriedToPut += 2;
                }
            }

            if (Die)
            {
                if (ressourcesEntity.Pause == true)
                    destroy();
            }

            ressourcesEntity.Update(deltaTime);
            _children.ToList().ForEach(i => i.update(deltaTime, World));
        }

        public override bool IsCollidingEntity(AWorld World, List<AEntity> CollidingEntities)
        {
            bool baseResult = base.IsCollidingEntity(World, CollidingEntities);


            CollidingEntities.ForEach(i =>
            {
                i.attemptDamage(this, _damageType);
            });

            return baseResult;
        }
    }
}
