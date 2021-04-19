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


using System.Drawing;
using System.Numerics;


namespace LEA_2021
{
    using Vec3 = Vector3;
    using Point3 = Vector3;


    public class PointLight
    {
        #region Properties

        public Point3 Position { get; set; }

        public Color Color { get; set; }

        // TODO: Remove, Brightness is encoded in the Color
        public float Brightness { get; set; }

        #endregion

        #region Constructors

        public PointLight(Point3 position, Color color, float brightness)
        {
            Position   = position;
            Color      = color;
            Brightness = brightness;
        }


        /// Brightness is 100%
        public PointLight(Point3 position, Color color)
        {
            Position   = position;
            Color      = color;
            Brightness = 1f;
        }


        /// Color is white
        public PointLight(Point3 position, float brightness)
        {
            Position   = position;
            Color      = Color.White;
            Brightness = brightness;
        }


        /// Color is white, brightness is 100%
        public PointLight(Point3 position)
        {
            Position   = position;
            Color      = Color.White;
            Brightness = 1f;
        }

        #endregion
    }
}