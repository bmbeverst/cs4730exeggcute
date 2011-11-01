namespace Exeggcute.src
{
    /// <summary>
    /// Entry point for the XNA game. This class should not be modified except
    /// to possibly permit commandline arguments.
    /// </summary>
    static class EntryPoint
    {
        static void Main(string[] args)
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}
