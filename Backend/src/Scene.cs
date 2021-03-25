#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Json;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;
    using Point2 = Vector2;


    public class Scene : INotifyPropertyChanged
    {
        #region Fields

        private readonly float specularExponent = 200f;

        #endregion

        #region Properties

        public string Name { get; set; }

        public Metadata Metadata { get; set; }

        public List<Object> Objects { get; set; }

        public Bitmap Image { get; set; }

        public int Percentage { get; set; }

        public Camera Camera { get; set; }

        public static Random Rng { get; set; }

        public List<PointLight> PointLights { get; set; }

        public Color BackgroundColor { get; set; }

        public string backgroundValue { get; set; }

        #endregion

        #region Constructors

        // Default background color is black
        public Scene(Metadata metadata, Camera camera)
        {
            Metadata = metadata;
            Objects  = new List<Object>();
            Image    = new Bitmap(metadata.Width, metadata.Height);
            Camera   = camera;

            Name            = "Untitled";
            backgroundValue = "Black";
            SetBackground(Color.Black);
            PointLights     = new List<PointLight>();
            Rng             = new Random();
            BackgroundColor = Color.Black;
        }


        // Default-constructed camera with black background
        public Scene(Metadata metadata)
        {
            Metadata = metadata;
            Objects  = new List<Object>();
            Image    = new Bitmap(metadata.Width, metadata.Height);
            Camera   = new Camera();

            Name            = "Untitled";
            backgroundValue = "Black";
            SetBackground(Color.Black);
            PointLights     = new List<PointLight>();
            Rng             = new Random();
            BackgroundColor = Color.Black;
        }


        public Scene(string configName)
        {
            JsonValue value = JsonValue.Parse(File.ReadAllText($@"../../../../Backend/scenes/{configName}.json"));
            PointLights = new List<PointLight>();
            Name        = configName;

            Metadata = new Metadata((int) value["Metadata"]["Width"],
                                    (int) value["Metadata"]["Height"],
                                    (int) value["Metadata"]["Num_Iterations"]
                                   );

            Image = new Bitmap(Metadata.Width, Metadata.Height);

            Camera = new Camera(new Point3(value["Camera"]["Position"]["X"],
                                           value["Camera"]["Position"]["Y"],
                                           value["Camera"]["Position"]["Z"]
                                          ),
                                new Vec3(value["Camera"]["Direction"]["X"],
                                         value["Camera"]["Direction"]["Y"],
                                         value["Camera"]["Direction"]["Z"]
                                        ),
                                Util.DegreesToRadians((int) value["Camera"]["FOV"])
                               );


            // create object classes
            Objects = new List<Object>();

            foreach (JsonValue obj in value["Objects"])
            {
                Shape shapeClass = null;

                switch ((string) obj["Shape"])
                {
                    case "Cuboid":
                        shapeClass = new Cuboid((int) obj["Properties"]["Width"],
                                                (int) obj["Properties"]["Height"],
                                                (int) obj["Properties"]["Length"],
                                                new Vec3(obj["Properties"]["Orientation"]["X"],
                                                         obj["Properties"]["Orientation"]["Y"],
                                                         obj["Properties"]["Orientation"]["Z"]
                                                        )
                                               );

                        break;

                    case "Sphere":
                        shapeClass = new Sphere((float) obj["Properties"]["Radius"]);

                        break;

                    case "FinitePlane":
                        shapeClass = new FinitePlane(new Vec3(obj["Properties"]["Orientation"]["X"],
                                                              obj["Properties"]["Orientation"]["Y"],
                                                              obj["Properties"]["Orientation"]["Z"]
                                                             ),
                                                     (int) obj["Properties"]["Width"],
                                                     (int) obj["Properties"]["Length"]
                                                    );

                        break;
                }

                Objects.Add(new Object(new Material(obj["Material"]),
                                       shapeClass,
                                       new Vector3(obj["Position"]["X"],
                                                   obj["Position"]["Y"],
                                                   obj["Position"]["Z"]
                                                  ),
                                       obj["Name"]
                                      )
                           );
            }

            foreach (JsonValue obj in value["PointLights"])
            {
                PointLights.Add(new PointLight(new Vector3(obj["Position"]["X"],
                                                           obj["Position"]["Y"],
                                                           obj["Position"]["Z"]
                                                          ),
                                               Color.FromName(obj["Color"]),
                                               (float) obj["Brightness"]
                                              )
                               );
            }

            Rng             = new Random();
            backgroundValue = value["Background"];
            DetectBackground();
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;


        public void DetectBackground()
        {
            if (Regex.IsMatch(backgroundValue, @"^#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$"))
                // Background is hex color
            {
                SetBackground(ColorTranslator.FromHtml(backgroundValue));
            }
            else if (Regex.IsMatch(backgroundValue, @"[A-Za-z0-9 -_\/]*[\/.](gif|jpg|jpeg|tiff|png)$"))
                // background is path to image
            {
                SetBackground(backgroundValue);
            }
            else
                // background must be color word
            {
                SetBackground(Color.FromName(backgroundValue));
            }
        }


        public void SetBackground(string image_path)
        {
            Image = new Bitmap(System.Drawing.Image.FromFile(image_path));
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public override string ToString()
        {
            return $"{Name}";
        }


        public void SetBackground(Color color)
        {
            for (int i = 0; i < Image.Width; i++)
            {
                for (int j = 0; j < Image.Height; j++)
                {
                    Image.SetPixel(i, j, color);
                }
            }
        }


        // Refraction according to snell's law
        public Vec3 Refract(
            Vec3  direction,
            Vec3  surfaceNormal,
            float n2,
            float n1 = 1f
        )
        {
            // TODO: Debug with center pixel of screen
            float cos_i = Vec3.Dot(-surfaceNormal, direction);

            // Ray is inside of the object, invert the refraction
            if (cos_i < 0)
            {
                // Console.WriteLine("Leaving material");
                surfaceNormal = -surfaceNormal;

                Util.Swap(ref n1, ref n2);
            }
            else
            {
                cos_i = -cos_i;
            }

            float refractiveRatio = n1 / n2;
            float radicand        = 1f - Util.Square(refractiveRatio) * (1 - Util.Square(cos_i));

            // Internal reflection, ray won't leave object
            if (radicand < 0)
            {
                // Console.WriteLine("Internal reflection");
                return Vec3.Reflect(direction, surfaceNormal);
            }

            float cos_o = (float) Math.Sqrt(radicand);

            Vec3 refracted = refractiveRatio * direction + surfaceNormal * (refractiveRatio * cos_i - cos_o);

            return Vec3.Normalize(refracted);
        }


        private HitRecord FindClosestHit(Ray ray)
        {
            Object closestObject   = null;
            float  closestDistance = float.PositiveInfinity;

            foreach (var currentObject in Objects)
            {
                float currentDistance = currentObject.Intersect(ray);

                // We hit an object
                if (currentDistance > float.Epsilon && currentDistance < closestDistance)
                {
                    closestObject   = currentObject;
                    closestDistance = currentDistance;
                }
            }

            return new HitRecord(closestObject, closestDistance);
        }


        // TODO: Implement reflections
        private Color TraceRay(LightBeam lightBeam, Color currentColor, int currentDepth = 0)
        {
            // // Russian roulette as base case 
            // if (currentDepth > 5 && Rng.NextDouble() >= lightBeam.Brightness)
            // {
            //     return currentColor;
            // }
            //
            // lightBeam.Brightness *= 1 / lightBeam.Brightness;

            if (currentDepth > Constants.MaxReflectionDepth)
            {
                return currentColor;
            }
            // TODO: Generate random ray


            HitRecord closestHit      = FindClosestHit(lightBeam.Ray);
            Object?   closestObject   = closestHit.Object;
            float     closestDistance = closestHit.Distance;


            if (!float.IsPositiveInfinity(closestDistance) && closestObject is not null)
            {
                Vector3 intersection = lightBeam.Ray.At(closestDistance);


                Vector3 surfaceNormal =
                    closestObject.GetSurfaceNormal(intersection);


                float brightnessDiffuse  = 0f;
                float brightnessSpecular = 0f;


                // Calculate direct illumination
                // Raise point above objects surface to prevent self-intersection
                Vector3 intersectionOffsetShadow = intersection + Constants.ShadowOffset * surfaceNormal;

                foreach (var pointLight in PointLights)
                {
                    Vector3 objectToLight  = Vec3.Normalize(Util.FromAToB(closestObject.Position, pointLight.Position));
                    Vector3 surfaceToLight = Util.FromAToB(intersection, pointLight.Position);


                    float distanceToLight = Vec3.Distance(intersection, pointLight.Position);

                    // Cast shadows by not adding specular or diffuse light if path to light is not clear
                    closestDistance = FindClosestHit(new Ray(intersectionOffsetShadow, Vec3.Normalize(surfaceToLight)))
                       .Distance;

                    if (closestDistance
                      < distanceToLight)
                    {
                        continue;
                    }

                    // TODO: Build diffuse color instead
                    // Lambertian diffuse lighting
                    brightnessDiffuse +=
                        pointLight.Brightness * Math.Max(0f, Vec3.Dot(objectToLight, surfaceNormal));

                    // Blinn-Phong specular reflection
                    Vector3 surfaceToCamera = Util.FromAToB(intersection, Camera.Position);

                    Vector3 halfVector  = Vec3.Normalize(surfaceToLight + surfaceToCamera);
                    float   facingRatio = Vec3.Dot(halfVector, surfaceNormal);

                    brightnessSpecular += (float) Math.Pow(Math.Max(0f, facingRatio), specularExponent);
                }

                Vector2 uv     = closestObject.GetUvCoordinates(intersection, closestObject.Position);
                Bitmap  albedo = closestObject.Material.Albedo;

                Color colorOrig =
                    albedo.GetPixel((int) (uv.X * (albedo.Width - 1)), (int) (uv.Y * (albedo.Height - 1)));

                Bitmap roughness = closestObject.Material.Roughness;

                // Since the roughness map is monochrome, we can pick any of its color channels
                int roughnessRaw = roughness.GetPixel((int) uv.X * (roughness.Width  - 1),
                                                      (int) uv.Y * (roughness.Height - 1)
                                                     )
                                            .R;

                // Gloss is opposite of roughness
                float gloss = 1f
                            - Util.RescaleToRange(roughnessRaw, 0f, 255f, 0f, 1f);

                Vector3 directionOrig = lightBeam.Ray.Direction;

                lightBeam.Ray.Origin    = intersectionOffsetShadow;
                lightBeam.Ray.Direction = Vec3.Reflect(directionOrig, surfaceNormal);

                Color reflectionColor = TraceRay(lightBeam, currentColor, currentDepth + 1);

                Vec3 intersectionOffsetRefraction = intersection - 1 * surfaceNormal;
                lightBeam.Ray.Origin = intersectionOffsetRefraction;

                lightBeam.Ray.Direction = Refract(directionOrig,
                                                  surfaceNormal,
                                                  closestObject.Material.RefractiveIndex
                                                 );

                Color refractionColor = TraceRay(lightBeam, currentColor, currentDepth + 1);

                currentColor = Util.ClampedColorScale(colorOrig, 1 - closestObject.Material.Transparency);

                currentColor = Util.ClampedColorScale(currentColor, brightnessDiffuse);

                currentColor =
                    Util.ClampedColorAdd(currentColor, Util.ClampedColorScale(currentColor, brightnessSpecular));


                currentColor =
                    Util.ClampedColorAdd(currentColor, Util.ClampedColorScale(reflectionColor, gloss));

                currentColor =
                    Util.ClampedColorAdd(currentColor,
                                         Util.ClampedColorScale(refractionColor, closestObject.Material.Transparency)
                                        );

                // // Gamma correction
                // color = Color.FromArgb(255,
                //               (int)(255f * Math.Pow(color.R/255f, 1f/2.2f)),
                //               (int)(255f * Math.Pow(color.G/255f, 1f/2.2f)),
                //               (int)(255f * Math.Pow(color.B/255f, 1f/2.2f))
                //              );
            }
            else
            {
                currentColor = Color.FromName(backgroundValue);
            }

            // return TraceRay(lightBeam, currentColor, currentDepth + 1);
            return currentColor;
        }


        private Ray CastPrimaryRay(int row, int column)
        {
            // Build Normalized Device Coordinates (NDC)
            // Value range is [0,1], 0.5f makes points appear in the center of a pixel
            float ndcX = (row    + 0.5f) / Metadata.Width;
            float ndcY = (column + 0.5f) / Metadata.Height;

            // Convert NDC to Screen space by remapping increasing x values to the range [-1,1]
            // and increasing y-values to the range [1,1-]
            float screenX = 2f * ndcX - 1f;
            float screenY = 1f        - 2f * ndcY;

            // Because the image is not square (Usually images are wider than they are high),
            // pixels are now rectangular.
            // Making them square again can be achieved by multiplying the x-values by the images aspect ratio
            screenX *= Metadata.GetAspectRatio();

            // Transform to camera space by accounting for field of view
            float cameraX = screenX * (float) Math.Tan(Camera.Fov / 2f);
            float cameraY = screenY * (float) Math.Tan(Camera.Fov / 2f);

            // For now, the camara always faces forward, hence we set the z-component to -1
            Vector3 rayDirection = Vec3.Normalize(Util.FromAToB(Camera.Position,
                                                                new Vector3(cameraX,
                                                                            cameraY,
                                                                            -1f
                                                                           )
                                                               )
                                                 )
                ;

            return new Ray(Camera.Position, rayDirection);
        }


        private void RenderSingleThreaded()
        {
            DetectBackground();
            Percentage = 0;

            for (int i = 0; i < Metadata.NumIterations; ++i)
            {
                Percentage = Convert.ToInt32(i / (float) Metadata.NumIterations * 100);
                OnPropertyChanged("Percentage");

                for (int column = 0; column < Metadata.Height; ++column)
                {
                    for (int row = 0; row < Metadata.Height; ++row)
                    {
                        Ray primaryRay = CastPrimaryRay(column, row);

                        Color pixel = TraceRay(new LightBeam(primaryRay), new Color());
                        Image.SetPixel(column, row, pixel);
                    }
                }

                OnPropertyChanged("Image");
            }

            Image.Save($"{Constants.OutputDir}/{Name}.png", ImageFormat.Png);
        }


        private void RenderMultiThreaded()
        {
            DetectBackground();
            Percentage = 0;

            for (int i = 0; i < Metadata.NumIterations; ++i)
            {
                Percentage = Convert.ToInt32(i / (float) Metadata.NumIterations * 100);
                OnPropertyChanged("Percentage");

                Parallel.For(0,
                             Metadata.Width,
                             column =>
                             {
                                 for (int row = 0; row < Metadata.Height; ++row)
                                 {
                                     Ray primaryRay = CastPrimaryRay(column, row);

                                     Color pixel = TraceRay(new LightBeam(primaryRay), new Color());
                                     Image.SetPixel(column, row, pixel);
                                 }
                             }
                            );

                OnPropertyChanged("Image");
            }

            Image.Save($"{Constants.OutputDir}/{Name}.png", ImageFormat.Png);
        }


        public void Render(bool singleThreaded = false)
        {
            if (singleThreaded)
            {
                RenderSingleThreaded();
            }
            else
            {
                RenderMultiThreaded();
            }
        }


        public void Save()
        {
            Dictionary<dynamic, dynamic>       jsonData       = new();
            List<Dictionary<dynamic, dynamic>> objectList     = new();
            List<Dictionary<dynamic, dynamic>> pointLightList = new();

            jsonData.Add("Background", backgroundValue);

            jsonData.Add("Metadata",
                         new Dictionary<string, int>
                         {
                             {"Width", Metadata.Width},
                             {"Height", Metadata.Height},
                             {"Num_Iterations", Metadata.NumIterations}
                         }
                        );

            jsonData.Add("Camera",
                         new Dictionary<string, dynamic>
                         {
                             {"Position", Camera.Position},
                             {"Direction", Camera.Direction},
                             {"FOV", Util.RadiansToDegree(Camera.Fov)}
                         }
                        );

            foreach (Object o in Objects)
            {
                objectList.Add(new Dictionary<dynamic, dynamic>
                               {
                                   {"Name", o.Name},
                                   {"Shape", o.Shape.GetType().Name},
                                   {"Material", o.Material.Name},
                                   {"Properties", o.Shape},
                                   {"Position", o.Position}
                               }
                              );
            }

            jsonData.Add("Objects", objectList);


            foreach (PointLight light in PointLights)
            {
                pointLightList.Add(new Dictionary<dynamic, dynamic>
                                   {
                                       {"Color", light.Color},
                                       {"Brightness", light.Brightness},
                                       {"Position", light.Position}
                                   }
                                  );
            }

            jsonData.Add("PointLights", pointLightList);


            File.WriteAllText($@"{Constants.SceneDir}/{Name}.json",
                              JsonConvert.SerializeObject(jsonData, Formatting.Indented)
                             );
        }
    }
}