using System;
using System.Collections.Generic;
using MathLib.Models;

namespace MathLib {
    public class DPLogic {
        #region Fields
        private List<Box> boxesSet = new List<Box> ();
        private List<Container> containersSet = new List<Container> ();

        private LinkedList<List<OptimalSolution>> optimalSolutionsSet =
            new LinkedList<List<OptimalSolution>> ();
        private List<FilledContainer> loadingPlan =
            new List<FilledContainer> ();

        // private Logger.ILogger logger;

        #endregion

        #region Constructors

        public DPLogic () {
            // this.logger = new Logger.NLogger ();
        }

        // public DPLogic (Logger.ILogger logger) {
        //     this.logger = logger;
        // }

        #endregion

        #region Public methods

        public List<FilledContainer> GetLoadProgram (List<Box> boxes,
            List<Container> containers) {
            if (boxes == null || boxes.Count == 0) {
                // logger.LoggerError ("Trying to pass null or empty boxes list");
                throw new ArgumentException ("The list of boxes can not be null or empty");
            }

            if (containers == null || containers.Count == 0) {
                // logger.LoggerError ("Trying to pass null or empty containers list");
                throw new ArgumentException ("The list of containers can not be null or empty");
            }

            // logger.LoggerInfo ("Starting counting loading program...");

            SetValues (boxes, containers);

            foreach (Container container in containersSet) {
                // logger.LoggerInfo ("Starting reverse sweep...");

                foreach (Box box in boxesSet)
                    ReverseSweep (box, container);

                // logger.LoggerInfo ("Starting direct sweep...");
                DirectSweep (container);

                // logger.LoggerInfo ("Counting optimal solution...");
                optimalSolutionsSet.Clear ();
                Check (container);
            }

            // logger.LoggerInfo ("Counting loading program 
            //                    is successfully finished");

            return loadingPlan;
        }

        #endregion

        #region Private methods

        private void SetValues (List<Box> boxes,
            List<Container> containers) {
            boxes.Sort ((a, b) => a.UnitWeight.CompareTo (b.UnitWeight));
            boxes.Reverse ();

            containers.Sort ((a, b) => a.Capacity.CompareTo (b.Capacity));
            containers.Reverse ();

            boxesSet = boxes;
            containersSet = containers;
        }

        private double CountBoxesMaxQuantityForTheContainer (Box box,
            Container container) {
            return Math.Floor (container.Capacity / box.Weight);
        }

        private int FindMaxProfit (List<OptimalSolution> list,
            out int capacity) {
            double profit = 0;
            int quantity = capacity = 0;

            for (int i = 0; i < list.Count; i++)
                if (list[i].MaxProfit > profit) {
                    profit = list[i].MaxProfit;
                    quantity = list[i].OptimalQuantity;
                    capacity = i;
                }

            return quantity;
        }

        private void CheckSize (Container container) {
            FilledContainer currentContainer = null;

            foreach (FilledContainer fc in loadingPlan)
                if (fc.container.Equals (container))
                    currentContainer = fc;
            foreach (Result r in currentContainer.placedBoxes)
                if (r.Box.Length > currentContainer.container.Length ||
                    r.Box.Length > currentContainer.container.Width ||
                    r.Box.Width > currentContainer.container.Length ||
                    r.Box.Width > currentContainer.container.Width ||
                    r.Box.Height > currentContainer.container.Height) {
                    r.Quantity = 0;
                }
        }

        private double CountTotalBoxesVolume (List<Result> listResult) {
            double volume = 0;

            foreach (Result r in listResult)
                volume += r.Box.Volume * r.Quantity;

            return volume;
        }

        private double CountTotalBoxesWeight (List<Result> listResult) {
            double weight = 0;

            foreach (Result r in listResult)
                weight += r.Box.Weight * r.Quantity;

            return weight;
        }

        private void CheckVolume (Container container) {
            FilledContainer currentContainer = null;

            foreach (FilledContainer fc in loadingPlan)
                if (fc.container.Equals (container))
                    currentContainer = fc;

            foreach (Result r in currentContainer.placedBoxes)
                while (CountTotalBoxesVolume (currentContainer.placedBoxes) >
                    currentContainer.container.Volume)
                    if (r.Quantity != 0) r.Quantity--;
                    else { break; }
        }

        private bool DoesMeetTheWeightLimit (Container container,
            List<Result> listResult) {
            return CountTotalBoxesWeight (listResult) < container.Capacity;
        }

