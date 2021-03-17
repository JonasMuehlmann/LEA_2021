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
         * Calculate intersection point given the following quadratic formula:
         */
        public override Vec3? Intersect(Ray ray, Point3 center)
        {
            var R_D      = ray.Direction;
            var CP       = center - ray.Origin;
            var CpDotR_D = Vec3.Dot(CP, R_D);

            float discriminant = Util.Square(CpDotR_D)
                               - (Vec3.Dot(R_D, R_D) * Vec3.Dot(CP, CP)
                                - Util.Square(Radius));


            if (discriminant < 0)
            {
                return null;
            }


            var   discriminant_squared = (float) Math.Sqrt(discriminant);
            float t1                   = CpDotR_D + discriminant_squared / Vec3.Dot(R_D, R_D);
            float t2                   = CpDotR_D - discriminant_squared / Vec3.Dot(R_D, R_D);

            float closest_t    = t1 < t2 ? t1 : t2;
            Vec3  intersection = ray.Origin + closest_t * R_D;

            return intersection;
        }
    }
}