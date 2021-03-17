using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Plane : Oriented
    {
        #region Properties

        public int Width { get; set; }

        public int Height { get; set; }

        #endregion

        #region Constructors

        // Oriented, rectangular plane
        public Plane(Vec3 orientation, int width, int height)
        {
            Orientation = orientation;
            Width       = width;
            Height      = height;
        }


        // Oriented, square plane
        public Plane(Vec3 orientation, int dimensions)
        {
            Orientation = orientation;
            Width       = dimensions;
            Height      = dimensions;
        }


        // Y-axis aligned, rectangular plane
        public Plane(int width, int height)
        {
            Orientation = Vec3.UnitY;
            Width       = width;
            Height      = height;
        }


        // Y-axis aligned, square plane
        public Plane(int dimensions)
        {
            Orientation = Vec3.UnitY;
            Width       = dimensions;
            Height      = dimensions;
        }

        #endregion


        public override Vec3? Intersect(Ray ray, Point3 center)
        {
            float denominator = Vec3.Dot(Orientation, ray.Direction);

            // Check if denominator is approximately 0
            if (denominator > -0.0001f)
            {
                // Ray does not intersect with plane
                return null;
            }

            float t = Vec3.Dot(center - ray.Origin, Orientation) / denominator;

            return ray.Origin + t * ray.Direction;
        }
    }
}