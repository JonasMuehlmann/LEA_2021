using System;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Sphere : Shape
    {
        #region Properties

        public float Radius { get; set; }

        #endregion

        #region Constructors

        public Sphere(float radius)
        {
            Radius = radius;
        }


        // Unit sphere
        public Sphere()
        {
            Radius = 1f;
        }

        #endregion


        /*
         * Calculate intersection point given the quadratic formula
         */
        public override Vec3? Intersect(Ray ray, Point3 center)
        {
            float a            = Vec3.Dot(ray.Direction, ray.Direction);
            float b            = 2 * Vec3.Dot(center - ray.Origin, ray.Direction);
            float c            = Vec3.Dot(center     - ray.Origin, center - ray.Origin) - Util.Square(Radius);
            float discriminant = Util.Square(b)                                         - 4 * a * c;

            // No intersection
            if (discriminant < 0)
            {
                return null;
            }


            var discriminant_sqrt = (float) Math.Sqrt(discriminant);
            // float t1                = CpDotR_D + discriminant_sqrt / Vec3.Dot(ray.Direction, ray.Direction);
            // float t2                = CpDotR_D - discriminant_sqrt / Vec3.Dot(ray.Direction, ray.Direction);
            float t1 = -b + discriminant_sqrt / 2 * a;
            float t2 = -b - discriminant_sqrt / 2 * a;

            // Ray intersects, but is shot away from sphere
            if (t1 < 0 && t2 < 0)
            {
                return null;
            }

            // Ray intersects but is shot from within sphere
            if (t1 < 0 || t2 < 0)
            {
                return null;
            }
            // Both t are positive, intersection is valid
            else
            {
                // Line can intersect sphere twice (at the front and at the back)
                // only the closer one is visible
                float closest_t    = t1 < t2 ? t1 : t2;
                Vec3  intersection = ray.Origin + closest_t * ray.Direction;

                return intersection;
            }
        }
    }
}