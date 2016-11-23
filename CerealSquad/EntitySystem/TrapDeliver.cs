using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CerealSquad.AEntity;
using CerealSquad.Graphics;
using SFML.System;
using SFML.Graphics;

namespace CerealSquad.EntitySystem
{
    class TrapDeliver : Drawable
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

        private EntityResources ResourcesEntity = new EntityResources();

        private APlayer Player;
        private Timer TimerCoolDown = new Timer(Time.Zero);
        private Timer TimerToPut;

        public TrapDeliver(APlayer player)
        {
            Player = player;
            Step = EStep.NOTHING;
            Target = EMovement.Up;
            Factories.TextureFactory.Instance.load("Cursor", "Assets/Effects/Cursor.png");
            Factories.TextureFactory.Instance.load("ConstructionCloud", "Assets/GameplayElement/ConstructionCloud.png");

            ResourcesEntity.JukeBox.loadSound("Construction", "Construction");

            ResourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));

            ResourcesEntity.AddAnimation((uint)EAnimation.CURSOR, "Cursor", new List<uint> { 0, 1, 2, 3 }, new Vector2u(64, 64));
            ResourcesEntity.AddAnimation((uint)EAnimation.CONSTRUCTION, "ConstructionCloud", new List<uint> { 0, 1, 2, 3 }, new Vector2u(128, 128), 100);

            IsTargetValid = true;
        }

        public bool IsDelivering()
        {
            return !Step.Equals(EStep.NOTHING);
        }

        public void Cancel()
        {
            Step = EStep.NOTHING;
        }

        public void Update(Time DeltaTime, GameWorld.AWorld World, List<EMovement> Input, bool TrapPressed)
        {
            if (TimerToPut == null)
                TimerToPut = new Timer(Time.FromSeconds(0.4f * World.WorldEntity.PlayerNumber));
            Processing(Input, World, TrapPressed);
            ResourcesEntity.Update(DeltaTime);
        }

        private void Processing(List<EMovement> Input, GameWorld.AWorld World, bool TrapPressed)
        {
            // Player have nothing to put on map.
            if (Player.TrapInventory.Equals(e_TrapType.NONE))
                return;


            ResourcesEntity.CollisionBox = Factories.TrapFactory.GetCollisionBox(Player.TrapInventory);

            if (TrapPressed && Step == EStep.NOTHING)
            {
                ResourcesEntity.PlayAnimation((uint)EAnimation.CURSOR);
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

                ResourcesEntity.Position = pos;
                // CHECK 4 points
                if (!Target.Equals(EMovement.None) && TimerCoolDown.IsTimerOver())
                {
                    if (World.IsCollidingWithWall(ResourcesEntity)
                        || World.WorldEntity.GetCollidingEntities(ResourcesEntity).Count > 0)
                        IsTargetValid = false;
                    else
                        IsTargetValid = true;
                }
                else
                    IsTargetValid = false;

                ((AnimatedSprite)ResourcesEntity.sprite).SetColor((IsTargetValid) ? Color.Green : Color.Red);
            }

            if (!TrapPressed && Step == EStep.START_SELECTING)
            {
                if (IsTargetValid)
                {
                    ((AnimatedSprite)ResourcesEntity.sprite).SetColor(Color.White);
                    ResourcesEntity.PlayAnimation((uint)EAnimation.CONSTRUCTION);
                    TimerToPut.Start();
                    ResourcesEntity.JukeBox.PlaySound("Construction");
                }
                Step = EStep.END_SELECTING;
            }
            else if (!TrapPressed && Step == EStep.END_SELECTING && TimerToPut.IsTimerOver())
            {
                if (IsTargetValid)
                {
                    ATrap trap = Factories.TrapFactory.CreateTrap(Player, Player.TrapInventory);
                    trap.setPosition(Target);
                    Player.addChild(trap);
                    TimerCoolDown = new Timer(trap.Cooldown);
                    TimerCoolDown.Start();
                    ResourcesEntity.JukeBox.StopSound("Construction");
                }
                Step = EStep.NOTHING;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (Step > EStep.NOTHING)
            {
                ResourcesEntity.Draw(target, states);
            }
        }
    }
}
