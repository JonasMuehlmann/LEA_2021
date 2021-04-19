// Copyright 2021 Jonas Muehlmann, Tim Dreier
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;
    using Point2 = Vector2;


    public class Object
    {
        #region Properties

        public string Name { get; set; }

        public Material Material { get; set; }

        public Shape Shape { get; set; }

        public Point3 Position { get; set; }

        #endregion

        #region Constructors

        public Object(Material material, Shape shape, Point3 position, string name)
        {
            Material = material;
            Shape    = shape;
            Position = position;
            Name     = name;
        }


        public Object(Material material, Shape shape, Point3 position)
        {
            Material = material;
            Shape    = shape;
            Position = position;
        }


        /// Centered at origin
        public Object(Material material, Shape shape)
        {
            Material = material;
            Shape    = shape;
            Position = Vec3.Zero;
        }

        #endregion


        public override string ToString()
        {
            return $"{Name} (Material: {Material.Name})";
        }


        /// <summary>
        ///     Make an intersection test with the ray and this object
        /// </summary>
        /// <param name="ray">A ray to test for intersection with the object</param>
        /// <returns>A value >0 representing the distance to intersection along the ray or -1f if no intersection occurs</returns>
        public float Intersect(Ray ray)
        {
            return Shape.Intersect(ray, Position);
        }


        /// <summary>
        ///     Get the UV coordinates of the surfacePoint
        /// </summary>
        /// <param name="surfacePoint">A point on the surface of the object</param>
        /// <returns>UV-coordinates of the surfacePoint</returns>
        public Vector2 GetUvCoordinates(Vector3 surfacePoint)
        {
            return Shape.GetUvCoordinates(surfacePoint, Position);
        }


        /// <summary>
        ///     Get the surface normal at the given surfacePoint
        /// </summary>
        /// <param name="surfacePoint">A point on the surface of the object</param>
        /// <returns>The surface normal at the surfacePoint</returns>
        public Vec3 GetSurfaceNormal(Point3 surfacePoint)
        {
            return Shape.GetSurfaceNormal(surfacePoint, Position);
        }
    }
}