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
            var scene = new Scene(@"../../../scenes/scene_01.json");

            scene.SetBackground(Color.Black);
            scene.Render();
        }
    }
}