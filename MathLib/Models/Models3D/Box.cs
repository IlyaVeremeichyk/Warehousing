using System;

namespace MathLib.Models.Models3D
{
    public class Box : IEquatable<Box>
    {
        #region Properties

        public string Name { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Quantity { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int ZPos { get; set; }
        public int Volume { get { return Length * Width * Height; } }
        public int BaseArea { get { return Length * Width; } }

        #endregion




        #region Constructors

        public Box() { }

        public Box(string name, int length = 0, int width = 0, 
                   int height = 0, int quantity = 0, int x = 0, int y = 0, 
                   int z = 0)
        {
            Name = name;
            Length = length;
            Width = width;
            Height = height;
            Quantity = quantity;
            XPos = x;
            YPos = y;
            ZPos = z;
        }

        #endregion

        #region Public methods

        public bool Equals(Box box)
        {
            if (box == null) return false;

            return Name.Equals(box.Name) && Length.Equals(box.Length) && 
                   Width.Equals(box.Width) && Height.Equals(box.Height);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is Box))
                throw new InvalidCastException("Passed object is not an object of a 'Box' class");
            else return Equals(obj as Box);
        }

        public static bool operator ==(Box b1, Box b2)
        {
            if (((object)b1) == null || ((object)b2) == null)
                return Object.Equals(b1, b2);

            return b1.Equals(b2);
        }

        public static bool operator !=(Box b1, Box b2)
        {
            return !(b1 == b2);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }


        #endregion
    }
}