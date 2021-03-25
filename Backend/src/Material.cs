using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Json;
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

        public Bitmap Displacement { get; set; }

        public Bitmap Emission { get; set; }

        public float RefractiveIndex { get; set; }

        public float Transparency { get; set; }

        #endregion

        #region Constructors

        public Material(
            Bitmap albedo,
            Bitmap metalness,
            Bitmap roughness,
            Bitmap ambientOcclusion,
            Bitmap normal,
            Bitmap displacement,
            Bitmap emission,
            float refractiveIndex,
            float transparency
        )
        {
            Albedo = albedo;
            Metalness = metalness;
            Roughness = roughness;
            AmbientOcclusion = ambientOcclusion;
            Normal = normal;
            Displacement = displacement;
            Emission = emission;
            RefractiveIndex = refractiveIndex;
            Transparency = transparency;
        }


        public Material(string name)
        {
            var materialConfig = JsonValue.Parse(File.ReadAllText($"{Constants.MaterialsDir}/{name}/config.json"));

            Name = name;
            RefractiveIndex = materialConfig["refractiveIndex"];
            Transparency = materialConfig["transparency"];

            List<string> neededBitmaps = new()
            {
                "albedo",
                "metalness",
                "roughness",
                "ambientOcclusion",
                "normal",
                "displacement",
                "displacement",
                "emission"
            };

            if (Directory.Exists($"{Constants.MaterialsDir}/{name}"))
                foreach (var file in Directory.GetFiles($"{Constants.MaterialsDir}/{name}"))
                {
                    if (Path.GetExtension(file) == ".json") continue;

                    var bitmapName = Path.GetFileNameWithoutExtension(file);
                    neededBitmaps.Remove(bitmapName);

                    var propInfo =
                        typeof(Material).GetProperty(char.ToUpper(bitmapName[0]) + bitmapName.Substring(1));

                    propInfo.SetValue(this, Image.FromFile(file), null);
                }


            // iterate non-found bitmaps to set default values
            foreach (var bitmap in neededBitmaps)
            {
                var propInfo = typeof(Material).GetProperty(char.ToUpper(bitmap[0]) + bitmap.Substring(1));
                propInfo.SetValue(this, new Bitmap($"{Constants.MaterialsDir}/checkerboard.jpg"), null);
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