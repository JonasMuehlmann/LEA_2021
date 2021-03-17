using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public abstract class Shape
    {
        public abstract Vec3? Intersect(Ray ray, Point3 center);
    }
}