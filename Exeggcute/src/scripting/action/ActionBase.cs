using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    enum CommandType
    {
        MoveRelative,
        MoveTo,
        Move,
        SetPos,
        Wait,
        Vanish,
        Set,
        Stop,
        End,
        Spawn,
        Aim,
        Shoot
    }


    abstract class ActionBase
    {
        /// <summary>
        /// Uses double dispatch to call the correct processing method in
        /// CommandEntity
        /// </summary>
        public virtual void Process(CommandEntity entity)
        {
            entity.Process(this);
        }

        public abstract ActionBase Copy();

    }
}
