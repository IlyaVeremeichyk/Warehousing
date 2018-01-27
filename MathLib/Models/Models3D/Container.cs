using System;

namespace MathLib.Models.Models3D
{
    public class Container: IEquatable<Container>
    {
        #region Properties

        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        #endregion

        #region Constructors

        public Container() { }

        public Container(int length, int width, int height)
        {
            Length = length;
            Width = width;
            Height = height;
        }

        #endregion

        #region Public methods

        public bool Equals(Container container)
        {
            if (container == null) return false;
            return Length.Equals(container.Length) && 
                   Width.Equals(container.Width) && 
                   Height.Equals(container.Height);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is Container))
                throw new InvalidCastException("Passed object is not an object of a 'Container' class");
            else return Equals(obj as Box);
        }

        public static bool operator ==(Container b1, Container b2)
        {
            if (((object)b1) == null || ((object)b2) == null)
                return Object.Equals(b1, b2); 
            return b1.Equals(b2);
        }

        public static bool operator !=(Container b1, Container b2)
        {
            return !(b1 == b2);
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode();
        }

        #endregion
    }

}