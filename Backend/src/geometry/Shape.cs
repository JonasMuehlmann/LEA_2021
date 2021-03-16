using System.Numerics;


namespace LEA_2021
{
    public abstract class Shape
    {
        public abstract Vector<float>? Intersect(Ray ray, Vector<float> center);
    }
}