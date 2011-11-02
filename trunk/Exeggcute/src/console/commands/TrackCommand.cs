using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.console.commands
{
    class TrackCommand : ConsoleCommand
    {
        public static string Usage =
@"
    Track ID FORMAT PARAMS (FREQ) (TYPE)           
                        Track the given PARAMS for the entity with id ID.
                        The format for FORMAT is the same as for string.Format.
                        FREQ denotes the number of frames between each sample.
                        If TYPE is not specified, it is written to stdout";

        public int ID { get; protected set; }
        public string Format { get; protected set; }
        public int[] Indices { get; protected set; }
        public int Frequency { get; protected set; }
        public string Type { get; protected set; }

        public TrackCommand(DevConsole console, int id, string format, List<int> indices, int frequency, string type)
            : base(console)
        {
            this.ID = id;
            this.Format = format;
            this.Indices = indices.ToArray();
            this.Frequency = frequency;
            this.Type = type;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
