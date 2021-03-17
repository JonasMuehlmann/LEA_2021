using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Camera
    {
        #region Properties

        public Point3 Position { get; set; }

        public Vec3 Direction { get; set; }

        // Horizontal Field of View in radians
        public float Fov { get; set; }

        #endregion

        #region Constructors

        public Camera(Point3 position, Vec3 direction, float fov)
        {
            Position  = position;
            Direction = direction;
            Fov       = fov;
        }


        //Looking along Z-axis
        public Camera(float fov, Point3 position)
        {
            Position  = position;
            Direction = Vec3.UnitZ;
            Fov       = fov;
        }


        // Centered at origin
        public Camera(Vec3 direction, float fov)
        {
            Position  = Vec3.Zero;
            Direction = direction;
            Fov       = fov;
        }


        // Centered at origin, looking along Z-axis
        public Camera(float fov)
        {
            Position  = Vec3.Zero;
            Direction = Vec3.UnitZ;
            Fov       = fov;
        }


        // Centered at origin, looking along Z-axis, 60 or 1.0472 rad deg FOV
        public Camera()
        {
            Position  = Vec3.Zero;
            Direction = Vec3.UnitZ;
            Fov       = 1.0472f;
        }

        #endregion
    }
}