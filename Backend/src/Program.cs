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
            var camera = new Camera(new Point3(0, 0, 0), new Vec3(0, 0, 1), Util.DegreesToRadians(120));

            var scene = new Scene(new Metadata(1920, 1080, 5), camera);

            var empty_material = new Material(new Bitmap(1, 1),
                                              new Bitmap(1, 1),
                                              new Bitmap(1, 1),
                                              new Bitmap(1, 1),
                                              new Bitmap(1, 1),
                                              new Bitmap(1, 1),
                                              new Bitmap(1, 1)
                                             );

            // scene.Objects.Add(new Object(empty_material,
            //                              new Plane(new Vec3(0, 0, 1), 5, 10),
            //                              new Point3(0, 0, 0)
            //                             )
            //                  );

            scene.Objects.Add(new Object(empty_material, new Sphere(10), new Point3(0, 10, 50)));

            scene.Render();
        }
    }
}