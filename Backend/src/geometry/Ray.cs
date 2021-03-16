using System.Numerics;


namespace LEA_2021
{
    public class Ray
    {
        #region Properties

        public Vector3 Origin { get; }

        public Vector3 Direction { get; }

        #endregion

        #region Constructors

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin    = origin;
            Direction = direction;
        }

        #endregion
    }
}