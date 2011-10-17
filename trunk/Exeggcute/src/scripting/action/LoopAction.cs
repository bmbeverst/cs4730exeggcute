﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class LoopAction : ActionBase
    {
        public int Pointer { get; protected set; }
        public LoopAction(int ptr)
        {
            Pointer = ptr;
        }
        public LoopAction()
        {
            Pointer = 0;
        }
        public override void Process(CommandEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new LoopAction(Pointer);
        }
    }
}
