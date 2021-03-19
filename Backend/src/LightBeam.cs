using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class LightBeam
    {
        #region Properties

        public Ray Ray { get; set; }

        public float Brightness { get; set; }

        #endregion

        #region Constructors

        ///  
        /// LightBeam with 100% brightness
        ///  
        public LightBeam(Ray ray)
        {
            Ray        = ray;
            Brightness = 1f;
        }


        public LightBeam(Ray ray, float brightness)
        {
            Ray        = ray;
            Brightness = brightness;
        }

        #endregion
    }
}