using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplineComponentsTester
{
    public class MathF
    {
        public const float PI = 3.1415926535897931f;

        public static float Clamp(float min, float max, float val)
        {
            return val < min ? min : val > max ? max : val;
        }

        public static float Sin(float a)
        {
            return (float)Math.Sin(a);
        }

        public static float Cos(float d)
        {
            return (float)Math.Cos(d);
        }

        public static float Tan(float a)
        {
            return (float)Math.Tan(a);
        }

        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static float Pow(float x, float y)
        {
            return (float)Math.Pow(x, y);
        }

        public static float Sqrt(float d)
        {
            return (float)Math.Sqrt(d);
        }

        public static float Floor(float d)
        {
            return (float)Math.Floor(d);
        }

        public static int Sign(float value)
        {
            return Math.Sign(value);
        }

        public static bool Signbit(float value)
        {
            return Math.Sign(value) < 0;
        }
    }
}
