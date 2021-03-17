using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public abstract class Oriented : Shape
    {
        #region Properties

        public Vec3 Orientation { get; set; }

        #endregion
    }
}