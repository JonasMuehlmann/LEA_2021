using System.Numerics;

namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Object
    {
        public Vector3? Intersect(Ray ray)
        {
            return Shape.Intersect(ray, Position);
        }

        public override string ToString()
        {
            return $"{Name} (Material: {Material})";
        }

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
            Shape = shape;
            Position = position;
            Name = name;
        }

        public Object(Material material, Shape shape, Point3 position)
        {
            Material = material;
            Shape = shape;
            Position = position;
        }


        // Centered at origin
        public Object(Material material, Shape shape)
        {
            Material = material;
            Shape = shape;
            Position = Vec3.Zero;
        }

        #endregion
    }
}