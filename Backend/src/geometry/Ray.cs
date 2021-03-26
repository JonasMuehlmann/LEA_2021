using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Ray
    {
        #region Properties

        public Point3 Origin { get; set; }

        public Vec3 Direction { get; set; }

        #endregion

        #region Constructors

        public Ray(Point3 origin, Vec3 direction)
        {
            Origin    = origin;
            Direction = direction;
        }


        /// Originates in global origin  (0,0,0)
        public Ray(Vec3 direction)
        {
            Origin    = Vec3.Zero;
            Direction = direction;
        }

        #endregion


        /// <summary>
        ///     Get the point at Origin + t * Direction
        /// </summary>
        /// <param name="t">A distance to go along the ray</param>
        /// <returns>The point t units along the ray</returns>
        public Point3 At(float t)
        {
            return Origin + t * Direction;
        }
    }
}