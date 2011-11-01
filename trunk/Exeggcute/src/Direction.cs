
namespace Exeggcute.src
{
    /// <summary>
    /// Represents planar direction relative to an origin. No specific 
    /// orientation or origin in 3D space is assumed. If there is potential
    /// for ambiguity, use a Vector3 instead. Typical usage is for static
    /// elements which exist relative to screen space.
    /// </summary>
    /// <remarks>
    /// No particular order should be assumed when using this class for 
    /// portability and readability concerns. If you find yourself casting
    /// this type to an int, you probably should use a Dictionary or other
    /// conversion method.
    /// </remarks>
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
