using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;

namespace Exeggcute.src.gui
{
    // The sender of the event should probably be present too, but not
    // srictly necessary
    class MenuEvent
    {
        public MenuEventType Type { get; protected set; }
        public static MenuEvent Empty = new MenuEvent();
        public Direction Dir { get; protected set; }
        public ContextName NextID { get; protected set; }
        private MenuEvent()
        {
            Type = MenuEventType.None;
            NextID = ContextName.None;
        }
        public MenuEvent(Direction dir)
        {
            Type = MenuEventType.Move;
            Dir = dir;
        }
        public MenuEvent(ContextName next)
        {
            Type = MenuEventType.Push;
            NextID = next;
        }
    }
}
