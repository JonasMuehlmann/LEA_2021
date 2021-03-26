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
            // First program argument decides, if multithreading is enabled (1=On, 0=off)
            scene.Render(args.Length > 0 && args[0] == "0");
        }
    }
}