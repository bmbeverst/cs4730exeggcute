﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    class Enemy : CommandEntity
    {

        public bool IsDestroyed
        {
            get { return Health <= 0; }
        }

        public Enemy(ModelName modelName, ScriptName scriptName, ArsenalName arsenalName, HashList<Shot> shotList)
            : base(modelName, scriptName, arsenalName, shotList)
        {

        }

    }
}