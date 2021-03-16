using System.Numerics;


namespace LEA_2021
{
    public class Camera
    {
        #region Properties

        public Vector3 Position { get; set; }

        public Vector3 Direction { get; set; }

        #endregion

        #region Constructors

        public Camera(Vector3 position, Vector3 direction)
        {
            Position  = position;
            Direction = direction;
        }

        #endregion
    }
}