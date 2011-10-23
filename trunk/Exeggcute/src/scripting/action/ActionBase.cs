using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    public enum CommandType
    {
        /// <summary>
        /// Tells the entity to move the amount specified by the input vector.
        /// </summary>
        MoveRelative,

        /// <summary>
        /// Tells the entity to move to an absolute position in world space.
        /// </summary>
        MoveTo,

        /// <summary>
        /// Sets the entity's speed parameters to particular values.
        /// </summary>
        Move,

        SetParam,

        /// <summary>
        /// Tells the entity to not process further commands until the action 
        /// duration expires.
        /// </summary>
        Wait,

        /// <summary>
        /// Stops all velocities and speeds on the entity.
        /// </summary>
        Stop,

        /// <summary>
        /// undocumented
        /// </summary>
        Aim,

        /// <summary>
        /// Toggles whether or not the entity is firing its weapon.
        /// </summary>
        Shoot,

        /// <summary>
        /// Jumps the action pointer back to the specified position, or
        /// the beginning of the script if no parameter is given.
        /// </summary>
        Loop,

        /// <summary>
        /// Tells the level to remove this entity from the world before 
        /// calling its update method again.
        /// </summary>
        Delete,

        /// <summary>
        /// Spawns a shot.
        /// </summary>
        Spawn,

        AimPlayer
    }


    abstract class ActionBase
    {
        /// <summary>
        /// Uses double dispatch to call the correct processing method in
        /// CommandEntity
        /// </summary>
        public virtual void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public abstract ActionBase Copy();

    }
}
