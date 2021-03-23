using System;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;
    using Point2 = Vector2;


    public class FinitePlane : Oriented
    {
        #region Properties

        public int Width { get; set; }

        public int Length { get; set; }

        #endregion

        #region Constructors

        // Oriented, rectangular plane
        public FinitePlane(Vec3 orientation, int width, int height)
        {
            Orientation = orientation;
            Width       = width;
            Length      = height;
        }


        // Oriented, square plane
        public FinitePlane(Vec3 orientation, int dimensions)
        {
            Orientation = orientation;
            Width       = dimensions;
            Length      = dimensions;
        }


        // Y-axis aligned, rectangular plane
        public FinitePlane(int width, int height)
        {
            Orientation = Vec3.UnitY;
            Width       = width;
            Length      = height;
        }


        // Y-axis aligned, square plane
        public FinitePlane(int dimensions)
        {
            Orientation = Vec3.UnitY;
            Width       = dimensions;
            Length      = dimensions;
        }

        #endregion


        private bool IsInBounds(Point3 center, Point3 intersection)
        {
            if (
                Math.Abs(Orientation.Y) == 1f
            )
            {
                if (Math.Abs(Util.ScalarDistance(center.X, intersection.X)) > Length * 0.5f
                 || Math.Abs(Util.ScalarDistance(center.Z, intersection.Z)) > Width  * 0.5f)
                {
                    return false;
                }
            }

            if (
                Math.Abs(Orientation.X) == 1f
            )
            {
                if (Math.Abs(Util.ScalarDistance(center.Z, intersection.Z)) > Length * 0.5f
                 || Math.Abs(Util.ScalarDistance(center.Y, intersection.Y)) > Width  * 0.5f)
                {
                    return false;
                }
            }

            else
            {
                if (Math.Abs(Util.ScalarDistance(center.Y, intersection.Y)) > Length * 0.5f
                 || Math.Abs(Util.ScalarDistance(center.X, intersection.X)) > Width  * 0.5f)
                {
                    return false;
                }
            }

            return true;
        }


        public override Point2 GetUvCoordinates(Point3 center, Point3 intersection)
        {
            if (Math.Abs(Orientation.Y) == 1f)
            {
                float u = Util.RescaleToRange(Util.ScalarDistance(center.Z, intersection.Z),
                                              -Length * 0.5f,
                                              Length  * 0.5f,
                                              0,
                                              1
                                             );

                float v = Util.RescaleToRange(Util.ScalarDistance(center.X, intersection.X),
                                              -Width * 0.5f,
                                              Width  * 0.5f,
                                              0,
                                              1
                                             );

                return new Point2(u, v);
            }

            if (Math.Abs(Orientation.X) == 1f)
            {
                float u = Util.RescaleToRange(Util.ScalarDistance(center.Z, intersection.Z),
                                              -Length * 0.5f,
                                              Length  * 0.5f,
                                              0,
                                              1
                                             );

                float v = Util.RescaleToRange(Util.ScalarDistance(center.Y, intersection.Y),
                                              -Width * 0.5f,
                                              Width  * 0.5f,
                                              0,
                                              1
                                             );

                return new Point2(u, v);
            }

            else
            {
                float u = Util.RescaleToRange(Util.ScalarDistance(center.Y, intersection.Y),
                                              -Length * 0.5f,
                                              Length  * 0.5f,
                                              0,
                                              1
                                             );

                float v = Util.RescaleToRange(Util.ScalarDistance(center.X, intersection.X),
                                              -Width * 0.5f,
                                              Width  * 0.5f,
                                              0,
                                              1
                                             );

                return new Point2(u, v);
            }
        }


        public override float Intersect(Ray ray, Point3 center)
        {
            float denominator = Vec3.Dot(Orientation, ray.Direction);


            // If the denominator is 0, the ray and plane are parallel
            if (Math.Abs(denominator) < float.Epsilon)
            {
                // Ray does not intersect with plane
                return -1f;
            }

            float t = Vec3.Dot(Util.FromAToB(ray.Origin, center), Orientation) / denominator;

            if (t < float.Epsilon)
            {
                // Ray intersects, but is shot away from the plane
                return -1f;
            }


            // Ray intersects with the infinite plane, check if it also intersects with the bounded plane
            Point3 intersection = ray.At(t);


            if (!IsInBounds(center, intersection))
            {
                return -1f;
            }


            return t;
        }
    }
}