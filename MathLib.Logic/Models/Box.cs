﻿using System;

namespace MathLib.Logic.Models
{
    public class Box : IEquatable<Box>
    {
        public string Name { get; }

        public double Weight { get; }

        public int Cost { get; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int OrderQuantity { get; set; }

        public int XPos { get; set; } // x coordinate of a box

        public int YPos { get; set; } // y coordinate of a box

        public int ZPos { get; set; } // z coordinate of a box

        public int Volume => Length * Width * Height;

        public int BaseArea => Length * Width; // the square of a box bottom

        public Box(string name = null, double weight = 0, int cost = 0, int length = 0, int width = 0,
                   int height = 0, int orderQuantity = 0, int x = 0, int y = 0, int z = 0)
        {
            Name = name;
            Weight = weight;
            Cost = cost;
            OrderQuantity = orderQuantity;
            Length = length;
            Width = width;
            Height = height;
            XPos = x;
            YPos = y;
            ZPos = z;
        }

        public bool Equals(Box box)
        {
            if (box == null)
            {
                return false;
            }

            return Name.Equals(box.Name) &&
                   Length.Equals(box.Length) &&
                   Width.Equals(box.Width) &&
                   Height.Equals(box.Height);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return base.Equals(obj);
            }

            if (!(obj is Box))
            {
                return false;
            }

            return Equals((Box)obj);
        }

        public static bool operator ==(Box b1, Box b2)
        {
            if ((object)b1 == null || (object)b2 == null)
            {
                return Equals(b1, b2);
            }

            return b1.Equals(b2);
        }

        public static bool operator !=(Box b1, Box b2)
        {
            return !(b1 == b2);
        }

        public override int GetHashCode()
        {
            return (int)(Name.GetHashCode() + Weight + Cost);
        }

        public override string ToString()
        {
            return $"The box with name '{Name}'";
        }
    }
}
