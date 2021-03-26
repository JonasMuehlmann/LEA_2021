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