using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SplineComponentsTester
{
    public struct Vector2 : IEquatable<Vector2>, ISwappable<Vector2>
    {
        // ---- VARIABLES ----- \\

        public float x { get; set; }
        public float y { get; set; }

        // ----- INITIALISERS ----- \\

        public Vector2(float a_x, float a_y)
        {
            x = a_x;
            y = a_y;
        }

        public Vector2(PointF a_cpy)
        {
            x = a_cpy.X;
            y = a_cpy.Y;
        }

        public Vector2(Vector2 a_cpy)
        {
            x = a_cpy.x;
            y = a_cpy.y;
        }

        // ----- FUNCTIONS ----- \\

        public float Magnitude()
        {
            return MathF.Sqrt(Squared());
        }

        public float Radians()
        {
            return MathF.Atan2(y, x);
        }

        public float Squared()
        {
            return (x * x) + (y * y);
        }

        public void Negate()
        {
            x = -x; y = -y;
        }

        public void Normalise()
        {
            float Mag = Magnitude();

            this /= Mag == 0f ? 1f : Mag;
        }

        // ----- STATIC FUNCTIONS ----- \\

        public static Vector2 Bezier(Vector2 a_p1, Vector2 a_p2, Vector2 a_p3, float t)
        {
            float ts = t * t;

            float lp = 1f - t;
            float sp = lp * lp;

            return (a_p1 * sp) + (a_p2 * 2f * lp * t) + (a_p3 * ts);
        }

        public static Vector2 Bezier(Vector2 a_p1, Vector2 a_p2, Vector2 a_p3, Vector2 a_p4, float t)
        {
            float ts = t * t;
            float tc = ts * t;

            float lp = 1f - t;
            float sp = lp * lp;
            float cp = sp * lp;

            return (a_p1 * cp) + (3f * a_p2 * sp * t) + (3f * a_p3 * lp * ts) + (a_p4 * tc);
        }

        public static float Dot(Vector2 a_v1, Vector2 a_v2)
        {
            return (a_v1.x * a_v2.x) + (a_v1.y + a_v2.y);
        }

        public static Vector2 Hermite(Vector2 a_p1, Vector2 a_p2, Vector2 a_t1, Vector2 a_t2, float t)
        {
            float ts = t * t;
            float tc = ts * t;

            float h00 = 2f * tc - 3f * ts + 1;
            float h01 = -2f * tc + 3f * ts;
            float h10 = tc - 2f * ts + t;
            float h11 = tc - ts;

            return h00 * a_p1 + h10 * a_t1 + h01 * a_p2 + h11 * a_t2;
        }

        public static Vector2 Lerp(Vector2 a_start, Vector2 a_end, float t)
        {
            return a_start + t * (a_end - a_start);
        }

        public static Vector2 Negate(Vector2 a_v)
        {
            Vector2 vec = a_v;

            vec.Negate();

            return vec;
        }

        public static Vector2 RLerp(Vector2[] a_points, float time)
        {
            int size = a_points.Length;

            if (size == 0) { return new Vector2(); }

            while (size > 1)
            {
                for (int i = 0; i < size - 1; ++i)
                {
                    a_points[i] = Lerp(a_points[i], a_points[i + 1], time);
                }

                --size;
            }

            return a_points[0];
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", x, y);
        }

        public bool Equals(Vector2 other)
        {
            return this == other;
        }

        public void Swap(ref Vector2 other)
        {
            Vector2 temp = other;
            other = this;
            this = temp;
        }

        // ---- OPERATORS ---- \\

        public static bool operator == (Vector2 lhs, Vector2 rhs)
        {
            return (lhs.x == rhs.x) && (lhs.y == rhs.y);
        }
        
        public static bool operator != (Vector2 lhs, Vector2 rhs)
        {
            return (lhs.x != rhs.x) || (lhs.y != rhs.y);
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static Vector2 operator *(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x * rhs.x, lhs.y * rhs.y);
        }

        public static Vector2 operator *(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.x * rhs, lhs.y * rhs);
        }

        public static Vector2 operator *(float lhs, Vector2 rhs)
        {
            return new Vector2(lhs * rhs.x, lhs * rhs.y);
        }

        public static Vector2 operator /(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x / rhs.x, lhs.y / rhs.y);
        }

        public static Vector2 operator /(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.x / rhs, lhs.y / rhs);
        }

        public static Vector2 operator /(float lhs, Vector2 rhs)
        {
            return new Vector2(lhs / rhs.x, lhs / rhs.y);
        }

        public static implicit operator Vector2(PointF rhs)
        {
            return new Vector2(rhs);
        }

        public static implicit operator PointF(Vector2 rhs)
        {
            return new PointF(rhs.x, rhs.y);
        }
    }
}