        private bool DoesMeetTheVolumeLimit (Container container,
            List<Result> listResult) {
            return CountTotalBoxesVolume (listResult) < container.Capacity;
        }

        private void DowngradeToOrderSizeLevel (Container container) {
            FilledContainer currentContainer = null;

            foreach (FilledContainer fc in loadingPlan)
                if (fc.container.Equals (container))
                    currentContainer = fc;

            foreach (Result r in currentContainer.placedBoxes)
                if (r.Quantity != r.Box.OrderQuantity)
                    if (r.Quantity > r.Box.OrderQuantity)
                        r.Quantity = r.Box.OrderQuantity;
        }

        private void RaiseToOrderLevel (Container container) {
            FilledContainer currentContainer = null;

            foreach (FilledContainer fc in loadingPlan)
                if (fc.container.Equals (container))
                    currentContainer = fc;

            foreach (Result r in currentContainer.placedBoxes) {
                bool condition = true;

                if (r.Quantity != r.Box.OrderQuantity)
                    while (condition) {
                        r.Quantity++;

                        if (!DoesMeetTheVolumeLimit (currentContainer.container,
                                currentContainer.placedBoxes) ||
                            !DoesMeetTheWeightLimit (currentContainer.container,
                                currentContainer.placedBoxes) ||
                            r.Quantity > r.Box.OrderQuantity) {
                            r.Quantity--;
                            condition = false;
                        }
                    }
            }
        }

        private void CheckOrder (Container container) {
            DowngradeToOrderSizeLevel (container);
            RaiseToOrderLevel (container);
        }

        private void ReverseSweep (Box box, Container container) {
            int quantity = 0;
            double profit = 0;
            double maxj = CountBoxesMaxQuantityForTheContainer (box,
                container);
            List<OptimalSolution> maxProfitMatrix =
                new List<OptimalSolution> ();

            for (int i = 0; i <= container.Capacity; i++) {
                for (int j = 0; j <= maxj; j++) {

                    if (j * box.Weight <= i) {
                        if (optimalSolutionsSet.Count == 0) {
                            if (box.Cost * j > profit) {
                                profit = box.Cost * j;
                                quantity = j;
                            }
                        } else {
                            double tempProfit = box.Cost * j +
                                optimalSolutionsSet.First.Value[(int) (i - box.Weight * j)].MaxProfit;

                            if (tempProfit > profit) {
                                profit = tempProfit;
                                quantity = j;
                            }
                        }
                    }
                }

                maxProfitMatrix.Add (new OptimalSolution (box, profit,
                    quantity));
                profit = 0;
                quantity = 0;
            }

            optimalSolutionsSet.AddFirst (maxProfitMatrix);
        }

        private void DirectSweep (Container container) {
            LinkedListNode<List<OptimalSolution>> Current =
                optimalSolutionsSet.First;
            List<Result> listResult = new List<Result> ();
            int capacity = 0;
            int quantity = 0;

            for (int i = 0; i < optimalSolutionsSet.Count; i++) {
                if (Current == optimalSolutionsSet.First) {
                    quantity = FindMaxProfit (Current.Value, out capacity);
                    listResult.Add (new Result (Current.Value[capacity].Box,
                        quantity));
                    Current = Current.Next;
                } else {
                    int index = (int) (capacity - quantity *
                        Current.Previous.Value[capacity].Box.Weight);
                    listResult.Add (new Result (Current.Value[index].Box,
                        Current.Value[index].OptimalQuantity));
                    capacity = index;
                    quantity = Current.Value[index].OptimalQuantity;
                    Current = Current.Next;
                }
            }

            loadingPlan.Add (new FilledContainer (container, listResult));
        }

        private Box FindBoxInBoxesSet (Box box) {
            foreach (Box b in boxesSet)
                if (b.Equals (box))
                    return b;

            return null;
        }

        private void Check (Container container) {
            CheckSize (container);
            CheckVolume (container);
            CheckOrder (container);

            FilledContainer currentContainer = null;

            foreach (FilledContainer fc in loadingPlan)
                if (fc.container.Equals (container))
                    currentContainer = fc;

            foreach (Result r in currentContainer.placedBoxes) {
                Box properBox = FindBoxInBoxesSet (r.Box);

                properBox.OrderQuantity -= r.Quantity;
            }
        }

        #endregion

    }
}