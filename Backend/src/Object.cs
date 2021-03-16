using System.Numerics;


namespace LEA_2021
{
    class Object
    {
        #region Properties

        public Material Material { get; set; }

        public Shape Shape { get; set; }

        public Vector3 Position { get; set; }

        #endregion

        #region Constructors

        public Object(Material material, Shape shape, Vector3 position)
        {
            Material = material;
            Shape    = shape;
            Position = position;
        }

        #endregion


        public Vector3? Intersect(Ray ray)
        {
            return Shape.Intersect(ray, Position);
        }
    }
}