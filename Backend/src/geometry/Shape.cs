using System.Numerics;


namespace LEA_2021
{
    public abstract class Shape
    {
        public abstract Vector3? Intersect(Ray ray, Vector3 center);
    }
}