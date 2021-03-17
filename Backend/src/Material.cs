using System.Drawing;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Material
    {
        #region Properties

        public Bitmap Albedo { get; set; }

        public Bitmap Metalness { get; set; }

        public Bitmap Roughness { get; set; }

        public Bitmap AmbientOcclusion { get; set; }

        public Bitmap Normal { get; set; }

        public Bitmap Bump { get; set; }

        public Bitmap Emission { get; set; }

        #endregion

        #region Constructors

        public Material(
            Bitmap albedo,
            Bitmap metalness,
            Bitmap roughness,
            Bitmap ambientOcclusion,
            Bitmap normal,
            Bitmap bump,
            Bitmap emission
        )
        {
            Albedo           = albedo;
            Metalness        = metalness;
            Roughness        = roughness;
            AmbientOcclusion = ambientOcclusion;
            Normal           = normal;
            Bump             = bump;
            Emission         = emission;
        }

        #endregion

        // TODO: Find sensible defaults for texture maps to implement constructor with default maps
    }
}