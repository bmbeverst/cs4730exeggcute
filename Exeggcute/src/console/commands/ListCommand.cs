using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    enum FileType
    {
        Behavior,
        Spawn,
        Trajectory,
        Body,
        Boss,
        Campaign,
        Enemy,
        GibBatch,
        Item,
        Level,
        Option,
        Player,
        Model,
        Sfx,
        Song,
        Texture,
        Sprite,
        Font

    }
    class ListCommand : ConsoleCommand
    {
        static ListCommand()
        {
            string baseUsage = "";
            foreach (FileType type in Enum.GetValues(typeof(FileType)))
            {
                baseUsage += type + "\n";
            }

            ValidTypes = baseUsage;
        }
        public static string Usage = 
@"
    List FILETYPE       Lists all loaded files of type TYPE.";
        public static string ValidTypes = ConsoleCommand.StaticUsage;
        public FileType Type { get; protected set; }
        public ListCommand(DevConsole devConsole, FileType type)
            : base(devConsole)
        {
            this.Type = type;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
