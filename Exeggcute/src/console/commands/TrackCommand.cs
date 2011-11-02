using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.console.commands
{
    abstract class Tracker
    {
        protected Entity3D entity;
        protected string format;
        protected int[] indices;
        protected int frequency;
        protected int frame;

        public Tracker(Entity3D entity, string format, int[] indices, int frequency)
        {
            this.entity = entity;
            this.format = format;
            this.indices = indices;
            this.frequency = frequency;
        }

        public virtual void Update()
        {
            frame += 1;
            if (frame % frequency == 0)
            {
                Emit();
            }
        }

        public abstract void Emit();
    }

    class ConsoleTracker : Tracker
    {
        public ConsoleTracker(Entity3D entity, string format, int[] indices, int frequency)
            : base(entity, format, indices, frequency)
        {

        }
        public override void Emit()
        {

            Console.WriteLine(entity.FormattedQuery(format, indices));
        }
    }

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
