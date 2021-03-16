using System.Numerics;


namespace LEA_2021
{
    public class Rectangle : Oriented
    {
        #region Properties

        public int Width { get; set; }

        public int Height { get; set; }

        public int Length { get; set; }

        #endregion

        #region Constructors

        public Rectangle(int width, int height, int length, Vector<float> orientation)
        {
            Width       = width;
            Height      = height;
            Length      = length;
            Orientation = orientation;
        }

        #endregion


        public override Vector<float>? Intersect(Ray ray, Vector<float> center)
        {
            throw new System.NotImplementedException();
        }
    }
}