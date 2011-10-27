using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    enum TaskType
    {
        /// <summary>
        /// Spawns an enemy with the specified parameters from the roster.
        /// Syntax:
        /// spawn id pos angle
        /// int id: The index in the roster from which to copy the enemy
        /// Float3 pos: the position at which to spawn the enemy. This should
        ///     be offscreen so that the enemy doesnt blink into existence
        ///     to the player.
        /// FloatValue angle: ??? (degrees)
        /// 
        /// example
        /// spawn 0 ([-10,10], 0, 0) 45
        /// spawns the 0th enemy from the roster at a random position between
        /// (-10,0,0) and (10,0,0). Not sure what angle does...
        /// 
        /// </summary>
        Spawn,

        /// <summary>
        /// Waits for the specified number of frames before proceeding to the next 
        /// task.
        /// Syntax
        /// wait duration
        /// frame duration: the number of frames to wait
        /// 
        /// example
        /// wait 60
        /// wait one second before executing the next task
        /// </summary>
        Wait,

        /// <summary>
        /// Kills all enemies in the world instantly.
        /// Syntax:
        /// killall
        /// </summary>
        KillAll,

        /// <summary>
        /// loads the boss into the level's boss parameter
        /// Syntax:
        /// boss
        /// </summary>
        Boss,

        /// <summary>
        /// fades out the current song, useful for before the boss.
        /// Syntax:
        /// songfade frames
        /// int frames: the number of frames it should take to go to zero.
        /// </summary>
        SongFade
    }
}
