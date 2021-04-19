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


    public abstract class Oriented : Shape
    {
        #region Fields

        private Vec3 _orientation;

        #endregion

        #region Properties

        /// <summary>
        ///     An axis-aligned unit vector representing the top side of the object
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the direction is not an axis-aligned unit vector</exception>
        public Vec3 Orientation
        {
            get => _orientation;
            set
            {
                if (
                    !(Equals(value, Vec3.UnitY)
                   || Equals(value, -Vec3.UnitY)
                   || Equals(value, Vec3.UnitX)
                   || Equals(value, -Vec3.UnitX)
                   || Equals(value, Vec3.UnitZ)
                   || Equals(value, -Vec3.UnitZ)
                        ))
                {
                    throw new ArgumentException("Orientation can only be axis aligned");
                }

                _orientation = value;
            }
        }

        #endregion
    }
}