using System;
using System.Collections.Generic;
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


    public class Scene
    {
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


        public void SetBackground(string image_path)
        {
            Image = new Bitmap(Bitmap.FromFile(image_path));
        }


        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH")]
        public void Render()
        {
            for (int i = 0; i < Metadata.NumIterations; i++)
            {
                for (int row = 0; row < Metadata.Width; ++row)
                {
                    for (int column = 0; column < Metadata.Height; ++column)
                    {
                        // Build Normalized Device Coordinates (NDC)
                        // Value range is [0,1], 0.5f makes points appear in the center of a pixel
                        float NDC_x = (row + 0.5f) / Metadata.Width;
                        float NDC_y = (column + 0.5f) / Metadata.Height;

                        // Convert NDC to Screen space by remapping increasing x values to the range [1,-1]
                        // and increasing y-values to the range [-1,1]
                        float Pixel_x = 1 - 2 * NDC_x;
                        float Pixel_y = 2 * NDC_y - 1;

                        // Because the image is not square (Usually images are wider than they are high),
                        // pixels are now rectangular.
                        // Making them square again can be achieved by multiplying the x-values by the images aspect ratio
                        Pixel_x *= Metadata.GetAspectRatio();

                        // Account for field of view
                        Pixel_x *= (float) Math.Tan(Camera.Fov / 2);
                        Pixel_y *= (float) Math.Tan(Camera.Fov / 2);

                        // For now, the camara always faces forward, hence we set the z-component to -1
                        // TODO: Camera to world transformation
                        var ray_direction =
                            Vec3.Normalize(new Vector3(Pixel_x,
                                               Pixel_y,
                                               -1
                                           )
                                           - Camera.Position
                            );

                        foreach (var _object in Objects)
                        {
                            Vector3? intersection = _object.Intersect(new Ray(Camera.Position, ray_direction));

                            if (intersection != null)
                            {
                                Image.SetPixel(row, column, Color.White);
                            }
                        }
                    }
                }

                Image.Save($"../../../../Backend/out/{Name}.png", ImageFormat.Png);
            }
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

        public Camera Camera { get; set; }

        #endregion

        #region Constructors

        // Default background color is black
        public Scene(Metadata metadata, Camera camera)
        {
            Metadata = metadata;
            Objects = new List<Object>();
            Image = new Bitmap(metadata.Width, metadata.Height);
            Camera = camera;
            SetBackground(Color.Black);
        }


        // Default-constructed camera with black background
        public Scene(Metadata metadata)
        {
            Metadata = metadata;
            Objects = new List<Object>();
            Image = new Bitmap(metadata.Width, metadata.Height);
            Camera = new Camera();
            SetBackground(Color.Black);
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
                        shapeClass = new Cuboid((int) obj["Properties"]["Width"],
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
        }

        #endregion
    }
}