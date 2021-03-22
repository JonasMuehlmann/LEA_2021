using System;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class Cuboid : Oriented
    {
        #region Properties

        public float Width { get; set; }

        public float Height { get; set; }

        public float Length { get; set; }

        #endregion

        #region Constructors

        // Oriented cuboid
        public Cuboid(float width, float height, float length, Vec3 orientation)
        {
            Width       = width;
            Height      = height;
            Length      = length;
            Orientation = orientation;
        }


        // Oriented Cube
        public Cuboid(float dimensions, Vec3 orientation)
        {
            Width       = dimensions;
            Height      = dimensions;
            Length      = dimensions;
            Orientation = orientation;
        }


        // Y-axis aligned cuboid
        public Cuboid(float width, float height, float length)
        {
            Width       = width;
            Height      = height;
            Length      = length;
            Orientation = Vec3.UnitY;
        }


        // Y-axis aligned cube
        public Cuboid(float dimensions)
        {
            Width       = dimensions;
            Height      = dimensions;
            Length      = dimensions;
            Orientation = Vec3.UnitY;
        }


        // Y-axis aligned Unit-Cube
        public Cuboid()
        {
            Width       = 1;
            Height      = 1;
            Length      = 1;
            Orientation = Vec3.UnitY;
        }

        #endregion


        public override float Intersect(Ray ray, Point3 center)
        {
            throw new NotImplementedException();
        }


        public override Vector2 GetUvCoordinates(Vector3 intersection, Vector3 position)
        {
            throw new NotImplementedException();
        }
    }
}