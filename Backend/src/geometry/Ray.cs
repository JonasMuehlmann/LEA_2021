using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Ray
    {
        #region Properties

        public Vec3 Origin { get; set; }

        public Vec3 Direction { get; set; }

        #endregion

        #region Constructors

        public Ray(Vec3 origin, Vec3 direction)
        {
            Origin    = origin;
            Direction = direction;
        }


        // Originates in global origin  (0,0,0)
        public Ray(Vec3 direction)
        {
            Origin    = Vec3.Zero;
            Direction = direction;
        }

        #endregion


        public Point3 At(float t)
        {
            return Origin + t * Direction;
        }
    }
}