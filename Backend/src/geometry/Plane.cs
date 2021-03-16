using System;
using System.Numerics;


namespace LEA_2021
{
    public class Plane : Oriented
    {
        #region Properties

        public int Width { get; set; }

        public int Height { get; set; }

        #endregion

        #region Constructors

        public Plane(Vector3 orientation, int width, int height)
        {
            Orientation = orientation;
            Width       = width;
            Height      = height;
        }

        #endregion


        public override Vector3? Intersect(Ray ray, Vector3 center)
        {
            float denominator = Vector3.Dot(Orientation, ray.Direction);

            // Check if denominator is approximately 0
            if (Math.Abs(0 - denominator) > 0.0001f)
            {
                // Ray does not intersect with plane
                return null;
            }

            float t = Vector3.Dot(center - ray.Origin, Orientation) / denominator;

            return ray.Origin + t * ray.Direction;
        }
    }
}