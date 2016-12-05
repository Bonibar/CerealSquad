using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CerealSquad.EntitySystem.AEntity;
using CerealSquad.Graphics;
using SFML.System;
using SFML.Graphics;
using CerealSquad.GameWorld;

namespace CerealSquad.EntitySystem
{
    class TrapDeliver : AEntity
    {
        public enum EStep
        {
            NOTHING             = 0,
            START_SELECTING     = 1,
            END_SELECTING       = 3,
            DELIVER             = 4
        }

        private enum EAnimation
        {
            CURSOR = 0,
            CONSTRUCTION
        }

        public EStep Step { get; private set; }
        public EMovement Target { get; private set; }
        public bool IsTargetValid { get; private set; }
        public float MaxCooldownTrap { get { return TimerCoolDown.Time.AsMilliseconds(); } }
        public float CurrentCooldownTrap { get { return TimerCoolDown.Current.AsMilliseconds(); } }

        //private EntityResources ressourcesEntity = new EntityResources();

        private APlayer Player;
        private EntityTimer TimerCoolDown = new EntityTimer(Time.FromMilliseconds(200));
        private EntityTimer TimerToPut;

        private e_TrapType InventoryTampon = e_TrapType.NONE;

        public TrapDeliver(IEntity owner, APlayer player) : base(owner)
        {
            Player = player;
            Step = EStep.NOTHING;
            Target = EMovement.Up;
            _type = e_EntityType.DeliverCloud;
            Factories.TextureFactory.Instance.load("Cursor", "Assets/Effects/Cursor.png");
            Factories.TextureFactory.Instance.load("ConstructionCloud", "Assets/GameplayElement/ConstructionCloud.png");
            ressourcesEntity = new EntityResources();

            ressourcesEntity.JukeBox.loadSound("Construction", "Construction");

            ressourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            ressourcesEntity.AddAnimation((uint)EAnimation.CURSOR, "Cursor", new List<uint> { 0, 1, 2, 3 }, new Vector2u(64, 64));
            ressourcesEntity.AddAnimation((uint)EAnimation.CONSTRUCTION, "ConstructionCloud", new List<uint> { 0, 1, 2, 3 }, new Vector2u(128, 128), 100);
            ((AnimatedSprite)ressourcesEntity.sprite).Displayed = false;

            IsTargetValid = true;
            _CollidingType.Add(e_EntityType.Player);
            _CollidingType.Add(e_EntityType.Ennemy);

            _ressources.CollisionBox = new FloatRect(21f, 0, 17f, 24f);
        }

        public bool IsDelivering()
        {
            return Step.Equals(EStep.END_SELECTING);
        }

        public void Cancel()
        {
            Step = EStep.NOTHING;
        }

        public override void update(Time deltaTime, AWorld world)
        {
            if (Step > EStep.NOTHING)
            {
                ((AnimatedSprite)ressourcesEntity.sprite).Displayed = true;
                ressourcesEntity.EnableCollision = true;
                ressourcesEntity.Update(deltaTime);
            }
            else
            {
                ((AnimatedSprite)ressourcesEntity.sprite).Displayed = false;
                ressourcesEntity.EnableCollision = false;
            }
            
            System.Diagnostics.Debug.WriteLine("POSITION" + ressourcesEntity.Position);
        }

        public void Update(GameWorld.AWorld World, List<EMovement> Input, bool TrapPressed)
        {
            if (TimerToPut == null)
                TimerToPut = new EntityTimer(Time.FromSeconds(0.4f * World.WorldEntity.PlayerNumber));
            Processing(Input, World, TrapPressed);
        }

        private void Processing(List<EMovement> Input, GameWorld.AWorld World, bool TrapPressed)
        {
            // Player have nothing to put on map.
            if (Player.TrapInventory.Equals(e_TrapType.NONE))
                return;


            ressourcesEntity.CollisionBox = Factories.TrapFactory.GetCollisionBox(Player.TrapInventory);

            if (TrapPressed && Step == EStep.NOTHING)
            {
                ressourcesEntity.PlayAnimation((uint)EAnimation.CURSOR);
                Step = EStep.START_SELECTING;
            }

            if (Step == EStep.START_SELECTING)
            {
                Target = (Input.Count > 0) ? Input.ElementAt(Input.Count - 1) : EMovement.None;

                Vector2f pos = new Vector2f();
                if (Target.Equals(EMovement.Down))
                    pos = new Vector2f(Player.ressourcesEntity.Position.X, Player.ressourcesEntity.Position.Y + Player.ressourcesEntity.sprite.Size.Y);
                else if (Target.Equals(EMovement.Up))
                    pos = new Vector2f(Player.ressourcesEntity.Position.X, Player.ressourcesEntity.Position.Y - Player.ressourcesEntity.sprite.Size.Y);
                else if (Target.Equals(EMovement.Right))
                    pos = new Vector2f(Player.ressourcesEntity.Position.X + Player.ressourcesEntity.sprite.Size.X, Player.ressourcesEntity.Position.Y);
                else if (Target.Equals(EMovement.Left))
                    pos = new Vector2f(Player.ressourcesEntity.Position.X - Player.ressourcesEntity.sprite.Size.X, Player.ressourcesEntity.Position.Y);
                else
                    pos = new Vector2f(Player.ressourcesEntity.Position.X, Player.ressourcesEntity.Position.Y);

                ressourcesEntity.Position = pos;
                Vector2f size = Player.ressourcesEntity.sprite.Size;
                Pos = new s_position((pos.X - size.X / 2.0f) / 64.0f, (pos.Y - size.Y / 2.0f) / 64.0f);
                // CHECK 4 points
                if (!Target.Equals(EMovement.None) && TimerCoolDown.IsTimerOver())
                {
                    if (World.IsCollidingWithWall(ressourcesEntity)
                        || World.WorldEntity.GetCollidingEntities(ressourcesEntity).Count > 0)
                        IsTargetValid = false;
                    else
                        IsTargetValid = true;
                }
                else
                    IsTargetValid = false;

                ((AnimatedSprite)ressourcesEntity.sprite).SetColor((IsTargetValid) ? Color.Green : Color.Red);
            }

            if (!TrapPressed && Step == EStep.START_SELECTING)
            {
                if (IsTargetValid)
                {
                    ((AnimatedSprite)ressourcesEntity.sprite).SetColor(Color.White);
                    ressourcesEntity.PlayAnimation((uint)EAnimation.CONSTRUCTION);
                    TimerToPut.Start();
                    ressourcesEntity.JukeBox.PlaySound("Construction");
                    InventoryTampon = Player.TrapInventory;
                }
                Step = EStep.END_SELECTING;
            }
            else if (!TrapPressed && Step == EStep.END_SELECTING && TimerToPut.IsTimerOver())
            {
                if (IsTargetValid)
                {
                    ATrap trap = Factories.TrapFactory.CreateTrap(Player, InventoryTampon);
                    trap.setPosition(ressourcesEntity.Position);
                    TimerCoolDown = new EntityTimer(trap.Cooldown);
                    TimerCoolDown.Start();
                    ressourcesEntity.JukeBox.StopSound("Construction");
                }
                Step = EStep.NOTHING;
            }
        }
    }
}
