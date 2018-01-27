namespace MathLib.Models
{
   public class Result
    {
        #region Properties

        public Box Box { get; internal set; }
        public int Quantity { get; internal set; }

        #endregion


        #region Constructors

        internal Result() { }

        internal Result(Box box, int quantity)
        {
            Box = box;
            Quantity = quantity;
        }

        #endregion
    }

}