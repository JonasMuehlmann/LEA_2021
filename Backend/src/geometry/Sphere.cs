using System;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;
    using Point2 = Vector2;


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


        // Return UV coordinates in the range of [0,1]
        public override Point2 GetUvCoordinates(Point3 surfacePoint, Vec3 centerPosition)
        {
            Vector3 surfaceNormal = Vec3.Normalize(Util.FromAToB(centerPosition, surfacePoint));

            // Upper bound of the calculation is too low, magic number for correction
            float u = 1.79f * (float) ((Math.PI + Math.Atan2(-surfaceNormal.Z, surfaceNormal.X)) / (2 * Math.PI));

            if (u > 1)
            {
                throw new
                    ApplicationException("Scuffed u term calculation caused invalid result, clamp the result or fix the algorithm"
                                        );
            }

            float v = (float) (Math.Acos(surfaceNormal.Y) / Math.PI);

            return new Point2(u, v);
        }


        /// Calculate intersection point given the quadratic formula
        public override float Intersect(Ray ray, Point3 center)
        {
            Vector3 CO = Util.FromAToB(ray.Origin, center);

            float tca = Vec3.Dot(ray.Direction, CO);
            float d2  = Vector3.Dot(CO, CO) - Util.Square(tca);

            // No intersection
            if (d2 > Util.Square(Radius))
            {
                return -1f;
            }


            float thc = (float) Math.Sqrt(Util.Square(Radius) - d2);

            float t1 = tca - thc;
            float t2 = tca + thc;

            // Both intersections are behind the rays origin, the object is behind the rays direction
            if (t1 < float.Epsilon && t2 < float.Epsilon)
            {
                return -1f;
            }

            // One intersection is behind the rays origin, we must be inside the sphere
            if (t1 < float.Epsilon || t2 < float.Epsilon)
            {
                return -1f;
            }

            // Both intersections are in the correct direction of the ray
            // Line can intersect sphere twice entering and leaving the sphere
            // We only care about the entrance-intersection
            float closestT = t1 < t2 ? t1 : t2;

            return closestT;
        }
    }
}