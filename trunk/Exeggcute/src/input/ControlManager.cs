using System;
using System.Collections.Generic;
using System.Linq;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;

namespace Exeggcute.src
{

    /// <summary>
    /// Keeps track of pressed controls based on a file read from disk. 
    /// Wraps the functionality of an InputManager to provide information 
    /// about the duration of a keypress.
    /// </summary>
    class ControlManager
    {
        private const string CONFIG_FILE = "controls.cfg";
        static ControlManager()
        {
            defaults[Ctrl.Up]        = new Control(Keys.W,        Buttons.LeftThumbstickUp);
            defaults[Ctrl.Down]      = new Control(Keys.S,        Buttons.LeftThumbstickDown);
            defaults[Ctrl.Left]      = new Control(Keys.A,        Buttons.LeftThumbstickLeft);
            defaults[Ctrl.Right]     = new Control(Keys.D,        Buttons.LeftThumbstickRight);
            defaults[Ctrl.Start]     = new Control(Keys.Enter,    Buttons.Start);
            defaults[Ctrl.Select]    = new Control(Keys.Space,    Buttons.RightTrigger);
            defaults[Ctrl.Action]    = new Control(Keys.N,        Buttons.A);
            defaults[Ctrl.Cancel]    = new Control(Keys.M,        Buttons.X);
            defaults[Ctrl.Focus]     = new Control(Keys.Space,    Buttons.Y);
            defaults[Ctrl.LShoulder] = new Control(Keys.H,        Buttons.LeftShoulder);
            defaults[Ctrl.RShoulder] = new Control(Keys.J,        Buttons.RightShoulder);
            defaults[Ctrl.Skip]      = new Control(Keys.LeftShift,Buttons.X);
            defaults[Ctrl.Quit]      = new Control(Keys.Escape,   Buttons.RightThumbstickLeft);
            defaults[Ctrl.Console]   = new Control(Keys.OemTilde, Buttons.RightThumbstickRight);

        }
        public static Ctrl[] AllControls = (Ctrl[])Enum.GetValues(typeof(Ctrl));

        /// <summary>
        /// Holds the default values. Used in case the config file is 
        /// deleted or is missing entries.
        /// </summary>
        private static Dictionary<Ctrl, Control> defaults = new Dictionary<Ctrl, Control>();
        
        private Dictionary<Ctrl, Control> controls = new Dictionary<Ctrl, Control>();
        private Dictionary<Ctrl, Keyflag> keyFlags = new Dictionary<Ctrl, Keyflag>();
        private InputManager Input;

        private bool IsCustomizing = false;
        private int CustomPtr = 0;

        private KeyboardState kbState, kbStatePrev;
        private GamePadState gpState, gpStatePrev;
        private MouseState mouseState;
        public bool ControllerUnplugged { get; protected set; }

        protected int prevMouseWheel;
        public int MouseWheelDelta { get; protected set; }

        /// <summary>
        /// Creates a new control manager wrapper around an InputManager object.
        /// </summary>
        public ControlManager(InputManager input)
        {
            Input = input;
            kbState = kbStatePrev = Input.GetKeyboard().GetState();
            gpState = gpStatePrev = Input.GetGamePad(ExtendedPlayerIndex.Five).GetState();
            InitFromFile();
            foreach (Ctrl ctrl in AllControls)
            {
                keyFlags[ctrl] = new Keyflag();
            }

        }

        /// <summary>
        /// Sets the given ctrl to a new keyboard key.
        /// </summary>
        public void SetKey(Ctrl c, Keys key)
        {
            controls[c].SetKey(key);
        }

        /// <summary>
        /// Sets the given ctrl to a new joypad button.
        /// </summary>
        /// <param name="ctrl">the ctrl to be changed</param>
        /// <param name="button">the new joypad button</param>
        public void SetButton(Ctrl ctrl, Buttons button)
        {
            controls[ctrl].SetButton(button);
        }

        /// <summary>
        /// Computes the keys which were pressed on this frame, but were
        /// not pressed on the previous frame.
        /// </summary>
        public Keys[] GetNewlyPressedKeys()
        {
            Keys[] thisFrame = kbState.GetPressedKeys();
            Keys[] lastFrame = kbStatePrev.GetPressedKeys();
            return thisFrame.Except(lastFrame).ToArray();
        }

        /// <summary>
        /// Computes the joypad buttons which were pressed on this frame,
        /// but were not pressed on the previous frame.
        /// </summary>
        private Buttons[] newlyPressedButtons()
        {
            Buttons[] thisFrame = Util.GetPressedButtons(gpState);
            Buttons[] lastFrame = Util.GetPressedButtons(gpStatePrev);
            return thisFrame.Except(lastFrame).ToArray();
        }

