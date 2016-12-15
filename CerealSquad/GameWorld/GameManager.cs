using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CerealSquad.Sounds;

namespace CerealSquad.GameWorld
{
    class GameManager
    {
        public enum e_GameSlot
        {
            Slot1,
            Slot2,
            Slot3
        }

        public Game CurrentGame { get; private set; }
        private Renderer _Renderer;
        private InputManager.InputManager _InputManager;

        public GameManager(Renderer renderer, InputManager.InputManager inputManager)
        {
            if (renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");
            if (inputManager == null)
                throw new ArgumentNullException("Input Manager cannot be null");
            _Renderer = renderer;
            _InputManager = inputManager;
        }

        public void newGame()
        {
            System.Diagnostics.Debug.WriteLine("New GAME");
            CurrentGame = new Game(_Renderer, _InputManager);
        }

        public void Update(SFML.System.Time DeltaTime)
        {
            if (CurrentGame != null)
            {
                if (CurrentGame.State == Game.GameState.Running)
                    CurrentGame.Update(DeltaTime);
                else if (CurrentGame.State == Game.GameState.Exit)
                {
                    //CurrentGame.WorldEntity.destroy();
                    CurrentGame = null;
                }
            }
        }
    }
}
