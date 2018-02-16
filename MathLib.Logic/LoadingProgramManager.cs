using System;
using System.Collections.Generic;
using System.Linq;
using MathLib.Logic.Models;

namespace MathLib.Logic
{
    /// <summary>
    /// Contains the logic of a dynamic programming mathematical model
    /// </summary>
    public class LoadingProgramManager
    {

        #region Fields

        /// <summary>
        /// Intermediate data: the collection of optimal solutions set for each box
        /// Item 2 - MaxProfit
        /// Item 3 - Box quantity
        /// </summary>
        private readonly LinkedList<List<Tuple<Box, double, int>>> _optimalSolutionsSet;

        /// <summary>
        /// Output data: the collection of filled containers
        /// </summary>
        private readonly List<Container> _filledContainers;

        #endregion

        #region Constructors

        public LoadingProgramManager()
        {
            _optimalSolutionsSet = new LinkedList<List<Tuple<Box, double, int>>>();
            _filledContainers = new List<Container>();
        }

        #endregion

        #region Public methods

        public List<Container> GetLoadProgram(List<Box> boxes, List<Container> containers)
        {
            if (boxes == null || !boxes.Any())
            {
                throw new ArgumentException("The list of boxes can not be null or empty");
            }

            if (containers == null || !containers.Any())
            {
                throw new ArgumentException("The list of containers can not be null or empty");
            }

            if (!IsAllPAssedDataValid(boxes, containers))
            {
                throw new ArgumentException("Passed data contains invalid content: weight, cost or order quantity of a box or capacity of a container can not be 0 or negative.");
            }

            SortPassedValues(boxes, containers);

            foreach (Container container in containers)
            {
                foreach (Box box in boxes)
                {
                    // counting optimal solutions matrix for each box for the current container
                    ReverseSweep(box, container);
                }

                // counting optimal boxes combination for the current container
                DirectSweep(container);

                _optimalSolutionsSet.Clear();

                CheckConstraints(boxes, container);
            }

            return _filledContainers;
        }

        #endregion

        #region Private methods

