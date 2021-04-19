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


    public class Camera
    {
        #region Properties

        public Point3 Position { get; set; }

        public Vec3 Direction { get; set; }

        /// Horizontal Field of View in radians
        public float Fov { get; set; }

        #endregion

        #region Constructors

        public Camera(Point3 position, Vec3 direction, float fov)
        {
            Position  = position;
            Direction = direction;
            Fov       = fov;
        }


        /// Looking along negative Z-axis
        public Camera(float fov, Point3 position)
        {
            Position  = position;
            Direction = -Vec3.UnitZ;
            Fov       = fov;
        }


        /// Centered at origin
        public Camera(Vec3 direction, float fov)
        {
            Position  = Vec3.Zero;
            Direction = direction;
            Fov       = fov;
        }


        /// Centered at origin, looking along negative Z-axis
        public Camera(float fov)
        {
            Position  = Vec3.Zero;
            Direction = -Vec3.UnitZ;
            Fov       = fov;
        }


        /// Centered at origin, looking along negative Z-axis, 60 degree or 1.0472 radians FOV
        public Camera()
        {
            Position  = Vec3.Zero;
            Direction = Vec3.UnitZ;
            Fov       = 1.0472f;
        }

        #endregion
    }
}