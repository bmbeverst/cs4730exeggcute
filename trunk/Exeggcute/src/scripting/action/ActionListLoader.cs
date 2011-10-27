using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting;
using Exeggcute.src.entities;
using System.Text.RegularExpressions;
using System.IO;

namespace Exeggcute.src.scripting.action
{
    class ScriptLoader : ScriptParser<ActionBase>
    {


        /// <summary>
        /// I trusted you! I trusted you! I trusted you! I trusted you!
        /// I trusted you! I trusted you! I trusted you! I trusted you!
        /// I trusted you! I trusted you! I trusted you! I trusted you!
        /// I trusted you! I trusted you! I trusted you! I trusted you!
        /// I trusted you! I trusted you! I trusted you! I trusted you!
        /// </summary>

        public ScriptBase Make(string filepath)
        {
            string name = getName(filepath);
            return new ScriptBase(name, RawFromFile(filepath));

        }


        protected override List<ActionBase> parseElement(Stack<string> tokens)
        {
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
            else if (type == CommandType.Move)
            {
                FloatValue speed           = Util.ParseFloatValue(tokens.Pop());
                FloatValue angularVelocity = Util.ParseFloatValue(tokens.Pop());
                FloatValue linearAccel     = Util.ParseFloatValue(tokens.Pop());
                FloatValue angularAccel    = Util.ParseFloatValue(tokens.Pop());
                FloatValue velocityZ       = Util.ParseFloatValue(tokens.Pop());
                return new List<ActionBase> {
                    new MoveAction(speed, angularVelocity, linearAccel, angularAccel, velocityZ)
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
                    index = PlanarEntity3D.ParamMap[paramName];
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
            else
            {
                throw new ParseError("Unhandled token type {0}", type);
            }
        }
    }
}
