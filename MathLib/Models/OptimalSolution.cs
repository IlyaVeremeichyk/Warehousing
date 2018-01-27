namespace MathLib.Models
{
    internal class OptimalSolution
    {
        #region Properties

        internal Box Box { get; set; }
        internal double MaxProfit { get; set; }
        internal int OptimalQuantity { get; set; }

        #endregion

        #region Constructors

        internal OptimalSolution() { }

        internal OptimalSolution(Box box, double profit, int quantity)
        {
            Box = box;
            MaxProfit = profit;
            OptimalQuantity = quantity;
        }

        #endregion
    }
}