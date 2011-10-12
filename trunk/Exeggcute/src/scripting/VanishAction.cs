﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting
{
    class VanishAction : ActionBase
    {
        public VanishAction()
            : base()
        {

        }

        public override void Process(CommandEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new VanishAction();
        }


    }
}
