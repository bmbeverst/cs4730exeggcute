using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class SongFadeTask : Task
    {
        static SongFadeTask()
        {
            docs[typeof(SongFadeTask)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "SongFade FRAMES" },
                { Info.Args, 
@"int FRAMES
    The number of frames it should take before the song is silent."},
                { Info.Description, 
@"Fades out the current playing song over the specified number of frames.
Additionally erects a barrier so that subsequent tasks are not processed
until the song has faded out completely."},
                { Info.Example, 
@"fadeout 60
    Fade out the current playing song over one second."}
            };
        }

        public int NumFrames { get; protected set; }

        public SongFadeTask(int frames)
        {
            this.NumFrames = frames;
        }

        public override void Process(Sandbox level)
        {
            level.Process(this);
        }
    }
}
