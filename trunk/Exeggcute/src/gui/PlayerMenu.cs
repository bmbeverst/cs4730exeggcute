using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.gui
{
    /// <summary>
    /// Player select menu
    /// </summary>
    class PlayerMenu : Menu
    {

        public PlayerMenu(List<Button> buttons, Rectangle bounds)
            : base(buttons, bounds, false)
        {
            
        }
    }
}
