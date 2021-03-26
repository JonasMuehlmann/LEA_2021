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
        #region Properties

        public string Name { get; set; }

        public Metadata Metadata { get; set; }

        public List<Object> Objects { get; set; }

        public Bitmap Image { get; set; }

        public int Progress { get; set; }

        public Camera Camera { get; set; }

        public static Random Rng { get; set; }

        public List<PointLight> PointLights { get; set; }

        public Color BackgroundColor { get; set; }

        public string BackgroundValue { get; set; }

        #endregion

        #region Constructors

        /// Default background color is black
        public Scene(Metadata metadata, Camera camera)
        {
            Metadata = metadata;
            Objects  = new List<Object>();
            Image    = new Bitmap(metadata.Width, metadata.Height);
            Camera   = camera;

            Name            = "Untitled";
            BackgroundValue = "Black";
            SetBackground(Color.Black);
            PointLights     = new List<PointLight>();
            Rng             = new Random();
            BackgroundColor = Color.Black;
        }


        /// Default-constructed camera with black background
        public Scene(Metadata metadata)
        {
            Metadata = metadata;
            Objects  = new List<Object>();
            Image    = new Bitmap(metadata.Width, metadata.Height);
            Camera   = new Camera();

            Name            = "Untitled";
            BackgroundValue = "Black";
            SetBackground(Color.Black);
            PointLights     = new List<PointLight>();
            Rng             = new Random();
            BackgroundColor = Color.Black;
        }


        /// <summary>
        ///     Initialize a scene from a given name
        /// </summary>
        /// <param name="configName">The name of a file in the scenes folder, without a file extension</param>
        public Scene(string configName)
        {
            JsonValue config = JsonValue.Parse(File.ReadAllText($@"{Constants.SceneDir}/{configName}.json"));
            PointLights     = new List<PointLight>();
            Name            = configName;
            Rng             = new Random();
            BackgroundValue = config["Background"];

            Metadata = new Metadata((int) config["Metadata"]["Width"],
                                    (int) config["Metadata"]["Height"],
                                    (int) config["Metadata"]["Num_Iterations"]
                                   );

            Image = new Bitmap(Metadata.Width, Metadata.Height);
            DetectAndSetBackground();

            Camera = new Camera(new Point3(config["Camera"]["Position"]["X"],
                                           config["Camera"]["Position"]["Y"],
                                           config["Camera"]["Position"]["Z"]
                                          ),
                                new Vec3(config["Camera"]["Direction"]["X"],
                                         config["Camera"]["Direction"]["Y"],
                                         config["Camera"]["Direction"]["Z"]
                                        ),
                                Util.DegToRad((int) config["Camera"]["FOV"])
                               );


            Objects = new List<Object>();

            foreach (JsonValue obj in config["Objects"])
            {
                Shape shapeInstance = null;

                switch ((string) obj["Shape"])
                {
                    case "Cuboid":
                        shapeInstance = new Cuboid((int) obj["Properties"]["Width"],
                                                   (int) obj["Properties"]["Height"],
                                                   (int) obj["Properties"]["Length"],
                                                   new Vec3(obj["Properties"]["Orientation"]["X"],
                                                            obj["Properties"]["Orientation"]["Y"],
                                                            obj["Properties"]["Orientation"]["Z"]
                                                           )
                                                  );

                        break;

                    case "Sphere":
                        shapeInstance = new Sphere((float) obj["Properties"]["Radius"]);

                        break;

                    case "FinitePlane":
                        shapeInstance = new FinitePlane(new Vec3(obj["Properties"]["Orientation"]["X"],
                                                                 obj["Properties"]["Orientation"]["Y"],
                                                                 obj["Properties"]["Orientation"]["Z"]
                                                                ),
                                                        (int) obj["Properties"]["Width"],
                                                        (int) obj["Properties"]["Length"]
                                                       );

                        break;
                }

                Objects.Add(new Object(new Material(obj["Material"]),
                                       shapeInstance,
                                       new Vector3(obj["Position"]["X"],
                                                   obj["Position"]["Y"],
                                                   obj["Position"]["Z"]
                                                  ),
                                       obj["Name"]
                                      )
                           );
            }

            foreach (JsonValue obj in config["PointLights"])
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
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;


        /// <summary>
        ///     Detect in what way the background is specified in the scene configuration and set it appropriately.
        ///     The background can be specified as a hex code (eg. #F0F0F0) a color Word (eg. Black) or a path to an image.
        /// </summary>
        private void DetectAndSetBackground()
        {
            // Background is hex color
            if (Regex.IsMatch(BackgroundValue, @"^#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$"))
            {
                SetBackground(ColorTranslator.FromHtml(BackgroundValue));
            }
            // Background is path to image
            else if (Regex.IsMatch(BackgroundValue, @"[A-Za-z0-9 -_\/]*[\/.](gif|jpg|jpeg|tiff|png)$"))
            {
                SetBackground(BackgroundValue);
            }
            // Background must be color word
            else
            {
                SetBackground(Color.FromName(BackgroundValue));
            }
        }


        public void SetBackground(string imagePath)
        {
            Image = new Bitmap(System.Drawing.Image.FromFile(imagePath));
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public override string ToString()
        {
            return Name;
        }


        /// <summary>
        ///     Set all pixels in the background to the specified color
        /// </summary>
        /// <param name="color">A color to set all pixels of the background to</param>
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


        /// <summary>
        ///     Refract a ray according to snell's law
        /// </summary>
        /// <param name="direction">A vector to refract</param>
        /// <param name="surfaceNormal">the surface normal of a point to refract the direction ray from</param>
        /// <param name="n2">The refractive index of the medium to pass through</param>
        /// <param name="n1">The refractive index of the current medium, defaults to air (1.0f)</param>
        /// <returns>A ray refracted of the given surface normal</returns>
        public Vec3 Refract(
            Vec3  direction,
            Vec3  surfaceNormal,
            float n2,
            float n1 = 1f
        )
        {
            float cosIn = Vec3.Dot(-surfaceNormal, direction);

            // Ray is inside of the object, invert the refraction
            if (cosIn < 0)
            {
                surfaceNormal = -surfaceNormal;

                Util.Swap(ref n1, ref n2);
            }
            else
            {
                cosIn = -cosIn;
            }

            float refractiveRatio = n1 / n2;
            float radicand        = 1f - Util.Square(refractiveRatio) * (1 - Util.Square(cosIn));

            // Internal reflection, ray won't leave object
            if (radicand < 0)
            {
                return Vec3.Reflect(direction, surfaceNormal);
            }

            float cosOut = (float) Math.Sqrt(radicand);

            Vec3 refracted = refractiveRatio * direction + surfaceNormal * (refractiveRatio * cosIn - cosOut);

            return Vec3.Normalize(refracted);
        }


        /// <summary>
        ///     Make an intersection test with all objects
        /// </summary>
        /// <param name="ray">A ray to test for intersection with the scenes objects</param>
        /// <returns>
        ///     A hitrecord of the closest hit,
        ///     it's closestObject will be null and it's closestDistance will be positive infinity,
        ///     if the ray did not intersect with an object
        /// </returns>
        private HitRecord FindClosestHit(Ray ray)
        {
            Object closestObject   = null;
            float  closestDistance = float.PositiveInfinity;

            foreach (var currentObject in Objects)
            {
                float currentDistance = currentObject.Intersect(ray);

                // We hit an object and it is closer than the previously closest one
                if (currentDistance > float.Epsilon && currentDistance < closestDistance)
                {
                    closestObject   = currentObject;
                    closestDistance = currentDistance;
                }
            }

            return new HitRecord(closestObject, closestDistance);
        }


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

            HitRecord closestHit      = FindClosestHit(lightBeam.Ray);
            Object?   closestObject   = closestHit.Object;
            float     closestDistance = closestHit.Distance;


            // Nothing hit
            if (float.IsPositiveInfinity(closestDistance) || closestObject is null)
            {
                return Color.FromName(BackgroundValue);
            }

            Vector3 intersection = lightBeam.Ray.At(closestDistance);

            Vector3 surfaceNormal =
                closestObject.GetSurfaceNormal(intersection);

            // Raise point above objects surface to prevent self-intersection
            Vector3 intersectionOffsetShadow = intersection + Constants.ShadowOffset * surfaceNormal;

            // Calculate direct illumination
            float brightnessDiffuse  = 0f;
            float brightnessSpecular = 0f;

            foreach (var pointLight in PointLights)
            {
                Vector3 objectToLight  = Vec3.Normalize(Util.FromAToB(closestObject.Position, pointLight.Position));
                Vector3 surfaceToLight = Util.FromAToB(intersection, pointLight.Position);


                // Cast shadows by not adding specular or diffuse light if path to light is not clear
                closestDistance = FindClosestHit(new Ray(intersectionOffsetShadow, Vec3.Normalize(surfaceToLight)))
                   .Distance;

                if (closestDistance
                  < surfaceToLight.Length())
                {
                    continue;
                }

                // Lambertian diffuse lighting
                brightnessDiffuse +=
                    pointLight.Brightness * Math.Max(0f, Vec3.Dot(objectToLight, surfaceNormal));

                // Blinn-Phong specular reflection
                Vector3 surfaceToCamera = Util.FromAToB(intersection, Camera.Position);

                Vector3 halfVector  = Vec3.Normalize(surfaceToLight + surfaceToCamera);
                float   facingRatio = Vec3.Dot(halfVector, surfaceNormal);

                brightnessSpecular += (float) Math.Pow(Math.Max(0f, facingRatio), Constants.SpecularExponent);
            }

            Vector2 uv              = closestObject.GetUvCoordinates(intersection);
            Bitmap  albedo          = closestObject.Material.Albedo;
            Bitmap  roughness       = closestObject.Material.Roughness;
            float   refractiveIndex = closestObject.Material.RefractiveIndex;
            float   transparency    = closestObject.Material.Transparency;

            Color colorOrig =
                albedo.GetPixel((int) (uv.X * (albedo.Width - 1)), (int) (uv.Y * (albedo.Height - 1)));


            // Since the roughness map is monochrome, we can pick any of its color channels
            int roughnessRaw = roughness.GetPixel((int) uv.X * (roughness.Width  - 1),
                                                  (int) uv.Y * (roughness.Height - 1)
                                                 )
                                        .R;

            // Gloss is opposite of roughness
            float gloss = 1f
                        - Util.Normalize(roughnessRaw, 0f, 255f);

            Vector3 directionOrig = lightBeam.Ray.Direction;

            // Calculate shadows
            lightBeam.Ray.Origin    = intersectionOffsetShadow;
            lightBeam.Ray.Direction = Vec3.Reflect(directionOrig, surfaceNormal);

            Color reflectionColor = TraceRay(lightBeam, currentColor, currentDepth + 1);

            // Calculate Refraction
            Vec3 intersectionOffsetRefraction = intersection + Constants.RefractionOffset * surfaceNormal;
            lightBeam.Ray.Origin = intersectionOffsetRefraction;

            lightBeam.Ray.Direction = Refract(directionOrig,
                                              surfaceNormal,
                                              refractiveIndex
                                             );

            Color refractionColor = TraceRay(lightBeam, currentColor, currentDepth + 1);

            currentColor = Util.ColorScale(colorOrig, 1 - transparency);

            currentColor = Util.ColorScale(currentColor, brightnessDiffuse);

            // Add specular highlights
            currentColor =
                Util.ColorAdd(currentColor,
                              Util.ColorScale(currentColor, brightnessSpecular)
                             );


            currentColor =
                Util.ColorAdd(currentColor,
                              Util.ColorScale(reflectionColor, gloss)
                             );

            currentColor =
                Util.ColorAdd(currentColor,
                              Util.ColorScale(refractionColor, transparency)
                             );

            return currentColor;
        }


        /// <summary>
        ///     Cast a ray through the viewport at position column, row
        /// </summary>
        /// <param name="row">The row or y coordinate of the viewport</param>
        /// <param name="column">The column or x coordinate of the viewport</param>
        /// <returns>A normalized ray from the camera to the specified viewport location</returns>
        private Ray CastPrimaryRay(int row, int column)
        {
            // Build Normalized Device Coordinates (NDC)
            // Value range is [0,1], 0.5f makes points appear in the center of a pixel
            float ndcX = Util.Normalize(row    + 0.5f, Metadata.Width);
            float ndcY = Util.Normalize(column + 0.5f, Metadata.Height);

            // Convert NDC to Screen space by remapping increasing x values to the range [-1,1]
            // and increasing y-values to the range [1,1-]
            float screenX = Util.RescaleToRange(ndcX, -1f, 1f);
            float screenY = Util.RescaleToRange(ndcY, 1f,  -1f);

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
            DetectAndSetBackground();
            Progress = 0;

            for (int i = 0; i < Metadata.NumIterations; ++i)
            {
                Progress = Convert.ToInt32(i / (float) Metadata.NumIterations * 100);
                OnPropertyChanged("Progress");

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
            DetectAndSetBackground();
            Progress = 0;

            for (int i = 0; i < Metadata.NumIterations; ++i)
            {
                Progress = Convert.ToInt32(i / (float) Metadata.NumIterations * 100);
                OnPropertyChanged("Progress");

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

            jsonData.Add("Background", BackgroundValue);

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
                             {"FOV", Util.RadToDeg(Camera.Fov)}
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