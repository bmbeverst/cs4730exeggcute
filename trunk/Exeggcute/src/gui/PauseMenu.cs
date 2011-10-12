using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;

namespace Exeggcute.src.gui
{
    class PauseMenu : Menu
    {
        public PauseMenu()
            : base(getButtons(), false)
        {

        }
        public override void Update(ControlManager controls)
        {
            if (controls[Ctrl.Start].DoEatPress())
            {
                World.Unpause();
            }
            base.Update(controls);
        }

        private static List<Button> getButtons()
        {
            return new List<Button>();
        }
    }
}
