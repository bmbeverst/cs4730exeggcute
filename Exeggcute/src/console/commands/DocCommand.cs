using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.task;
using Exeggcute.src.scripting.action;
using System.IO;

namespace Exeggcute.src.console.commands
{
    class DocCommand : ConsoleCommand
    {

        public static string Usage =
@"
    Doc TYPE (FILENAME)   
                        Writes the documentation for TYPE to FILENAME or to 
                        the console if not specified.
                        Valid options for TYPE are Task,Console,Action,All.";

        public string Filename { get; protected set; }
        public string Type { get; protected set; }

        public DocCommand(DevConsole console, string type, string filename)
            : base(console)
        {
            this.Filename = filename;
            this.Type = type;
        }

        public override void AcceptCommand(ConsoleContext context)
        {

            string docs;
            if (Util.StrEq(Type, "task"))
            {
                docs = Task.MakeDocs();
            }
            else if (Util.StrEq(Type, "action"))
            {
                docs = ActionBase.MakeDocs();
            }
            else if (Util.StrEq(Type, "console"))
            {
                docs = ConsoleCommand.MakeDocs(devConsole);
            }
            else if (Util.StrEq(Type, "all"))
            {
                docs = Task.MakeDocs() + '\n' +
                       ActionBase.MakeDocs() + '\n' +
                       ConsoleCommand.MakeDocs(devConsole);
            }
            else
            {
                devConsole.Write("Dont know any type called \"{0}\".", Type);
                return;
            }
            if (Filename == null)
            {
                devConsole.Write(docs);
            }
            else
            {
                try
                {
                    Util.WriteFile(Filename, docs);
                    devConsole.Write("Wrote documentation to {0}", Filename);
                }
                catch (IOException ioe)
                {
                    devConsole.Write("{0}\nUnable to write to file \"{1}\"", ioe.Message, Filename);
                }
            }
        }
    }
}
