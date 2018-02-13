namespace MathLib.Logic.Models
{
    public class BoxQunatityPair
    {
        #region Properties
        public Box Box { get; set; }

        public int Quantity { get; set; }

        #endregion

        #region Constructors
        public BoxQunatityPair(Box box = null, int quantity = 0)
        {
            Box = box ?? new Box();
            Quantity = quantity;
        }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return string.Format("Box quantity pair '{0}': {1}", Box.ToString(), Quantity);
        }

        #endregion
    }
}
