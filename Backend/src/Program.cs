using System.Drawing;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    internal class Program
    {
        private static void Main(string[] args)
        {
            Scene scene = new(@"scene_01");


            scene.SetBackground(Color.Black);
            // is z=20 or z=-20 correct for being next to the sphere?
            // scene.PointLights.Add(new PointLight(new Point3(20, -20, 20), 1f));
            scene.PointLights.Add(new PointLight(new Point3(20,  20, 20), 1f));
            scene.PointLights.Add(new PointLight(new Point3(-40, 0,  20), 0.5f));
            // scene.PointLights.Add(new PointLight(new Point3(0,   0,  0),  0.5f));
            scene.Render();
        }
    }
}