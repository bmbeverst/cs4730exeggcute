using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.task;
using Exeggcute.src.scripting.action;

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
                    Util.WriteFile(docs, Filename);
                    devConsole.Write("Wrote documentation to {0}", Filename);
                }
                catch
                {
                    devConsole.Write("Unable to write to file \"{0}\"", Filename);
                }
            }
        }
    }
}
