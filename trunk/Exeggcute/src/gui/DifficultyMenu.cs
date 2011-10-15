using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.gui
{
    class DifficultyMenu : Menu
    {

        public DifficultyMenu()
            : base(getButtons(), false)
        {

        }

        private static List<Button> getButtons()
        {
            return new List<Button>();
        }
    }
}
