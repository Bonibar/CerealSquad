﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.GameWorld
{
    /// <summary>
    /// 
    /// </summary>
    class Game
    {
        private AWorld currentWorld = null;
        public AWorld CurrentWorld {
            get { return currentWorld; }
            set { setCurrentWorld(value); }
        }
        //private HUD HUD;
        private List<AWorld> Worlds = new List<AWorld>();
        private List<IEntity> Players = new List<IEntity>();
        private WorldEntity worldEntity = new WorldEntity();
        public WorldEntity WorldEntity
        {
            get { return worldEntity; }
        }
        private Renderer renderer = null;
        private InputManager.InputManager _InputManager = null;

        public Game(Renderer _renderer, InputManager.InputManager manager)
        {
            if (_renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");
            if (manager == null)
                throw new ArgumentNullException("Input Manager cannot be null");

            renderer = _renderer;
            _InputManager = manager;

            characterSelection();
        }

        private void characterSelection()
        {
            Menus.CharacterSelectMenu _current = new Menus.CharacterSelectMenu(renderer, _InputManager);
            Menus.MenuManager.Instance.AddMenu(_current);
            _current.GameStart += _current_GameStart;
        }

        private void _current_GameStart(object source, Menus.CharacterSelectMenu.CharacterSelectionArgs e)
        {
            CurrentWorld = new AWorld("Maps/TestWorld.txt", worldEntity);
            Worlds.Add(CurrentWorld);

            foreach (Menus.Players.Player player in e.Players.Where(i => i.Type != Menus.Players.Type.Undefined))
            {
                switch (player.Selection)
                {
                    case 0:
                        Players.Add(new Mike(worldEntity, new s_position(5, 6, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId));
                        break;
                    case 1:
                        Players.Add(new Jack(worldEntity, new s_position(5, 6, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId));
                        break;
                    case 2:
                        Players.Add(new Orangina(worldEntity, new s_position(5, 6, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId));
                        break;
                    case 3:
                        Players.Add(new Tchong(worldEntity, new s_position(6, 6, 1), _InputManager, (int)player.Type, player.Type == 0 ? (int)player.KeyboardId : (int)player.ControllerId));
                        break;
                }
            }

            _InputManager.KeyboardKeyPressed += Im_KeyboardKeyPressed;
        }

        public void GameLoop(InputManager.InputManager im)
        {
            //CurrentWorld = new AWorld("Maps/TestWorld.txt", worldEntity);
            //Players.Add(new Orangina(WorldEntity, new s_position(2, 2, 1), im));
            //Players.Add(new Jack(WorldEntity, new s_position(5, 6, 1), im));
            //Players.Add(new Tchong(WorldEntity, new s_position(2, 4, 1), im));
            //new Ennemy(WorldEntity, new s_position(10, 10, 1));
            //new JackEnnemy(WorldEntity, new s_position(2, 10, 1));

            //im.KeyboardKeyPressed += Im_KeyboardKeyPressed;
        }

        private void Im_KeyboardKeyPressed(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (!Menus.MenuManager.Instance.isDisplayed()) // Are we on game
            {
                if (e.KeyCode == InputManager.Keyboard.Key.Escape)
                    renderer.Win.Close();
            }
        }

        public void AddWorld(AWorld World)
        {
            if (World == null)
                throw new ArgumentNullException("World cannot be null");
            Worlds.Add(World);
        }

        public void goToNextWorld()
        {
            var index = Worlds.FindIndex(a => a == CurrentWorld);
            CurrentWorld = Worlds.ElementAt(index + 1);
        }

        public List<AWorld> getWorlds()
        {
            return (Worlds);
        }

        private void setCurrentWorld(AWorld World)
        {
            currentWorld = World;
        }

        public void Update(SFML.System.Time deltaTime)
        {
            currentWorld.Update(deltaTime);
            worldEntity.update(deltaTime, currentWorld);
        }
    }
}
