using System.Numerics;


namespace LEA_2021
{
    class Object
    {
        #region Properties

        public Material Material { get; set; }

        public Shape Shape { get; set; }

        public Vector<float> Position { get; set; }

        #endregion

        #region Constructors

        public Object(Material material, Shape shape, Vector<float> position)
        {
            Material = material;
            Shape    = shape;
            Position = position;
        }

        #endregion


        public Vector<float>? Intersect(Ray ray)
        {
            return Shape.Intersect(ray, Position);
        }
    }
}