using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad
{
    class Sample
    {
        private Renderer renderer;
        private InputManager im;
        Graphics.EntityResource entity = new Graphics.EntityResource();
        Graphics.EntityResource entityAnimated = new Graphics.EntityResource();

        public Sample()
        {
            renderer = new Renderer();
            renderer.Initialization();

            renderer.SetFrameRate(1);
            renderer.ChangeResolution(Renderer.EResolution.R1920x1080);
            renderer.SetFullScreenEnabled(true);
            renderer.SetMouseCursorVisible(false);
            renderer.SetSyncVertical(true);

            im = new InputManager(renderer.Win);

            Graphics.TextureFactory tf = Graphics.TextureFactory.Instance;
            Graphics.SoundBufferFactory bf = Graphics.SoundBufferFactory.Instance;
            tf.initTextures();
            bf.initSoundBuffer();

            tf.load("DefaultCharacter", "Assets/CharacterDefault.png");
            bf.load("shotgun", "Assets/Shotgunsound.wav");

            Graphics.JukeBox world = new Graphics.JukeBox();
            world.loadMusic(0, "Assets/PokemonMysteryDonjon.wav");
            world.loadSound(0, "shotgun");

            
            entity.InitializationRegularSprite("DefaultCharacter", new SFML.System.Vector2i(32, 32), new SFML.Graphics.IntRect(32, 0, 32, 32));
            entity.Position = new SFML.System.Vector2f(200, 200);
            entity.Rotation = 15;

           
            entityAnimated.InitializationAnimatedSprite("DefaultCharacter", new SFML.System.Vector2i(32, 32));
            entityAnimated.JukeBox = world;
            entityAnimated.Position = new SFML.System.Vector2f(20, 20);

            entityAnimated.JukeBox.PlayMusic(0);

            im.KeyboardKeyPressed += Im_KeyboardKeyPressed;

            FrameClock clock = new FrameClock();

            while (renderer.isOpen())
            {
                renderer.DispatchEvents();

                entityAnimated.Update(clock.Restart());

                renderer.Clear(SFML.Graphics.Color.White);
                renderer.Draw(entity);
                renderer.Draw(entityAnimated);
                renderer.Display();
            }
        }

        private void Im_KeyboardKeyPressed(object source, Keyboard.KeyEventArgs e)
        {
           if (e.KeyCode.Equals(Keyboard.Key.Q)) {
                entityAnimated.Position = new SFML.System.Vector2f(entityAnimated.Position.X - 1f, entityAnimated.Position.Y);
                entityAnimated.PlayAnimation(Graphics.EStateEntity.WALKING_LEFT);
                entityAnimated.JukeBox.PlaySound(0);
            } else if (e.KeyCode.Equals(Keyboard.Key.D)) {
                entityAnimated.Position = new SFML.System.Vector2f(entityAnimated.Position.X + 1f, entityAnimated.Position.Y);
                entityAnimated.PlayAnimation(Graphics.EStateEntity.WALKING_RIGHT);
                entityAnimated.JukeBox.PlaySound(0);
            } else if (e.KeyCode.Equals(Keyboard.Key.Z)) {
                entityAnimated.Position = new SFML.System.Vector2f(entityAnimated.Position.X, entityAnimated.Position.Y - 1f);
                entityAnimated.PlayAnimation(Graphics.EStateEntity.WALKING_UP);
                entityAnimated.JukeBox.PlaySound(0);
            } else if (e.KeyCode.Equals(Keyboard.Key.S)) {
                entityAnimated.Position = new SFML.System.Vector2f(entityAnimated.Position.X, entityAnimated.Position.Y + 1f);
                entityAnimated.PlayAnimation(Graphics.EStateEntity.WALKING_DOWN);
                entityAnimated.JukeBox.PlaySound(0);
            } else if (e.KeyCode.Equals(Keyboard.Key.P)) {
                renderer.ChangeResolution(Renderer.EResolution.R1920x1080);
                renderer.SetFullScreenEnabled(true);
            } else if (e.KeyCode.Equals(Keyboard.Key.M)) {
                renderer.ChangeResolution(Renderer.EResolution.R800x600);
                renderer.SetFullScreenEnabled(false);
            } else if (e.KeyCode.Equals(Keyboard.Key.A)) {
                entityAnimated.JukeBox.PauseMusic(0);
            }
        }
    }
}
