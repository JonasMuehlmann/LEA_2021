using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        public Scene(Metadata metadata, Camera camera)
        {
            Metadata = metadata;
            Objects  = new List<Object>();
            Image    = new Bitmap(metadata.Width, metadata.Height);
            Camera   = camera;
        }


        // Default-constructed camera
        public Scene(Metadata metadata)
        {
            Metadata = metadata;
            Objects  = new List<Object>();
            Image    = new Bitmap(metadata.Width, metadata.Height);
            Camera   = new Camera();
        }

        #endregion


        public void Render()
        {
            for (int row = 0; row < Metadata.Width; ++row)
            {
                for (int column = 0; column < Metadata.Height; ++column)
                {
                    float ray_direction_x = (row
                                           + 0.5f
                                           - Metadata.Width)
                                          * (float) Math.Tan(Camera.Fov / 2f)
                                          * Metadata.GetAspectRatio();

                    float ray_direction_y = column + 0.5f - Metadata.Width * 0.5f * (float) Math.Tan(Camera.Fov / 2f);
                    float ray_direction_z = Metadata.Height / (2f * (float) Math.Tan(Camera.Fov / 2d));

                    var ray_direction =
                        Vec3.Normalize(new Vector3(ray_direction_y,
                                                   ray_direction_y,
                                                   ray_direction_z
                                                  )
                                     - Camera.Position
                                      );

                    foreach (var _object in Objects)
                    {
                        Vector3? intersection = _object.Intersect(new Ray(Camera.Position, ray_direction));

                        if (intersection is null)
                        {
                            Image.SetPixel(row, column, Color.Black);
                        }
                        else
                        {
                            if (_object.GetType() == typeof(Plane))
                            {
                                Image.SetPixel(row, column, Color.Gray);
                            }
                            else
                            {
                                Image.SetPixel(row, column, Color.White);
                            }
                        }
                    }
                }
            }

            Image.Save("../../../out/foo.png", ImageFormat.Png);
        }
    }
}