        /// <summary>
        /// Called when IsCustomizing is true. Work in progress to customize
        /// keys at runtime.
        /// </summary>
        public void Customize()
        {
            Ctrl currentCtrl = AllControls[CustomPtr];
            kbStatePrev = kbState;
            gpStatePrev = gpState;
            kbState = Input.GetKeyboard().GetState();
            gpState = Input.GetGamePad(ExtendedPlayerIndex.Five).GetState();

            Keys[] kbPressed = GetNewlyPressedKeys();
            Buttons[] gpPressed = newlyPressedButtons();
            if (kbPressed.Length > 0)
            {
                Keys pressed = kbPressed[0];
                SetKey(currentCtrl, pressed);
                //Console.WriteLine("Setting {0} to {1}", currentCtrl, pressed);
                CustomPtr += 1;
            }
            else if (gpPressed.Length > 0)
            {
                Buttons pressed = gpPressed[0];
                SetButton(currentCtrl, pressed);
                //Console.WriteLine("Setting {0} to {1}", currentCtrl, pressed);
                CustomPtr += 1;
            }
            if (CustomPtr >= AllControls.Length) IsCustomizing = false;
        }

        /// <summary>
        /// Accessor method for keyflag values. Indicates the number of 
        /// consecutive frames that the given ctrl has been pressed. Maxes
        /// out at 255, does not wrap to 0.
        /// </summary>
        /// <param name="ctrl">the ctrl to be queried</param>
        public Keyflag this[Ctrl ctrl]
        {
            get { return keyFlags[ctrl]; }
        }

        /// <summary>
        /// Must be called once per frame to ensure that the state of
        /// this object is up to date.
        /// </summary>
        public void Update()
        {
            UpdateInput();
            var kbState = Input.GetKeyboard(PlayerIndex.One).GetState();
            var gpState = Input.GetGamePad(ExtendedPlayerIndex.Five).GetState();
            foreach (var pair in controls)
            {
                Ctrl ctrl = pair.Key;
                bool witnessed = pair.Value.IsActive(kbState, gpState);
                keyFlags[ctrl].Update(witnessed);
            }
            if (IsCustomizing) Customize();

            // Mouse
            mouseState = Input.GetMouse().GetState();

            int mouseWheelValue = mouseState.ScrollWheelValue;
            MouseWheelDelta = Math.Sign(mouseWheelValue - prevMouseWheel);
            prevMouseWheel = mouseWheelValue;
        }

        public void UpdateInput()
        {
            try
            {
                Input.Update();
            }
            catch (Exception error)
            {
                Console.WriteLine("{0}\nController (un)plugged", error.Message);

                ControllerUnplugged = true;
                throw new ExeggcuteError("controller unplugged");
            }

            
                
        }

        /// <summary>
        /// Loads the control settings from disk.
        /// </summary>
        public void InitFromFile()
        {
            List<string> lines = Util.ReadLines(CONFIG_FILE);
            for (int i = 0; i < lines.Count; i += 1)
            {
                try
                {
                    string[] tokens = lines[i].Split(':');
                    Ctrl ctrl;
                    string name = tokens[0].Trim('[', ']', ' ');
                    bool parsed = Enum.TryParse<Ctrl>(name, out ctrl);
                    if (!parsed) throw new Exception();
                    controls[ctrl] = new Control(tokens[1]);
                }
                catch
                {
                    Util.Warn("Failure to parse line {0}", lines[i]);
                }
            }

            foreach (Ctrl ctrl in AllControls)
            {
                if (!controls.ContainsKey(ctrl))
                {
                    Util.Debug("Did not find {0}", ctrl);
                    controls[ctrl] = defaults[ctrl];
                }
            }
         }

        /// <summary>
        /// Writes the current controls to disk, overwriting the old settings.
        /// Relevant for when controls can be customized in-game.
        /// </summary>
        public void WriteToFile()
        {
            string output = "";
            foreach (Ctrl ctrl in AllControls)
            {
                if (!controls.ContainsKey(ctrl))
                {
                    controls[ctrl] = defaults[ctrl];
                }
                output += string.Format("[{0}]:{1}\n", 
                                        ctrl.ToString().PadLeft(9),
                                        controls[ctrl].ToString());
            }
            Console.WriteLine(output);
            Util.WriteFile(CONFIG_FILE, output);
        }
    }
}
