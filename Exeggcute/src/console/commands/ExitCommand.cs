
namespace Exeggcute.src.console.commands
{

    class ExitCommand : ConsoleCommand
    {
        public const string Usage =
@"
    Exit                Quits the game, calling cleanup and saving functions
                        if necessary.";

        public ExitCommand(DevConsole devConsole)
            : base(devConsole)
        {
            
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
