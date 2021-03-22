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


        // TODO: Fix
        // Return UV coordinates in the range of [0,1]
        public static Point2 GetUvCoordinates(Point3 surfacePoint, Vec3 centerPosition)
        {
            // Vec3 surfaceNormal = Vec3.Normalize(centerPosition - surfacePoint);
            var surfaceNormal = Vec3.Normalize(surfacePoint - centerPosition);

            var u = (float) (0.5f + Math.Atan2(-surfaceNormal.Z, surfaceNormal.X) / (2f * Math.PI));
            var v = (float) (0.5f - Math.Asin(surfaceNormal.Y)                    / Math.PI);
            // float u = (float) ((Math.Atan(-surfaceNormal.X / -surfaceNormal.Z) +  Math.PI / 2f) / Math.PI);
            // float v = (float) (Math.Acos(-surfaceNormal.Y) / Math.PI);

            // float u =  (float)((Math.Atan2(-surfaceNormal.Z, surfaceNormal.X)+ Math.PI) / (2f * Math.PI));
            // float v =  (float)(Math.Acos(-surfaceNormal.Y / surfaceNormal.Length()) / Math.PI);

            //float u = Util.RescaleToRange((float)Math.Atan2(-surfaceNormal.Z, surfaceNormal.X), (float)-Math.PI, (float)Math.PI, 0f, 1f);
            return new Point2(u, v);
        }


        /// Calculate intersection point given the quadratic formula
        public override float Intersect(Ray ray, Point3 center)
        {
            var CO = Util.FromAToB(ray.Origin, center);

            var tca = Vec3.Dot(ray.Direction, CO);
            var d2  = Vector3.Dot(CO, CO) - Util.Square(tca);

            // No intersection
            if (d2 > Util.Square(Radius))
            {
                return -1f;
            }


            var thc = (float) Math.Sqrt(Util.Square(Radius) - d2);

            var t1 = tca - thc;
            var t2 = tca + thc;

            // if (t1 < 0)
            // {
            //     t1 = t2;
            // }
            // if (t1 < 0)
            // {
            //     return -1f;
            // }
            // else
            // {
            //     return t1;
            // }

            // // Original
            // float t1 = (-b + discriminantSqrt) / (2f * a);
            // float t2 = (-b - discriminantSqrt) / (2f * a);

            // float t1 = -(-b + discriminantSqrt) / (2f * a);
            // float t2 = -(-b - discriminantSqrt) / (2f * a);

            // // New
            // float q = -0.5f * (b + Math.Sign(b) * discriminantSqrt);
            //
            // float t1 = q / a;
            // float t2 = c / q;

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
            var closestT = t1 < t2 ? t1 : t2;

            return closestT;
        }
    }
}