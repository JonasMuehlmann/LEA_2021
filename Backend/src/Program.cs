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
            scene.PointLights.Add(new PointLight(new Point3(-20, 15, 20),   0.6f));
            scene.PointLights.Add(new PointLight(new Point3(20,  15, 20),   0.6f));
            scene.PointLights.Add(new PointLight(new Point3(0,   0,  -320), 0.8f));
            scene.Render();
        }
    }
}