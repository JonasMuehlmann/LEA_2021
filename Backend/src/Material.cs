// Copyright 2021 Jonas Muehlmann, Tim Dreier
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Json;
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
            float  refractiveIndex,
            float  transparency
        )
        {
            Albedo           = albedo;
            Metalness        = metalness;
            Roughness        = roughness;
            AmbientOcclusion = ambientOcclusion;
            Normal           = normal;
            Displacement     = displacement;
            Emission         = emission;
            RefractiveIndex  = refractiveIndex;
            Transparency     = transparency;
        }


        /// <summary>
        ///     Construct a material from the name of a material configuration.
        ///     Sets a checkerboard pattern for all texture maps that are not found in the given material's directory
        /// </summary>
        /// <param name="name">The name of a material to construct</param>
        public Material(string name)
        {
            JsonValue materialConfig =
                JsonValue.Parse(File.ReadAllText($"{Constants.MaterialsDir}/{name}/config.json"));

            Name            = name;
            RefractiveIndex = materialConfig["refractiveIndex"];
            Transparency    = materialConfig["transparency"];

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

            if (Directory.Exists($"{Constants.MaterialsDir}/{name}"))
            {
                foreach (string file in Directory.GetFiles($"{Constants.MaterialsDir}/{name}"))
                {
                    if (Path.GetExtension(file) == ".json")
                    {
                        continue;
                    }

                    string? bitmapName = Path.GetFileNameWithoutExtension(file);
                    neededBitmaps.Remove(bitmapName);

                    PropertyInfo? propInfo =
                        typeof(Material).GetProperty(char.ToUpper(bitmapName[0]) + bitmapName.Substring(1));

                    propInfo.SetValue(this, Image.FromFile(file), null);
                }
            }


            // Set defaults for missing texture maps
            foreach (string bitmap in neededBitmaps)
            {
                PropertyInfo? propInfo = typeof(Material).GetProperty(char.ToUpper(bitmap[0]) + bitmap.Substring(1));
                propInfo.SetValue(this, new Bitmap($"{Constants.MaterialsDir}/checkerboard.jpg"), null);
            }
        }

        #endregion


        // TODO: Find sensible defaults for texture maps to implement constructor with
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}