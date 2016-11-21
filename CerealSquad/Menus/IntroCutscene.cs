using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace CerealSquad.Menus
{
    class IntroCutscene : Menu
    {
        #region Event
        public delegate void IntroCutsceneEventHandler(object source, CutsceneEventArgs e);

        public class CutsceneEventArgs
        {
            public State State { get; }

            public CutsceneEventArgs(State state)
            {
                State = state;
            }
        }

        /// <summary>
        /// Event fired when a Game is started
        /// </summary>
        public event IntroCutsceneEventHandler Ended;
        #endregion

        public enum State
        {
            Unknow = -1,
            Waiting = 0,
            Running,
            Finished,
            Cancelled
        }

        private Graphics.RegularSprite _Cutscene;
        private Text _HelpText;

        private Renderer _Renderer;
        private InputManager.InputManager _InputManager;

        private State _State = State.Unknow;
        private int _CurrentFrame { get { return _TimeFrame.Count(i => i == -1); } }
        private int _FrameCount { get { return _TimeFrame.Count; } }
        private List<float> _TimeFrame = new List<float> { 3600, 3600, 3600, 3600 };
        private float _StartingSpeed = .01f;
        private float _MinSpeed = .01f;
        private float _MaxSpeed = 10f;
        private float _Acceleration = .01f;
        private float _CurrentSpeed;
        private float _FrameOffset = 0;

        public IntroCutscene(Renderer renderer, InputManager.InputManager inputManager)
        {
            if (renderer == null)
                throw new ArgumentNullException("Renderer cannot be null");
            if (inputManager == null)
                throw new ArgumentNullException("Input Manager cannot be null");

            _Renderer = renderer;
            _InputManager = inputManager;

            Factories.TextureFactory.Instance.load("CUTSCENE_intro", "Assets/Background/cutscene_1024.png");
            _Cutscene = new Graphics.RegularSprite(Factories.TextureFactory.Instance.getTexture("CUTSCENE_intro"),
                new Vector2i((int)_Renderer.Win.GetView().Size.X, (int)_Renderer.Win.GetView().Size.Y * 4), new IntRect(0, 0, 1024, 2359));
            _Cutscene.Position = new Vector2f(0, 0);

            _InputManager.JoystickButtonPressed += _InputManager_JoystickButtonPressed;
            _InputManager.JoystickButtonReleased += _InputManager_JoystickButtonReleased;
            _InputManager.KeyboardKeyPressed += _InputManager_KeyboardKeyPressed;
            _InputManager.KeyboardKeyReleased += _InputManager_KeyboardKeyReleased;
            _CancelTimer.Elapsed += _CancelTimer_Elapsed;

            _HelpText = new Text("Hold A / Enter to skip", Factories.FontFactory.FontFactory.Instance.getFont(Factories.FontFactory.FontFactory.Font.ReenieBeanie));
            _HelpText.CharacterSize = 64;
            _HelpText.Position = new Vector2f(_Renderer.Win.GetView().Size.X - (_HelpText.GetLocalBounds().Left + _HelpText.GetLocalBounds().Width) - 10,
                _Renderer.Win.GetView().Size.Y - (_HelpText.GetLocalBounds().Top + _HelpText.GetLocalBounds().Height) - 10);

            _State = State.Waiting;
        }

        #region Keybinding
        private System.Timers.Timer _CancelTimer = new System.Timers.Timer(1000);
        private void _InputManager_JoystickButtonReleased(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            if (e.Button == 0 && Displayed)
                _CancelTimer.Stop();
        }
        private void _InputManager_JoystickButtonPressed(object source, InputManager.Joystick.ButtonEventArgs e)
        {
            if (e.Button == 0 && Displayed)
                _CancelTimer.Start();
        }
        private void _InputManager_KeyboardKeyReleased(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (e.KeyCode == InputManager.Keyboard.Key.Return && Displayed)
                _CancelTimer.Stop();
        }
        private void _InputManager_KeyboardKeyPressed(object source, InputManager.Keyboard.KeyEventArgs e)
        {
            if (e.KeyCode == InputManager.Keyboard.Key.Return && Displayed)
                _CancelTimer.Start();
        }
        private void _CancelTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Displayed && _State != State.Cancelled)
            {
                _CancelTimer.Stop();
                _State = State.Cancelled;
                Ended?.Invoke(this, new CutsceneEventArgs(_State));
            }
        }
        #endregion

        private void UpdateCutscene(Time DeltaTime)
        {
            if (_State == State.Finished)
            {
                Ended?.Invoke(this, new CutsceneEventArgs(_State));
                _State = State.Unknow;
            }
            else if (_State == State.Waiting)
            {
                _TimeFrame[_CurrentFrame] -= DeltaTime.AsMilliseconds();
                if (_TimeFrame[_CurrentFrame] <= 0)
                {
                    _TimeFrame[_CurrentFrame] = 0;
                    if (_CurrentFrame >= _FrameCount - 1)
                        _State = State.Finished;
                    else
                        _State = State.Running;
                    _FrameOffset = 0;
                    _CurrentSpeed = _StartingSpeed;
                }
            }
            else if (_State == State.Running)
            {
                _FrameOffset += _CurrentSpeed * DeltaTime.AsMilliseconds();
                if (_FrameOffset >= _Renderer.Win.GetView().Size.Y)
                    _FrameOffset = _Renderer.Win.GetView().Size.Y;
                _Cutscene.Position = new Vector2f(0, -_CurrentFrame * _Renderer.Win.GetView().Size.Y - _FrameOffset);

                if (_FrameOffset >= _Renderer.Win.GetView().Size.Y)
                {
                    _TimeFrame[_CurrentFrame] = -1;
                    _State = State.Waiting;
                }

                if (_FrameOffset <= _Renderer.Win.GetView().Size.Y / 4 && _CurrentSpeed < _MaxSpeed)
                    _CurrentSpeed += _Acceleration;
                if (_FrameOffset >= 3 * _Renderer.Win.GetView().Size.Y / 4 && _CurrentSpeed > _MinSpeed)
                    _CurrentSpeed -= _Acceleration;
            }
        }

        public override void Update(Time DeltaTime)
        {
            UpdateCutscene(DeltaTime);
            base.Update(DeltaTime);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_Cutscene, states);
            target.Draw(_HelpText, states);
            base.Draw(target, states);
        }
    }
}
