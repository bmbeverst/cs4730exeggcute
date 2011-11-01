
namespace Exeggcute.src
{
    /// <summary>
    /// Made this cause I was really sick of writing KeyValuePair all the 
    /// time.
    /// </summary>
    class Pair<K, V>
    {
        public K First;
        public V Second;
        public Pair(K first, V second)
        {
            First = first;
            Second = second;
        }
        public override string ToString()
        {
            return string.Format("({0},{1})", First, Second);
        }
    }
}
