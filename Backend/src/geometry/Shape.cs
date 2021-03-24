using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;
    using Point2 = Vector2;


    public abstract class Shape
    {
        public abstract float Intersect(Ray ray, Point3 center);

        public abstract Point2 GetUvCoordinates(Point3 intersection, Point3 position);

        public abstract Vec3 GetSurfaceNormal(Point3 intersection, Point3 position);
    }
}