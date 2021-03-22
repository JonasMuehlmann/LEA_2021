namespace LEA_2021
{
    public struct HitRecord
    {
        public readonly Object Object;
        public readonly float  Distance;


        public HitRecord(Object o, float distance)
        {
            Object   = o;
            Distance = distance;
        }
    }
}