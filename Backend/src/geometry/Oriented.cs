using System.Numerics;


namespace LEA_2021
{
    public abstract class Oriented : Shape
    {
        #region Properties

        public Vector<float> Orientation { get; set; }

        #endregion
    }
}