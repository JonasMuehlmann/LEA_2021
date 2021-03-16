using System.Numerics;


namespace LEA_2021
{
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

        #endregion


        public override Vector<float>? Intersect(Ray ray, Vector<float> center)
        {
            throw new System.NotImplementedException();
        }
    }
}