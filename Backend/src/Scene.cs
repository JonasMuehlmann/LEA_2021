#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Json;
using System.Numerics;
using System.Text.RegularExpressions;

namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Scene : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void DetectBackground()
        {
            if (Regex.IsMatch(backgroundValue, @"^#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$"))
            {
                // Background is hex color
                SetBackground(ColorTranslator.FromHtml(backgroundValue));
            }
            else if (Regex.IsMatch(backgroundValue, @"[A-Za-z0-9 -_\/]*[\/.](gif|jpg|jpeg|tiff|png)$"))
            {
                // background is path to image
                SetBackground(backgroundValue);
            }
            else
            {
                // background must be color word
                SetBackground(Color.FromName(backgroundValue));
            }
        }



        public void SetBackground(string image_path)
        {
            Image = new Bitmap(Bitmap.FromFile(image_path));
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        #region Properties

        public String Name { get; set; }

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
            Objects = new List<Object>();
            Image = new Bitmap(metadata.Width, metadata.Height);
            Camera = camera;

            Name = "Untitled";
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
            Objects = new List<Object>();
            Image = new Bitmap(metadata.Width, metadata.Height);
            Camera = new Camera();

            Name = "Untitled";
            backgroundValue = "Black";
            SetBackground(Color.Black);
            PointLights     = new List<PointLight>();
            Rng             = new Random();
            BackgroundColor = Color.Black;
        }


        // Default background color is black
        public Scene(string configName)
        {
            JsonValue value = JsonValue.Parse(File.ReadAllText($@"../../../../Backend/scenes/{configName}.json"));
            Name = configName;

            Metadata = new Metadata(
                (int) value["Metadata"]["Width"],
                (int) value["Metadata"]["Height"],
                (int) value["Metadata"]["Num_Iterations"]
            );

            Image = new Bitmap(Metadata.Width, Metadata.Height);

            Camera = new Camera(new Point3(value["Camera"]["Position"][0],
                    value["Camera"]["Position"][1],
                    value["Camera"]["Position"][1]
                ),
                new Vec3(value["Camera"]["Direction"][0],
                    value["Camera"]["Direction"][1],
                    value["Camera"]["Direction"][1]
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
                        shapeClass = new Cuboid(
                            (int) obj["Properties"]["Width"],
                            (int) obj["Properties"]["Height"],
                            (int) obj["Properties"]["Length"],
                            new Vec3(obj["Properties"]["Orientation"][0],
                                obj["Properties"]["Orientation"][1],
                                obj["Properties"]["Orientation"][2]
                            )
                        );

                        break;

                    case "Sphere":
                        shapeClass = new Sphere((float) obj["Properties"]["Radius"]);

                        break;

                    case "Plane":
                        shapeClass = new Plane(new Vec3(obj["Properties"]["Orientation"][0],
                                obj["Properties"]["Orientation"][1],
                                obj["Properties"]["Orientation"][2]
                            ),
                            (int) obj["Properties"]["Width"],
                            (int) obj["Properties"]["Height"]
                        );

                        break;
                }

                Objects.Add(new Object(
                        new Material(obj["Material"]),
                        shapeClass,
                        new Vector3(obj["Properties"]["Position"][0],
                            obj["Properties"]["Position"][1],
                            obj["Properties"]["Position"][2]
                        ),
                        obj["Name"]
                    )
                );
            }


            if (Regex.IsMatch(value["Background"], @"^#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$"))
            {
                // Background is hex color
                SetBackground(ColorTranslator.FromHtml(value["Background"]));
            }
            else if (Regex.IsMatch(value["Background"], @"[A-Za-z0-9 -_\/]*[\/.](gif|jpg|jpeg|tiff|png)$"))
            {
                // background is path to image
                SetBackground(value["Background"]);
            }
            else
            {
                // background must be color word
                SetBackground(Color.FromName(value["Background"]));
            }

            PointLights     = new List<PointLight>();
            Rng             = new Random();
            BackgroundColor = Color.Black;
            backgroundValue = value["Background"];
            DetectBackground();
        }

        #endregion


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





        private Vec3 Reflect(Vec3 direction, Vec3 surfaceNormal)
        {
            return direction - (2f * surfaceNormal * Vec3.Dot(surfaceNormal, direction));
        }


        private Vec3 Refract(Vec3 direction, Vec3 surfaceNormal)
        {
            return Vec3.Zero;
        }


        private Color? TraceRay(LightBeam lightBeam, Color currentColor, int currentDepth = 0)
        {
            // // Russian roulette as base case 
            // if (currentDepth > 5 && Rng.NextDouble() >= lightBeam.Brightness)
            // {
            //     return currentColor;
            // }
            //
            // lightBeam.Brightness *= 1 / lightBeam.Brightness;

            if (currentDepth > 5)
            {
                return currentColor;
            }
            // TODO: Generate random ray


            foreach (var _object in Objects)
            {
                float t = _object.Intersect(lightBeam.Ray);

                // We hit an object
                if (t > Single.Epsilon)
                {
                    // return Color.White;
                    Point3 intersection = lightBeam.Ray.Origin + t * lightBeam.Ray.Direction;

                    // Console.WriteLine(t);
                    // Console.WriteLine($"{intersection.X},{intersection.Y},{intersection.Z}");
                    // Console.WriteLine($"{intersection.Z}");
                    // TODO: Seems working until at least here
                    Vec3  surfaceNormal      = Vec3.Normalize(Util.FromAToB(_object.Position, intersection));
                    float brightnessDiffuse  = 0f;
                    float brightnessSpecular = 0f;

                    // Calculate direct illumination
                    foreach (var pointLight in PointLights)
                    {
                        Vec3 objectToLight = Vec3.Normalize(Util.FromAToB(_object.Position, pointLight.Position));


                        // Lambertian diffuse lighting
                        brightnessDiffuse +=
                            pointLight.Brightness * Math.Max(0f, Vec3.Dot(objectToLight, surfaceNormal));
                        // From tinyraytracer:
                        //  specular_light_intensity += powf(std::max(0.f, -reflect(-light_dir, N)*dir), material.specular_exponent)*lights[i].intensity;

                        // Blinn-Phong specular highlights
                        //(OL + OC) * N
                        Vec3 surfaceToLight  = Util.FromAToB(intersection, pointLight.Position);
                        Vec3 surfaceToCamera = Util.FromAToB(intersection, Camera.Position);
                        brightnessSpecular += Vec3.Dot(Vec3.Normalize(surfaceToLight + surfaceToCamera), surfaceNormal);
                    }

                    // Console.WriteLine(brightnessDiffuse);

                    // x and y range from -8.6 to 8.6, z from -15 to -10 (-30, -10 for tinyraytrcer)
                    // x and y range therefore is less than the spheres diameter, which is wrong, tinyraytracer does it right
                    // Error Must be in intersection function
                    // TODO: Get Object color instead
                    Color color = Color.Gray;

                    // Diffuse lighting
                    color = Color.FromArgb(255,
                                           (int) Math.Clamp(color.R * brightnessDiffuse, 0, 255),
                                           (int) Math.Clamp(color.G * brightnessDiffuse, 0, 255),
                                           (int) Math.Clamp(color.B * brightnessDiffuse, 0, 255)
                                          );

                    // TODO: Replace with glossiness of surface
                    // TODO: Cleanup
                    float specularExponent = 50;

                    // Specular lighting
                    color = Color.FromArgb(255,
                                           (int) Math.Clamp(color.R
                                                          + color.R * Math.Pow(brightnessSpecular, specularExponent),
                                                            0,
                                                            255
                                                           ),
                                           (int) Math.Clamp(color.G
                                                          + color.G * Math.Pow(brightnessSpecular, specularExponent),
                                                            0,
                                                            255
                                                           ),
                                           (int) Math.Clamp(color.B
                                                          + color.B * Math.Pow(brightnessSpecular, specularExponent),
                                                            0,
                                                            255
                                                           )
                                          );

                    // Gamma correction
                    // color = Color.FromArgb(255,
                    //               (int)(255f * Math.Pow(color.R/255f, 1f/2.2f)),
                    //               (int)(255f * Math.Pow(color.G/255f, 1f/2.2f)),
                    //               (int)(255f * Math.Pow(color.B/255f, 1f/2.2f))
                    //              );
                    //

                    return color;
                    // return Color.FromArgb(255,
                    //                       (int) (255f * surfaceNormal.X),
                    //                       (int) (255f * surfaceNormal.Y),
                    //                       (int) (255f * -surfaceNormal.Z)
                    //                      );
                    // Console.WriteLine(intersection.Z);

                    // return Color.FromArgb(255,
                    //                       (int) (125f * (1 + surfaceNormal.X)),
                    //                       (int) (255f * Math.Abs(0)),
                    //                       (int) (255f * Math.Abs(0))
                    //                      );
                    // // Sphere shape = _object.Shape as Sphere;
                    // Console.WriteLine(shape.Radius - Util.FromAToB(_object.Position, (Vec3) intersection).Length());
                    // Console.WriteLine(Util.FromAToB(_object.Position, (Vec3) intersection));
                    // Console.WriteLine(intersection);
                    // return Color.FromArgb(255, (int) (Math.Abs(surfaceNormal.X*255f)), (int)(Math.Abs(surfaceNormal.Y * 255f)), (int)(Math.Abs(surfaceNormal.Z * 255f)));
                    // TODO: Diffuse reflection
                    // TODO: Specular reflection
                    // TODO: Shadows
                    // if (_object.Shape.GetType() == typeof(Plane))
                    // {
                    //     int distance = (int) (10 * Vector3.Distance((Vector3) intersection, _object.Position));
                    //     Image.SetPixel(row, column, Color.FromArgb(100, distance, distance, distance));
                    // }
                    // else
                    //
                    // {
                    //     Image.SetPixel(row, column, Color.White);
                    // }

                    // Point2 uv    = Sphere.GetUVCoordinates((Vec3) intersection, _object.Position);
                    // Color  pixel = people.GetPixel((int)(uv.X * 1920), (int)(uv.Y * 1080));
                }
                else
                {
                    currentColor = Color.FromArgb(255,
                                                  (int) (lightBeam.Brightness * BackgroundColor.R),
                                                  (int) (lightBeam.Brightness * BackgroundColor.G),
                                                  (int) (lightBeam.Brightness * BackgroundColor.B)
                                                 );
                }
            }

            return TraceRay(lightBeam, currentColor, currentDepth + 1);
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
            // TODO: Camera to world transformation
            var rayDirection = Vec3.Normalize(Util.FromAToB(Camera.Position,
                                                            new Vector3(cameraX,
                                                                        cameraY,
                                                                        -1f
                                                                       )
                                                           )
                                             )
                ;

            return new Ray(Camera.Position, rayDirection);
        }


        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH")]
        public void Render()
        {
            DetectBackground();
            Percentage = 0;
            for (int i = 0; i < Metadata.NumIterations; ++i)
            {
                Percentage = Convert.ToInt32(i / (float) Metadata.NumIterations * 100);
                OnPropertyChanged("Percentage");
                for (int column = 0; column < Metadata.Height; ++column)
                {
                    for (int row = 0; row < Metadata.Width; ++row)
                    {
                        Ray primaryRay = CastPrimaryRay(row, column);

                        // Console.WriteLine($"{primaryRay.Direction.X} {primaryRay.Direction.Y} {primaryRay.Direction.Z}");

                        Color? pixel = TraceRay(new LightBeam(primaryRay), new Color());

                        if (pixel is not null)
                        {
                            Image.SetPixel(row, column, (Color) pixel);
                        }
                    }
                }

            Image.Save($"../../../../Backend/out/{Name}.png", ImageFormat.Png);
            OnPropertyChanged("Image");
            }
        }
    }
}