using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Json;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    class Scene
    {
        #region Properties

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
            Objects  = new List<Object>();
            Image    = new Bitmap(metadata.Width, metadata.Height);
            Camera   = camera;
            SetBackground(Color.Black);
        }


        // Default-constructed camera with black background
        public Scene(Metadata metadata)
        {
            Metadata = metadata;
            Objects  = new List<Object>();
            Image    = new Bitmap(metadata.Width, metadata.Height);
            Camera   = new Camera();
            SetBackground(Color.Black);
        }


        // Default background color is black
        public Scene(string configPath)
        {
            // deserialize JSON directly from a file
            // var serializedConfig = JsonSerializer.Deserialize<Scene>(File.ReadAllText(@configPath));

            JsonValue  value  = JsonValue.Parse(File.ReadAllText(@configPath));
            JsonObject result = value as JsonObject;

            Console.WriteLine((int) value["Metadata"]["Width"]);
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


        public void SetBackground(string image_path)
        {
            Image = new Bitmap(Bitmap.FromFile(image_path));
        }


        public void Render()
        {
            for (int row = 0; row < Metadata.Width; ++row)
            {
                // TODO: Maybe invert loop?
                for (int column = 0; column < Metadata.Height; ++column)
                {
                    // Build Normalized Device Coordinates (NDC)
                    // Value range is [0,1], 0.5f makes points appear in the center of a pixel
                    float NDC_x = (row    + 0.5f) / Metadata.Width;
                    float NDC_y = (column + 0.5f) / Metadata.Height;

                    // Convert NDC to Screen space by remapping increasing x values to the range [1,-1]
                    // and increasing y-values to the range [-1,1]
                    float Pixel_x = 1 - 2 * NDC_x;
                    float Pixel_y = 2     * NDC_y - 1;

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

                    // // Debug camera by setting pixel to rays x-distance from camera
                    // int distance_z = 255 - (int) (1f + 2.5f * 254f * (1f + ray_direction.Z) / 2f);
                    // Image.SetPixel(row,
                    //                column,
                    //                Color.FromArgb(100,
                    //                               distance_z,distance_z,distance_z
                    //                              )
                    //               );


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

            Image.Save("../../../out/foo.png", ImageFormat.Png);
        }
    }
}