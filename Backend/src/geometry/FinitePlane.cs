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


using System;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;
    using Point2 = Vector2;


    public class FinitePlane : Oriented
    {
        #region Properties

        public int Width { get; set; }

        public int Length { get; set; }

        #endregion

        #region Constructors

        /// Oriented, rectangular plane
        public FinitePlane(Vec3 orientation, int width, int height)
        {
            Orientation = orientation;
            Width       = width;
            Length      = height;
        }


        /// Oriented, square plane
        public FinitePlane(Vec3 orientation, int sideLengths)
        {
            Orientation = orientation;
            Width       = sideLengths;
            Length      = sideLengths;
        }


        /// Y-axis aligned, rectangular plane
        public FinitePlane(int width, int height)
        {
            Orientation = Vec3.UnitY;
            Width       = width;
            Length      = height;
        }


        /// Y-axis aligned, square plane
        public FinitePlane(int sideLengths)
        {
            Orientation = Vec3.UnitY;
            Width       = sideLengths;
            Length      = sideLengths;
        }

        #endregion


        /// <summary>
        ///     Check if the surfacePoint is in bounds of the Finite Plane
        /// </summary>
        /// <param name="center">The center position of the plane</param>
        /// <param name="surfacePoint">A point on the oriented face of the Plane</param>
        /// <returns>True, when the surfacePoint is withing the planes bounds, false otherwise</returns>
        private bool IsInBounds(Point3 center, Point3 surfacePoint)
        {
            if (
                Math.Abs(Orientation.Y) == 1f
            )
            {
                if (Math.Abs(Util.ScalarDistance(center.X, surfacePoint.X)) > Length * 0.5f
                 || Math.Abs(Util.ScalarDistance(center.Z, surfacePoint.Z)) > Width  * 0.5f)
                {
                    return false;
                }
            }

            if (
                Math.Abs(Orientation.X) == 1f
            )
            {
                if (Math.Abs(Util.ScalarDistance(center.Z, surfacePoint.Z)) > Length * 0.5f
                 || Math.Abs(Util.ScalarDistance(center.Y, surfacePoint.Y)) > Width  * 0.5f)
                {
                    return false;
                }
            }

            else
            {
                if (Math.Abs(Util.ScalarDistance(center.Y, surfacePoint.Y)) > Length * 0.5f
                 || Math.Abs(Util.ScalarDistance(center.X, surfacePoint.X)) > Width  * 0.5f)
                {
                    return false;
                }
            }

            return true;
        }


        // Not an elegant function, but the compiler was not satisfied with others
        public override Vector3 GetSurfaceNormal(Vector3 surfacePoint3, Vector3 position)
        {
            return Orientation;
        }


        /// <summary>
        ///     Get the UV coordinates of the surfacePoint
        /// </summary>
        /// <param name="surfacePoint">A point on the oriented face of the plane</param>
        /// <param name="position">The center position of the plane</param>
        /// <returns>UV-coordinates of the surfacePoint</returns>
        public override Point2 GetUvCoordinates(Point3 position, Point3 surfacePoint)
        {
            if (Math.Abs(Orientation.Y) == 1f)
            {
                float u = Util.Normalize(Util.ScalarDistance(position.Z, surfacePoint.Z),
                                         -Length * 0.5f,
                                         Length  * 0.5f
                                        );

                float v = Util.Normalize(Util.ScalarDistance(position.X, surfacePoint.X),
                                         -Width * 0.5f,
                                         Width  * 0.5f
                                        );

                return new Point2(u, v);
            }

            if (Math.Abs(Orientation.X) == 1f)
            {
                float u = Util.Normalize(Util.ScalarDistance(position.Z, surfacePoint.Z),
                                         -Length * 0.5f,
                                         Length  * 0.5f
                                        );

                float v = Util.Normalize(Util.ScalarDistance(position.Y, surfacePoint.Y),
                                         -Width * 0.5f,
                                         Width  * 0.5f
                                        );

                return new Point2(u, v);
            }

            else
            {
                float u = Util.Normalize(Util.ScalarDistance(position.Y, surfacePoint.Y),
                                         -Length * 0.5f,
                                         Length  * 0.5f
                                        );

                float v = Util.Normalize(Util.ScalarDistance(position.X, surfacePoint.X),
                                         -Width * 0.5f,
                                         Width  * 0.5f
                                        );

                return new Point2(u, v);
            }
        }


        /// <summary>
        ///     Make an intersection test with the ray and this plane
        /// </summary>
        /// <param name="ray">A ray to test for intersection with the plane</param>
        /// <param name="center">The center position of the plane</param>
        /// <returns>A value >0 representing the distance to intersection along the ray or -1f if no intersection occurs</returns>
        public override float Intersect(Ray ray, Point3 center)
        {
            float denominator = Vec3.Dot(Orientation, ray.Direction);


            // If the denominator (dot product) is 0, the ray and plane are parallel
            if (Math.Abs(denominator) < float.Epsilon)
            {
                return -1f;
            }

            float t = Vec3.Dot(Util.FromAToB(ray.Origin, center), Orientation) / denominator;

            // Ray intersects but opposite of it's direction
            if (t < 0)
            {
                return -1f;
            }


            // Ray intersects with the infinite plane, check if it also intersects with the bounded plane
            Point3 intersection = ray.At(t);


            if (!IsInBounds(center, intersection))
            {
                return -1f;
            }


            return t;
        }
    }
}