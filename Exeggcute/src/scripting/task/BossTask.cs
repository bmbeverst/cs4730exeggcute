﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class BossTask : Task
    {
        public override void Process(Level level)
        {
            level.Process(this);
        }
    }
}
