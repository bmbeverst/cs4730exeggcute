using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    /// <summary>
    /// There are two alignments. The same alignment's shots do not
    /// hurt that same alignment's members.
    /// </summary>
    enum Alignment
    {
        None=0,
        Enemy=1,
        Player=2
    }
}
