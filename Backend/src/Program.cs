using System.Drawing;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    class Program
    {
        static void Main(string[] args)
        {
            var camera = new Camera(new Point3(0, 0, 0), new Vec3(0, 0, 1), Util.DegreesToRadians(60));

            var scene = new Scene(@"../../../scenes/scene_01.json");
            

            // scene.Objects.Add(new Object(new Material(new Bitmap(1, 1),
            //                                           new Bitmap(1, 1),
            //                                           new Bitmap(1, 1),
            //                                           new Bitmap(1, 1),
            //                                           new Bitmap(1, 1),
            //                                           new Bitmap(1, 1),
            //                                           new Bitmap(1, 1)
            //                                          ),
            //                              new Sphere(10),
            //                              new Point3(0, 0, 20)
            //                             )
            //                  );

            scene.Objects.Add(new Object(new Material(new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1)
                                                     ),
                                         new Sphere(5),
                                         new Point3(20, 10, 50)
                                        )
                             );

            scene.Objects.Add(new Object(new Material(new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1),
                                                      new Bitmap(1, 1)
                                                     ),
                                         new Sphere(5),
                                         new Point3(-20, -10, 50)
                                        )
                             );

            scene.Render();
        }
    }
}