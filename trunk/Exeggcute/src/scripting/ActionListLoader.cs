using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting;

namespace Exeggcute.src.scripting
{
    class ActionListLoader
    {
        ///<summary>File extention for command list scripts</summary>
        public const string EXT = "cl";

        public static ActionList Load(ScriptName name)
        {
            List<ActionBase> actions = new List<ActionBase>();
            try
            {
                string filepath = string.Format("data/{0}.{1}", name, EXT);
                List<string> lines = Util.StripComments('#', filepath, true);
                //lines.ForEach(lin => Console.WriteLine(lin));
                lines.Reverse();
                Stack<string> lineStack = new Stack<string>(lines);

                /*
                // We don't get the model from an action list
                // get it from the enemy script (not existent yet)
                string modelString = lineStack.Pop();

                string[] modelTokens = modelString.Split(' ');
                ModelName modelName = Util.ParseEnum<ModelName>(modelTokens[1]);
                */

                int size = lineStack.Count;
                for (int i = 0; i < size; i += 1)
                {
                    string line = lineStack.Pop();
                    try
                    {
                        List<ActionBase> parsed = parseCommand(line);
                        foreach (ActionBase cmd in parsed)
                        {
                            actions.Add(cmd);
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
            return new ActionList(actions);
        }

        public static List<ActionBase> parseCommand(string line)
        {
            string[] tokens = line.Split(' ');
            CommandType type = Util.ParseEnum<CommandType>(tokens[0]);
            if (type == CommandType.MoveTo)
            {
                Vector2 start = Util.ParseVector2(tokens[1]);
                Vector2 target = Util.ParseVector2(tokens[2]);
                int duration = int.Parse(tokens[3]);
                float distance = Vector2.Distance(start, target);
                float speed = (distance / (duration - 1))*2;
                float angle = FastTrig.Atan2(target.Y - start.Y, target.X - start.X);
                Console.WriteLine(angle);
                float linearAccel = -(speed / duration);
                return new List<ActionBase> {
                    new AimAction(angle),
                    new MoveAction(speed, 0, linearAccel, 0, 0),
                    new WaitAction(duration),
                    new SetAction(target),
                    new StopAction()
                };
            }
            else if (type == CommandType.Move)
            {
                float speed           = float.Parse(tokens[1]);
                float angularVelocity = float.Parse(tokens[2]);
                float linearAccel     = float.Parse(tokens[3]);
                float velocityZ       = float.Parse(tokens[4]);
                return new List<ActionBase> {
                    new MoveAction(speed, angularVelocity, linearAccel, velocityZ)
                };
            }
            else if (type == CommandType.Aim)
            {
                float angle = float.Parse(tokens[1]) * FastTrig.degreesToRadians;;
                return new List<ActionBase> {
                    new AimAction(angle)
                };
            }
            else if (type == CommandType.Wait)
            {
                int duration = int.Parse(tokens[1]);
                return new List<ActionBase> { new WaitAction(duration) };
            }
            else if (type == CommandType.Stop)
            {
                return new List<ActionBase> { new StopAction() };
            }
            else if (type == CommandType.Vanish)
            {
                return new List<ActionBase> { new VanishAction() };
            }
            else if (type == CommandType.End)
            {
                return new List<ActionBase> { new EndAction() };
            }
            else if (type == CommandType.Spawn)
            {
                float angleDeg = float.Parse(tokens[1]);
                float distance = float.Parse(tokens[2]);
                int id = int.Parse(tokens[3]);
                return new List<ActionBase> {
                    new SpawnAction(angleDeg, distance, id)
                };
            }
            else if (type == CommandType.Set)
            {
                Vector3 pos = Util.ParseVector3(tokens[1]);
                return new List<ActionBase> { new SetAction(pos) };
            }
            else
            {
                throw new ParseError("Unable to parse type {0}", tokens[0]);
            }
        }


    }
}
