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
        public static readonly string EXT = "cl";
        public static readonly string ROOT = "data/scripts";
        public static string[] allFiles;
        protected override string getFilepath(string name)
        {
            return name;
        }
        protected string[] getAllFiles()
        {
            return Directory.GetFiles(ROOT);
        }
        public ScriptLoader()
        {
            Delim = ' ';
        }
        public string Join(object name, string ext)
        {
            return string.Format("{0}.{1}", name.ToString(), ext);
        }

        public List<ActionBase> LoadBehavior(string behavior)
        {
            string name = Join(behavior, "cl");
            Console.WriteLine("name withext = {0}", name);
            return Load(name);
        }

        public List<ActionBase> LoadSpawn(string spawn)
        {
            string name = Join(spawn, "spawn");
            return Load(name);
        }

        public List<ActionBase> LoadShot(string shot)
        {
            string name = Join(shot, "traj");
            return Load(name);
        }


        protected override List<ActionBase> parseElement(string[] tokens)
        {
            CommandType type = Util.ParseEnum<CommandType>(tokens[0]);
            if (type == CommandType.MoveTo)
            {
                Vector3 destination = Util.ParseVector3(tokens[1]);
                int duration = int.Parse(tokens[2]);
                return new List<ActionBase> {
                    new MoveToAction(destination, duration),
                    new WaitAction(duration),
                    new SetAction(destination),
                    new StopAction()
                };
            }
            else if (type == CommandType.MoveRelative)
            {
                Vector3 displacement = Util.ParseVector3(tokens[1]);
                int duration = int.Parse(tokens[2]);
                return new List<ActionBase> {
                    new MoveRelativeAction(displacement, duration),
                    new WaitAction(duration),
                    new StopAction()
                };
            }
            else if (type == CommandType.Move)
            {
                float speed = float.Parse(tokens[1]);
                float angularVelocity = float.Parse(tokens[2]);
                float linearAccel = float.Parse(tokens[3]);
                float velocityZ = float.Parse(tokens[4]);
                return new List<ActionBase> {
                    new MoveAction(speed, angularVelocity, linearAccel, velocityZ)
                };
            }
            else if (type == CommandType.Aim)
            {
                float angle = float.Parse(tokens[1]) * FastTrig.degreesToRadians; ;
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
            else if (type == CommandType.Delete)
            {
                return new List<ActionBase> { new DeleteAction() };
            }
            else if (type == CommandType.Shoot)
            {
                int id = int.Parse(tokens[1]);
                int duration;
                bool found = int.TryParse(tokens[2], out duration);
                if (!found)
                {
                    duration = -1;
                }
                return new List<ActionBase> { new ShootAction(id, duration, true) };
            }
            else if (type == CommandType.Loop)
            {
                int ptr;
                if (tokens.Length != 2)
                {
                    ptr = 0;
                }
                else
                {
                    ptr = int.Parse(tokens[1]);
                }

                return new List<ActionBase> { 
                    new LoopAction(ptr)
                };

            }
            else if (type == CommandType.Spawn)
            {
                float distance = float.Parse(tokens[1]);
                float angle = float.Parse(tokens[2]) * FastTrig.degreesToRadians;
                return new List<ActionBase> {
                    new SpawnAction(distance, angle)
                };
            }
            else if (type == CommandType.Set)
            {
                Vector3 pos = Util.ParseVector3(tokens[1]);
                return new List<ActionBase> { new SetAction(pos) };
            }
            else if (type == CommandType.SpawnerLock)
            {
                bool lockPosition;
                bool lockAngle;

                string posString = tokens[1];
                string angleString = tokens[2];
                string positionValue = posString.Split(':')[1];
                string angleValue = angleString.Split(':')[1];



                if (Regex.IsMatch(positionValue, "[oO]ff"))
                {
                    lockPosition = false;
                }
                else if (Regex.IsMatch(positionValue, "[oO]n"))
                {
                    lockPosition = true;
                }
                else
                {
                    throw new ParseError("Valid values for \"{0}\" parameters are off and on", posString);
                }

                if (Regex.IsMatch(angleValue, "[oO]ff"))
                {
                    lockAngle = false;
                }
                else if (Regex.IsMatch(angleValue, "[oO]n"))
                {
                    lockAngle = true;
                }
                else
                {
                    throw new ParseError("Valid values for \"{0}\" parameters are off and on", angleString);

                }

                return new List<ActionBase> {
                    new SpawnerLockAction(lockPosition, lockAngle)
                };
            }
            else if (type == CommandType.SpawnerSet)
            {
                float posAngle = float.Parse(tokens[1]) * FastTrig.degreesToRadians;
                float distance = float.Parse(tokens[2]);
                float angle = float.Parse(tokens[3]) * FastTrig.degreesToRadians;
                return new List<ActionBase> {
                    new SpawnerSetAction(posAngle, distance, angle)
                };
            }
            else
            {
                throw new ParseError("Unhandled token type {0}", tokens[0]);
            }
        }
    }
}
