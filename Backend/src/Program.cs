using System.Drawing;
using System.Numerics;


namespace LEA_2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var camera = new Camera(new Vector3(0f, 10f, 0f), new Vector3(0, 0, -1));

            var scene = new Scene(new Metadata(1920, 1080, (int) Util.DegreesToRadians(120), 5), camera);

            scene.Objects.Add(new Object(new Material(new Bitmap(1, 1),
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