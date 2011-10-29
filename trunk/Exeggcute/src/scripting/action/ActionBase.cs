using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    /// <summary>
    /// The general form of a behavior script is a list of Commands, given by the following BNF grammar
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// Tells the entity to move the amount specified by the input vector.
        /// Syntax:
        /// moverelative vector duration
        /// Float3 vector: the amount to move
        /// frame duration: how long it should take to get there in frames
        /// 
        /// example:
        /// moverel (10,0,0) 60
        /// moves the entity ten units in the positive x direction over the course of 60 frames
        /// </summary>
        MoveRel,

        /// <summary>
        /// Tells the entity to move to an absolute position in game space.
        /// Syntax:
        /// moveabs vector duration
        /// Float3 vector: the position in game space to move to
        /// frame duration: how long it should take to get there in frames
        /// 
        /// example:
        /// moverel (10,0,0) 60
        /// moves the entity to position (10,0,0) in game space in 60 frames
        /// </summary>
        MoveAbs,

        /// <summary>
        /// Sets the entity's speed parameters to particular values.
        /// DEPRECATED
        /// </summary>
        Move,

        /// <summary>
        /// Sets an arbitrary parameter to the given float value.
        /// See ScriptedEntity for which parameters can be set (dictionary).
        /// Syntax:
        /// SetParam paramname value
        /// string paramname: the case-sensitive string name of the parameter as given in the 
        /// parameter dictionary in ScriptedEntity.cs
        /// FloatValue value: the new desired value of the parameter
        /// 
        /// example
        /// setparam PositionX [0,1]
        /// sets the x position in game space of the entity to a random value between 0 and 1
        /// </summary>
        SetParam,

        /// <summary>
        /// Instructs the entity to not process further commands for a specified number of frames.
        /// Syntax:
        /// wait duration
        /// frame duration: the number of frames (plus or minus one) to wait for
        /// 
        /// example
        /// wait 60
        /// waits for 60 frames before moving to the next command
        /// </summary>
        Wait,

        /// <summary>
        /// Stops all velocities and speeds on the entity. Preserves angle.
        /// Syntax:
        /// stop
        /// </summary>
        Stop,

        /// <summary>
        /// Toggles whether or not the entity is firing its weapon. If no 
        /// duration is given, the entity will fire until another shoot 
        /// command with that index is received.
        /// Syntax:
        /// shoot id (duration)
        /// FloatValue id: the index of the spawner in the arsenal which should begin/cease firing
        /// frame duration: the amount of frames to continue firing from this spawner
        /// 
        /// example
        /// shoot 0 120
        /// instructs the entity to fire its 0th spawner for 120 frames
        /// </summary>
        Shoot,

        /// <summary>
        /// Jumps the action pointer back to the specified position.
        /// NOTE: Currently it jumps based on the actual number of commands
        /// rather than line numbers in the script. Some commands are 
        /// translated into multiple actions, so this might not behave
        /// as expected. This should be fixed.
        /// Syntax:
        /// loop (ptr)
        /// int ptr: the index into the action list to jump to
        /// 
        /// example
        /// loop 3
        /// jumps the action pointer back to actions[3]
        /// </summary>
        Loop,

        /// <summary>
        /// Tells the level to remove this entity from the world before 
        /// calling its update method again. Equivalent to calling 
        /// QueueDelete() on the entity
        /// Syntax:
        /// delete
        /// 
        /// </summary>
        Delete,

        /// <summary>
        /// Spawns a shot with a given angular offset, which can be either 
        /// absolute or relative to the entity's spawner's mover's current angle.
        /// Syntax:
        /// spawn type angle 
        /// AngleType type: whether the angle is relative or absolute
        /// FloatValue angle: the angle in degrees to aim the shot at 
        ///     (see the AngleType enum for possible values)
        /// example
        /// 
        /// spawn rel 45
        /// spawn a shot at 45 degrees relative to the entity's mover's angle
        /// 
        /// spawn abs 90
        /// spawn a shot aimed at 90 degrees
        /// </summary>
        Spawn,

        /// <summary>
        /// Aim's a spawner's mover to aim at the player.
        /// 
        /// Syntax:
        /// aimplayer 
        /// </summary>
        AimPlayer,

        /// <summary>
        /// Play a sound in an entity-specific way. As of writing this is used
        /// for spawners to make shot sounds.
        /// 
        /// Syntax:
        /// sound
        /// </summary>
        Sound,

        /// <summary>
        /// Upgrades the player's weapon. Used only in demo mode.
        /// Syntax:
        /// upgrade
        /// </summary>
        Upgrade
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
