
namespace Exeggcute.src
{
    /// <summary>
    /// A 2d array whose seocnd index wraps on either end.
    /// </summary>
    class Wrapped2DArray<T>
    {
        protected T[,] array;

        public int Cols { get; protected set; }
        public int Rows { get; protected set; }

        public T this[int i, int j]
        {
            get { return array[i, (j + Rows) % Rows]; }
            set { array[i, (j + Rows) % Rows] = value; }
        }

        public Wrapped2DArray(int cols, int rows)
        {
            this.Rows = rows;
            this.Cols = cols;
            this.array = new T[Cols, Rows];
        }
    }
}
