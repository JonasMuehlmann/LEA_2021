using System;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public static class Util
    {
        public static float RadiansToDegree(float radians)
        {
            return (float) (radians / Math.PI * 180d);
        }


        public static double RadiansToDegree(double radians)
        {
            return radians / Math.PI * 180d;
        }


        public static float RadiansToDegree(int radians)
        {
            return (float) (radians / Math.PI * 180d);
        }


        public static float DegreesToRadians(float degrees)
        {
            return degrees * (float) Math.PI / 180f;
        }


        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180d;
        }


        public static float DegreesToRadians(int degrees)
        {
            return (float) (degrees * Math.PI / 180d);
        }


        public static int Square(int value)
        {
            return value * value;
        }


        public static float Square(float value)
        {
            return value * value;
        }


        public static double Square(double value)
        {
            return value * value;
        }
    }
}