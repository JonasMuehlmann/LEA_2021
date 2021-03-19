using System;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Plane : Oriented
    {
        #region Properties

        public int Width { get; set; }

        public int Length { get; set; }

        #endregion

        #region Constructors

        // Oriented, rectangular plane
        public Plane(Vec3 orientation, int width, int height)
        {
            Orientation = orientation;
            Width       = width;
            Length      = height;
        }


        // Oriented, square plane
        public Plane(Vec3 orientation, int dimensions)
        {
            Orientation = orientation;
            Width       = dimensions;
            Length      = dimensions;
        }


        // Y-axis aligned, rectangular plane
        public Plane(int width, int height)
        {
            Orientation = Vec3.UnitY;
            Width       = width;
            Length      = height;
        }


        // Y-axis aligned, square plane
        public Plane(int dimensions)
        {
            Orientation = Vec3.UnitY;
            Width       = dimensions;
            Length      = dimensions;
        }

        #endregion


        public override float Intersect(Ray ray, Point3 center)
        {
            var denominator = Vec3.Dot(Orientation, ray.Direction);

            // Check if denominator is approximately 0

            if (Math.Abs(denominator) < float.Epsilon)
            {
                // Ray does not intersect with plane
                return -1f;
            }

            var t = Vec3.Dot(center - ray.Origin, Orientation) / denominator;

            if (t > float.Epsilon)
            {
                // Ray intersects, but is shot in opposite direction
                return -1f;
            }

            return t;
        }
    }
}