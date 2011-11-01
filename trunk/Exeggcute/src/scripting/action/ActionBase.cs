using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    /// <summary>
    /// The general form of a behavior script is a list of Commands, given by the following BNF grammar
    /// </summary>
    public enum CommandType
    {
        MoveRel,
        MoveAbs,
        SetParam,
        Wait,
        Stop,
        Shoot,
        Loop,
        Delete,
        Spawn,
        AimPlayer,
        Sound,
        Upgrade
    }


    abstract class ActionBase
    {
        public enum Info
        {
            Syntax,
            Args,
            Description,
            Example
        }
        public static Dictionary<Type, Dictionary<Info, string>> docs =
            new Dictionary<Type, Dictionary<Info, string>>();

        public virtual void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public abstract ActionBase Copy();

        public static string MakeDocs()
        {
            string docString = "";
            foreach (var pair in docs)
            {
                docString += pair.Key.Name + '\n';
                Dictionary<Info, string> taskInfos = pair.Value;
                int level = 8;
                docString += makeDocSection(taskInfos, level);

            }
            return docString;
        }
        protected static string makeDocSection(Dictionary<Info, string> taskInfos, int level)
        {
            string syntax = string.Format("    Syntax:\n{0}", Util.Indent(taskInfos[Info.Syntax], level));
            string argString = taskInfos[Info.Args];
            string args = argString == null ?
                "" :
                string.Format("    Arguments:\n{0}", Util.Indent(argString, level));
            string description = string.Format("    Description:\n{0}", Util.Indent(taskInfos[Info.Description], level));
            string exString = taskInfos[Info.Example];
            string example = exString == null ?
                "" :
                string.Format("    Example:\n{0}", Util.Indent(taskInfos[Info.Example], level));
            return string.Format("{0}{1}{2}{3}", syntax, args, description, example);
        }
    }
}
