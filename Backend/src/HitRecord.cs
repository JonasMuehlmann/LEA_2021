namespace LEA_2021
{
    public struct HitRecord
    {
        public Object Object;
        public float  Distance;


        public HitRecord(Object o, float distance)
        {
            Object   = o;
            Distance = distance;
        }
    }
}