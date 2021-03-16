using System;


namespace LEA_2021
{
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
    }
}