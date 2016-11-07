using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Global
{
    public struct s_Pos<T>
    {
        public s_Pos(T x, T y)
        {
            X = x;
            Y = y;
        }

        public T X;
        public T Y;

        public static s_Pos<T> operator+(s_Pos<T> a, s_Pos<T> b)
        {
            return new s_Pos<T>(Sum(a.X, b.X), Sum(a.Y, b.Y));
        }

        public static s_Pos<T> operator-(s_Pos<T> a, s_Pos<T> b)
        {
            return new s_Pos<T>(Sub(a.X, b.X), Sub(a.Y, b.Y));
        }

        public static s_Pos<T> operator*(s_Pos<T> a, s_Pos<T> b)
        {
            return new s_Pos<T>(Mult(a.X, b.X), Mult(a.Y, b.Y));
        }

        public static s_Pos<T> operator/(s_Pos<T> a, s_Pos<T> b)
        {
            return new s_Pos<T>(Div(a.X, b.X), Div(a.Y, b.Y));
        }

        public static s_Pos<T> operator%(s_Pos<T> a, s_Pos<T> b)
        {
            return new s_Pos<T>(Mod(a.X, b.X), Mod(a.Y, b.Y));
        }

        private static T Mod(T a, T b)
        {
            return (dynamic)a % (dynamic)b;
        }

        private static T Div(T a, T b)
        {
            return (dynamic)a / (dynamic)b;
        }

        private static T Mult(T a, T b)
        {
            return (dynamic)a * (dynamic)b;
        }

        private static T Sub(T a, T b)
        {
            return (dynamic)a - (dynamic)b;
        }

        private static T Sum(T a, T b)
        {
            return (dynamic)a + (dynamic)b;
        }
    }
}
