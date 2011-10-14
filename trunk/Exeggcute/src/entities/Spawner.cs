﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    class Spawner : CommandEntity
    {
        public Spawner(ScriptName script, List<Shot> spawnList, HashList<Shot> shotList)
            : base(script, spawnList, shotList)
        {

        }
    }
}
