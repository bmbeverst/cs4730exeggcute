using System;
using Microsoft.Xna.Framework.Input;

namespace Exeggcute.src.input
{
    /// <summary>
    /// Keeps track of whether or not the physical inputs corresponding to a
    /// Ctrl are pressed or not. Can keep track of both a keyboard key and
    /// GamePad button.
    /// </summary>
    class Control
    {
        public Buttons WatchedButton { get; protected set; }
        public Keys WatchedKey { get; protected set; }

        public const char DELIM = ',';

        public Control(Keys key, Buttons button)
        {
            WatchedKey = key;
            WatchedButton = button;
        }

        public Control(string data) 
        {
            string[] tokens = data.Split(DELIM);
            WatchedKey = (Keys)int.Parse(tokens[0]);
            Joy joy = (Joy)int.Parse(tokens[1]);
            WatchedButton = ButtonWrapper.ButtonsOfJoy[joy];
        }

        public void SetKey(Keys key)
        {
            WatchedKey = key;
        }

        public void SetButton(Buttons button)
        {
            WatchedButton = button;
        }

        public bool IsActive(KeyboardState kbState, GamePadState gpState)
        {
            return kbState.IsKeyDown(WatchedKey) || gpState.IsButtonDown(WatchedButton);
        }

        public override string ToString()
        {
            Joy joy = ButtonWrapper.JoyOfButtons[WatchedButton];
            return String.Format("{0},{1}", (int)WatchedKey,(int)joy);
        }
    }
}
