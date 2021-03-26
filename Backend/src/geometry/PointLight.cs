using System.Drawing;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class PointLight
    {
        #region Properties

        public Point3 Position { get; set; }

        public Color Color { get; set; }

        // TODO: Remove, Brightness is encoded in the Color
        public float Brightness { get; set; }

        #endregion

        #region Constructors

        public PointLight(Point3 position, Color color, float brightness)
        {
            Position   = position;
            Color      = color;
            Brightness = brightness;
        }


        /// Brightness is 100%
        public PointLight(Point3 position, Color color)
        {
            Position   = position;
            Color      = color;
            Brightness = 1f;
        }


        /// Color is white
        public PointLight(Point3 position, float brightness)
        {
            Position   = position;
            Color      = Color.White;
            Brightness = brightness;
        }


        /// Color is white, brightness is 100%
        public PointLight(Point3 position)
        {
            Position   = position;
            Color      = Color.White;
            Brightness = 1f;
        }

        #endregion
    }
}