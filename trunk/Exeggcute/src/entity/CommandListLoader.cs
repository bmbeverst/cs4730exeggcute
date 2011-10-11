using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.entity
{
    class CommandListLoader
    {
        ///<summary>File extention for command list scripts</summary>
        public const string EXT = "cl";

        public static List<Command> Load(string filepath)
        {
            List<Command> result = new List<Command>();
            try
            {
                List<string> lines = Util.StripComments('#', filepath, true);
                lines.Reverse();
                Stack<string> lineStack = new Stack<string>(lines);
                string modelString = lineStack.Pop();

                string[] modelTokens = modelString.Split(' ');
                ModelName modelName = Util.ParseEnum<ModelName>(modelTokens[1]);
                for (int i = 0; i < lineStack.Count; i += 1)
                {
                    Command next = parseCommand(lineStack.Pop());

                }
            }
            catch
            {
                throw new ParseError("failed to parse script");
            }
            return result;
        }

        public static Command parseCommand(string line)
        {
            string[] tokens = line.Split(' ');
            CommandType type = Util.ParseEnum<CommandType>(tokens[0]);


            return null;
        }
    }
}
