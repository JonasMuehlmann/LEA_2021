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

        public Rectangle(int width, int height, int length, Vector3 orientation)
        {
            Width       = width;
            Height      = height;
            Length      = length;
            Orientation = orientation;
        }

        #endregion


        public override Vector3? Intersect(Ray ray, Vector3 center)
        {
            throw new System.NotImplementedException();
        }
    }
}