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

        public EStep Step { get; private set; }
        public EMovement Target { get; private set; }
        public bool IsTargetValid { get; private set; }


        private EntityResources ResourcesEntity = new EntityResources();
        //private AnimatedSprite Sprite;
        private APlayer Player;
        private Timer Timer = new Timer(Time.FromSeconds(1));

        public Time Cooldown { get { return Timer.Time; } set { Timer.Time = value; } }

        public TrapDeliver(APlayer player)
        {
            Player = player;
            Step = EStep.NOTHING;
            Target = EMovement.Up;
            Factories.TextureFactory.Instance.load("Cursor", "Assets/Effects/Cursor.png");
            ResourcesEntity.InitializationAnimatedSprite(new Vector2u(64, 64));
            //Sprite = new AnimatedSprite(64, 64);
            ((AnimatedSprite)ResourcesEntity.sprite).addAnimation(EStateEntity.IDLE, "Cursor", new List<uint> { 0, 1, 2, 3 }, new Vector2u(64, 64));
            ((AnimatedSprite)ResourcesEntity.sprite).SetColor(Color.Green);
            ((AnimatedSprite)ResourcesEntity.sprite).Speed = Time.FromMilliseconds(100);

            IsTargetValid = true;
        }

        public bool IsNotDelivering()
        {
            return Step.Equals(EStep.NOTHING);
        }

        public void Update(SFML.System.Time DeltaTime, GameWorld.AWorld World, List<EMovement> Input, bool TrapPressed)
        {
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
                Step = EStep.START_SELECTING;

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
                if (!Target.Equals(EMovement.None) && Timer.IsTimerOver())
                {
                    if (World.IsCollidingWithWall(ResourcesEntity)
                        || World.WorldEntity.GetCollidingEntity(ResourcesEntity).Count > 0)
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
                    ATrap trap = Factories.TrapFactory.CreateTrap(Player, Player.TrapInventory);
                    trap.setPosition(Target);
                    Player.addChild(trap);
                }
                Step = EStep.END_SELECTING;
            }
            else if (!TrapPressed && Step == EStep.END_SELECTING)
            {
                Step = EStep.NOTHING;
                // Restart timer to launch cooldown
                if (IsTargetValid)
                    Timer.Start();
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
