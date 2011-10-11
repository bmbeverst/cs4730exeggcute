using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.commands;

namespace Exeggcute.src.commands
{
    class CommandListLoader
    {
        ///<summary>File extention for command list scripts</summary>
        public const string EXT = "cl";

        public static List<Command> Load(string name)
        {
            List<Command> result = new List<Command>();
            try
            {
                string filepath = string.Format("{0}.{1}", name, EXT);
                List<string> lines = Util.StripComments('#', filepath, true);
                //lines.ForEach(lin => Console.WriteLine(lin));
                lines.Reverse();
                Stack<string> lineStack = new Stack<string>(lines);

                string modelString = lineStack.Pop();

                string[] modelTokens = modelString.Split(' ');
                ModelName modelName = Util.ParseEnum<ModelName>(modelTokens[1]);

                int size = lineStack.Count;
                for (int i = 0; i < size; i += 1)
                {
                    string line = lineStack.Pop();
                    try
                    {
                        
                        Console.WriteLine(line);
                        List<Command> parsed = parseCommand(line);
                        foreach (Command cmd in parsed)
                        {
                            result.Add(cmd);
                        }
                    }
                    catch
                    {
                        throw new ParseError("failed to parse line {0}", line);
                    }
                }
            }
            catch (ParseError pe)
            {
                throw pe;
            }
            catch
            {
                throw new ParseError("parse error");
            }
            return result;
        }

        public static List<Command> parseCommand(string line)
        {
            string[] tokens = line.Split(' ');
            CommandType type = Util.ParseEnum<CommandType>(tokens[0]);
            if (type == CommandType.MoveTo)
            {
                Vector2 start = Util.ParseVector2(tokens[1]);
                Vector2 target = Util.ParseVector2(tokens[2]);
                int duration = int.Parse(tokens[3]);
                float distance = Vector2.Distance(start, target);
                float speed = (distance / duration)*2;
                float angle = FastTrig.Atan2(target.Y - start.Y, target.X - start.X);
                Console.WriteLine(angle);
                float linearAccel = -(speed / duration);
                return new List<Command> {
                    new MoveCommand(angle, speed, 0, linearAccel, 0, 0),
                    new WaitCommand(duration)
                };
            }
            else if (type == CommandType.Wait)
            {
                int duration = int.Parse(tokens[1]);
                return new List<Command> { new WaitCommand(duration) };
            }
            else if (type == CommandType.Reset)
            {
                Vector3 position = Util.ParseVector3(tokens[1]);
                return new List<Command> { new ResetCommand(position) };
            }
            else
            {
                throw new ParseError("Unable to parse type {0}", tokens[0]);
            }
        }


    }
}
