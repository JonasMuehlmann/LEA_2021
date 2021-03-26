using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;
    using Point2 = Vector2;


    public class Object
    {
        #region Properties

        public string Name { get; set; }

        public Material Material { get; set; }

        public Shape Shape { get; set; }

        public Point3 Position { get; set; }

        #endregion

        #region Constructors

        public Object(Material material, Shape shape, Point3 position, string name)
        {
            Material = material;
            Shape    = shape;
            Position = position;
            Name     = name;
        }


        public Object(Material material, Shape shape, Point3 position)
        {
            Material = material;
            Shape    = shape;
            Position = position;
        }


        /// Centered at origin
        public Object(Material material, Shape shape)
        {
            Material = material;
            Shape    = shape;
            Position = Vec3.Zero;
        }

        #endregion


        public override string ToString()
        {
            return $"{Name} (Material: {Material.Name})";
        }


        /// <summary>
        ///     Make an intersection test with the ray and this object
        /// </summary>
        /// <param name="ray">A ray to test for intersection with the object</param>
        /// <returns>A value >0 representing the distance to intersection along the ray or -1f if no intersection occurs</returns>
        public float Intersect(Ray ray)
        {
            return Shape.Intersect(ray, Position);
        }


        /// <summary>
        ///     Get the UV coordinates of the surfacePoint
        /// </summary>
        /// <param name="surfacePoint">A point on the surface of the object</param>
        /// <returns>UV-coordinates of the surfacePoint</returns>
        public Vector2 GetUvCoordinates(Vector3 surfacePoint)
        {
            return Shape.GetUvCoordinates(surfacePoint, Position);
        }


        /// <summary>
        ///     Get the surface normal at the given surfacePoint
        /// </summary>
        /// <param name="surfacePoint">A point on the surface of the object</param>
        /// <returns>The surface normal at the surfacePoint</returns>
        public Vec3 GetSurfaceNormal(Point3 surfacePoint)
        {
            return Shape.GetSurfaceNormal(surfacePoint, Position);
        }
    }
}