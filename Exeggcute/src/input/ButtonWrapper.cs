using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Exeggcute.src.input
{
    /// <summary>
    /// Wraps XNA's inane Xbox controller interface to something a human would
    /// be willing to provide. Buttons are converted to and from 'Joy', an 
    /// enum which lists button names sequentially to 'Buttons' which holds
    /// names as power of two, skipping values randomly.
    /// </summary>
    static class ButtonWrapper
    {
        /// <summary>
        /// Dictionary used to map from XNA Buttons to human readable Joy values.
        /// </summary>
        public static Dictionary<Buttons, Joy> JoyOfButtons = new Dictionary<Buttons, Joy>();

        /// <summary>
        /// Dictionary used to map from human readable Joy values to XNA buttons.
        /// </summary>
        public static Dictionary<Joy, Buttons> ButtonsOfJoy = new Dictionary<Joy, Buttons>();
        static ButtonWrapper()
        {
            JoyOfButtons[(Buttons)0] = Joy.None;
            ButtonsOfJoy[Joy.None] = (Buttons)0;
            Buttons[] allButtons = (Buttons[])Enum.GetValues(typeof(Buttons));
            for (int i = 0; i < allButtons.Length; i += 1)
            {
                Joy currentJoy = (Joy)(i + 1);
                Buttons currentButtons = allButtons[i];
                JoyOfButtons[currentButtons] = currentJoy;
                ButtonsOfJoy[currentJoy] = currentButtons;
            }
        }
    }
}
