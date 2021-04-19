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


    public class Ray
    {
        #region Properties

        public Point3 Origin { get; set; }

        public Vec3 Direction { get; set; }

        #endregion

        #region Constructors

        public Ray(Point3 origin, Vec3 direction)
        {
            Origin    = origin;
            Direction = direction;
        }


        /// Originates in global origin  (0,0,0)
        public Ray(Vec3 direction)
        {
            Origin    = Vec3.Zero;
            Direction = direction;
        }

        #endregion


        /// <summary>
        ///     Get the point at Origin + t * Direction
        /// </summary>
        /// <param name="t">A distance to go along the ray</param>
        /// <returns>The point t units along the ray</returns>
        public Point3 At(float t)
        {
            return Origin + t * Direction;
        }
    }
}