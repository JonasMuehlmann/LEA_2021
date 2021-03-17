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

            var scene = new Scene(@"scene_01");
            

            scene.SetBackground(Color.Black);
            scene.Render();
        }
    }
}