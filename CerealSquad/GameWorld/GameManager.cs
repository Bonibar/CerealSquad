using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CurrentGame = new Game(_Renderer);
            CurrentGame.GameLoop(_InputManager);
        }
    }
}
