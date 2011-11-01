using System;

namespace Exeggcute.src
{
    /// <summary>
    /// Fast math functions and helpers.
    /// Code originally by Riven of JavaGaming.org.
    /// </summary>
    class FastTrig
    {
        public const float PI = 3.1415927f;
        public const float TWOPI = PI * 2;

	    private const int SIN_BITS = 13; // Adjust for accuracy.
	    private const int SIN_MASK = ~(-1 << SIN_BITS);
	    private const int SIN_COUNT = SIN_MASK + 1;

	    private const float radFull = PI * 2;
	    private const float degFull = 360;
	    private const float radToIndex = SIN_COUNT / radFull;
	    private const float degToIndex = SIN_COUNT / degFull;

	    public const float radiansToDegrees = 180f / PI;
	    public const float degreesToRadians = PI / 180;

        private static readonly float[] sin = new float[SIN_COUNT];
        private static readonly float[] cos = new float[SIN_COUNT];

        public static float Sin(float rad) 
        {
		    return sin[(int)(rad * radToIndex) & SIN_MASK];
	    }

	    public static float Cos(float rad) 
        {
		    return cos[(int)(rad * radToIndex) & SIN_MASK];
	    }

        public static float Tan(float rad)
        {
            float cos = Cos(rad);
            if (cos == 0.0f) throw new DivideByZeroException();
            return Sin(rad) / cos;
        }

        public static float SinDeg(float deg) 
        {
		    return sin[(int)(deg * degToIndex) & SIN_MASK];
	    }

        public static float CosDeg(float deg) 
        {
		    return cos[(int)(deg * degToIndex) & SIN_MASK];
	    }

	    private const int ATAN2_BITS = 7; // Adjust for accuracy.
	    private const int ATAN2_BITS2 = ATAN2_BITS << 1;
	    private const int ATAN2_MASK = ~(-1 << ATAN2_BITS2);
	    private const int ATAN2_COUNT = ATAN2_MASK + 1;
        private static readonly int ATAN2_DIM = (int)Math.Sqrt(ATAN2_COUNT);
        private static readonly float INV_ATAN2_DIM_MINUS_1 = 1.0f / (ATAN2_DIM - 1);
        private static readonly float[] atan2 = new float[ATAN2_COUNT];
	    static FastTrig() 
        {
            for (int i = 0; i < SIN_COUNT; i += 1)
            {
                float a = (i + 0.5f) / SIN_COUNT * radFull;
                sin[i] = (float)Math.Sin(a);
                cos[i] = (float)Math.Cos(a);
            }
            for (int i = 0; i < 360; i += 90)
            {
                sin[(int)(i * degToIndex) & SIN_MASK] = (float)Math.Sin(i * degreesToRadians);
                cos[(int)(i * degToIndex) & SIN_MASK] = (float)Math.Cos(i * degreesToRadians);
            }
		    for (int i = 0; i < ATAN2_DIM; i += 1)
            {
			    for (int j = 0; j < ATAN2_DIM; j += 1) 
                {
				    float x0 = (float)i / ATAN2_DIM;
				    float y0 = (float)j / ATAN2_DIM;
				    atan2[j * ATAN2_DIM + i] = (float)Math.Atan2(y0, x0);
			    }
		    }
	    }

	    public static float Atan2 (float y, float x) 
        {
		    float add, mul;
		    if (x < 0) 
            {
			    if (y < 0) 
                {
				    y = -y;
				    mul = 1;
			    } 
                else
                {
				    mul = -1;
                }
			    x = -x;
			    add = -3.141592653f;
		    } 
            else 
            {
			    if (y < 0) 
                {
				    y = -y;
				    mul = -1;
			    } 
                else
                {
				    mul = 1;
                }
			    add = 0;
		    }
		    float invDiv = 1 / ((x < y ? y : x) * INV_ATAN2_DIM_MINUS_1);
		    int xi = (int)(x * invDiv);
		    int yi = (int)(y * invDiv);
		    return (atan2[yi * ATAN2_DIM + xi] + add) * mul;
	    }



        public static int NextPowerOfTwo(int value) 
        {
		    if (value == 0) return 1;
		    value--;
		    value |= value >> 1;
		    value |= value >> 2;
		    value |= value >> 4;
		    value |= value >> 8;
		    value |= value >> 16;
		    return value + 1;
	    }

        public static bool IsPowerOfTwo(int value)
        {
		    return value != 0 && (value & value - 1) == 0;
	    }
    }
}
