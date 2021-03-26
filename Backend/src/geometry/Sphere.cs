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


        /// Unit sphere
        public Sphere()
        {
            Radius = 1f;
        }

        #endregion


        /// <summary>
        ///     Get the UV coordinates of the surfacePoint
        /// </summary>
        /// <param name="surfacePoint">A point on the surface of the sphere</param>
        /// <param name="position">The center position of the sphere</param>
        /// <returns>UV-coordinates of the surfacePoint</returns>
        public override Point2 GetUvCoordinates(Point3 surfacePoint, Vec3 position)
        {
            Vector3 surfaceNormal = Vec3.Normalize(Util.FromAToB(position, surfacePoint));

            float u = (float) ((Math.PI + Math.Atan2(-surfaceNormal.Z, surfaceNormal.X)) / (2 * Math.PI));
            // Upper bound of the calculation is too low, using magic number and clamping for correction
            u = Math.Clamp(1.77f * u, 0f, 1f);

            float v = (float) (Math.Acos(surfaceNormal.Y) / Math.PI);

            return new Point2(u, v);
        }


        /// <summary>
        ///     Make an intersection test with the ray and this sphere
        /// </summary>
        /// <param name="ray">A ray to test for intersection with the plane</param>
        /// <param name="center">The center position of the sphere</param>
        /// <returns>A value >0 representing the distance to intersection along the ray or -1f if no intersection occurs</returns>
        public override float Intersect(Ray ray, Point3 center)
        {
            // Based on https://github.com/ssloy/tinyraytracer/wiki/Part-1:-understandable-raytracing
            Vector3 Oc = Util.FromAToB(ray.Origin, center);

            float tca = Vec3.Dot(ray.Direction, Oc);
            float d2  = Vector3.Dot(Oc, Oc) - Util.Square(tca);

            // No intersection
            if (d2 > Util.Square(Radius))
            {
                return -1f;
            }


            float thc = (float) Math.Sqrt(Util.Square(Radius) - d2);

            float t1 = tca - thc;
            float t2 = tca + thc;

            // Both intersections are behind the rays origin, the object is opposite of the rays direction
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


        /// <summary>
        ///     Get the surface normal at a given surfacePoint
        /// </summary>
        /// <param name="surfacePoint">A point on the surface of the sphere</param>
        /// <param name="position">The center position of the sphere</param>
        /// <returns>The surface normal at the surfacePoint</returns>
        public override Vec3 GetSurfaceNormal(Point3 surfacePoint, Point3 position)
        {
            return Vec3.Normalize(Util.FromAToB(position, surfacePoint));
        }
    }
}