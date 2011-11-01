using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    /// <summary>
    /// Generates random numbers which are cached at startup. Purposefully
    /// does not allow static access so that each object requiring random
    /// numbers uses its own, and changes to other objects' Random calls
    /// do not affect any other object's calls. This is imperative for
    /// implementing replays.
    /// </summary>
    class Random
    {
        private const int SIZE = 10000;
        private const int SEED = 0;
        private static readonly float[] cache = new float[SIZE];
        static Random()
        {
            System.Random rng = new System.Random(SEED);
            for (int i = 0; i < cache.Length; i += 1)
            {
                cache[i] = (float)rng.NextDouble();
            }
        }

        private int ptr;

        public Random()
        {
            ptr = 0;
        }

        /// <summary>
        /// Returns a float between 0.0f and 1.0f
        /// </summary>
        public float Next()
        {
            ptr %= SIZE;
            return cache[ptr++];
        }

        /// <summary>
        /// Returns a float between 0 and 2*Pi.
        /// </summary>
        public float NextRadian()
        {
            return Next() * FastTrig.TWOPI;
        }

        /// <summary>
        /// Returns an int between 0 and max - 1 inclusive
        /// </summary>
        public int NextInt(int max)
        {
            return (int)(Next() * max);
        }

        /// <summary>
        /// Returns 1 or -1 with equal probability.
        /// </summary>
        public int NextSign()
        {
            return Next() > 0.5f ? -1 : 1;
        }

        public Color NextColor()
        {
            return new Color(NextInt(255), 
                             NextInt(255),
                             NextInt(255), 
                             NextInt(255));
        }
    }
}
