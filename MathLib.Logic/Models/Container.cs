using System;

namespace MathLib.Logic.Models
{
    public class Container : IEquatable<Container>
    {
        public string Name { get; }

        public double Capacity { get; }

        public int Length { get; }

        public int Width { get; }

        public int Height { get; }

        public int Volume => Length * Width * Height;

        public Container(string name = "", double capacity = 0, int length = 0, int width = 0, int height = 0)
        {
            Name = name;
            Capacity = capacity;
            Length = length;
            Width = width;
            Height = height;
        }

        public bool Equals(Container container)
        {
            if (container == null)
            {
                return false;
            }

            return Name.Equals(container.Name) &&
                   Length.Equals(container.Length) &&
                   Width.Equals(container.Width) &&
                   Height.Equals(container.Height);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return base.Equals(obj);
            }

            if (!(obj is Container))
            {
                return false;
            }

            return Equals((Container)obj);
        }

        public static bool operator ==(Container c1, Container c2)
        {
            if ((object)c1 == null || (object)c2 == null)
            {
                return Equals(c1, c2);
            }

            return c1.Equals(c2);
        }

        public static bool operator !=(Container c1, Container c2)
        {
            return !(c1 == c2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Volume * Name.GetHashCode() + Width + Height * 10000000;
            }
        }

        public override string ToString()
        {
            return $"Container with name '{Name}'";
        }
    }
}