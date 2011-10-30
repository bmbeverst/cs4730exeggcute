using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    enum SpawnType
    {
        Player,
        Enemy,
        Boss
    }
    class SpawnCommand : ConsoleCommand
    {
        static SpawnCommand()
        {
            string usageBase = 
@"Usage: Spawn 
    Spawn TYPE NAME     Spawns an entity of type TYPE with name NAME. 
                        Possible TYPEs include: ({0})"; 
            SpawnType[] typeValues = (SpawnType[])Enum.GetValues(typeof(SpawnType));

            string types = Util.Join(typeValues.ToList(), ',');

            Usage = string.Format(usageBase, types);
        }
        public static string Usage = "this member is implemented in a static constructor and was referenced too early in execution. Sorry!";


        public SpawnType Type { get; protected set; }
        public string Name { get; protected set; }

        public SpawnCommand(DevConsole devConsole, SpawnType type, string name)
            : base(devConsole)
        {
            this.Type = type;
            this.Name = name;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
