﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting;

namespace Exeggcute.src.scripting
{
    class ActionListLoader : ScriptParser<ActionBase, ScriptName>
    {
        public static readonly string EXT = "cl";
        public static readonly string ROOT = "data";
        public override string getFilepath(string name)
        {
            return string.Format("{0}/{1}.{2}", ROOT, name, EXT);
        }

        public ActionListLoader()
        {
            Delim = ' ';
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
