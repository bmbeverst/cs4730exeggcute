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

        /// <summary>
        /// ???
        /// Instantly sets the entity's position to the given vector.
        /// </summary>
        SetPos,

        /// <summary>
        /// Tells the entity to not process further commands until the action 
        /// duration expires.
        /// </summary>
        Wait,

        /// <summary>
        /// undocumented
        /// </summary>
        Set,

        /// <summary>
        /// Stops all velocities and speeds on the entity.
        /// </summary>
        Stop,

        /// <summary>
        /// Tells the entity to spawn an entity from its arsenal into
        /// its shotListHandle.
        /// </summary>
        Spawn,

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
        /// Changes the state of the entity's spawner's position.
        /// When locked, which is the default, an entity's spawner
        /// follows its position and angle exactly. If you want
        /// to turn off this behavior, use this type.
        /// </summary>
        SpawnerLock,

        /// <summary>
        /// Moves the entity's spawner by the vector specified and/or sets the
        /// angle at which the spawner is facing.
        /// </summary>
        SpawnerSet,

        /// <summary>
        /// Tells the level to remove this entity from the world before 
        /// calling its update method again.
        /// </summary>
        Delete
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
