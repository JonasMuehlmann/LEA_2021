using System;
using System.Drawing;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public static class Util
    {
        /// <summary>
        ///     Unclamped/Unvalidated conversion from radians to degrees
        /// </summary>
        /// <param name="radians">A given measure of radians to be converted</param>
        /// <returns>A given measure of radians converted to degrees</returns>
        public static float RadToDeg(float radians)
        {
            return (float) (radians / Math.PI * 180d);
        }


        /// <summary>
        ///     Unclamped/Unvalidated conversion from radians to degrees
        /// </summary>
        /// <param name="radians">A given measure of radians to be converted</param>
        /// <returns>A given measure of radians converted to degrees</returns>
        public static double RadToDeg(double radians)
        {
            return radians / Math.PI * 180d;
        }


        /// <summary>
        ///     Unclamped/Unvalidated conversion from radians to degrees
        /// </summary>
        /// <param name="radians">A given measure of radians to be converted</param>
        /// <returns>A given measure of radians converted to degrees</returns>
        public static float RadToDeg(int radians)
        {
            return (float) (radians / Math.PI * 180d);
        }


        /// <summary>
        ///     Unclamped/Unvalidated conversion from degrees to radians
        /// </summary>
        /// <param name="degrees">A given measure of degrees to be converted</param>
        /// <returns>A given measure of degrees converted to radians</returns>
        public static float DegToRad(float degrees)
        {
            return degrees * (float) Math.PI / 180f;
        }


        /// <summary>
        ///     Unclamped/Unvalidated conversion from degrees to radians
        /// </summary>
        /// <param name="degrees">A given measure of radians to be converted</param>
        /// <returns>A given measure of degrees converted to radians</returns>
        public static double DegToRad(double degrees)
        {
            return degrees * Math.PI / 180d;
        }


        /// <summary>
        ///     Unclamped/Unvalidated conversion from degrees to radians
        /// </summary>
        /// <param name="degrees">A given measure of degrees to be converted</param>
        /// <returns>A given measure of degrees converted to radians</returns>
        public static float DegToRad(int degrees)
        {
            return (float) (degrees * Math.PI / 180d);
        }


        /// Return value * value
        public static int Square(int value)
        {
            return value * value;
        }


        /// Return value * value
        public static float Square(float value)
        {
            return value * value;
        }


        /// Return value * value
        public static double Square(double value)
        {
            return value * value;
        }


        /// <summary>
        ///     Normalize x to the range [0,1] given x's old value range
        /// </summary>
        /// <param name="x">A value to normalize</param>
        /// <param name="oldMin">The lowest value x can take before normalization</param>
        /// <param name="oldMax">The highest value x can take before normalization</param>
        /// <returns>x's equivalent in the range of [0,1]</returns>
        public static float Normalize(float x, float oldMin, float oldMax)
        {
            return (x - oldMin) / (oldMax - oldMin);
        }


        /// <summary>
        ///     Normalize x to the range [0,1] given x's old value range
        /// </summary>
        /// <param name="x">A value to normalize</param>
        /// <param name="oldMin">The lowest value x can take before normalization</param>
        /// <param name="oldMax">The highest value x can take before normalization</param>
        /// <returns>x's equivalent in the range of [0,1]</returns>
        public static double Normalize(double x, double oldMin, double oldMax)
        {
            return (x - oldMin) / (oldMax - oldMin);
        }


        /// <summary>
        ///     Normalize x to the range [0,1] given x's old value range
        /// </summary>
        /// <param name="x">A value to normalize</param>
        /// <param name="oldMin">The lowest value x can take before normalization</param>
        /// <param name="oldMax">The highest value x can take before normalization</param>
        /// <returns>x's equivalent in the range of [0,1]</returns>
        public static float Normalize(int x, int oldMin, int oldMax)
        {
            return (x - oldMin) / (float) (oldMax - oldMin);
        }


        /// <summary>
        ///     Normalize x to the range [0,1] given x's old value range
        /// </summary>
        /// <param name="x">A value to normalize</param>
        /// <param name="oldMax">The highest value x can take before normalization</param>
        /// <returns>x's equivalent in the range of [0,1]</returns>
        public static float Normalize(float x, float oldMax)
        {
            return x / oldMax;
        }


        /// <summary>
        ///     Normalize x to the range [0,1] given x's old value range
        /// </summary>
        /// <param name="x">A value to normalize</param>
        /// <param name="oldMax">The highest value x can take before normalization</param>
        /// <returns>x's equivalent in the range of [0,1]</returns>
        public static double Normalize(double x, double oldMax)
        {
            return x / oldMax;
        }


        /// <summary>
        ///     Normalize x to the range [0,1] given x's old value range
        /// </summary>
        /// <param name="x">A value to normalize</param>
        /// <param name="oldMax">The highest value x can take before normalization</param>
        /// <returns>x's equivalent in the range of [0,1]</returns>
        public static float Normalize(int x, int oldMax)
        {
            return x / (float) oldMax;
        }


        /// <summary>
        ///     Rescale x to the range [newMin, newMax], given x's old range [oldMin, oldMax]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="oldMin">The lowest value x can take before rescaling</param>
        /// <param name="oldMax">The highest value x can take before rescaling</param>
        /// <param name="newMin">The lowest value x can take after rescaling</param>
        /// <param name="newMax">The highest value x can take after rescaling</param>
        /// <returns>x's equivalent in the range of [newMin, newMax]</returns>
        public static float RescaleToRange(float x, float oldMin, float oldMax, float newMin, float newMax)
        {
            return (x - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        }


        /// <summary>
        ///     Rescale x to the range [newMin, newMax], given x's old range [oldMin, oldMax]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="oldMin">The lowest value x can take before rescaling</param>
        /// <param name="oldMax">The highest value x can take before rescaling</param>
        /// <param name="newMin">The lowest value x can take after rescaling</param>
        /// <param name="newMax">The highest value x can take after rescaling</param>
        /// <returns>x's equivalent in the range of [newMin, newMax]</returns>
        /// static float RescaleToRange(int x, int oldMin, int oldMax, int newMin, int newMax)
        public static float RescaleToRange(int x, int oldMin, int oldMax, int newMin, int newMax)
        {
            return (x - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        }


        /// <summary>
        ///     Rescale x to the range [newMin, newMax], given x's old range [oldMin, oldMax]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="oldMin">The lowest value x can take before rescaling</param>
        /// <param name="oldMax">The highest value x can take before rescaling</param>
        /// <param name="newMin">The lowest value x can take after rescaling</param>
        /// <param name="newMax">The highest value x can take after rescaling</param>
        /// <returns>x's equivalent in the range of [newMin, newMax]</returns>
        /// static float RescaleToRange(int x, int oldMin, int oldMax, int newMin, int newMax)
        public static double RescaleToRange(double x, double oldMin, double oldMax, double newMin, double newMax)
        {
            return (x - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        }


        /// <summary>
        ///     Rescale x to the range [newMin, newMax], given x's old range [0,1]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="newMin">The lowest value x can take after rescaling</param>
        /// <param name="newMax">The highest value x can take after rescaling</param>
        /// <returns>x's equivalent in the range of [newMin, newMax]</returns>
        public static float RescaleToRange(float x, float newMin, float newMax)
        {
            return x * (newMax - newMin) + newMin;
        }


        /// <summary>
        ///     Rescale x to the range [newMin, newMax], given x's old range [0,1]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="newMin">The lowest value x can take after rescaling</param>
        /// <param name="newMax">The highest value x can take after rescaling</param>
        /// <returns>x's equivalent in the range of [newMin, newMax]</returns>
        public static float RescaleToRange(int x, int newMin, int newMax)
        {
            return x * (newMax - newMin) + newMin;
        }


        /// <summary>
        ///     Rescale x to the range [newMin, newMax], given x's old range [0,1]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="newMin">The lowest value x can take after rescaling</param>
        /// <param name="newMax">The highest value x can take after rescaling</param>
        /// <returns>x's equivalent in the range of [newMin, newMax]</returns>
        public static double RescaleToRange(double x, double newMin, double newMax)
        {
            return x * (newMax - newMin) + newMin;
        }


        /// <summary>
        ///     Return a Vector from a to b
        /// </summary>
        public static Vec3 FromAToB(Vec3 a, Vec3 b)
        {
            return b - a;
        }


        /// <summary>
        ///     Measure the distance between a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float ScalarDistance(float a, float b)
        {
            return Math.Abs(a) - Math.Abs(b);
        }


        /// <summary>
        ///     Measure the distance between a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double ScalarDistance(double a, double b)
        {
            return Math.Abs(a) - Math.Abs(b);
        }


        /// <summary>
        ///     Measure the distance between a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ScalarDistance(int a, int b)
        {
            return Math.Abs(a) - Math.Abs(b);
        }


        /// <summary>
        ///     Clamped, component-wise scaling of a color
        /// </summary>
        /// <param name="color">A color to scale</param>
        /// <param name="factor">A factor to scale the color by</param>
        /// <returns>color.channel * factor for all channels R,G,B</returns>
        public static Color ColorScale(Color color, float factor)
        {
            return Color.FromArgb(255,
                                  (int) Math.Clamp(color.R * factor, 0, 255),
                                  (int) Math.Clamp(color.G * factor, 0, 255),
                                  (int) Math.Clamp(color.B * factor, 0, 255)
                                 );
        }


        /// <summary>
        ///     Clamped, component-wise scaling of a color
        /// </summary>
        /// <param name="color">A color to scale</param>
        /// <param name="factor">A factor to scale the color by</param>
        /// <returns>color.channel * factor for all channels R,G,B</returns>
        public static Color ColorScale(Color color, int factor)
        {
            return Color.FromArgb(255,
                                  Math.Clamp(color.R * factor, 0, 255),
                                  Math.Clamp(color.G * factor, 0, 255),
                                  Math.Clamp(color.B * factor, 0, 255)
                                 );
        }


        /// <summary>
        ///     Clamped, component-wise addition of two colors
        /// </summary>
        /// <returns>color.channel + color2.channel for all channels R,G,B</returns>
        public static Color ColorAdd(Color color, Color color2)
        {
            return Color.FromArgb(255,
                                  Math.Clamp(color.R + color2.R, 0, 255),
                                  Math.Clamp(color.G + color2.G, 0, 255),
                                  Math.Clamp(color.B + color2.B, 0, 255)
                                 );
        }


        /// <summary>
        ///     Clamped component-wise addition of a color and a value
        /// </summary>
        /// <param name="color">A color to add to</param>
        /// <param name="value">A value to add to the color</param>
        /// <returns>color.channel + value for all channels R,G,B</returns>
        public static Color ColorAdd(Color color, int value)
        {
            return Color.FromArgb(255,
                                  Math.Clamp(color.R + value, 0, 255),
                                  Math.Clamp(color.G + value, 0, 255),
                                  Math.Clamp(color.B + value, 0, 255)
                                 );
        }


        /// <summary>
        ///     Clamped, component-wise subtraction of two colors
        /// </summary>
        /// <returns>color.channel - color2.channel for all channels R,G,B</returns>
        public static Color ColorSubtract(Color color, Color color2)
        {
            return Color.FromArgb(255,
                                  Math.Clamp(color.R - color2.R, 0, 255),
                                  Math.Clamp(color.G - color2.G, 0, 255),
                                  Math.Clamp(color.B - color2.B, 0, 255)
                                 );
        }


        /// <summary>
        ///     Clamped component-wise addition of a color and a value
        /// </summary>
        /// <param name="color">A color to add to</param>
        /// <param name="value">A value to add to the color</param>
        /// <returns>color.channel - value for all channels R,G,B</returns>
        public static Color ColorSubtract(Color color, int value)
        {
            return Color.FromArgb(255,
                                  Math.Clamp(color.R - value, 0, 255),
                                  Math.Clamp(color.G - value, 0, 255),
                                  Math.Clamp(color.B - value, 0, 255)
                                 );
        }


        /// Swap the values of variables a and b
        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
}