namespace MathLib.Logic.Models
{
    public class BoxQuantityPair
    {
        public Box Box { get; set; }

        public int Quantity { get; set; }

        public BoxQuantityPair(Box box = null, int quantity = 0)
        {
            Box = box ?? new Box();
            Quantity = quantity;
        }

        public override string ToString()
        {
            return string.Format("Box quantity pair '{0}': {1}", Box.ToString(), Quantity);
        }
    }
}
