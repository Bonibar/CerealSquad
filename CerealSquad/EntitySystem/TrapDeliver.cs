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
    class TrapDeliver : Transformable, Drawable
    {
        public enum EStep
        {
            NOTHING             = 0,
            START_SELECTING     = 1,
            SELECTING           = 2,
            END_SELECTING       = 3,
            DELIVER             = 4
        }

        public EStep Step { get; private set; }
        public EMovement Target { get; private set; }
        public bool IsTargetValid { get; private set; }


        private AnimatedSprite Sprite;
        private APlayer Player;
        private Timer Timer = new Timer(Time.FromSeconds(1));

        public Time Cooldown { get { return Timer.Time; } set { Timer.Time = value; } }

        public TrapDeliver(APlayer player)
        {
            Player = player;
            Step = EStep.NOTHING;
            Target = EMovement.Up;
            Factories.TextureFactory.Instance.load("Cursor", "Assets/Effects/Cursor.png");
            Sprite = new AnimatedSprite(64, 64);
            Sprite.addAnimation(EStateEntity.IDLE, "Cursor", new List<uint> { 0, 1, 2, 3 }, new Vector2u(64, 64));
            Sprite.SetColor(Color.Green);
            Sprite.Speed = Time.FromMilliseconds(100);
            IsTargetValid = true;
        }

        public bool IsNotDelivering()
        {
            return Step.Equals(EStep.NOTHING);
        }

        public void Update(SFML.System.Time deltaTime, GameWorld.AWorld world, APlayer.s_input input)
        {
            Processing(input, world);
            Sprite.Update(deltaTime);
        }

        private void Processing(APlayer.s_input input, GameWorld.AWorld world)
        {
            // Player have nothing to put on map.
            if (Player.TrapInventory.Equals(e_TrapType.NONE))
                return;

            // Player can't put on map because of cooldown.
            if (!Timer.IsTimerOver())
                return;

            if (input._isTrapDownPressed && Step == EStep.NOTHING)
                Step = EStep.START_SELECTING;
            else if (!input._isTrapDownPressed && EStep.START_SELECTING == Step)
                Step = EStep.SELECTING;

            if (Step == EStep.START_SELECTING || Step == EStep.SELECTING)
            {
                
                if (input._isRightPressed && !input._isLeftPressed && !input._isDownPressed && !input._isUpPressed)
                    Target = EMovement.Right;
                else if (!input._isRightPressed && input._isLeftPressed && !input._isDownPressed && !input._isUpPressed)
                    Target = EMovement.Left;
                else if (!input._isRightPressed && !input._isLeftPressed && input._isDownPressed && !input._isUpPressed)
                    Target = EMovement.Down;
                else if (!input._isRightPressed && !input._isLeftPressed && !input._isDownPressed && input._isUpPressed)
                    Target = EMovement.Up;

                Vector2i pos = new Vector2i();
                if (Target.Equals(EMovement.Down))
                    pos = new Vector2i(Player.Pos._x, Player.Pos._y + 1);
                else if (Target.Equals(EMovement.Up))
                    pos = new Vector2i(Player.Pos._x, Player.Pos._y - 1);
                else if (Target.Equals(EMovement.Right))
                    pos = new Vector2i(Player.Pos._x + 1, Player.Pos._y);
                else if (Target.Equals(EMovement.Left))
                    pos = new Vector2i(Player.Pos._x - 1, Player.Pos._y);

                Position = new Vector2f(pos.X * 64, pos.Y * 64);
                if (world.getPosition(pos.X, pos.Y) == GameWorld.RoomParser.e_CellType.Wall)
                    IsTargetValid = false;
                else
                    IsTargetValid = true;
                
                Sprite.SetColor((IsTargetValid) ? Color.Green : Color.Red);
            }

            if (input._isTrapDownPressed && Step == EStep.SELECTING && IsTargetValid)
            {
                if (Target != EMovement.None)
                {
                    //TODO check position of futur Trap

                    ATrap trap = Factories.TrapFactory.createTrap(Player, Player.TrapInventory);
                    trap.setPosition(Target);
                    Player.addChild(trap);
                }
                Step = EStep.END_SELECTING;
            }
            else if (!input._isTrapDownPressed && Step == EStep.END_SELECTING)
            {
                Step = EStep.NOTHING;
                // Restart timer to launch cooldown
                Timer.Start();
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (Step > EStep.NOTHING && !Target.Equals(EMovement.None))
            {
                states.Transform *= Transform;
                ((Drawable)Sprite).Draw(target, states);
            }
        }
    }
}
