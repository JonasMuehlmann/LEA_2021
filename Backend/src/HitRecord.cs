namespace LEA_2021
{
    public readonly struct HitRecord
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