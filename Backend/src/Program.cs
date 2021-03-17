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

            // var scene = new Scene(@"C:\Users\info\RiderProjects\LEA_2021\Backend\scenes\scene_01.json");
            

            scene.Objects.Add(new Object(new Material(
                        new Bitmap(1, 1),
                        new Bitmap(1, 1),
                        new Bitmap(1, 1),
                        new Bitmap(1, 1),
                        new Bitmap(1, 1),
                        new Bitmap(1, 1),
                        new Bitmap(1, 1)
                    ),
                    new Plane(new Vector3(0, 1, 0), 5, 10),
                    new Vector3(0, 0, 0)
                )
            );

            scene.Render();
        }
    }
}