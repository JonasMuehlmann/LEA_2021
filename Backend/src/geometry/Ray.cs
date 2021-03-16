using System.Numerics;


namespace LEA_2021
{
    public class Ray
    {
        #region Properties

        public Vector<float> Origin { get; }

        public Vector<float> Direction { get; }

        #endregion

        #region Constructors

        public Ray(Vector<float> origin, Vector<float> direction)
        {
            Origin    = origin;
            Direction = direction;
        }

        #endregion
    }
}