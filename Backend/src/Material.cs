using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Reflection;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Material
    {
        #region Properties

        public string Name { get; set; }

        public Bitmap Albedo { get; set; }

        public Bitmap Metalness { get; set; }

        public Bitmap Roughness { get; set; }

        public Bitmap AmbientOcclusion { get; set; }

        public Bitmap Normal { get; set; }

        public Bitmap Displacement { get; set; }

        public Bitmap Emission { get; set; }

        #endregion

        #region Constructors

        public Material(
            Bitmap albedo,
            Bitmap metalness,
            Bitmap roughness,
            Bitmap ambientOcclusion,
            Bitmap normal,
            Bitmap displacement,
            Bitmap emission
        )
        {
            Albedo           = albedo;
            Metalness        = metalness;
            Roughness        = roughness;
            AmbientOcclusion = ambientOcclusion;
            Normal           = normal;
            Displacement     = displacement;
            Emission         = emission;
        }


        public Material(string name)
        {
            Name = name;

            List<string> neededBitmaps = new()
                                         {
                                             "albedo",
                                             "metalness",
                                             "roughness",
                                             "ambientOcclusion",
                                             "normal",
                                             "displacement",
                                             "emission"
                                         };

            if (Directory.Exists($"{Constants.SceneDir}/{name}"))
            {
                foreach (string file in Directory.GetFiles($"{Constants.SceneDir}/{name}"))
                {
                    string? bitmapName = Path.GetFileNameWithoutExtension(file);
                    neededBitmaps.Remove(bitmapName);

                    PropertyInfo? propInfo =
                        typeof(Material).GetProperty(char.ToUpper(bitmapName[0]) + bitmapName.Substring(1));

                    propInfo.SetValue(this, Image.FromFile(file), null);
                }
            }


            // iterate non-found bitmaps to set default values
            foreach (string bitmap in neededBitmaps)
            {
                PropertyInfo? propInfo = typeof(Material).GetProperty(char.ToUpper(bitmap[0]) + bitmap.Substring(1));
                propInfo.SetValue(this, new Bitmap(1, 1), null);
            }
        }

        #endregion


        // TODO: Find sensible defaults for texture maps to implement constructor with default maps
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}