using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace Exeggcute.src.scripting.action
{
    class ScriptLoader : ScriptParser<ActionBase>
    {
        public ScriptBase Make(string filepath)
        {
            string name = getName(filepath);
            return new ScriptBase(name, RawFromFile(filepath));

        }


        public override List<ActionBase> ParseElement(string input)
        {
            string[] split = input.Split(' ');
            Stack<string> tokens = Util.Stackify<string>(split);
            CommandType type = Util.ParseEnum<CommandType>(tokens.Pop());

            if (type == CommandType.MoveAbs)
            {
                Float3 destination = Util.ParseFloat3(tokens.Pop());
                int duration = int.Parse(tokens.Pop());
                return new List<ActionBase> {
                    new MoveAbsAction(destination, duration),
                    new WaitAction(duration),
                    new SetParamAction("PositionX", destination.X),
                    new SetParamAction("PositionY", destination.Y),
                    new SetParamAction("PositionZ", destination.Z),
                    new StopAction()
                };
            }
            else if (type == CommandType.MoveRel)
            {
                Float3 displacement = Util.ParseFloat3(tokens.Pop());
                int duration = int.Parse(tokens.Pop());
                return new List<ActionBase> {
                    new MoveRelAction(displacement, duration),
                    new WaitAction(duration),
                    new StopAction()
                };
            }
            else if (type == CommandType.Upgrade)
            {
                int max = int.Parse(tokens.Pop());
                return new List<ActionBase> {
                    new UpgradeAction(max)
                };
            }
            else if (type == CommandType.Wait)
            {
                int duration = int.Parse(tokens.Pop());
                return new List<ActionBase> { new WaitAction(duration) };
            }
            else if (type == CommandType.Stop)
            {
                return new List<ActionBase> { new StopAction() };
            }
            else if (type == CommandType.Delete)
            {
                return new List<ActionBase> { new DeleteAction() };
            }
            else if (type == CommandType.SetParam)
            {
                string paramName = tokens.Pop();
                FloatValue value = Util.ParseFloatValue(tokens.Pop());
                int index;
                try
                {
                    index = Entity3D.ParamMap[paramName.ToLower()];
                }
                catch (KeyNotFoundException knf)
                {
                    throw new ParseError("{0}\n{1} is not a settable parameter", knf.Message, paramName);
                }
                return new List<ActionBase> {
                    new SetParamAction(index, value)
                };
            }

            else if (type == CommandType.Shoot)
            {
                FloatValue id = Util.ParseFloatValue(tokens.Pop());
                int duration;
                bool found = int.TryParse(tokens.Pop(), out duration);
                if (!found)
                {
                    duration = -1;
                }
                return new List<ActionBase> { new ShootAction(id, duration) };
            }
            else if (type == CommandType.Loop)
            {
                int ptr;
                if (tokens.Count != 2)
                {
                    ptr = 0;
                }
                else
                {
                    ptr = int.Parse(tokens.Pop());
                }

                return new List<ActionBase> { 
                    new LoopAction(ptr)
                };

            }
            else if (type == CommandType.Spawn)
            {
                AngleType atype = Util.ParseEnum<AngleType>(tokens.Pop());
                FloatValue angle = Util.ParseFloatValue(tokens.Pop()).FromDegrees();

                return new List<ActionBase> {
                    new SpawnAction(atype, angle)
                };
            }
            else if (type == CommandType.AimPlayer)
            {
                return new List<ActionBase> {
                    new AimPlayerAction()
                };
            }
            else if (type == CommandType.Sound)
            {
                return new List<ActionBase>{
                    new SoundAction()
                };
            }
            else if (type == CommandType.For)
            {
                string varName = "&";
                Point3 bounds = Point3.Parse(tokens.Pop());
                List<ActionBase> list = new List<ActionBase>();
                string loopCommand = "";
                int iMax = tokens.Count;
                for (int i = 0; i < iMax; i += 1)
                {
                    loopCommand += tokens.Pop() + ' ';
                }

                for (int i = bounds.X; i < bounds.Y; i += bounds.Z)
                {
                    string newCommand = Regex.Replace(loopCommand, varName, i + "");
                    List<ActionBase> looped = ParseElement(newCommand);
                    list.AddRange(looped);

                }

                return list;

            }
            else if (type == CommandType.If)
            {
                string paramName = tokens.Pop();
                string op = tokens.Pop();
                FloatValue value = FloatValue.Parse(tokens.Pop());
               

                int paramIndex = Entity3D.NameToIndex(paramName);
                if (paramIndex == -1)
                {
                    throw new ParseError("{0} is not a settable parameter", paramName);
                }
                string toDoString = Util.JoinStack(tokens, " ");

                List<ActionBase> toDo = ParseElement(toDoString);
                if (toDo.Count > 1)
                {
                    throw new NotImplementedException("If block not implemented, single actions only");
                }
                return new List<ActionBase> {
                    new IfAction(paramIndex, op, value, toDo[0])
                };
            }
            else
            {
                throw new ParseError("Unhandled token type {0}", type);
            }
        }
    }
}
