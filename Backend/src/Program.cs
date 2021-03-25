using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    internal class Program
    {
        private static void Main(string[] args)
        {
            Scene scene = new(@"forest");
            // First program argument (1 or 0) decides, if multithreading is enabled
            scene.Render();
        }
    }
}