using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Exeggcute.src.input;

namespace Exeggcute.src.console
{
    class KeyboardManager
    {
        public bool IsShifting { get; protected set; }
        public KeyboardState CurrentState { get; protected set; }
        public KeyboardState PreviousState { get; protected set; }
        private Dictionary<Keys, Keyflag> keyFlags =
            new Dictionary<Keys, Keyflag>();

        private static Dictionary<Keys, bool> printable =
            new Dictionary<Keys, bool>();
        private static Dictionary<Keys, char> shiftConversion =
            new Dictionary<Keys, char>
            {
                { Keys.OemComma, '<' },
                { Keys.OemOpenBrackets,'{' },
                { Keys.OemCloseBrackets, '}' },
                { Keys.OemPipe, '|' },
                { Keys.D0, ')' },
                { Keys.D1, '!' },
                { Keys.D2, '@' },
                { Keys.D3, '#' },
                { Keys.D4, '$' },
                { Keys.D5, '%' },
                { Keys.D6, '^' },
                { Keys.D7, '&' },
                { Keys.D8, '*' },
                { Keys.D9, '(' },
                { Keys.OemPeriod, '>'},
                { Keys.Space, ' ' }

            };
        private static Dictionary<Keys, char> conversion =
            new Dictionary<Keys, char>
            {
                { Keys.OemComma, ',' },
                { Keys.OemOpenBrackets,'[' },
                { Keys.OemCloseBrackets, ']' },
                { Keys.OemPipe, '\\' },
                { Keys.D0, '0' },
                { Keys.D1, '1' },
                { Keys.D2, '2' },
                { Keys.D3, '3' },
                { Keys.D4, '4' },
                { Keys.D5, '5' },
                { Keys.D6, '6' },
                { Keys.D7, '7' },
                { Keys.D8, '8' },
                { Keys.D9, '9' },
                { Keys.OemPeriod, '.'},
                { Keys.Space, ' ' }



            };

        public Keys[] PressedThisFrame { get; protected set; }

        static Keys[] printableExtra = new Keys[] {
            
            


        };

        static KeyboardManager()
        {
            for (int i = 65; i < 90; i += 1)
            {
                Keys upperKey = (Keys)i;
                char upperChar = (char)i;
                char lowerChar = (char)(i + 32);
                conversion[upperKey] = lowerChar;

                shiftConversion[upperKey] = upperChar;
            }

        }

        public KeyboardManager()
        {

        }


        public bool IsKeyPressed(Keys key)
        {
            return CurrentState.IsKeyDown(key);
        }
        public bool IsPrintable(Keys key)
        {
            if (IsShifting)
            {
                return shiftConversion.ContainsKey(key);
            }
            else
            {
                return conversion.ContainsKey(key);
            }
        }

        public char GetPrintable(Keys key)
        {
            if (IsShifting)
            {
                return shiftConversion[key];
            }
            else
            {
                return conversion[key];
            }
        }

        public void Update()
        {
            IsShifting = CurrentState.IsKeyDown(Keys.LeftShift) ||
                         CurrentState.IsKeyDown(Keys.RightShift);
            PreviousState = CurrentState;
            CurrentState = Keyboard.GetState();
            Keys[] thisFrame = CurrentState.GetPressedKeys();
            Keys[] lastFrame = PreviousState.GetPressedKeys();
            PressedThisFrame = thisFrame.Except(lastFrame).ToArray();
        }


    }
}
