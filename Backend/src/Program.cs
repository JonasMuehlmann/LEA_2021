using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    internal class Program
    {
        private static void Main(string[] args)
        {
            Scene scene = new(@"scene_02");
            // First program argument (1 or 0) decides, if multithreading is enabled
            scene.Render(args.Length > 0 && args[0] == "0");
        }
    }
}