        private bool IsAllPAssedDataValid(List<Box> boxes, List<Container> containers)
        {
            foreach (var box in boxes)
            {
                if (box.Cost <= 0 || box.OrderQuantity <= 0 || box.Weight <= 0)
                {
                    return false;
                }
            }

            foreach (var container in containers)
            {
                if (container.Capacity <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void SortPassedValues(List<Box> boxes, List<Container> containers)
        {
            boxes.Sort((a, b) => b.Cost.CompareTo(a.Cost));

            containers.Sort((a, b) => b.Capacity.CompareTo(a.Capacity));
        }

        private double CountTheMaximumBoxesQuantityForTheContainer(Box box, Container container)
        {
            return Math.Floor(container.Capacity / box.Weight);
        }

        private int FindQuantityForMaxProfitInOptimalSolutionMatrix(List<Tuple<Box, double, int>> optimalSolutionMatrix, out int leftContainerCapacity)
        {
            double maxProfit = 0;
            int optimalQuantity = leftContainerCapacity = 0;

            // Item2 - MaxProfit
            // Item3 - OptimalQuantity
            foreach (var solution in optimalSolutionMatrix)
            {
                if (solution.Item2 > maxProfit)
                {
                    maxProfit = solution.Item2;
                    optimalQuantity = solution.Item3;
                    leftContainerCapacity = optimalSolutionMatrix.IndexOf(solution);
                }
            }

            return optimalQuantity;
        }

        private void CheckConstraintsForSize(Container container)
        {
            var placedBoxQuantity = container.LoadingProgram.Count - 1;

            for (int i = placedBoxQuantity; i >= 0; i--)
            {
                var box = container.LoadingProgram[i].Box;

                // Constraint: a box can be placed only on its base
                if (box.Length > container.Length || box.Length > container.Width ||
                    box.Width > container.Length || box.Width > container.Width ||
                    box.Height > container.Height)
                {
                    container.LoadingProgram.RemoveAt(i);
                }
            }
        }

        private void CheckConstraintsForVolume(Container container)
        {
            for (int i = 0; i < container.LoadingProgram.Count; i++)
            {
                while (CountTotalBoxesVolume(container.LoadingProgram) > container.Volume)
                {
                    if (container.LoadingProgram[i].Quantity != 0)
                    {
                        container.LoadingProgram[i].Quantity--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void CheckConstraintsForOrder(Container container)
        {
            DowngradeToOrderSizeLevel(container);
            RaiseToOrderLevel(container);
        }

        private void DowngradeToOrderSizeLevel(Container container)
        {
            for (int i = 0; i < container.LoadingProgram.Count; i++)
            {
                var orderQuantity = container.LoadingProgram[i].Box.OrderQuantity;

                if (container.LoadingProgram[i].Quantity > orderQuantity)
                {
                    container.LoadingProgram[i].Quantity = orderQuantity;
                }
            }
        }

        private void RaiseToOrderLevel(Container container)
        {
            for (int i = 0; i < container.LoadingProgram.Count; i++)
            {
                var box = container.LoadingProgram[i].Box;
                var quantity = container.LoadingProgram[i].Quantity;
                var isMaxPossibleQuantity = false;

                if (quantity < box.OrderQuantity)
                {
                    while (!isMaxPossibleQuantity)
                    {
                        container.LoadingProgram[i].Quantity++;

                        if (!DoesMeetTheVolumeLimit(container) ||
                            !DoesMeetTheWeightLimit(container) ||
                            quantity > box.OrderQuantity)
                        {
                            container.LoadingProgram[i].Quantity--;
                            isMaxPossibleQuantity = true;
                        }
                    }
                }
            }
        }

        private bool DoesMeetTheWeightLimit(Container container)
        {
            return CountTotalBoxesWeight(container.LoadingProgram) < container.Capacity;
        }

        private bool DoesMeetTheVolumeLimit(Container container)
        {
            return CountTotalBoxesVolume(container.LoadingProgram) < container.Volume;
        }

        private double CountTotalBoxesVolume(List<BoxQunatityPair> boxesSet)
        {
            double volume = 0;

            foreach (var box in boxesSet)
            {
                volume += box.Box.Volume * box.Quantity;
            }

            return volume;
        }

        private double CountTotalBoxesWeight(List<BoxQunatityPair> boxesSet)
        {
            double weight = 0;

            foreach (var box in boxesSet)
            {
                weight += box.Box.Weight * box.Quantity;
            }

            return weight;
        }

        private void ReverseSweep(Box box, Container container)
        {
            int boxesQuantityForMaxProfit = 0;
            double maxProfit = 0;

            double maxBoxQuantityForTheContainer = CountTheMaximumBoxesQuantityForTheContainer(box, container);

            List<Tuple<Box, double, int>> maxProfitMatrix = new List<Tuple<Box, double, int>>();

            for (int containerCapacity = 0; containerCapacity <= container.Capacity; containerCapacity++)
            {
                for (int boxesQuantity = 0; boxesQuantity <= maxBoxQuantityForTheContainer; boxesQuantity++) // counting one row of a table
                {
                    if (boxesQuantity * box.Weight <= containerCapacity)
                    {
                        if (_optimalSolutionsSet.Count == 0) // counting for the first box
                        {
                            if (box.Cost * boxesQuantity > maxProfit)
                            {
                                maxProfit = box.Cost * boxesQuantity;
                                boxesQuantityForMaxProfit = boxesQuantity;
                            }
                        }
                        else // counting for the boxes left
                        {
                            // .First() is because we add results as AddFirst -> the results are reversed
                            //  Item2 is MaxProfit
                            double tempProfit = box.Cost * boxesQuantity + _optimalSolutionsSet.First.Value[(int)(containerCapacity - box.Weight * boxesQuantity)].Item2;

                            if (tempProfit > maxProfit)
                            {
                                maxProfit = tempProfit;
                                boxesQuantityForMaxProfit = boxesQuantity;
                            }
                        }
                    }
                }

                maxProfitMatrix.Add(new Tuple<Box, double, int>(box, maxProfit, boxesQuantityForMaxProfit));
                maxProfit = 0;
                boxesQuantityForMaxProfit = 0;
            }

            _optimalSolutionsSet.AddFirst(maxProfitMatrix);
        }

        private void DirectSweep(Container container)
        {
            var optimalBoxCombination = new List<BoxQunatityPair>();
            int leftContainerCapacity = 0;
            int optimalBoxQuantity = 0;

            var currentBoxOptimalsMatrix = _optimalSolutionsSet.First;

            foreach (var optimalSolution in _optimalSolutionsSet)
            {
                var currentBox = optimalSolution.First().Item1; // Item1 - Box object

                if (currentBoxOptimalsMatrix == _optimalSolutionsSet.First)
                {
                    optimalBoxQuantity = FindQuantityForMaxProfitInOptimalSolutionMatrix(currentBoxOptimalsMatrix.Value, out leftContainerCapacity);
                    optimalBoxCombination.Add(new BoxQunatityPair(currentBox, optimalBoxQuantity));
                }
                else
                {
                    var previousBoxWeight = currentBoxOptimalsMatrix.Previous.Value.First().Item1.Weight;

                    var containerCapacityForMaxProfit = (int)(leftContainerCapacity - optimalBoxQuantity * previousBoxWeight);

                    optimalBoxQuantity = currentBoxOptimalsMatrix.Value.ElementAt(containerCapacityForMaxProfit).Item3; // Item3 - OptimalQuantity

                    optimalBoxCombination.Add(new BoxQunatityPair(currentBox, optimalBoxQuantity));

                    leftContainerCapacity = containerCapacityForMaxProfit;
                }

                currentBoxOptimalsMatrix = currentBoxOptimalsMatrix.Next;
            }

            container.LoadingProgram = optimalBoxCombination;
            _filledContainers.Add(container);
        }

        private void CheckConstraints(List<Box> boxes, Container container)
        {
            CheckConstraintsForSize(container);
            CheckConstraintsForVolume(container);
            CheckConstraintsForOrder(container);

            DecreaseBoxesOrderQuantityAfterContainerFilled(boxes, container);
        }

        private void DecreaseBoxesOrderQuantityAfterContainerFilled(List<Box> boxes, Container container)
        {
            for (int i = 0; i < container.LoadingProgram.Count; i++)
            {
                var box = container.LoadingProgram[i].Box;
                var quantity = container.LoadingProgram[i].Quantity;

                var boxToChange = boxes.SingleOrDefault(b => b.Equals(box));

                if (boxToChange != null)
                {
                    boxToChange.OrderQuantity -= quantity;
                }
            }
        }

        #endregion
    }
}