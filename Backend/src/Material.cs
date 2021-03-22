using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;


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


        public Material(string name)
        {
            Name = name;

            var neededBitmaps = new List<string>
                                {
                                    "albedo",
                                    "metalness",
                                    "roughness",
                                    "ambientOcclusion",
                                    "normal",
                                    "bump",
                                    "emission"
                                };

            if (Directory.Exists($"..\\..\\..\\scenes\\materials\\{name}"))
            {
                foreach (var file in Directory.GetFiles($"..\\..\\..\\scenes\\materials\\{name}"))
                {
                    var bitmapName = Path.GetFileNameWithoutExtension(file);
                    neededBitmaps.Remove(bitmapName);

                    var propInfo =
                        typeof(Material).GetProperty(char.ToUpper(bitmapName[0]) + bitmapName.Substring(1));

                    propInfo.SetValue(this, Image.FromFile(file), null);
                }
            }


            // iterate non-found bitmaps to set default values
            foreach (var bitmap in neededBitmaps)
            {
                var propInfo = typeof(Material).GetProperty(char.ToUpper(bitmap[0]) + bitmap.Substring(1));
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