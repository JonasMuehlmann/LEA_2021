namespace LEA_2021
{
    public class Metadata
    {
        #region Properties

        public int Width { get; set; }

        public int Height { get; set; }

        // Horizontal Field of View in radians
        public int Fov { get; set; }

        public int NumIterations { get; set; }

        #endregion

        #region Constructors

        public Metadata(int width, int height, int fov, int numIterations)
        {
            Width         = width;
            Height        = height;
            Fov           = fov;
            NumIterations = numIterations;
        }

        #endregion


        public float GetAspectRatio()
        {
            return (float) Width / Height;
        }
    }
}