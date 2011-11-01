
namespace Exeggcute.src
{
    /// <summary>
    /// Thrown when a subclass failed to override a method which it must
    /// override in order to sensibly use.
    /// </summary>
    class SubclassShouldImplementError : ExeggcuteError
    {
        public SubclassShouldImplementError(string message, params object[] args)
            : base(message, args)
        {

        }

        public SubclassShouldImplementError()
            : base("the subclass should implement this method")
        {

        }
    }
}
