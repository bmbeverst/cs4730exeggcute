using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.graphics
{
    enum TileColor
    {
        Red, Blue, Green, Yellow
    }
    class WangArray
    {
        /// <summary>
        /// Specifies the number of distinct wang tiles we use (8, 13, etc)
        /// </summary>
        public int WangCount { get; protected set; }
        public int Cols { get; protected set; }
        public int Rows { get; protected set; }
        public byte[,] grid;
        int[] counts;
        
        /// <summary>
        /// Gets the number of tiles with a particular wang index
        /// </summary>
        public int GetCount(int index)
        {
            return counts[index];
        }

        public int this[int i, int j]
        {
            get { return grid[i, Rows - 1 - j]; }
        }

        public WangArray(int cols, int rows)
        {
            WangCount = 8;
            this.counts = new int[WangCount];
            this.Cols = cols;
            this.Rows = rows;
            this.grid = new byte[cols, rows];
            for (int i = 0; i < cols; i += 1)
            {
                for (int j = 0; j < rows; j += 1)
                {
                    byte? left;
                    byte? above;
                    if (i <= 0) left = null;
                    else left = grid[i - 1, j];

                    if (j <= 0) above = null;
                    else above = grid[i, j - 1];


                    grid[i, j] = WangTile.GetTile(left, above);
                    counts[grid[i, j]] += 1;
                    

                }
            }
        }
    }
    class WangTile
    {
        public TileColor[] Colors;
        public static TileColor[][] Wang8 = 
        {
            new TileColor[4] { TileColor.Red, TileColor.Yellow, TileColor.Green, TileColor.Blue },
            new TileColor[4] { TileColor.Green, TileColor.Blue, TileColor.Green, TileColor.Blue },
            new TileColor[4] { TileColor.Red, TileColor.Yellow, TileColor.Red, TileColor.Yellow },
            new TileColor[4] { TileColor.Green, TileColor.Blue, TileColor.Red, TileColor.Yellow },
            new TileColor[4] { TileColor.Red, TileColor.Blue, TileColor.Green, TileColor.Yellow },
            new TileColor[4] { TileColor.Green, TileColor.Yellow, TileColor.Green, TileColor.Yellow },
            new TileColor[4] { TileColor.Red, TileColor.Blue, TileColor.Red, TileColor.Blue },
            new TileColor[4] { TileColor.Green, TileColor.Yellow, TileColor.Red, TileColor.Blue}
        };
        public TileColor Top { get { return Colors[0]; } }
        public TileColor Right { get { return Colors[1]; } }
        public TileColor Bottom { get { return Colors[2]; } }
        public TileColor Left { get { return Colors[3]; } }
        public static Random random = new Random();
        public static byte GetTile(byte? left, byte? above)
        {
            WangTile tileLeft = left.HasValue? new WangTile(left.Value) : null;
            WangTile tileAbove = above.HasValue? new WangTile(above.Value) : null;
            return GetTile(tileLeft, tileAbove).Index;
        }
        public static WangTile GetTile(WangTile left, WangTile above)
        {
            int[] tiles = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            if (left == null && above == null)
            {
                return new WangTile(random.NextInt(8));
            }
            else if (left == null) //above is not null
            {
                for (int i = 0; i < 8; i += 1)
                {
                    if (Wang8[i][0] != above.Bottom)
                    {
                        tiles[i] = -1;
                    }
                }
            }
            //0 - top
            //1 - right
            //2 - bottom
            //3 - left
            else if (above == null) //left is not null
            {
                for (int i = 0; i < 8; i += 1)
                {
                    if (Wang8[i][3] != left.Right)
                    {
                        tiles[i] = -1;
                    }

                }
            }
            else // left and top are non null
            {
                for (int i = 0; i < 8; i += 1)
                {

                    if (Wang8[i][0] != above.Bottom || Wang8[i][3] != left.Right)
                    {
                        tiles[i] = -1;
                    }
                }
            }
            Array.Sort(tiles);
            Array.Reverse(tiles);
            int possible = 0;
            for (int i = 0; i < 8; i += 1)
            {
                if (tiles[i] != -1)
                {
                    possible += 1;
                }
            }
            int nexttile = tiles[random.NextInt(possible)];
            WangTile result = new WangTile(nexttile);
            return result;
        }
        public byte Index { get; protected set; }
        public WangTile(int tilenum)
        {
            Index = (byte)tilenum;
            Colors = new TileColor[4];
            Array.Copy(Wang8[tilenum], Colors, 4);
        }
    }
}
