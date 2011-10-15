using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Exeggcute.src.scripting.action
{
    /// <summary>
    /// A read-only wrapper for a List of ActionBase.
    /// </summary>
    class ActionList
    {
        public List<ActionBase> actions;
        public int Count
        {
            get { return actions.Count; }
        }
        public ActionList(List<ActionBase> actions)
        {
            this.actions = actions;
        }

        public ActionBase this[int ptr]
        {
            get { return actions[ptr]; }
        }

        /// <summary>
        /// Returns an editable copy of this list, useful if the entity wants
        /// to learn or otherwise modify its behavior
        /// </summary>
        /// <returns></returns>
        public List<ActionBase> GetEditable()
        {
            List<ActionBase> result = new List<ActionBase>();
            foreach (ActionBase cmd in actions)
            {
                result.Add(cmd.Copy());
            }
            return result;
        }
    }
}
