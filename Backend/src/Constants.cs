namespace LEA_2021
{
    public class Constants
    {
        public static readonly string SceneDir           = "../../../../Backend/scenes";
        public static readonly string MaterialsDir       = SceneDir + "/materials";
        public static readonly string OutputDir          = "../../../../Backend/out";
        public static readonly float  ShadowOffset       = 1e-4f;
        public static readonly float  RefractionOffset   = -1e-4f;
        public static readonly int    MaxReflectionDepth = 10;
        public static readonly float  SpecularExponent   = 200f;
    }
}