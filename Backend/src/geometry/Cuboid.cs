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

        /// Oriented cuboid
        public Cuboid(float width, float height, float length, Vec3 orientation)
        {
            Width       = width;
            Height      = height;
            Length      = length;
            Orientation = orientation;
        }


        /// Oriented Cube
        public Cuboid(float sideLengths, Vec3 orientation)
        {
            Width       = sideLengths;
            Height      = sideLengths;
            Length      = sideLengths;
            Orientation = orientation;
        }


        /// Y-axis aligned cuboid
        public Cuboid(float width, float height, float length)
        {
            Width       = width;
            Height      = height;
            Length      = length;
            Orientation = Vec3.UnitY;
        }


        /// Y-axis aligned cube
        public Cuboid(float sideLengths)
        {
            Width       = sideLengths;
            Height      = sideLengths;
            Length      = sideLengths;
            Orientation = Vec3.UnitY;
        }


        /// Y-axis aligned Unit-Cube
        public Cuboid()
        {
            Width       = 1;
            Height      = 1;
            Length      = 1;
            Orientation = Vec3.UnitY;
        }

        #endregion


        /// <summary>
        ///     Make an surfacePoint test with the ray and this cuboid
        /// </summary>
        /// <param name="ray">A ray to test for surfacePoint with the cuboid</param>
        /// <param name="center">The center position of the Cuboid</param>
        /// <returns>A value >0 representing the distance to surfacePoint along the ray or -1f, if no intersection occurs</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override float Intersect(Ray ray, Point3 center)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///     Get the UV coordinates of the surfacePoint
        /// </summary>
        /// <param name="surfacePoint">A point on the surface of the object</param>
        /// <param name="position">The center position of the object</param>
        /// <returns>UV-coordinates of the surfacePoint</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Vector2 GetUvCoordinates(Point3 surfacePoint, Point3 position)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///     Get the surface normal at a given surfacePoint
        /// </summary>
        /// <param name="surfacePoint">A point on the surface of the object</param>
        /// <param name="position">The center position of the object</param>
        /// <returns>The surface normal at the surfacePoint</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Vector3 GetSurfaceNormal(Point3 surfacePoint, Point3 position)
        {
            throw new NotImplementedException();
        }
    }
}